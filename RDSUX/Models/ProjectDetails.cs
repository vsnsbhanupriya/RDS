using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace RDSUX.Models
{
    public class ProjectDetailsModel
    {
        public int ProejctId { get; set; }

        [Required(ErrorMessage = "Please enter Project name.")]
        public string ProjectName { get; set; }

        public string Notes { get; set; }

       // public string PurchaseOrder { get; set; }
        public string AssignDate { get; set; }

        public string CreateDate { get; set; }
        public string CreateBy { get; set; }

        [Required]
        public string JobNumber { get; set; }

        public int ProjectTypeId { get; set; }
        public int ScopeOfWorkId { get; set; }
        public string StatusId { get; set; }
        public string JobSheetName { get; set; }
        public string PurchaseOrderFileName { get; set; }
        public string BarCodeId { get; set; }
        public HttpPostedFileBase JobSheet { get; set; }
        public HttpPostedFileBase PurchaseOrder { get; set; }
        public HttpPostedFileBase ContractDWGS { get; set; }
        public HttpPostedFileBase RFIResponses { get; set; }
        public HttpPostedFileBase ShopDrawings { get; set; }
        public HttpPostedFileBase EngineerReviewDrawings { get; set; }
        public IEnumerable<SelectListItem> ProjectTypeList { get; set; }

        //  public List<ProjectType> ProjectType { get; set; }
        // public List<ScopeOfWork> ScopeOfWork { get; set; }
        public HttpPostedFileBase[] ContractDWGSFiles { get; set; }

        public IEnumerable<SelectListItem> ScopeOfWork { get; set; }
        public List<Status> Status { get; set; }
        public BarCode BarCode { get; set; }
    }
}