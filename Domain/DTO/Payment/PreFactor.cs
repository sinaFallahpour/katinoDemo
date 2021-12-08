using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class PreFactor
    {
        public string Date { get; set; }
        public decimal PurePrice { get; set; }
        public decimal Discount { get; set; }
        public decimal Tax { get; set; }
        public decimal FinalPrice { get; set; }

    }
}
