using System;
using AsbaBank.Queries;

namespace AsbaBank.Presentation.Shell.ConsoleViews
{
    public class AllClientsView : ConsoleView<ClientDto>
    {
        private readonly ClientQueries clientQueries;

        public override string Key { get { return "AllClients"; } }
        public override string Usage { get { return "AllClients"; } }

        public AllClientsView(ClientQueries clientQueries)
        {
            this.clientQueries = clientQueries;
        }

        protected override string GetLine(ClientDto item)
        {
            return String.Format("{0,-4} {1, -15} {2, -15} {3, -11}", item.Id, item.Surname, item.Name, item.PhoneNumber);
        }

        protected override string GetHeading()
        {
            return String.Format("{0,-4} {1, -15} {2, -15} {3, -11}", "Id", "Surname", "Name", "Telephone");
        }

        public override void Print(string[] args)
        {
            Print(clientQueries.All());
        }
    }
}