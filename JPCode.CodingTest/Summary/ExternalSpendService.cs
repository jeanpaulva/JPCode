using TDD.CodingTest.External;
using TDD.CodingTest.Invoices;
using TDD.CodingTest.Suppliers;
using System;
using System.Linq;

namespace TDD.CodingTest.Summary
{
    public class ExternalSpendService : SpendService
    {
        private ICircuitBreaker circuitBreaker;

        public ExternalSpendService(
            ISupplierService supplierService,
            ICircuitBreaker circuitBreaker
            )
        {
            this.supplierService = supplierService;
            this.circuitBreaker = circuitBreaker;
        }

        public override SpendSummary GetTotalSpend(int supplierId)
        {
            SpendSummary result = new SpendSummary();

            result.Name = supplierService.GetById(supplierId).Name;
            result.Years = circuitBreaker.GetSpendDetail(supplierId);

            return result;
        }
    }
}
