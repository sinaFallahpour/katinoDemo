using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class CompanyCategory
    {
        public int Id { get; set; }
        //navigation prop   
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        public string CompanyId { get; set; }
        public User Company { get; set; }
        public DateTime CreateAt { get; set; }


    }
}
