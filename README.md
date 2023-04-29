# POC-Kafka
POC on Apache Kafka as a messaging service, creating topics, using consumers, and so on.
=========

## Propriedades de configuração interessantes

ConsumerConfig.EnableAutoCommit - Por padrão, conforme o consumer lê as mensagens do Kafka, ele confirma periodicamente seu offset atual para as partições que ele está lendo. Para ter mais controle sobre exatamente quando os offsets são confirmados você pode definir ConsumerConfig.EnableAutoCommit como false e chamar o método commit no consumer.



## Docker

## Comandos
Nesse momento com o Kafka rodando, vamos criar um tópico chamado "Test".
Vamos criar o tópico utilizando [kafka-console-producer command line tool](kafka-console-producer command line tool) 
~~~ps1
docker exec kafka kafka-topics --create --bootstrap-server localhost:29092 --partitions 1 --replication-factor 1 --topic Test
~~~
Obs: os tópicos também podem ser criados em C# utilizando a [Kafka .NET Client Library](https://docs.confluent.io/clients-confluent-kafka-dotnet/current/overview.html)

## References
[Guide to Setting Up Apache Kafka Using Docker](https://www.baeldung.com/ops/kafka-docker-setup)

[Event-Driven Architecture with Apache Kafka for .NET Developers Part 2 - Event Consumer](https://thecloudblog.net/post/event-driven-architecture-with-apache-kafka-for-.net-developers-part-2-event-consumer/)