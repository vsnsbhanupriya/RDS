using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RDSDatabase.Entities
{
   public  class ContractDWGS
    {
       public string ProjectId { get; set; }
        public string FileName { get; set; }
        public string TImeStamp { get; set; } = DateTime.Now.ToString();
    }
}
