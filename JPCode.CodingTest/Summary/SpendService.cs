using TDD.CodingTest.External;
using TDD.CodingTest.Invoices;
using TDD.CodingTest.Suppliers;
using System.Collections.Generic;
using System.Linq;
using Unity;

namespace TDD.CodingTest.Summary
{
    public abstract class SpendService
    {
        protected ISupplierService supplierService;

        public abstract SpendSummary GetTotalSpend(int supplierId);
    }
}
