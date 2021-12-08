using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class DateTimeViewModel
    {
        public List<DateViewModel> DateTimes { get; set; }
        public List<string> Times { get; set; }
        public List<string> TodayTimes { get; set; }
    }
    public class DateViewModel
    {
        public string Date { get; set; }
        public string NumbericDate { get; set; }
        public string Time { get; set; }
    }
}
