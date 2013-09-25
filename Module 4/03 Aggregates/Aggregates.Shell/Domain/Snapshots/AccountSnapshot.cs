namespace Aggregates.Shell.Domain.Snapshots
{
    public class AccountSnapshot : IMemento
    {
        public Transaction[] Transactions { get; set; }
        public IHaveIdentity Identity { get; set; }
    }
}