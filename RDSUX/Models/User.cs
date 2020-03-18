using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RDSUX.Models
{
    public class User
    {
        [Required]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(15)]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public int UserType { get; set; }

        public string? CompanyName { get; set; }
        public DateTime? DateofBirth { get; set; }
        public string? Role { get; set; }
        public DateTime? DateofJoin { get; set; }
        public Double Expereince { get; set; }
        public IEnumerable<SelectListItem> UserTypes { get; set; }
    }
}