using DNTPersianUtils.Core;
using Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class RefrenceRegisterDTO
    {
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [ValidIranianMobileNumber(ErrorMessage = "لطفا{0} را درست وارد کنید")]
        [Display(Name = "شماره موبایل")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MinLength(4, ErrorMessage = "{0} باید حداقل {1} کاراکتر باشد")]
        [MaxLength(20, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [Display(Name = "نام و نام خانوادگی")]
        public string FullName { get; set; }

        //[MaxLength(50, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        //public string Logo { get; set; }


        [MaxLength(11, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public string EmergencPhone { get; set; }


        [Display(Name = "شهر ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public string City { get; set; }

        [Display(Name = "آدرس ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public string Address { get; set; }


        [Display(Name = "وضعیت تاهل ")]
        public bool IsMarried { get; set; }
       

        [Display(Name = " جنسیت ")]                                  
        public Gender Gender { get; set; }

        [Display(Name = " شماره شبا ")]
        [ValidIranShebaNumber(ErrorMessage = "شماره شبا را درست وارد کنید")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ShebaNumber { get; set; }

    }
}
