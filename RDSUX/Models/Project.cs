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
         public int StockLength { get; set; }
        public string BarCodeGrade { get; set; }
        public int StandardSplice { get; set; }
        public int MechanicSplice { get; set; }
        public string  JobSheetName { get; set; }

        public string ContractDWGSName { get; set; }
        public string RFIResponsesName { get; set; }

        public string EngineerReviewDrawingsName { get; set; }

    }

}
