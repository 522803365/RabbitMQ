using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace RabbitMQRead
{
    class Program
    {
        static void Main(string[] args)
        {


            Console.WriteLine("Start");

            IConnectionFactory conFactory = new ConnectionFactory
            {
                HostName = "42.194.214.79",
                Port = 5672,
                UserName = "admin",
                Password = "admin"
            };
            using (IConnection conn = conFactory.CreateConnection())
            {
                using (IModel channel = conn.CreateModel())
                {
                    String queueName = String.Empty;
                    if (args.Length > 0)
                        queueName = args[0];
                    else
                        queueName = "queue1";
                    channel.QueueDeclare(
                      queue: queueName,
                      durable: false,
                      exclusive: false,
                      autoDelete: false,
                      arguments: null
                       );
                    var consumer = new EventingBasicConsumer(channel);
                    int i = 0;
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine("第"+(i++)+"次读取消息队列信息为:【" + message+"】");
                    };
                    channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
                    Console.ReadKey();
                }
            }

        }
    }
}
