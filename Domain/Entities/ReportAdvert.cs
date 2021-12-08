using Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class ReportAdvert
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public JobAdvertisement JobAdvertisement { get; set; }
        public int JobAdvertisementId { get; set; }
        public ReportAdvertStatus Status { get; set; }
        public ReportAdvertType Type { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public bool IsSeen { get; set; }
        public DateTime Date { get; set; }
    }
}
