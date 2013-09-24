namespace EventDriven.Shell.Domain
{
    public class AccountOpened : DomainEvent
    {
        public AccountType AccountType { get; protected set; }

        public AccountOpened(AccountType accountType)
        {
            AccountType = accountType;
        }
    }
}