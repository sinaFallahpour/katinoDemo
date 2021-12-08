using Domain.DTO.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DTO
{
    public class AddUserJobPreferencesDTO
    {


        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [Display(Name = "شهر")]

        public string City { get; set; }


        [Display(Name = "نوع قرارداد")]
        [Required(ErrorMessage = "لطفا  {0}  را وارد کنید")]
        public TypeOfCooperation TypeOfCooperation { get; set; }

        [Display(Name = "سطح ارشدیت")]
        [Required(ErrorMessage = "لطفا  {0}  را وارد کنید")]
        public Senioritylevel Senioritylevel { get; set; }


        [Display(Name = " حقوق")]
        [Required(ErrorMessage = "لطفا  {0}  را وارد کنید")]
        public Salary Salary { get; set; }

        [Display(Name = " ترفیع")]
        [Required(ErrorMessage = "لطفا  {0}  را وارد کنید")]
        public bool Promotion { get; set; }

        [Display(Name = " بیمه")]
        [Required(ErrorMessage = "لطفا  {0}  را وارد کنید")]
        public bool Insurance { get; set; }

        [Display(Name = " دوره های آموزشی")]
        [Required(ErrorMessage = "لطفا  {0}  را وارد کنید")]
        public bool EducationCourses { get; set; }

        [Display(Name = " ساعت کاری منعطف")]
        [Required(ErrorMessage = "لطفا  {0}  را وارد کنید")]
        public bool FlexibleWorkingTime { get; set; }

        [Display(Name = " غذا به عهده شرکت")]
        [Required(ErrorMessage = "لطفا  {0}  را وارد کنید")]
        public bool HasMeel { get; set; }

        [Display(Name = " سرویس رفت وآمد")]
        [Required(ErrorMessage = "لطفا  {0}  را وارد کنید")]
        public bool TransportationService { get; set; }

        public List<int> CategoryIds { get; set; }


    }

}
