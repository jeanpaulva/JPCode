using TDD.CodingTest.Invoices;
using System;
using System.Collections.Generic;
using System.Text;

namespace TDD.CodingTest.Summary
{
    public interface ICircuitBreaker
    {
        int MaxTries { get; }

        int ClosedTimeSeconds { get; }

        int ObsoleteDaysLimit { get; }

        bool IsOpenState { get; set; }

        DateTime FailedTimestamp { get; set; }

        void Reset();

        List<SpendDetail> GetSpendDetail(int supplierId);
    }
}
