using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RDSUX.Models
{
    public class UserType
    {
        public string Name { get; set; }
        public int Id { get; set; }

        public UserType()
        {
        }

        public UserType(int id, string name)
        {
            this.Name = name;
            this.Id = id;
        }
    }
}