using System;
using ValueObjects.Shared;
using ValueObjects.ValueTypes;
using ValueObjects.ValueTypes.AccountType;

namespace ValueObjects.WithValueObjects
{
    public class Account
    {
        private Ledger ledger;
        private AccountType accountType;

        internal Account(AccountTypeEnum accountType, Money initialDeposit)
        {
            this.accountType = accountType;
            ledger = new Ledger(accountType);
            this.accountType.ValidateBalance(initialDeposit);
            ledger.DepositMoney(initialDeposit);
        }

        public void WithDrawMoney(Money amount)
        {
            ledger.WithDrawMoney(amount);
        }

        public void DepositMoney(Money amount)
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
