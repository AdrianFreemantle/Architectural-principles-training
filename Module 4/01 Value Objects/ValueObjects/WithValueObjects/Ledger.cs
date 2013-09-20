using System;
using System.Collections.Generic;
using System.Linq;
using ValueObjects.Shared;
using ValueObjects.ValueTypes;
using ValueObjects.ValueTypes.AccountType;
using ValueObjects.ValueTypes.Transaction;

namespace ValueObjects.WithValueObjects
{
    public class Ledger
    {
        private readonly HashSet<Transaction> transactions;
        private AccountType accountType;

        internal Ledger(AccountTypeEnum accountType)
        {
            transactions = new HashSet<Transaction>();
            this.accountType = accountType;
        }

        public void DepositMoney(Money amount)
        {           
            transactions.Add(new Transaction(TransactionTypeEnum.Deposit, amount));
        }

        public void WithDrawMoney(Money amount)
        {
            decimal pendingBalance = ((decimal)GetBalance()) - amount;
            accountType.ValidateBalance(pendingBalance);
            transactions.Add(new Transaction(TransactionTypeEnum.Withdrawal, amount));
        }

        public Money GetBalance()
        {
            return new Money(transactions.Sum(transaction => transaction));
        }
    }
}