using Domain.DTO.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class EmployeeFactor
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }

        //navigation Prop
        public string EmployeeId { get; set; }
        public User Employee { get; set; }

        public int? EmployeePlanId { get; set; }
        public EmployeePlan EmployeePlan { get; set; }

        public string TrackingCode { get; set; }
        public bool IsBackMOney { get; set; }
        public PaymnetType PaymnetType { get; set; }
    }
}
