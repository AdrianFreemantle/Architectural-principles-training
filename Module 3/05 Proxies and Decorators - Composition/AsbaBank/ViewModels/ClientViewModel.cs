using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AsbaBank.Presentation.Mvc.ViewModels
{
    public class ClientViewModel
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string PhoneNumber { get; set; }
        public string StreetNumber { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public IEnumerable<SimpleAccountViewModel> Accounts { get; set; }
    }
}