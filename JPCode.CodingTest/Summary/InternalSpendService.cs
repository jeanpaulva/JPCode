using TDD.CodingTest.Invoices;
using TDD.CodingTest.Suppliers;
using System.Linq;

namespace TDD.CodingTest.Summary
{
    public class InternalSpendService : SpendService
    {
        private IInvoiceRepository InvoiceRepository;

        public InternalSpendService(
            ISupplierService supplierService,
            IInvoiceRepository invoiceRepository
            )
        {
            this.supplierService = supplierService;
            this.InvoiceRepository = invoiceRepository;
        }

        public override SpendSummary GetTotalSpend(int supplierId)
        {
            Supplier supplier = supplierService.GetById(supplierId);

            SpendSummary result = new SpendSummary();
            result.Name = supplier.Name;
            result.Years = InvoiceRepository.Get().Where(i => i.SupplierId == supplier.Id)
                .GroupBy(
                i => i.InvoiceDate.Year,
                i => i.Amount,
                (key, g) => new SpendDetail() { Year = key, TotalSpend = g.Sum() }
                ).ToList();

            return result;
        }
    }
}
