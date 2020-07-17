using Blog.Entities.Events;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Extensions
{
    public static class RabbitMQExtensions
    {
        public static void AddRabbitMQ(this IServiceCollection services)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "root",
                Password = "root"
            };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            services.AddSingleton(channel);
        }

        public static void Send<T>(this IModel channel, T message)
            where T : IEvent
        {
            string ClassName = message.GetType().Name;

            channel.QueueDeclare(queue: "events",
                                        durable: false,
                                        exclusive: false,
                                        autoDelete: false,
                                        arguments: null);


            string deserialize = JsonConvert.SerializeObject(message, settings: new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            var body = Encoding.UTF8.GetBytes(deserialize);

            IBasicProperties basicProperties = channel.CreateBasicProperties();

            Dictionary<string, object> headers = new Dictionary<string, object>
            {
                { "class", ClassName }
            };
            basicProperties.Headers = headers;
            channel.BasicPublish(exchange: "",
                                    routingKey: "events",
                                    basicProperties: basicProperties,
                                    body: body);
        }
    }
}
