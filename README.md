# POC-Kafka
POC sobre o Apache Kafka como serviço de mensageria.
=========

## Comandos
Criar um tópico
~~~ps1
docker exec kafka kafka-topics --create --bootstrap-server localhost:29092 --partitions 1 --replication-factor 1 --topic Test
~~~

## Docker

### Alterar para o Docker daemon Linux
Antes de rodar o comando docker-compose, é necessário(caso esteja em ambiente windows) alterar com qual daemon (Linux ou Windows) o Docker CLI se comunica. 
Para alterar para o daemon Linux, que é o que o Kafka utiliza, basta rodar o seguinte comando.
~~~ps1
& 'C:\Program Files\Docker\Docker\DockerCli.exe' -SwitchDaemon
~~~
[Documentação sobre o assunto](https://docs.docker.com/desktop/windows/#:~:text=From%20the%20Docker%20Desktop%20menu,Linux%20containers%20(the%20default).)

## Referências
[Guide to Setting Up Apache Kafka Using Docker](https://www.baeldung.com/ops/kafka-docker-setup)
[Event-Driven Architecture with Apache Kafka for .NET Developers Part 2 - Event Consumer](https://thecloudblog.net/post/event-driven-architecture-with-apache-kafka-for-.net-developers-part-2-event-consumer/)