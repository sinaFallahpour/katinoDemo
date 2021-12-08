using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class SiteVisit
    {

        public SiteVisit()
        {
            CreatedAt = DateTime.Now;
        }
        public int Id { get; set; }

        public string IP { get; set; }
        public string Browser { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
