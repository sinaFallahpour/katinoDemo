using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Image
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string UserId { get; set; }
        public DateTime Date { get; set; }
    }
}
