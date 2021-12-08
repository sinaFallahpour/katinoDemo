using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class CreateSettingDTO
    {
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "کلید")]
        public string Key { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "مقدار")]
        public string Value { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "عنوان")]
        public string Title { get; set; }
    }
}
