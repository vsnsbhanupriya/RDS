using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace RDSUX.Models
{
    public class Project
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
        public string StockLength { get; set; }
        public bool BarCodeGrade { get; set; }
        public string StandardSplice { get; set; }
        public string MechanicSplice { get; set; }
        public string JobSheetName { get; set; }
        public string StatusName { get; set; }
    }
}