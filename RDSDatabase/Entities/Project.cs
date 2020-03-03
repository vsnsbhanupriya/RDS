using System;
using System.Collections.Generic;
using System.Text;

namespace RDSDatabase.Entities
{
    class Project
    {
        public int ProejctId { get; set; }
        public string ProjectName { get; set; }
        public string Notes { get; set; }
        public string PurchaseOrder { get; set; }
        public DateTime AssignDate { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public int JobNumber { get; set; }
        public int ProjetTypeId { get; set; }
        public int ScopeOfWorkId { get; set; }
        public int StatusId { get; set; }
    }
}
