using System;

namespace ValueObjects.ValueTypes
{
    public struct Money : IEquatable<Money>
    {
        private readonly decimal amount;

        public static Money Zero { get {return new Money(0); } }
        
        public Money(decimal amount)
            : this()
        {
            if (amount < 0)
            {
                throw new ArgumentException("A monetary value cannot be smaller than zero");
            }

            this.amount = amount;
        }

        public static implicit operator decimal(Money money)
        {
            return money.amount;
        }

        public override int GetHashCode()
        {
            return amount.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is Money && Equals((Money)obj);
        }

        public bool Equals(Money other)
        {
            return other.amount == amount;
        }

        public static Money operator +(Money left, Money right)
        {
            return new Money(left.amount + right.amount);
        }

        public static Money operator -(Money left, Money right)
        {
            return new Money(left.amount - right.amount);
        }

        public static bool operator ==(Money left, Money right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Money left, Money right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return String.Format("{0:C}", amount);
        }
    }
}