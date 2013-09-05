using System;
using AsbaBank.Queries;

namespace AsbaBank.Presentation.Shell.ConsoleViews
{
    public class ClientsWithNameView : ConsoleView<ClientDto>
    {
        private readonly ClientQueries clientQueries;

        public override string Key { get { return "ClientsWithName"; } }
        public override string Usage { get { return "ClientsWithName <Name>"; } }

        public ClientsWithNameView(ClientQueries clientQueries)
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
            if (args.Length != 1)
            {
                throw new ArgumentException(String.Format("Incorrect number of parameters. Usage is: {0}", Usage));
            }

            Print(clientQueries.WithName(args[0]));
        }
    }
}