using Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class EducationalBackground
    {
        public int Id { get; set; }
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
        public DateTime StartDate { get; set; }

        /// <summary>
        /// if EndDate=null ---> that means work at this now
        /// </summary>
        /// 
        [Display(Name = "تاریخ پایان")]
        public DateTime? EndDate { get; set; }


        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [Display(Name = "توضیحات ")]
        public string Description { get; set; }


        [Display(Name = "حذف شده")]
        public bool IsActive { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public int ResomeId { get; set; }
        public Resome Resome { get; set; }

    }
}
