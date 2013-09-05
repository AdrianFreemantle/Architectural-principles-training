using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DataAnnotationsExtensions;

namespace AsbaBank.Presentation.Mvc.Forms
{
    public class NewClientForm
    {
        [Required(ErrorMessage = "Please provide a client name.")]
        [Display(Name = "Client Name")]
        [MinLength(3, ErrorMessage = "A client name must be at leats than 3 characters")]
        public string ClientName { get; set; }

        [Digits(ErrorMessage = "A phone number may only have digits.")]
        [Required(ErrorMessage = "Please provide a phone number.")]
        [Display(Name = "Phone Number")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "A phone number must be 10 digits long.")]
        public string PhoneNumber { get; set; }
    }
}