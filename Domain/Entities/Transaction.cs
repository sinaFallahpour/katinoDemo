﻿using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Transaction
    {
        public int Id { get; set; }
        public string TransactionCode { get; set; }
       
        public DateTime DateTime { get; set; }
        public decimal Amount { get; set; }
        public decimal Discount { get; set; }
        public decimal FinallyAmountWithTax { get; set; }

        public string Information { get; set; }


        public int PaymentId { get; set; }
        public Payment Payment { get; set; }
       

       
    }
}
