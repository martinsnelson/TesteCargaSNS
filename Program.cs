using Amazon.SimpleNotificationService;
using System.Text.Json;


//Change this to control what SNS topic you are publishing to
const string TopicArn = "arn:aws:sns:sa-east-1:xxx:xxx";

//Change this to control how many messages to send in the load test
const int numberOfbookMessagesSend = 10;
const int NumberOfMessagesToSend = 10000;

await Run();

static async Task Run()
{
    int loteMSGTotal = 0;
    int msgTotalEnviadas = 0;

    string start = DateTime.Now.ToString("dd-MM-yyyy-HH:mm:ss-fff");
    Console.WriteLine($"Inicio: {start}");

    for (int b = 0; b < numberOfbookMessagesSend; b++) 
    {
        loteMSGTotal++;
        List<string> serializedMessages = new List<string>();

        for (int i = 0; i < NumberOfMessagesToSend; i++)
        {
            msgTotalEnviadas++;
            //Customise the content of your SNS messages as need be here
            var message = new
            {
                Property = $"MSG TEST NUMBER {i}"
            };
            Console.WriteLine($"MSG número {msgTotalEnviadas} criada");

            serializedMessages.Add(JsonSerializer.Serialize(message));
        }

        var snsClient = new AmazonSimpleNotificationServiceClient(Amazon.RegionEndpoint.SAEast1);
        IEnumerable<Task> tasks = serializedMessages.Select(async serializedMessage =>
        {
            try
            {
                await snsClient.PublishAsync(TopicArn, serializedMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        });

        await Task.WhenAll(tasks);
        Console.WriteLine($"Lote de {loteMSGTotal} MSG, MSG enviadas {msgTotalEnviadas} Done!");
    }

    string stop = DateTime.Now.ToString("dd-MM-yyyy-HH:mm:ss-fff");
    Console.WriteLine($"Inicio: {start}");
    Console.WriteLine($"Fim:    {stop}");

    Console.ReadLine();
}