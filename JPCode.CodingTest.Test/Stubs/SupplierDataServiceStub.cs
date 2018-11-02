using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDD.CodingTest.Suppliers;

namespace TDD.CodingTest.DataService.Stubs
{
    public class SupplierDataServiceStub : ISupplierDataService
    {
        private IList<Supplier> list = new List<Supplier>();

        public SupplierDataServiceStub()
        {
            list.Add(new Supplier() { Id = 1, IsExternal = false, Name = "Supplier Internal" });
            list.Add(new Supplier() { Id = 2, IsExternal = true, Name = "Supplier External" });
            list.Add(new Supplier() { Id = 3, IsExternal = true, Name = "Supplier External (Failover)" });
            list.Add(new Supplier() { Id = 4, IsExternal = true, Name = "Supplier External (Obsolete Data)" });
        }

        public Supplier GetById(int id)
        {
            Supplier result = list.Where(s => s.Id == id).FirstOrDefault();

            return result;
        }
    }
}
