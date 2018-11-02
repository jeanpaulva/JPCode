using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDD.CodingTest.Exceptions;
using TDD.CodingTest.External;
using TDD.CodingTest.Invoices;
using TDD.CodingTest.Summary;
using TDD.CodingTest.Suppliers;

namespace TDD.CodingTest.DataService.Stubs
{
    public class ExternalInvoiceServiceStub : IExternalSpendService
    {
        public ExternalInvoice[] GetInvoices(string supplierId)
        {
            if (int.Parse(supplierId) > 2)
                throw new WebTimeoutException();

            IList<ExternalInvoice> result = new List<ExternalInvoice>();

            result.Add(new ExternalInvoice() { Year = 2018, TotalAmount = 1000 });
            result.Add(new ExternalInvoice() { Year = 2017, TotalAmount = 1000 });

            return result.ToArray();
        }
    }
}
