using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RDSUX.Models
{
    public class PurchaseOrder
    {
        public PurchaseOrder()
        {
        }

        public PurchaseOrder(string fileName, string projectId)
        {
            this.FileName = fileName;
            this.ProjectId = projectId;
        }

        public string PurchaseOrderId { get; set; }
        public string ProjectId { get; set; }
        public string FileName { get; set; }
        public string TImeStamp { get; set; } = DateTime.Today.ToShortDateString();
    }
}