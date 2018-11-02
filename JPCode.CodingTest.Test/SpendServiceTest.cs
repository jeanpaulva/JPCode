using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TDD.CodingTest.DataService;
using TDD.CodingTest.DataService.Stubs;
using TDD.CodingTest.Exceptions;
using TDD.CodingTest.External;
using TDD.CodingTest.Invoices;
using TDD.CodingTest.Summary;
using TDD.CodingTest.Suppliers;
using System.Collections.Generic;
using System.Threading;
using Unity;

namespace TDD.CodingTest.Test
{
    [TestClass]
    public class SpendServiceTest
    {
        private UnityContainer container;

        [TestInitialize]
        public void TestInit()
        {
            container = new UnityContainer();
            container.RegisterType<ISupplierDataService, SupplierDataServiceStub>();
            container.RegisterType<ISupplierService, SupplierService>();
            container.RegisterType<IInvoiceRepository, InvoiceRepositoryStub>();
            container.RegisterType<IExternalSpendService, ExternalInvoiceServiceStub>();
            container.RegisterType<ICircuitBreaker, ExternalSpendServiceInvoker>();
            container.RegisterType<IFailoverInvoiceService, FailoverInvoiceServiceStub>();
        }

        [TestMethod]
        public void SpendService_InternalCustomer_Name_Test()
        {
            // Arrange
            int supplierId = 1;
            SpendService spendService = container.Resolve<InternalSpendService>();

            // Act
            SpendSummary result = spendService.GetTotalSpend(supplierId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Supplier Internal", result.Name);
        }

        [TestMethod]
        public void SpendService_InternalCustomer_Spend_Test()
        {
            // Arrange
            int supplierId = 1;
            SpendService spendService = container.Resolve<InternalSpendService>();

            // Act
            SpendSummary result = spendService.GetTotalSpend(supplierId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Years.Count);
            Assert.AreEqual(2000, result.Years[0].TotalSpend);
            Assert.AreEqual(1000, result.Years[1].TotalSpend);
        }

        [TestMethod]
        public void SpendService_ExternalCustomer_Name_Test()
        {
            // Arrange
            int supplierId = 2;
            SpendService spendService = container.Resolve<ExternalSpendService>();

            // Act
            SpendSummary result = spendService.GetTotalSpend(supplierId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Supplier External", result.Name);
        }

        [TestMethod]
        public void SpendService_ExternalCustomer_Spend_ActionCase_Test()
        {
            // Arrange
            int supplierId = 2;
            SpendService spendService = container.Resolve<ExternalSpendService>();

            // Act
            SpendSummary result = spendService.GetTotalSpend(supplierId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Years.Count);
            Assert.AreEqual(1000, result.Years[0].TotalSpend);
            Assert.AreEqual(1000, result.Years[1].TotalSpend);
        }

        [TestMethod]
        public void SpendService_ExternalCustomer_Spend_FailoverCase_Test()
        {
            // Arrange
            int supplierId = 3;
            SpendService spendService = container.Resolve<ExternalSpendService>();

            // Act
            SpendSummary result = spendService.GetTotalSpend(supplierId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Supplier External (Failover)", result.Name);
            Assert.AreEqual(1, result.Years.Count);
            Assert.AreEqual(900, result.Years[0].TotalSpend);
        }

        [TestMethod]
        public void SpendService_ExternalCustomer_Spend_FailoverCase_IsInClosedTime_Test()
        {
            // Arrange
            int supplierId = 3;
            SpendService spendService = container.Resolve<ExternalSpendService>();

            // Act
            SpendSummary result = spendService.GetTotalSpend(supplierId);
            result = spendService.GetTotalSpend(supplierId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Supplier External (Failover)", result.Name);
            Assert.AreEqual(1, result.Years.Count);
            Assert.AreEqual(900, result.Years[0].TotalSpend);
        }

        [TestMethod]
        [ExpectedException(typeof(ObsoleteDataException))]
        public void SpendService_ExternalCustomer_Spend_FailoverCase_ObsoleteDate_Test()
        {
            // Arrange
            int supplierId = 4;
            SpendService spendService = container.Resolve<ExternalSpendService>();

            // Act
            SpendSummary result = spendService.GetTotalSpend(supplierId);

            // Assert - See Exception
        }

        [TestMethod]
        public void SpendService_ExternalCustomer_Spend_ReturnToNormalAfter1Minute_Test()
        {
            // Arrange
            Mock<ICircuitBreaker> mockedInstance = new Mock<ICircuitBreaker>();
            mockedInstance.Setup(mc => mc.ClosedTimeSeconds).Returns(5); // Adjust Timeout to 5 Seconds
            mockedInstance.Setup(mc => mc.GetSpendDetail(2)).Returns( // Set Data
                new List<SpendDetail>(
                    new SpendDetail[] {
                        new SpendDetail() {TotalSpend =1001, Year = 2018 },
                        new SpendDetail() {TotalSpend =1001, Year = 2017 }
                    }
                    ));

            UnityContainer container = new UnityContainer();
            container.RegisterType<ISupplierDataService, SupplierDataServiceStub>();
            container.RegisterType<ISupplierService, SupplierService>();
            container.RegisterType<IInvoiceRepository, InvoiceRepositoryStub>();
            container.RegisterType<IExternalSpendService, ExternalInvoiceServiceStub>();
            container.RegisterType<ICircuitBreaker, ExternalSpendServiceInvoker>();
            container.RegisterType<IFailoverInvoiceService, FailoverInvoiceServiceStub>();

            SpendService spendService = container.Resolve<ExternalSpendService>();
            SpendSummary resultFailover = spendService.GetTotalSpend(3);

            Thread.Sleep(1000 * 5 + 1); // Wait 6 Seconds

            // Act
            container.RegisterInstance<ICircuitBreaker>(mockedInstance.Object);
            spendService = container.Resolve<ExternalSpendService>();
            SpendSummary resultNormal = spendService.GetTotalSpend(2);

            // Assert
            Assert.IsNotNull(resultFailover);
            Assert.AreEqual("Supplier External (Failover)", resultFailover.Name);
            Assert.AreEqual(1, resultFailover.Years.Count);
            Assert.AreEqual(900, resultFailover.Years[0].TotalSpend);

            Assert.IsNotNull(resultNormal);
            Assert.AreEqual("Supplier External", resultNormal.Name);
            Assert.AreEqual(2, resultNormal.Years.Count);
            Assert.AreEqual(1001, resultNormal.Years[0].TotalSpend);
            Assert.AreEqual(1001, resultNormal.Years[1].TotalSpend);
        }
    }
}
