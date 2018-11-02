using TDD.CodingTest.Suppliers;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDD.CodingTest.Suppliers
{
    public interface ISupplierDataService
    {
        Supplier GetById(int id);
    }
}
