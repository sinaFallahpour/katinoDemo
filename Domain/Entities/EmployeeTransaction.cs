using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class EmployeeTransaction
    {
        public int Id { get; set; }
        public string TransactionCode { get; set; }

        public DateTime DateTime { get; set; }
        public decimal Amount { get; set; }

        public string Information { get; set; }


        public int EmployeePaymentId { get; set; }
        public EmployeePayment EmployeePayment { get; set; }
    }
}
