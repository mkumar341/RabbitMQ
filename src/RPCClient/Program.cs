using RabbitMQ;
using RabbitMQ.Client;
using PublisherClient.Model;
using System;
using System.Text;

namespace PublisherClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Packet mailpPacket = new Packet { Id = 1, Address = "Steet 1", Country = "Finland" };

            using (var sendMessage = new SendMessage(RabbitMqConst.PackageExchange, RabbitMqConst.PackageQueue, ExchangeType.Direct, RabbitMqConst.RouteKey))
            {
                sendMessage.PublishMessage(mailpPacket);
            }

            
            Console.WriteLine(" Press [Enter] key to exit.");
            Console.ReadLine();
        }
    }
}
