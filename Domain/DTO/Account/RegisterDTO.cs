using DNTPersianUtils.Core;
using Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DTO.Account
{
    public class RegisterDTO
    {
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MinLength(11, ErrorMessage = "{0} باید حداقل {1} کاراکتر باشد")]
        [MaxLength(11, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [ValidIranianMobileNumber(ErrorMessage ="لطفا{0} را درست وارد کنید")]
        [Display(Name = "شماره موبایل")]
        public string PhoneNumber { get; set; }



        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MinLength(6, ErrorMessage = "{0} باید حداقل {1} کاراکتر باشد")]
        [MaxLength(20, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [Display(Name = "نام و نام خانوادگی")]
        public string FullName { get; set; }


        [Display(Name = "شرایط خاص")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public SpecialEmpolyee SpecialEmpolyee { get; set; }


    }
}
