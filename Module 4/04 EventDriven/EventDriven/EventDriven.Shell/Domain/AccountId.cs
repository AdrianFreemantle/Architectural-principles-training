namespace EventDriven.Shell.Domain
{
    public class AccountId : Identity<int>
    {
        public AccountId(int id)
            : base(id)
        {
        }

        public AccountId(AccountType id)
            : base((int)id)
        {
        }

        public static implicit operator AccountType(AccountId id)
        {
            return (AccountType)id.Id;
        } 
    }
}