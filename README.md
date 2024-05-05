# Benchmarks SNS, SQS e Lambda.

# Primícias:
- Lote de 10k msg enviado limitado a 256k.
- Obs: Avaliar possibilidade de envio do lote de 10k de msg dado o limite atual de 256k.
- Alternativa para envio de msg com até 2GB, consultar o link abaixo:
https://docs.aws.amazon.com/sns/latest/dg/large-message-payloads.html

# Tempo de leitura:
- 60 minutos, 1M msg.
- 6 minutos, 100k msg.
- 2 minutos, 10k msg. 
- 1 minuto, 1k msg.

# [TODOS Arquitetura]: ajustes, processamento em paralelo, lambda e C#.
- Processar 10k msg em 30 segundos.
- Processar 1M msg em 35m.