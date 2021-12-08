using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class CreatePlanDTO
    {

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public string Title { get; set; }


        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public string Content { get; set; }

        public decimal Price { get; set; }
        public double? Discount { get; set; }
        public int Duration { get; set; }
        public int? Logo { get; set; }
        public int priority { get; set; }
        public bool IsFree { get; set; }
        public int AdverExpireTime { get; set; }
        public int AdverCount { get; set; }
        public int ImmediateAdverCount { get; set; }
        public int StoryCount { get; set; }

        public bool IsUseResomeManegement { get; set; }
    }
}
