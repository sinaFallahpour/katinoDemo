using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class Plan
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public string Title { get; set; }


        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public string Content { get; set; }

        public decimal Price { get; set; }
        public double Discount { get; set; }
        //daay count for user can add adver nfrom this adver
        public int Duration { get; set; }
        //if select Has Logo Set time for show that else null
        public int? Logo { get; set; }
        public int AdverExpireTime { get; set; }
        public int priority { get; set; }
        public int AdverCount { get; set; }
        public int ImmediateAdverCount { get; set; }
        public int StoryCount{ get; set; }
        public bool IsUseResomeManegement { get; set; }
        public bool IsActive { get; set; }
        public bool IsFree { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdataAt { get; set; }

        //navigation Prop
        public ICollection<Factor> Factors { get; set; }
        public ICollection<User> Companies { get; set; }
        public ICollection<JobAdvertisement> JobAdvertisements { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
}
