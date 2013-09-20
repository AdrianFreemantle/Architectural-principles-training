using System.Collections.Generic;

namespace EventDriven.Shell.Domain
{
    public class PortfolioSnapshot : IMemento
    {
        public IHaveIdentity Identity { get; set; }
        public PortfolioState State { get; set; }
        public HashSet<IMemento> Entities { get; set; }
    }
}