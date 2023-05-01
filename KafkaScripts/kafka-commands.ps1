#list all topics
docker exec poc-kafka-kafka-1 kafka-topics --list --bootstrap-server localhost:9092

#list all messages from a topic
docker exec poc-kafka-kafka-1 kafka-console-consumer --topic EventTopic --bootstrap-server localhost:9092 --from-beginning  

#delete a topic
docker exec poc-kafka-kafka-1 kafka-topics --delete --bootstrap-server localhost:9092 --topic EventTopic