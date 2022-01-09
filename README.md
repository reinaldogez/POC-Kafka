# POC-Kafka
POC sobre o Apache Kafka como serviço de mensageria.
=========

## Comandos
Criar um tópico
~~~ps1
docker exec kafka kafka-topics --create --bootstrap-server localhost:29092 --partitions 1 --replication-factor 1 --topic Test
~~~

## Referências
