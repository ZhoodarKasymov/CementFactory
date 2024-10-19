using System;
using System.Collections.Generic;

namespace CementFactory.Models
{
    public class SaleRequest
    {
        public string Date { get; set; }
        public string CarNumber { get; set; }
        public string ClientGuid { get; set; }
        public List<SaleItem> Goods { get; set; }
    }

    public class SaleItem
    {
        public string Guid { get; set; }
        public double Weight { get; set; }
        public int Count { get; set; }
    }
}