using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class AllPlanForAdmin
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Content { get; set; }

        public decimal Price { get; set; }
        public double Discount { get; set; }
        public int Duration { get; set; }
        public int Logo { get; set; }
        public int priority { get; set; }

        public int AdverExpireTime { get; set; }
        public int AdverCount { get; set; }
        public int ImmediateAdverCount { get; set; }
        public bool IsUseResomeManegement { get; set; }
        public int StoryCount { get; set; }
        public bool IsActive { get; set; }
    }
}
