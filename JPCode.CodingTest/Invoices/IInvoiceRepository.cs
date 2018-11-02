using System.Linq;

namespace TDD.CodingTest.Invoices
{
    public interface IInvoiceRepository
    {
        IQueryable<Invoice> Get();
    }
}
