using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EnsekCodingChallenge.Models
{
    public class Employee
    {
        public int ID { get; set; }
        public ApplicationUser Linked_User { get; set; }

        [Required]
        [Display(Name = "Name")]
        public string Employee_name { get; set; }

        [Required]
        [Display(Name = "Salary")]
        public int Employee_salary { get; set; }

        [Required]
        [Display(Name = "Age")]
        public int Employee_age { get; set; }
    }
}