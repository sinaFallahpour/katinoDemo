using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain
{
    public class Payment
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public decimal FinallyAmountWithTax { get; set; }

        public bool IsSucceed { get; set; }
        public string InvoiceKey { get; set; }
        public string TransactionCode { get; set; }
        public DateTime Date { get; set; }
        public string TrackingNumber { get; set; }
        public string ErrorDescription { get; set; }
        public string ErrorCode { get; set; }
        public bool IsImmeditely { get; set; }
        public ICollection<Transaction> Transactions { get; set; }

        public string UserId { get; set; }
        public User User { get; set; }

        public int? PlanId { get; set; }
        public Plan Plan { get; set; }


        [ForeignKey("RefrenceTransationId")]
        public  RefrenceTransation  RefrenceTransations { get; set; }
        public int? RefrenceTransationId { get; set; }
    }
}
