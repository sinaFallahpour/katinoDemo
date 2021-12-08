using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{

    public class Log
    {
        public int Id { get; set; }
        public string ExceptionMessage { get; set; }
        public string ExceptionType { get; set; }
        public string MethodName { get; set; }
        public string TableName { get; set; }
        public string UserName { get; set; }
        public DateTime Date { get; set; }

    }
}
