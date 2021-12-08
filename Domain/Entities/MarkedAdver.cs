using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class MarkedAdver
    {
        public int Id { get; set; }
        public int AdverId { get; set; }
        public JobAdvertisement JobAdvertisement { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
