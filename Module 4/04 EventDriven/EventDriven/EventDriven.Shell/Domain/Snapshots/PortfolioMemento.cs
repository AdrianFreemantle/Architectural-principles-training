using System;

namespace EventDriven.Shell.Domain.Snapshots
{
    [Serializable]
    public class PortfolioMemento : Memento
    {
        public bool IsOpen { get; set; }
        public AccountSnapshot[] Accounts { get; set; }
    }
}