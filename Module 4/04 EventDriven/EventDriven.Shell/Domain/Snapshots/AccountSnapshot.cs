using System;

namespace EventDriven.Shell.Domain.Snapshots
{
    [Serializable]
    public class AccountSnapshot : Memento
    {
        public Money Balance { get; set; }
    }
}