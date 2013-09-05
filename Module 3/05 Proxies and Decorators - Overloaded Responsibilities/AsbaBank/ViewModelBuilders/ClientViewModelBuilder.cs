using System.Collections.Generic;
using System.Linq;
using AsbaBank.Domain.Models;
using AsbaBank.Infrastructure;
using AsbaBank.Presentation.Mvc.ViewModels;

namespace AsbaBank.Presentation.Mvc.ViewModelBuilders
{
    public class ClientViewModelBuilder
    {
        private readonly IRepository<Account> accountRepository;

        public ClientViewModelBuilder(IRepository<Account> accountRepository)
        {
            this.accountRepository = accountRepository;
        }


        public ClientViewModel Build(Client client)
        {
            return new ClientViewModel
            {
                Id = client.Id,
                ClientName = client.ClientName,
                StreetNumber = client.Address.StreetNumber,
                Street = client.Address.Street,
                City = client.Address.City,
                PostalCode = client.Address.PostalCode,
                PhoneNumber = client.PhoneNumber,
                Accounts = BuildSimpleAccountsCollection(client)
            };
        }

        private IEnumerable<SimpleAccountViewModel> BuildSimpleAccountsCollection(Client client)
        {
            return accountRepository
                .Where(account => account.ClientId == client.Id)
                .Select(account => new SimpleAccountViewModel
                {
                    AccountNumber = account.AccountNumber,
                    Id = account.Id,
                    Balance = account.GetAccountBalance().ToString("C"),
                    Status = account.Closed ? "Closed" : "Open",
                }).ToList();
        }
    }
}