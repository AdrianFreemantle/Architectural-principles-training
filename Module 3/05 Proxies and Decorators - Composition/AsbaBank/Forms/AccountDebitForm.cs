using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace AsbaBank.Presentation.Mvc.Forms
{
    public class AccountDebitForm
    {
        public int AccountId { get; set; }

        [Required(ErrorMessage = "Please provide an amount.")]
        [Numeric(ErrorMessage = "The amount must be a number.")]
        [Display(Name = "Debut Amount")]
        public int DebitAmount { get; set; }
    }
}