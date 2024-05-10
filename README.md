# Benchmarks SNS, SQS e Lambda.

# Primícias:
- Lote de 10k msg enviado limitado a 256k.
- Conta AWS com acesso via console ou CLI
- Permissão para configurar roles AWS IAM
- Obs: Avaliar possibilidade de envio do lote de 10k de msg dado o limite atual de 256k.
- Alternativa para envio de msg com até 2GB, consultar o link abaixo:
https://docs.aws.amazon.com/sns/latest/dg/large-message-payloads.html

# Tempo de leitura SQS:
- 36 minutos, 1M msg.
- 6 minutos, 100k msg.
- 2 minutos, 10k msg. 
- 1 minuto, 1k msg.

# Tempo de publicação
- 36m, 1M
- 1m:29s, 100k msg
- 2s:35ms, 10k msg
- 1s:3ms, 1k msg