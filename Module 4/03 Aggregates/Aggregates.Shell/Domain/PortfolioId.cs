namespace Aggregates.Shell.Domain
{
    public class PortfolioId 
        : Identity<string>
    {
        public PortfolioId(string id) : 
            base(id)
        {
        }
    }
}