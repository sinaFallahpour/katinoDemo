using Domain.DTO.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Ticket
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string SenderFile { get; set; }
        public string ReceiverFile { get; set; }
        public TicketPriorityStatus TicketPriorityStatus { get; set; }
        public DateTime CreateDate { get; set; }
        public string Answer { get; set; }
        public DateTime? AnswerDate { get; set; }

        public bool IsReciverSeen { get; set; }
        public DateTime? ReceiverSeenDate { get; set; }

        public bool IsSenderSeen { get; set; }
        public DateTime? SenderSeenDate { get; set; }
        public string SenderId { get; set; }
        public User Sender { get; set; }
        public string ReceiverId { get; set; }
        public User Receive { get; set; }


        public TicketStatus? TicketStatus { get; set; }

    }
}
