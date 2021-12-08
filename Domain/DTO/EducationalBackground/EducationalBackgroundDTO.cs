using DNTPersianUtils.Core;
using Domain.DTO.Response;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class EducationalBackgroundDTO
    {

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [Display(Name = "رشته تحصیلی")]
        public string FieldOfStudy { get; set; }


        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [Display(Name = "نام دانشگاه")]
        public string UniversityName { get; set; }

        [Display(Name = " مقطع تحصیلی")]
        public DegreeOfEducation DegreeOfEducation { get; set; }


        [Display(Name = "تاریخ شروع")]
        [ValidPersianDateTime(ErrorMessage = "با فرمت شمسی وارد شود")]
        public string StartDate { get; set; }


        [Display(Name = "تاریخ پایان")]
        [ValidPersianDateTime(ErrorMessage = "با فرمت شمسی وارد شود")]
        public string EndDate { get; set; }


        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [Display(Name = "توضیحات ")]
        public string Description { get; set; }



    }
}
