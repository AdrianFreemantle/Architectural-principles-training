using System;
using ValueObjects.EnumExtensions;
using ValueObjects.Shared;

namespace ValueObjects.ValueTypes.AccountType
{
    public struct AccountType : IEquatable<AccountType>
    {
        private readonly AccountTypeEnum accountType;

        public AccountType(AccountTypeEnum accountType)
            : this()
        {
            if (accountType == AccountTypeEnum.Unknown)
            {
                throw new InvalidOperationException("Please specifiy a valid account type");
            }

            this.accountType = accountType;
        }

        public decimal MinimumAllowedBalance()
        {
            var attribute = accountType.TryGetCustomAttribute<MinimumBalanceAttribute>();

            if (attribute == null)
                return Money.Zero;

            return attribute.Amount;
        }

        public void ValidateBalance(decimal amount)
        {
            var minimumBalance = MinimumAllowedBalance();

            if (minimumBalance > amount)
            {
                throw new InvalidOperationException(String.Format("The resulting balance of {0:C} is smaller than the allowed minimum balance of {1:C}", amount, minimumBalance));
            }
        }

        public static implicit operator AccountTypeEnum(AccountType accountType)
        {
            return accountType.accountType;
        }

        public static implicit operator AccountType(AccountTypeEnum accountType)
        {
            return new AccountType(accountType);
        }

        public static implicit operator int(AccountType accountType)
        {
            return (int)accountType.accountType;
        }

        public static implicit operator AccountType(int accountTypeCode)
        {
            return new AccountType((AccountTypeEnum)accountTypeCode);
        }

        public override int GetHashCode()
        {
            return accountType.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is AccountType && Equals((AccountType)obj);
        }

        public bool Equals(AccountType other)
        {
            return other.accountType == accountType;
        }

        public static bool operator ==(AccountType left, AccountType right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AccountType left, AccountType right)
        {
            return !Equals(left, right);
        }

        public override string ToString()
        {
            return accountType.GetDescription();
        }
    }
}
