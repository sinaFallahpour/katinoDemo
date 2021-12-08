using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class SendAdverByMail
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string City { get; set; }
        public string CompanyPersianName { get; set; }
        public string CompanyEngName { get; set; }
        public string WorkExperience { get; set; }
        public string Logo { get; set; }
        public bool IsImmadiate { get; set; }

    }
}
