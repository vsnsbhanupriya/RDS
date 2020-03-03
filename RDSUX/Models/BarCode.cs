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
        public string BarCodeGrade { get; set; }
        public int StandardSplice { get; set; }
        public int MachanicSplice { get; set; }
    }
}