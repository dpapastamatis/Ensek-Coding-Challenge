using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace EnsekCodingChallenge.Models
{
    public class Account
    {
        public int AccountID { get; set; }
        public ApplicationUser Linked_User { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string First_name { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string Last_name { get; set; }


    }
}