namespace TDD.CodingTest.Suppliers
{
    public class SupplierService : ISupplierService
    {
        protected ISupplierDataService SupplierDataService;

        public SupplierService(ISupplierDataService supplierDataService)
        {
            this.SupplierDataService = supplierDataService;
        }

        public Supplier GetById(int id)
        {
            var result = this.SupplierDataService.GetById(id);

            return result;
        }
    }
}
