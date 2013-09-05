using System.Collections.Generic;
using AsbaBank.Domain.Models;

namespace AsbaBank.Presentation.Mvc.ViewModels
{
    public class AccountViewModel
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string AccountNumber { get; set; }
        public string Balance { get; set; }
        public string Status { get; set; }
        public bool Closed { get; set; }
        public IEnumerable<Transaction> Ledger { get; set; }
        public IEnumerable<BankCard> BankCards { get; set; } 
    }
}