using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class UserWorkExperienceDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [Display(Name = " عنوان شغلی(سمت)")]
        public string WorkTitle { get; set; }


        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [Display(Name = "نام شرکت")]
        public string CompanyName { get; set; }


        [Display(Name = "تاریخ شروع")]
        [RegularExpression(@"^[1-4]\d{3}\/((0[1-6]\/((3[0-1])|([1-2][0-9])|(0[1-9])))|((1[0-2]|(0[7-9]))\/(30|31|([1-2][0-9])|(0[1-9]))))$"
, ErrorMessage = "تاریخ با فرمت شمسی وارد شود")]
        //[DNTPersianUtils.Core.ValidPersianDateTime]
        public string StartDate { get; set; }


        [Display(Name = "تاریخ پایان")]
        [RegularExpression(@"^[1-4]\d{3}\/((0[1-6]\/((3[0-1])|([1-2][0-9])|(0[1-9])))|((1[0-2]|(0[7-9]))\/(30|31|([1-2][0-9])|(0[1-9]))))$"
, ErrorMessage = "تاریخ با فرمت شمسی وارد شود")]
        public string EndDate { get; set; }


        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [Display(Name = "توضیحات ")]
        public string Description { get; set; }



    }
}
