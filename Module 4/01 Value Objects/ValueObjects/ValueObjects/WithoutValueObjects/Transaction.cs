using System;
using ValueObjects.Shared;

namespace ValueObjects.WithoutValueObjects
{
    public class Transaction
    {
        public decimal Amount { get; set; }
        public TransactionTypeEnum TransactionType { get; set; }
        public DateTime TransactionTime { get; set; }
    }
}