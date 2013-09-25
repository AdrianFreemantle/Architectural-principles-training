namespace EventDriven.Shell.Domain.Events
{
    public class AccountDebited : DomainEvent
    {
        public decimal Amount { get; set; } 

        public AccountDebited(decimal amount)
        {
            Amount = amount;
        }
    }
}