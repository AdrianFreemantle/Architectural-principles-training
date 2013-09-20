using System;
using System.Collections.Generic;
using System.Linq;
using ValueObjects.EnumExtensions;
using ValueObjects.Shared;

namespace ValueObjects.WithoutValueObjects
{
    public class Ledger
    {
        private readonly HashSet<Transaction> transactions;
        private readonly AccountTypeEnum accountType;

        internal Ledger(AccountTypeEnum accountType)
        {
            transactions = new HashSet<Transaction>();
            this.accountType = accountType;
        }

        public void DepositMoney(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("You cannot deposit a zero amount.");
            }

            transactions.Add(new Transaction
            {
                Amount = amount,
                TransactionTime = DateTime.Now,
                TransactionType = TransactionTypeEnum.Deposit
            });
        }

        public void WithDrawMoney(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("You cannot withdraw a zero amount.");
            }

            decimal pendingBalance = (GetBalance()) - amount;
            ValidateBalance(pendingBalance);

            transactions.Add(new Transaction
            {
                Amount = amount,
                TransactionTime = DateTime.Now,
                TransactionType = TransactionTypeEnum.Withdrawal
            });
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
                throw new InvalidOperationException(String.Format("The resulting balance of {0:C} is smaller than the allowed minimum balance of {1:C}", amount, minimumBalance));
            }
        }

        public decimal GetBalance()
        {
            return transactions.Sum(transaction => transaction.Amount);
        }
    }
}