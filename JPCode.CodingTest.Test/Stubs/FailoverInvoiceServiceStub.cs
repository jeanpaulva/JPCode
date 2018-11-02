using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDD.CodingTest.External;
using TDD.CodingTest.Invoices;
using TDD.CodingTest.Suppliers;

namespace TDD.CodingTest.DataService.Stubs
{
    public class FailoverInvoiceServiceStub : IFailoverInvoiceService
    {
        public FailoverInvoiceCollection GetInvoices(int supplierId)
        {
            FailoverInvoiceCollection result = new FailoverInvoiceCollection();

            if (supplierId == 2)
            {
                IList<ExternalInvoice> list = new List<ExternalInvoice>();
                list.Add(new ExternalInvoice() { Year = 2018, TotalAmount = 900 }); // Only 1 Year
                result.Timestamp = DateTime.Now;
                result.Invoices = list.ToArray();
            }
            else if (supplierId == 3)
            {
                IList<ExternalInvoice> list = new List<ExternalInvoice>();
                list.Add(new ExternalInvoice() { Year = 2018, TotalAmount = 900 }); // Only 1 Year
                result.Timestamp = DateTime.Now;
                result.Invoices = list.ToArray();
            }
            else if (supplierId == 4)
            {
                IList<ExternalInvoice> list = new List<ExternalInvoice>();
                list.Add(new ExternalInvoice() { Year = 2017, TotalAmount = 800 });
                result.Timestamp = DateTime.Now.AddDays(-32); // Obsolete Date
                result.Invoices = list.ToArray();
            }

            return result;
        }
    }
}
