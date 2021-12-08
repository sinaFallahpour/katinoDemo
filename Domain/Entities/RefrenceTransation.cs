using Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class RefrenceTransation
    {
        [Key]
        public int Id { get; set; }
        public decimal Amount { get; set; }


        //[ForeignKey("RefrenceId")]
        public User Refrence { get; set; }
        public string RefrenceId { get; set; }


        //[ForeignKey("UserId")]
        public User User { get; set; }
        public string UserId { get; set; }



        public int? PaymentId { get; set; }
        [ForeignKey("PaymentId")]
        public Payment Payment { get; set; }



        /// <summary>
        /// deposit or widhtral
        /// </summary>
        public DepositStatus DepositStatus { get; set; }

    }
}
