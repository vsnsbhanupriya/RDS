using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RDSUX.Models
{
    public class UserModel
    {
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string CompanyName { get; set; }
        public string Photo { get; set; }
        public int UserType { get; set; }
        public string DateOfBirth { get; set; }
        public string JoiningDate { get; set; }
        public float Experience { get; set; }
        public string Role { get; set; }
    }
}