using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commands;
using MassTransit;

namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://localhost:5672"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
            });

            while (true)
            {
                Console.WriteLine("Endter to listen or q to exit");
                var response = Console.ReadLine();
                if (response == "q")
                {
                    break;
                }
                var sendToUri = new Uri("rabbitmq://localhost:5672/PAYMENT_QUEUE");
                var endPoint = bus.GetSendEndpoint(sendToUri).Result;
                endPoint.Send<PerformPaymentCommand>(new PerformPaymentCommand { Id=Guid.NewGuid(),  Amount = 200, StatusMessage  = Guid.NewGuid().ToString() });
            }

            Console.WriteLine("Any key to exit");
        }
    }
}
