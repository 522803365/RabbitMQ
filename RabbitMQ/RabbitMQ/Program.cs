using EasyNetQ;
using RabbitMQ.Client;
using System;
using System.Text;

namespace RabbitMQ
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
            using (IConnection con = conFactory.CreateConnection())
            {
                using (IModel channel = con.CreateModel())
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
                    for (int i = 10000; i < 60000; i++)
                    {
                        String message = "消息内容: " + DateTime.Now.ToString("yyyy - MM - dd HH: mm: ss.fff") + "第" + i.ToString() + "写入";
                        Console.WriteLine(message);
                        byte[] body = Encoding.UTF8.GetBytes(message);
                        channel.BasicPublish(exchange: "", routingKey: queueName, basicProperties: null, body: body);
                        Console.WriteLine("成功发送消息:" + message);
                    }
                    Console.ReadLine();
                }
            }
        }


        //public class TextMessage
        //{
        //    public string Text { get; set; }
        //}

        //    static void Main(string[] args)
        //    {
        //        //方式1
        //        var connectionString = "host=42.194.214.79;virtualHost=/;username=admin;password=admin;timeout=60";
        //        var bus1 = RabbitHutch.CreateBus(connectionString);

        //        //方式2
        //        HostConfiguration host = new HostConfiguration();
        //        host.Host = "42.194.214.79";
        //        host.Port = 5672;

        //        ConnectionConfiguration connection = new ConnectionConfiguration();
        //        connection.Port = 5672;
        //        connection.Password = "admin";
        //        connection.UserName = "admin";
        //        connection.VirtualHost = "/";
        //        connection.Timeout = 60;
        //        connection.Hosts = new HostConfiguration[] { host };

        //        var bus2 = RabbitHutch.CreateBus(connection, services => { });

        //        //使用bus实现业务
        //        //关闭连接字需要调用bus的Dispose方法
        //        Console.ReadKey();
        //    }

    }
}
