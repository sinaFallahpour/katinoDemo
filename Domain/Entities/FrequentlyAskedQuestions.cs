using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class FrequentlyAskedQuestion
    {
        public int Id { get; set; }
        public DateTime CreateDate{ get; set; }
        public DateTime UpdateDate { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
