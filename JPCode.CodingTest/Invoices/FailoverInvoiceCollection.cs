using TDD.CodingTest.External;
using System;

namespace TDD.CodingTest.Invoices
{
    public class FailoverInvoiceCollection
    {
        public DateTime Timestamp { get; set; }
        public ExternalInvoice[] Invoices { get; set; }

        public FailoverInvoiceCollection()
        {
            this.Invoices = new ExternalInvoice[0];
        }
    }
}