using DNTPersianUtils.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DTO.Account
{
    public class RefreshDTO
    {
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MinLength(11, ErrorMessage = "{0} باید حداقل {1} کاراکتر باشد")]
        [MaxLength(11, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [ValidIranianMobileNumber(ErrorMessage = "لطفا{0} را درست وارد کنید")]
        [Display(Name = "شماره موبایل")]
        public string PhoneNumber { get; set; }


    }


     

}
