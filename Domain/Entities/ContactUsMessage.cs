using DNTPersianUtils.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class ContactUsMessage
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("نام و نام خانوادگی")]
        [Required(ErrorMessage = "لطفا  {0}  را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد ")]
        public string FullName { get; set; }

        [DisplayName("ایمیل ")]
        [Required(ErrorMessage = "لطفا  {0}  را وارد کنید")]
        [EmailAddress(ErrorMessage = "با فرمت ایمیل وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد ")]
        public string Email { get; set; }

        [DisplayName("متن پیام")]
        [Required(ErrorMessage = "لطفا  {0}  را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمیتواند بیشتر از {1} کاراکتر باشد ")]
        public string Comment { get; set; }

        [DisplayName("شماره تلفن")]
        [ValidIranianMobileNumber(ErrorMessage = "لطفا{0} را درست وارد کنید")]
        public string PhoneNumber { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
