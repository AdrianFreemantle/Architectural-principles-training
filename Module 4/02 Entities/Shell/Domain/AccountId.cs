using System;
using Entities;

namespace Shell.Domain
{
    public class AccountId : Identity<Guid>
    {
        public AccountId(Guid id)
            : base(id)
        {
        }
    }
}