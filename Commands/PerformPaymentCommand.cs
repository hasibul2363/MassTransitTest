using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Commands
{
    public class PerformPaymentCommand
    {
        public Guid Id { get; set; }
        public decimal Amount { get; set; }
        public string StatusMessage { get; set; }

    }
}
