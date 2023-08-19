using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using mohbeli.Models;

namespace mohbeli.Models
{
    public class MohBeliDetail
    {
        public int Id { get; set; }

        public string CustomerName { get; set; }

        public string MobileNumber { get; set; }

        public string Address { get; set; }

        public int TotalAmount { get; set; }

        public int TaxAmount { get; set; }

        public int NetAmount { get; set; }

        public List<Items> Items { get; set; }

        public MohBeliDetail()
        {
            Items = new List<Items>();
        }
    }
}