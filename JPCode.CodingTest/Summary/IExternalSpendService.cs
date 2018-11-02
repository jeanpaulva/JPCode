using TDD.CodingTest.External;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDD.CodingTest.Summary
{
    public interface IExternalSpendService
    {
        ExternalInvoice[] GetInvoices(string supplierId);
    }
}
