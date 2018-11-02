using TDD.CodingTest.External;

namespace TDD.CodingTest.Summary
{
    public class ExternalSpendServiceWrapper : IExternalSpendService
    {
        public ExternalInvoice[] GetInvoices(string supplierId)
        {
            var result = ExternalInvoiceService.GetInvoices(supplierId);

            return result;
        }
    }
}
