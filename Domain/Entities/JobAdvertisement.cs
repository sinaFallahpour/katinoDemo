using Domain.DTO.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class JobAdvertisement
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public string Title { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public string City { get; set; }
        public string ImageAddress { get; set; }
        public TypeOfCooperation TypeOfCooperation { get; set; }
        public Salary Salary { get; set; }
        public WorkExperience WorkExperience { get; set; }
        public DegreeOfEducation DegreeOfEducation { get; set; }
        public Gender Gender { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(200, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public string Military { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(1000, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public string DescriptionOfJob { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        //[MinLength(1000, ErrorMessage = "{0} باید حداقل {1} کاراکتر باشد")]
        //public string DescriptionOfCompany { get; set; }
        public AdverStatus AdverStatus { get; set; }
        public DateTime ExpireTime { get; set; }
        public long VisitThisAdver { get; set; }
        public long VisitThisAdverInSite { get; set; }
        public bool IsImmediate { get; set; }
        public bool IsStorySaz { get; set; }

        //navigation properties
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string CompanyId { get; set; }
        public User Company { get; set; }

        public int? PlanId { get; set; }
        public Plan Plan { get; set; }
        /// <summary>
        /// Age TRUE bood yani agahi batel shode age FALSE bood yani faale 
        /// </summary>
        /// 

        public bool IsActive { get; set; }
        public string AdminDescription { get; set; }
        public SpecialEmpolyee SpecialEmpolyee { get; set; }
        /// <summary>
        /// this property for count visit this adver in site 
        /// </summary>
        /// 

        public int Visit { get; set; }
        public AdverCreatationStatus AdverCreatationStatus { get; set; }


        public ICollection<MarkedAdver> MarkedAdvers { get; set; }
        public ICollection<AdvertismentNotification> AdvertismentNotifications { get; set; }

        public ICollection<AsignResome> AsignResomes { get; set; }

        //public string Telegram { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }





        public string WorkTime { get; set; }
        public string StaticNumber { get; set; }
        public int? ResomeColorId { get; set; }
    }


}
