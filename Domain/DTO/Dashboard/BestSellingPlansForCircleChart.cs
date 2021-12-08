using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
   public class BestSellingPlansForCircleChart
    {
        public string Name { get; set; }
        public double y { get; set; }
    }
    public class BestSellingPlansForLineChart
    {
        public string Lable { get; set; }
        public int  Data { get; set; }
    }
    public class BestSellingPlansForLine
    {
        public List<string> Lable { get; set; }
        public List<int> Data { get; set; }
    }

    public class InfoCountForColumnChart
    {
        public List<string> Lable { get; set; }
        public List<int> EmployeeCount { get; set; }
        public List<int> EmployerCount { get; set; }
        public List<int> AdverCount { get; set; }
    }
    public class GetUserCountForChart
    {
        public string Lable { get; set; }
        public string Data { get; set; }
    }
}
