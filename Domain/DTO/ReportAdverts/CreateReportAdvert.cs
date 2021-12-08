using Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DTO.ReportAdverts
{
    public class CreateReportAdvert
    {
        public int AdvertId { get; set; }
        [Display(Name ="عنوان")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "توضیحات تکمیلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public string Description { get; set; }
        [Display(Name = "نام")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Name { get; set; }
        [Display(Name = "تلغن همراه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string PhoneNumber { get; set; }
        public ReportAdvertType Type { get; set; }
    }
}
