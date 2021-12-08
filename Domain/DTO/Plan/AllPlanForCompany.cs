using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class AllPlanForCompany
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public decimal Price { get; set; }
        public double Discount { get; set; }
        public int Duration { get; set; }
        public int? Logo { get; set; }
        public int priority { get; set; }
        public bool IsFree { get; set; }
        public int AdverExpireTime { get; set; }
        public int AdverCount { get; set; }
        public int ImmediateAdverCount { get; set; }
        public bool IsUseResomeManegement { get; set; }
    }
    public class AllPlanForEmployee
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Duration { get; set; }
        public string AdverCount { get; set; }
    }
}
