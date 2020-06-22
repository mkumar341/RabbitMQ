using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RabbitMQ
{
    public class ReceiveMessage : IReceiveMessage,IDisposable
    {
        private readonly IModel _model;
        private readonly string _exchange;
        private readonly string _route;

        public ReceiveMessage(string exchange, string queue, string exchangeType, string route)
        {
            _exchange = exchange;
            _route = route;

            // Step 1 : Connection Factory
            var connectionFactory =
               new ConnectionFactory { HostName = RabbitMqConst.Host};

            // Step 2 : Create connection from factory
            var connection = connectionFactory.CreateConnection();
            // Step 3 : Create model 
            _model = connection.CreateModel();
                       

            // Step 4: Set Queue
            _model.QueueDeclare(
               queue: queue,
               durable: false, exclusive: false,
               autoDelete: false, arguments: null);

            // Step 5: Content setting
           _model.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);



        }

        public string ConsumeMessage()
        {
            var consumer = new EventingBasicConsumer(_model);
            string messageReceived = string.Empty;
            consumer.Received += (sender, eventArgs) =>
            {
                var mimeType = eventArgs.BasicProperties.ContentType;
                if (mimeType != RabbitMqConst.ContentType)
                    throw new ArgumentException(
                        $"Can't handle content type {mimeType}");
                var body = eventArgs.Body.ToArray();

               messageReceived = Encoding.UTF8.GetString(body);
                
                Console.WriteLine(messageReceived);

                _model.BasicAck(deliveryTag: eventArgs.DeliveryTag,
                    multiple: false);
            };
            _model.BasicConsume(
                queue: RabbitMqConst.PackageQueue,
                autoAck: true,
                consumer: consumer);

            return messageReceived;

        }

        public void Dispose()
        {
            if (!_model.IsClosed)
                _model.Close();
        }
    }
}
