using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RDSUX.Models
{
    public class ContractDWGS
    {
        public ContractDWGS(string fileName, string projectId)
        {
            this.ProjectId = projectId;
            this.FileName = fileName;
        }
        public string ProjectId { get; set; }
        public string FileName { get; set; }
        public string TImeStamp { get; set; } = DateTime.Now.ToString();
    }


    public class RFIResponse
    {
        public RFIResponse(string fileName, string projectId)
        {
            this.ProjectId = projectId;
            this.FileName = fileName;
        }
        public string ProjectId { get; set; }
        public string FileName { get; set; }
        public string TImeStamp { get; set; } = DateTime.Now.ToString();
    }

    public class EngineerReviewedDrawings
    {
        public EngineerReviewedDrawings(string fileName, string projectId)
        {
            this.ProjectId = projectId;
            this.FileName = fileName;
        }
        public string ProjectId { get; set; }
        public string FileName { get; set; }
        public string TImeStamp { get; set; } = DateTime.Now.ToString();
    }

}