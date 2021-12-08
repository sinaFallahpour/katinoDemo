using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class AllGiftCodeForAdmin
    {
        public int Id { get; set; }
        public string GiftCode { get; set; }
        public double Discount { get; set; }
        public string CreateDate { get; set; }
        public string UseDate { get; set; }
        public string ComoanyName { get; set; }
    }
}
