using System;
using ValueObjects.EnumExtensions;
using ValueObjects.Shared;

namespace ValueObjects.WithoutValueObjects
{
    public class Account
    {
        private Ledger ledger;
        private AccountTypeEnum accountType;

        internal Account(AccountTypeEnum accountType, decimal initialDeposit)
        {
            if (accountType == AccountTypeEnum.Unknown)
            {
                throw new InvalidOperationException("Please specifiy a valid account type");
            }

            this.accountType = accountType;
            ledger = new Ledger(accountType);
            ValidateBalance(initialDeposit);
            ledger.DepositMoney(initialDeposit);
        }

        public decimal MinimumAllowedBalance()
        {
            var attribute = accountType.TryGetCustomAttribute<MinimumBalanceAttribute>();

            if (attribute == null)
                return 0;

            return attribute.Amount;
        }

        public void ValidateBalance(decimal amount)
        {
            var minimumBalance = MinimumAllowedBalance();

            if (minimumBalance > amount)
            {
                throw new InvalidOperationException(String.Format("The amount {0:C} is smaller than the allowed minimum balance of {1:C}", amount, minimumBalance));
            }
        }

        public void WithDrawMoney(decimal amount)
        {
            ledger.WithDrawMoney(amount);
        }

        public void DepositMoney(decimal amount)
        {
            ledger.DepositMoney(amount);
        }

        public void CloseAccount()
        {
            //do stuff
        }

        public void CancelAccount()
        {
            //do stuff
        }
    }
}
