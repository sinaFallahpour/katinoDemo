using Domain.DTO.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class UserJobPreferences
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [Display(Name = "شهر")]

        public string City { get; set; }


        [Display(Name = "نوع قرارداد")]
        public TypeOfCooperation TypeOfCooperation { get; set; }

        [Display(Name = "سطح ارشدیت")]
        public Senioritylevel Senioritylevel { get; set; }


        [Display(Name = " حقوق")]
        public Salary Salary { get; set; }

        [Display(Name = " ترفیع")]
        public bool Promotion { get; set; }

        [Display(Name = " بیمه")]
        public bool Insurance { get; set; }

        [Display(Name = " دوره های آموزشی")]
        public bool EducationCourses { get; set; }

        [Display(Name = " ساعت کاری منعطف")]
        public bool FlexibleWorkingTime { get; set; }

        [Display(Name = " غذا به عهده شرکت")]
        public bool HasMeel { get; set; }

        [Display(Name = " سرویس رفت وآمد")]
        public bool TransportationService { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }



        //navigation Properties
        public ICollection<UserJobPreferenceCategory> UserJobPreferenceCategories { get; set; }


        public int ResomeId { get; set; }
        public Resome Resome { get; set; }

    }
}
