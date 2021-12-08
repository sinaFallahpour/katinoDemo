using Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class EmailNotification
    {
        public int Id { get; set; }
        public string CategoryIds { get; set; }
        public string KeyWord { get; set; }
        public TypeOfCooperation? TypeOfCooperation { get; set; }
        public string Cities { get; set; }
        public string Email { get; set; }
        public bool ShouldSend { get; set; }
        public EmailNotificationSendTime EmailNotificationSendTime { get; set; }
        public DateTime Date { get; set; }
    }
}
