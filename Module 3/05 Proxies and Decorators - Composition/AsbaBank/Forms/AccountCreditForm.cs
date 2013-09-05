using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DataAnnotationsExtensions;

namespace AsbaBank.Presentation.Mvc.Forms
{
    public class AccountCreditForm
    {
        public int AccountId { get; set; }

        [Required(ErrorMessage = "Please provide an amount.")]
        [Numeric(ErrorMessage = "The amount must be a number.")]
        [Display(Name = "Credit Amount")]
        public int CreditAmount { get; set; }
    }
}