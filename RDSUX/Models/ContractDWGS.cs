using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RDSUX.Models
{
    public class ContractDWGS
    {
        public ContractDWGS() { }
        public ContractDWGS(string fileName, string projectId)
        {
            this.ProjectId = projectId;
            this.FileName = fileName;
        }
        public string ContractDrawingId { get; set; }
        public string ProjectId { get; set; }
        public string FileName { get; set; }
        public string TImeStamp { get; set; } = DateTime.Today.ToShortDateString();
    }


    public class RFIResponse
    {
        public RFIResponse()
        {

        }
        public RFIResponse(string fileName, string projectId)
        {
            this.ProjectId = projectId;
            this.FileName = fileName;
        }
        public string RfiResponseId { get; set; }
        public string ProjectId { get; set; }
        public string FileName { get; set; }
        public string TImeStamp { get; set; } = DateTime.Today.ToShortDateString();
    }

    public class EngineerReviewedDrawings
    {
        public EngineerReviewedDrawings()
        {

        }
        public EngineerReviewedDrawings(string fileName, string projectId)
        {
            this.ProjectId = projectId;
            this.FileName = fileName;
        }
        public string EngineeringDrawingId { get; set; }
        public string ProjectId { get; set; }
        public string FileName { get; set; }
        public string TImeStamp { get; set; } = DateTime.Today.ToShortDateString();
    }
    public class ShopDrawings
    {
        public ShopDrawings()
        {

        }
        public ShopDrawings(string fileName, string projectId)
        {
            this.ProjectId = projectId;
            this.FileName = fileName;
        }
        public string ShopDrawingId { get; set; }
        public string ProjectId { get; set; }
        public string FileName { get; set; }
        public string TImeStamp { get; set; } = DateTime.Today.ToShortDateString();
    }



}