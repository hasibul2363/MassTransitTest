using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Consumer.Middleware;
using MassTransit;
using MassTransit.SimpleInjectorIntegration;
using SimpleInjector;
using SimpleInjector.Extensions.ExecutionContextScoping;

namespace Consumer
{
    class Program
    {
        public static Container Container;
        static void Main(string[] args)
        {

            var container = new Container();
            container.Options.DefaultScopedLifestyle = new ExecutionContextScopeLifestyle();
            container.Register<RequestInfo>(Lifestyle.Scoped);
            container.Register<PerformPaymentCommandHandler>();
            container.Verify();
            Container = container;

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

                cfg.UseTestMiddleware();
            });
           
            busControl.Start();
            Console.WriteLine("Listening...");
        }
    }
}
