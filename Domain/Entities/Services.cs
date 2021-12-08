using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Services
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int ParentId { get; set; }
        public ICollection<Services> Children { get; set; }
        public DateTime Date { get; set; }
    }
}
