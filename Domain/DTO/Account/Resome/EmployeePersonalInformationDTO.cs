using DNTPersianUtils.Core;
using Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class EmployeePersonalInformationDTO
    {
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [DataType(DataType.EmailAddress,ErrorMessage = "لطفا{0} را درست وارد کنید")]
        public string Email { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MinLength(11, ErrorMessage = "{0} باید حداقل {1} کاراکتر باشد")]
        [MaxLength(11, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [ValidIranianMobileNumber(ErrorMessage = "لطفا{0} را درست وارد کنید")]
        public string PhoneNumber { get; set; }


        [Display(Name = "شرایط خاص")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public SpecialEmpolyee SpecialEmpolyee { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(150, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public string City { get; set; }


        [MaxLength(500, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public string Address { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public Gender Gender { get; set; }

        [MaxLength(50, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public string Military { get; set; }

        public bool IsMarreid { get; set; }


        [Display(Name = " تاریخ پایان معافیت")]
        [ValidPersianDateTime(ErrorMessage = "با فرمت شمسی وارد شود")]
        public string ExemptionExpirestionDate { get; set; }

        [Display(Name = " تاریخ دریافت کارت خدمت ")]
        [ValidPersianDateTime(ErrorMessage ="با فرمت شمسی وارد شود")]
        public string ExemptionExpirestionRecieveDate { get; set; }
    }
    public class LoadEmployeePersonalInformationDTO: EmployeePersonalInformationDTO
    {
        public string FullName { get; set; }
        public string EmployeeAvatar { get; set; }
    }
}
