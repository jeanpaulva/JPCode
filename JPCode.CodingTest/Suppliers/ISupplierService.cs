using System;
using System.Collections.Generic;
using System.Text;

namespace TDD.CodingTest.Suppliers
{
    public interface ISupplierService
    {
        Supplier GetById(int id);
    }
}
