using System;
using ValueObjects.ValueTypes;

namespace ValueObjects.Shared
{
    public class MinimumBalanceAttribute : Attribute
    {
        public decimal Amount { get; private set; }

        public MinimumBalanceAttribute(string amount)
        {
            Amount = new Money(Decimal.Parse(amount));
        }

        public MinimumBalanceAttribute(int amount)
        {
            Amount = amount;
        }
    }
}