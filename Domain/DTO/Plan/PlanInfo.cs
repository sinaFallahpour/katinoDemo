using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class PlanInfo
    {
        public string PlanName { get; set; }
        public  string PlanAdverCount { get; set; }
        public  string PlanImmediateAdverCount { get; set; }
        public  string PlanStoryAdverCount { get; set; }
        public string RemainingDays { get; set; }
        public string RemainingAdversCount { get; set; }
        public string RemainingImmediateAdversCount { get; set; }
        public string RemainingStoryAdversCount { get; set; }
    }
}
