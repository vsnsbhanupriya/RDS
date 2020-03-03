using System;
using System.Collections.Generic;
using System.Text;

namespace RDSDataLayer.Entities
{
    class Project
    {
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Notes { get; set; }
        public string PurchaseOrder { get; set; }
        public string AssignDate { get; set; }
        public string CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string JobNumber { get; set; }
        public int ProjetTypeId { get; set; }
        public int ScopeOfWorkId { get; set; }
        public int StatusId { get; set; }
    }
}
