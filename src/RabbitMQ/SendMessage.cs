using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace RabbitMQ
{
    public class SendMessage : ISendMessage,IDisposable
    {
        private readonly IModel _model;
        private readonly string _exchange;
        private readonly string _route;

        public SendMessage(string exchange, string queue, string exchangeType, string route)
        {
            _exchange = exchange;
            _route = route;

            // Step 1 : Connection Factory
            var connectionFactory =
               new ConnectionFactory { HostName = RabbitMqConst.Host };


            // Step 2 : Create connection from factory
            var connection = connectionFactory.CreateConnection();
            // Step 3 : Create model 
            _model= connection.CreateModel();

            // Step 4: Set exchange
            _model.ExchangeDeclare(
      exchange: exchange,
      type: exchangeType);

            // Step 5: Set Queue
            _model.QueueDeclare(
               queue: queue,
               durable: false, exclusive: false,
               autoDelete: false, arguments: null);

            // Step 6: Bind queue
            _model.QueueBind(
                queue: queue,
                exchange: exchange,
                routingKey: route);
        }

        public void PublishMessage(object message)
        {
            
           

            var jsonMessage = JsonConvert.SerializeObject(message);

            var messageProperties = _model.CreateBasicProperties();
            messageProperties.ContentType = RabbitMqConst.ContentType;

            _model.BasicPublish(
                exchange: _exchange,
                routingKey: _route,
                basicProperties: messageProperties,
                body: Encoding.UTF8.GetBytes(jsonMessage));
        }
        public void Dispose()
        {
            if (!_model.IsClosed)
                _model.Close();
        }
    }
}
