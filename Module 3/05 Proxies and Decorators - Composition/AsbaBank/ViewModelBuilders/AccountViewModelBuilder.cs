using System;
using System.Collections.Generic;
using System.Linq;
using AsbaBank.Domain;
using AsbaBank.Domain.Models;
using AsbaBank.Infrastructure;
using AsbaBank.Presentation.Mvc.ViewModels;

namespace AsbaBank.Presentation.Mvc.ViewModelBuilders
{
    public class AccountViewModelBuilder
    {
        private readonly IRepository<Client> clientRepository;

        public AccountViewModelBuilder(IRepository<Client> clientRepository)
        {
            this.clientRepository = clientRepository;
        }

        public IEnumerable<AccountViewModel> Build(List<Account> accounts)
        {
            return accounts.Select(BuildViewModel);
        }

        private AccountViewModel BuildViewModel(Account account)
        {
            decimal balance = account.GetAccountBalance();
            Client client = clientRepository.Get(account.ClientId);

            return new AccountViewModel
            {
                AccountNumber = account.AccountNumber,
                Balance = balance.ToString("C"),
                Id = account.Id,
                ClientName = client.ClientName,
                Status = account.Closed ? "Closed" : "Open",
                Closed = account.Closed,
                Ledger = account.Ledger,
                BankCards = account.BankCards
            };
        }

        public AccountViewModel Build(Account account)
        {
            return account == null
                ? BuildInvalidAccountViewModel()
                : BuildViewModel(account);
        }

        private AccountViewModel BuildInvalidAccountViewModel()
        {
            return new AccountViewModel
            {
                AccountNumber = string.Empty,
                Balance = string.Empty,
                Id = 0,
                ClientName = String.Empty,
                Status = "Invalid Account",
                Ledger = new Transaction[0],
                BankCards = new BankCard[0],
            };
        }
    }
}