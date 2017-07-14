using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Commands;
using Common;
using MassTransit;

namespace Consumer
{
    public class PerformPaymentCommandHandler : IConsumer<PerformPaymentCommand>
    {

        public RequestInfo RequestInfo { get; set; }
        public PerformPaymentCommandHandler(RequestInfo requestInfo)
        {
            RequestInfo = requestInfo;
        }

        public async Task Consume(ConsumeContext<PerformPaymentCommand> context)
        {
            Console.WriteLine($"Handled Message with Id {context.Message.Id} and Amount {context.Message.Amount}");
            Console.WriteLine($"Wtih Token {RequestInfo.Token}");
        }
    }
}
