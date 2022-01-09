using System;
using System.Net;
using Confluent.Kafka;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace POC_Kafka
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Produce("Test");
        }

        static void Produce(string topic)
        {
            var config = new ProducerConfig
            {
                //BootstrapServers = "host1:9092,host2:9092",
                BootstrapServers = "localhost:29092",
                
                ClientId = Dns.GetHostName(),
                //...
            };


            using (var producer = new ProducerBuilder<string, string>(config).Build())
            {
             int numProduced = 0;
                int numMessages = 10;
                for (int i=0; i<numMessages; ++i)
                {
                    var key = "alice";
                    var val = JObject.FromObject(new { count = i }).ToString(Formatting.None);

                    Console.WriteLine($"Producing record: {key} {val}");

                    producer.Produce(topic, new Message<string, string> { Key = key, Value = val },
                        (deliveryReport) =>
                        {
                            if (deliveryReport.Error.Code != ErrorCode.NoError)
                            {
                                Console.WriteLine($"Failed to deliver message: {deliveryReport.Error.Reason}");
                            }
                            else
                            {
                                Console.WriteLine($"Produced message to: {deliveryReport.TopicPartitionOffset}");
                                numProduced += 1;
                            }
                        });
                }

                producer.Flush(TimeSpan.FromSeconds(10));

                Console.WriteLine($"{numProduced} messages were produced to topic {topic}");
            }
        }
    }
}





