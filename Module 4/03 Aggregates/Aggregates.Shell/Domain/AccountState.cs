using System;
using System.Collections.Generic;

namespace Aggregates.Shell.Domain
{
    [Serializable]
    public class AccountState : IMemento
    {
        public IHaveIdentity Identity { get; set; }
        public List<Transaction> Transactions { get; set; }

        public AccountState()
        {
            Transactions = new List<Transaction>();
        }
    }
}