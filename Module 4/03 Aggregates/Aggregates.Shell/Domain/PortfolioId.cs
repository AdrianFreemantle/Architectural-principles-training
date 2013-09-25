using System;
using System.Globalization;

namespace Aggregates.Shell.Domain
{
    public class PortfolioId 
        : Identity<string>
    {
        public PortfolioId(string id) : 
            base(id)
        {
        }

        internal static PortfolioId GenerateId()
        {
            var ticks = DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture);
            var id = ticks.Substring(ticks.Length - 7, 7);

            return new PortfolioId(id);
        }
    }
}