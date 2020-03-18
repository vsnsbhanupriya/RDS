using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RDSUX.Models
{
    public class UserLogin
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string UserType { get; set; }

        public IEnumerable<SelectListItem> UserTypes { get; set; }
    }
}