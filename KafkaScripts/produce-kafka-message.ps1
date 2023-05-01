# ProduceKafkaMessage.ps1

$bootstrapServer = "localhost:29092"
$topic = "EventTopic"
$guid = [guid]::NewGuid().ToString()


$jsonMessage = @{
    '$type' = 'PocKafka.Events.PostCreatedEvent, PocKafka'
    'Author' = 'Robert C. Martin'
    'Message' = 'Truth can only be found in one place: the code.'
    'DatePosted' = (Get-Date -Format s)
    'Id' = $guid
    'Type' = 'PostCreatedEvent'
    'Version' = 0
} | ConvertTo-Json

$message = $jsonMessage -replace '\r?\n', ' '


$KafkaContainerName = "poc-kafka-kafka-1"

echo $message | docker exec -i $KafkaContainerName kafka-console-producer --bootstrap-server $bootstrapServer --topic $topic