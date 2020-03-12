using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RDSUX.Models
{
    public class BarCode
    {
        public int BarCodeId { get; set; }
        public int StockLength { get; set; }
        public bool BarCodeGrade { get; set; }
        public string StandardSplice { get; set; }
        public int MachanicSplice { get; set; }
    }
}