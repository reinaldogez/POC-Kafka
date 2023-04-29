#list all topics
docker exec poc-kafka-kafka-1 kafka-topics --list --bootstrap-server localhost:9092

#delete a topic
docker exec poc-kafka-kafka-1 kafka-topics --delete --bootstrap-server localhost:9092 --topic my-topic