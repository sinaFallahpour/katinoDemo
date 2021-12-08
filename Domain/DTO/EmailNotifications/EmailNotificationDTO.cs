using Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DTO.EmailNotifications
{
    public class EmailNotificationDTO
    {
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "دسته بندی شغلی")]
        public string CategoryIds { get; set; }
        public string KeyWord { get; set; }
        public TypeOfCooperation? TypeOfCooperation { get; set; }
        public string Cities { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [Display(Name = "ایمیل")]
        public string Email { get; set; }
    }
}
