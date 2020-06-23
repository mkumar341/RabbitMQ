using Newtonsoft.Json;
using RabbitMQ;
using RabbitMQ.Client;
using System;
using System.Text;

namespace ConsumerServer
{
    class Program
    {
        static void Main(string[] args)
        {

            using (var recMessage = new ReceiveMessage (RabbitMqConst.PackageExchange, RabbitMqConst.PackageQueue, ExchangeType.Direct, RabbitMqConst.RouteKey))
            {
                var message  = recMessage.ConsumeMessage();
                Console.WriteLine();
            };


            Console.WriteLine(" Press [Enter] key to exit.");
            Console.ReadLine();
        }
    }
}

