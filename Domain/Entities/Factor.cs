using Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Factor
    {

        public int Id { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }

        //navigation Prop
        public string CompanyId { get; set; }
        public User Company { get; set; }

        public int? PlanId { get; set; }
        public Plan Plan { get; set; }
        public bool IsImmediately { get; set; }

        public string TrackingCode{ get; set; }
        public bool IsBackMOney { get; set; }
        public PaymnetType PaymnetType { get; set; }
    }

}
