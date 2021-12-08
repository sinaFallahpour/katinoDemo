using Domain.DTO.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class AdvertismentNotification
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsSeen { get; set; }
        public DateTime SeenDate { get; set; }

        //navigation properties
        public JobAdvertisement JobAdvertisement { get; set; }
        public int? JobAdvertisementId { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public NotificationType Type { get; set; }
        public Resome Resome { get; set; }
        public int? ResomeId { get; set; }

        public string EmployeeId { get; set; }
        public User Employee { get; set; }
    }
}
