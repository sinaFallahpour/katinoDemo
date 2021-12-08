using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class EmployeePayment
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }

        public bool IsSucceed { get; set; }
        public string InvoiceKey { get; set; }
        public string TransactionCode { get; set; }
        public DateTime Date { get; set; }
        public string TrackingNumber { get; set; }
        public string ErrorDescription { get; set; }
        public string ErrorCode { get; set; }
        public ICollection<EmployeeTransaction> EmployeeTransactions { get; set; }

        public string EmployeeId { get; set; }
        public User Employee { get; set; }

        public int? EmployeePlanId { get; set; }
        public EmployeePlan EmployeePlan { get; set; }
    }
}
