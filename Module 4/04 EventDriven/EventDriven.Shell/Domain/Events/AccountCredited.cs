namespace EventDriven.Shell.Domain.Events
{
    public class AccountCredited : DomainEvent
    {
        public decimal Amount { get; protected set; }

        public AccountCredited(decimal amount)
        {
            Amount = amount;
        }
    }
}