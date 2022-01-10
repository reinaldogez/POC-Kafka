# POC-Kafka
POC sobre o Apache Kafka como serviço de mensageria.
=========

## Propriedades de configuração interessantes

ConsumerConfig.EnableAutoCommit - Por padrão, conforme o consumer lê as mensagens do Kafka, ele confirma periodicamente seu offset atual para as partições que ele está lendo. Para ter mais controle sobre exatamente quando os offsets são confirmados você pode definir ConsumerConfig.EnableAutoCommit como false e chamar o método commit no consumer.


# A seguir um "step-by-step" para rodar o projeto.
## Docker

### Alterar para o Docker daemon Linux
Antes de rodar o comando docker-compose, é necessário(caso esteja em ambiente windows) alterar com qual daemon (Linux ou Windows) o Docker CLI se comunica. 
Para alterar para o daemon Linux, que é o que o Kafka utiliza, basta rodar o seguinte comando.
~~~ps1
& 'C:\Program Files\Docker\Docker\DockerCli.exe' -SwitchDaemon
~~~
[Documentação sobre o assunto](https://docs.docker.com/desktop/windows/#:~:text=From%20the%20Docker%20Desktop%20menu,Linux%20containers%20(the%20default).)

### docker-compose
Com o daemon Linux configurado, basta rodar o docker-compose
~~~ps1
docker-compose up -d
~~~
## Comandos
Nesse momento com o Kafka rodando, vamos criar um tópico chamado "Test".
Vamos criar o tópico utilizando [kafka-console-producer command line tool](kafka-console-producer command line tool) 
~~~ps1
docker exec kafka kafka-topics --create --bootstrap-server localhost:29092 --partitions 1 --replication-factor 1 --topic Test
~~~
Obs: os tópicos também podem ser criados em C# utilizando a [Kafka .NET Client Library](https://docs.confluent.io/clients-confluent-kafka-dotnet/current/overview.html)

## Referências
[Guide to Setting Up Apache Kafka Using Docker](https://www.baeldung.com/ops/kafka-docker-setup)
[Event-Driven Architecture with Apache Kafka for .NET Developers Part 2 - Event Consumer](https://thecloudblog.net/post/event-driven-architecture-with-apache-kafka-for-.net-developers-part-2-event-consumer/)