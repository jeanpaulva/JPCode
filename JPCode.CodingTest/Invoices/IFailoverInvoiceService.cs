using System;
using System.Collections.Generic;
using System.Text;

namespace TDD.CodingTest.Invoices
{
    public interface IFailoverInvoiceService
    {
        FailoverInvoiceCollection GetInvoices(int supplierId);
    }
}
