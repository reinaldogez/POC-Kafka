# POC-Kafka

Proof of Concept (POC) for Apache Kafka as a messaging service, including topic creation, using consumers, and more.

## Interesting Configuration Properties

`ConsumerConfig.EnableAutoCommit` - By default, as the consumer reads messages from Kafka, it periodically commits its current offset for the partitions it's reading. To have more control over when offsets are committed, you can set `ConsumerConfig.EnableAutoCommit` to `false` and call the `commit` method on the consumer.

## Docker

## Commands

With Kafka running, let's create a topic called "Test". We will create the topic using the [kafka-console-producer command line tool](https://kafka.apache.org/documentation/#quickstart_send):

~~~ps1
docker exec kafka kafka-topics --create --bootstrap-server localhost:29092 --partitions 1 --replication-factor 1 --topic Test
~~~

Note: Topics can also be created in C# using the [Kafka .NET Client Library](https://docs.confluent.io/clients-confluent-kafka-dotnet/current/overview.html)

## References
[Guide to Setting Up Apache Kafka Using Docker](https://www.baeldung.com/ops/kafka-docker-setup)

[Event-Driven Architecture with Apache Kafka for .NET Developers Part 2 - Event Consumer](https://thecloudblog.net/post/event-driven-architecture-with-apache-kafka-for-.net-developers-part-2-event-consumer/)