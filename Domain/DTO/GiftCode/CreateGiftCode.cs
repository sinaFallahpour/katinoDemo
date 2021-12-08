using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class CreateGiftCode
    {
        public List<string> EmployerId { get; set; }
        public string GiftCode { get; set; }
        public double Discount { get; set; }
        public int ExpireTime { get; set; }
    }
}
