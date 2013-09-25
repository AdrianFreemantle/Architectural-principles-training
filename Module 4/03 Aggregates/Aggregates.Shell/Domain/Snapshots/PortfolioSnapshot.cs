namespace Aggregates.Shell.Domain.Snapshots
{
    public class PortfolioSnapshot : IMemento
    {
        public bool IsOpen { get; set; }
        public IMemento[] Accounts { get; set; }
        public IHaveIdentity Identity { get; set; }
    }
}