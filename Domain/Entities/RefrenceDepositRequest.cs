using Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class RefrenceDepositRequest
    {

        [Key]
        public int Id { get; set; }
        public decimal Amount { get; set; }


        public DateTime CreateDate { get; set; }


        /// <summary>
        ///علت درخواست
        /// </summary>
        public string RefrenceDescription { get; set; }




        /// <summary>
        ///علت رد یا اکسپت
        /// </summary>
        public string  AdminDescriptions { get; set; }

        //[ForeignKey("RefrenceId")]
        public User Refrence { get; set; }
        public string RefrenceId { get; set; }


        //[ForeignKey("UserId")]
        //public User User { get; set; }
        //public string UserId { get; set; }


        public RefrenceTransationStatus RefrenceTransationStatus { get; set; }


        ///// <summary>
        ///// deposit or widhtral
        ///// </summary>
        //public DepositStatus DepositStatus { get; set; }

    }
}
