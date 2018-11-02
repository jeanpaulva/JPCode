using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TDD.CodingTest.Exceptions;
using TDD.CodingTest.External;
using TDD.CodingTest.Invoices;

namespace TDD.CodingTest.Summary
{
    public class ExternalSpendServiceInvoker : ICircuitBreaker
    {
        private IExternalSpendService action;
        private IFailoverInvoiceService failoverAction;

        public ExternalSpendServiceInvoker(
            IExternalSpendService action,
            IFailoverInvoiceService failoverAction
            )
        {
            this.action = action;
            this.failoverAction = failoverAction;

            IsOpenState = true;
        }

        public int MaxTries => 3;

        public int ClosedTimeSeconds => 60;

        public int ObsoleteDaysLimit => 31;

        public bool IsOpenState { get; set; }
        public DateTime FailedTimestamp { get; set; }

        public List<SpendDetail> GetSpendDetail(int supplierId)
        {
            List<SpendDetail> result = null;

            int failedCount = 0;

            if (!IsOpenState && IsInClosedTime())
            {
                result = GetSpendDetail(failoverAction.GetInvoices(supplierId).Invoices);

                return result;
            }

            for (; ; )
            {
                try
                {
                    result = GetSpendDetail(action.GetInvoices(supplierId.ToString()));

                    Reset();

                    return result;
                }
                catch (WebTimeoutException ex)
                {
                    failedCount++;

                    if (failedCount == MaxTries)
                    {
                        IsOpenState = false;
                        FailedTimestamp = DateTime.Now;

                        var resultFailover = failoverAction.GetInvoices(supplierId);
                        if (IsFailoverObsolete(resultFailover))
                            throw new ObsoleteDataException();

                        result = GetSpendDetail(resultFailover.Invoices);

                        return result;
                    }
                }
            }
        }

        private bool IsFailoverObsolete(FailoverInvoiceCollection resultFailover)
        {
            bool result = false;

            TimeSpan timeSpan = DateTime.Now - resultFailover.Timestamp;
            result = timeSpan.TotalDays > ObsoleteDaysLimit;

            return result;
        }

        private bool IsInClosedTime()
        {
            bool result = false;

            TimeSpan timeSpan = DateTime.Now - FailedTimestamp;
            result = timeSpan.TotalSeconds < ClosedTimeSeconds;

            return result;
        }

        private List<SpendDetail> GetSpendDetail(ExternalInvoice[] invoices)
        {
            var result= invoices.GroupBy(
                                   i => i.Year,
                                   i => i.TotalAmount,
                                   (key, g) => new SpendDetail() { Year = key, TotalSpend = g.Sum() }
                               ).ToList();

            return result;
        }

        public void Reset()
        {
            IsOpenState = true;
        }
    }
}
