using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class GiftCart
    {
        public GiftCart()
        {
            //ExpireTime = CreateAt.AddMonths(1);
        }
        public int Id { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(5)]
        public string GiftCode { get; set; }
        public double Discount { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime? UseAt { get; set; }
        public DateTime ExpireTime { get; set; }

        public bool IsUse { get; set; }

        //navigation prop
        public string EmployerId { get; set; }
        public User Employer { get; set; }
    }
}
