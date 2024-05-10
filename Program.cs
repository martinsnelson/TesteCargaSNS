using Amazon.SimpleNotificationService;
using System.Diagnostics;
using System.Text.Json;

//Change this to control how many messages to send in the load test
await Run();

static async Task Run()
{
    Stopwatch stopwatch = Stopwatch.StartNew();

   
    // Defina o ARN do tópico do SNS
    string TopicArn = "arn:aws:sns:sa-east-1:xxxx:xxxxxx";

    var snsClient = new AmazonSimpleNotificationServiceClient(Amazon.RegionEndpoint.SAEast1);

    // Variáveis globais de controle
    int msgTotalEnviadas = 0;
    int loteMSGTotal = 0;

    int numberOfbookMessagesSend = 1; // Número de lotes de mensagens para enviar
    int NumberOfMessagesToSend = 10000; // Número de mensagens por lote

    // Número de processadores lógicos disponíveis
    int maxDegreeOfParallelism = Environment.ProcessorCount;

    // Limite o número de mensagens que podem ser processadas simultaneamente
    // Por exemplo, limita a 10 mensagens simultâneas ou ajusta conforme necessário
    int bufferLimit = 100;
    using var semaphore = new SemaphoreSlim(bufferLimit);

    // Processamento de cada lote de mensagens
    for (int b = 0; b < numberOfbookMessagesSend; b++)
    {
        loteMSGTotal++;
        List<string> serializedMessages = new List<string>();

        // Cria mensagens no lote
        for (int i = 0; i < NumberOfMessagesToSend; i++)
        {
            msgTotalEnviadas++;
            var message = new
            {
                Property = $"MSG TEST NUMBER {i}"
            };

            // Serializa a mensagem
            serializedMessages.Add(JsonSerializer.Serialize(message));
        }

        // Usando Parallel.ForEach com opções para controlar o grau de paralelismo
        Parallel.ForEach(serializedMessages, new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism }, async serializedMessage =>
        {
            // Aguarda uma posição disponível no buffer
            await semaphore.WaitAsync();

            try
            {
                // Publica a mensagem
                await snsClient.PublishAsync(TopicArn, serializedMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // Libera uma posição do buffer
                semaphore.Release();
            }
        });
    }

    stopwatch.Stop();
    Console.WriteLine($"Tempo para enviar as mensagens lote de msg: {stopwatch.ElapsedMilliseconds} ms");

    Console.ReadLine();
}