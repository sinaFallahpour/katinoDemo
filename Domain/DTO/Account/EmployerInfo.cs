using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class EmployerInfo
    {

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [Display(Name = "نام و نام خانوادگی")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [Display(Name = "نام فارسی شرکت")]
        public string CompanyPersianName { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [Display(Name = "نام لاتین شرکت")]
        public string CompanyEngName { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }


        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [Display(Name = "توضیح مختصر")]
        public string ShortDescription { get; set; }
        
        public IFormFile Image { get; set; }
    }
    public class GetEmployerInfo
    {
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [Display(Name = "نام و نام خانوادگی")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [Display(Name = "نام فارسی شرکت")]
        public string CompanyPersianName { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [Display(Name = "نام لاتین شرکت")]
        public string CompanyEngName { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }


        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [Display(Name = "توضیح مختصر")]
        public string ShortDescription { get; set; }

        public string Image { get; set; }
    }
}
