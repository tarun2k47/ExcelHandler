using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExcelHandler.Models
{
    public class Voucher
    {
        public string VoucherNo { get; set; }
        public string Date { get; set; }
        public string SalesAccount { get; set; }
        public string CustomerName { get; set; }
        public string Items { get; set; }
        public int Quantity { get; set; }
        public string Price { get; set; }
        public string Discount { get; set; }
        public string Remarks { get; set; }
    }
}