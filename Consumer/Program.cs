using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using MassTransit;
using MassTransit.SimpleInjectorIntegration;
using SimpleInjector;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {

            var container = new Container();
            container.Register<RequestInfo>();
            container.Register<PerformPaymentCommandHandler>();


            var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri("rabbitmq://localhost:5672/"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                
                cfg.ReceiveEndpoint(host, "PAYMENT_QUEUE", e =>
                {
                    e.Consumer<PerformPaymentCommandHandler>(container);
                });
            });
           
            busControl.Start();
            Console.WriteLine("Listening...");
        }
    }
}
