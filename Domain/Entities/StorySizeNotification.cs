using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class StorySizeNotification
    {
        public int Id { get; set; }
        public JobAdvertisement JobAdvertisement { get; set; }
        public int JobAdvertisementId { get; set; }
        public string CompanyId { get; set; }
        public User Company { get; set; }
        public bool IsSeen { get; set; }
        public DateTime Date { get; set; }
    }
}
