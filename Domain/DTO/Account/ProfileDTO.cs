using Domain.DTO.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    //
    public class ProfileDTO
    {

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [EmailAddress(ErrorMessage ="فرمت ایمیل صحیح نمیباشد")]
        [Display(Name = " ایمیل")]
        public string Email { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [Display(Name = " نام و نام خانوادگی مدیریت")]
        public string ManagmentFullName { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [Display(Name = "نام شرکت به فارسی")]
        public string PersianFullName { get; set; }


        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [Display(Name = "نام شرکت به انگلیسی")]
        public string EngFullName { get; set; }

        

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(11, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [Display(Name = "شماره تماس")]
        public string EmergencPhone { get; set; }

        [Display(Name = "آدرس وبسایت")]
        public string url { get; set; }

       

        [Display(Name = "تعداد پرسنل شرکت")]
        public NumberOfStaff NumberOfStaff { get; set; }

        

        [Display(Name = " معرفی شرکت به کارجو")]
        public string ShortDescription { get; set; }

    }
    public class EditEmployerProfileDTO : ProfileDTO
    {
        [Display(Name = "حوزه فعالیت شرکت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public List<int> FieldOfActivity { get; set; }
        [Display(Name = " شهر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string City { get; set; }
        [Display(Name = "لوگوی شرکت")]
        public IFormFile Image { get; set; }
    }
    public class LoadProfile : ProfileDTO
    {
        [Display(Name = " شهر")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Cities { get; set; }

        [Display(Name = "لوگوی شرکت")]
        public string Image { get; set; }
        [Display(Name = "حوزه فعالیت شرکت")]
        public List<ListOfCategoriesForSelect> FieldOfActivity { get; set; }

       

    }
}
