using Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class ChangeAsignResomeStatus
    {
        public int AsignResomeId { get; set; }
        public AsingResomeStatus AsingResomeStatus { get; set; }
        public string Description { get; set; }
    }
}
