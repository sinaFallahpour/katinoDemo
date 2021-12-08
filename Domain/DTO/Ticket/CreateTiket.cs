using Domain.DTO.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class CreateTiket
    {
        public string Subject { get; set; }
        public string Content { get; set; }
        public string City { get; set; }
        public TicketPriorityStatus TicketPriorityStatus { get; set; }
        public IFormFile File { get; set; }
        /// <summary>
        /// this property for admin when send ticket to user
        /// </summary>
        public List<string> UserId { get; set; }

    }
    public class TicketAdminAnswer
    {
        public int Id { get; set; }
        public string Answer { get; set; }
        public IFormFile AnswerFile{ get; set; }
    }
}
