using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DTO.AnswerQuestion
{
    public class AnswerQuestion
    {
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [Display(Name = "شماره تماس")]
        public string Question { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [Display(Name = "شماره تماس")]
        public string Answer { get; set; }
    }
    public class EditAnswerQuestion:AnswerQuestion
    {
        public int Id { get; set; }
    }
    public class AllAnswerQuestion : EditAnswerQuestion
    {

    }
}
