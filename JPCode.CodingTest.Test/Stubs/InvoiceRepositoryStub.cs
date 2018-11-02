using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDD.CodingTest.Invoices;
using TDD.CodingTest.Suppliers;

namespace TDD.CodingTest.DataService.Stubs
{
    public class InvoiceRepositoryStub : IInvoiceRepository
    {
        public IQueryable<Invoice> Get()
        {
            IList<Invoice> result = new List<Invoice>();

            result.Add(new Invoice() { SupplierId = 1, InvoiceDate = new DateTime(2018, 1, 1), Amount = 1000 });
            result.Add(new Invoice() { SupplierId = 1, InvoiceDate = new DateTime(2018, 2, 1), Amount = 1000 });
            result.Add(new Invoice() { SupplierId = 1, InvoiceDate = new DateTime(2016, 1, 1), Amount = 1000 });

            return result.AsQueryable();
        }
    }
}
