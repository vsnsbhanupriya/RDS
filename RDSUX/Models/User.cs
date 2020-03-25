using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RDSUX.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [DisplayName("Name")]
        public string Name { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(15)]
        [DisplayName("User Name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password")]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [DisplayName("User Type")]
        public int UserType { get; set; }

        [RequiredIf("UserType", (int)enumUserType.User)]
        [DisplayName("Company Name")]
        public string? CompanyName { get; set; }

        [RequiredIf("UserType", (int)enumUserType.Detailer)]
        [DisplayName("Date of birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DateofBirth { get; set; }

        [RequiredIf("UserType", (int)enumUserType.Detailer)]
        public string? Role { get; set; }

        [DisplayName("Date of join")]
        [RequiredIf("UserType", (int)enumUserType.Detailer)]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DateofJoin { get; set; }

        [RequiredIf("UserType", (int)enumUserType.Detailer)]
        public float Expereince { get; set; }

        public string PhotoName { get; set; }
        public IEnumerable<SelectListItem> UserTypes { get; set; }

        public HttpPostedFileBase Photo { get; set; }
    }
}