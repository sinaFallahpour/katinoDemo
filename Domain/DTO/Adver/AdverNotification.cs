using Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class AdverNotification
    {
        public List<Advernotif> Advernotifs { get; set; }
        public int NotificationCount { get; set; }
    }
   public class Advernotif
    {
        public int Id { get; set; }
        public string AdminDescription { get; set; }
        public string Title { get; set; }
        public int AdverId { get; set; }
        public int? ResomeId { get; set; }
        public AdverCreatationStatus AdverCreatationStatus { get; set; }
        public NotificationType Type { get; set; }
        public string Description { get; set; }
    }
}
