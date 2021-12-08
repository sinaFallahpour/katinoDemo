using DNTPersianUtils.Core;
using Domain.DTO.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DTO.Account
{
    public class EmployerProfileDTO
    {
        //[Required]
        //public string UserId { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MinLength(3, ErrorMessage = "{0} باید حداقل {1} کاراکتر باشد")]
        [MaxLength(50, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [Display(Name = "نام شرکت به فارسی")]
        public string PersianFullName { get; set; }


        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MinLength(3, ErrorMessage = "{0} باید حداقل {1} کاراکتر باشد")]
        [MaxLength(50, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [Display(Name = "نام شرکت به انگلیسی")]
        public string EngFullName { get; set; }

        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "لوگوی شرکت")]
        public IFormFile Image { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(11, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [Display(Name = "شماره تماس")]
        public string EmergencPhone { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [EmailAddress(ErrorMessage ="ایمیل را درست وارد کنید")]
        [Display(Name = " ایمیل")]
        public string Email { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [Display(Name = " نام و نام خانوادگی مدیریت")]
        public string ManagmentFullName { get; set; }

        [Display(Name = "آدرس وبسایت")]
        public string url { get; set; }

        [Display(Name = "حوزه فعالیت شرکت")]
        public List<int> FieldOfActivity { get; set; }


        [Display(Name = "تعداد پرسنل شرکت")]
        public NumberOfStaff NumberOfStaff { get; set; }
        
    }   
    public class EmployeeAvatar
    {
        public IFormFile Image { get; set; }
    }
}
