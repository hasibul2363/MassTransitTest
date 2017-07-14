using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using GreenPipes;
using MassTransit;
using SimpleInjector.Extensions.ExecutionContextScoping;

namespace Consumer.Middleware
{

    public class TestFilter<T> : IFilter<T> where T : class, PipeContext
    {
        public void Probe(ProbeContext context)
        {
           
        }

        public async Task Send(T context, IPipe<T> next)
        {
            //I want to set value from middleware like following (and then want to get this value from consumer/PerformPaymentCommandHandler)
            Program.Container.GetInstance<RequestInfo>().Token = "Value from Middleware";
            //But above line is throwing " Fault: The RequestInfo is registered as 'Execution Context Scope' lifestyle, but the instance is requested outside the context of a Execution Context Scope."


            //Then tried with following but no luck
            /*
            using (Program.Container.BeginExecutionContextScope())
            {
                Program.Container.GetInstance<RequestInfo>().Token = "Value from Middleware";
                await next.Send(context);
            }*/
        }
    }

    public class TestSpecification<T> : IPipeSpecification<T> where T : class, PipeContext
    {
        public IEnumerable<ValidationResult> Validate()
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public void Apply(IPipeBuilder<T> builder)
        {
            builder.AddFilter(new TestFilter<T>());
            
        }
    }

    public static class ExampleMiddlewareConfiguratorExtensions
    {
        public static void UseTestMiddleware<T>(this IPipeConfigurator<T> configurator) where T : class, PipeContext
        {
            configurator.AddPipeSpecification(new TestSpecification<T>());
        }
    }
}
