using DNTPersianUtils.Core;
using Domain.DTO.Response;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class User : IdentityUser
    {


        /// <summary>
        /// پلن هایی که تا حالا استفاده کرده
        /// </summary>
        public string UsedPlans { get; set; }


        /// <summary>
        /// darsad refrence
        /// </summary>
        public double RefrencePercent { get; set; }


        ///// <summary>
        ///// آیا کاربر از پلن رایگان استفاده کرده؟
        ///// </summary>
        //public bool IsUsedFreePlan { get; set; }

        public decimal RefrenceTotalPrice { get; set; }


        /// <summary>
        /// تلفن ثابت
        /// </summary>
        public string StaticPhoneNumber { get; set; }

        public bool AcceptRule { get; set; }
        public string Role { get; set; }
        public string SerialNumber { get; set; }
        public int? verificationCode { get; set; }
        public DateTime verificationCodeExpireTime { get; set; }

        [MaxLength(50, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public string Fullname { get; set; }

        [MaxLength(50, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public string CompanyEngName { get; set; }
        [MaxLength(50, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public string CompanyPersianName { get; set; }

        [MaxLength(50, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public string Logo { get; set; }

        [MaxLength(50, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public string Url { get; set; }

        [MaxLength(11, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public string EmergencPhone { get; set; }
        public NumberOfStaff NumberOfStaff { get; set; }
        public bool IsActive { get; set; }
        public DateTime RegisterationDate { get; set; }
        public DateTime ModifiedDate { get; set; }

        [Display(Name = "توضیح مختصر")]
        [MaxLength(1000, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public string ShortDescription { get; set; }

        [Display(Name = "شهر ")]
        [MaxLength(50, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public string City { get; set; }
        [Display(Name = "آدرس ")]
        [MaxLength(200, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public string Address { get; set; }
        [Display(Name = "وضعیت تاهل ")]
        [MaxLength(200, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public bool? IsMarried { get; set; }
        [Display(Name = "تاریخ تولد ")]
        [MaxLength(10, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public DateTime? Birthday { get; set; }
        [Display(Name = " جنسیت ")]
        public Gender Gender { get; set; }
        [Display(Name = " وضعیت خدمت سربازی ")]
        public string Military { get; set; }


        [Display(Name = " تاریخ پایان معافیت")]
        public DateTime? ExemptionExpirestionDate { get; set; }

        [Display(Name = " تاریخ دریافت کارت خدمت ")]
        public DateTime? ExemptionExpirestionRecieveDate { get; set; }
        [Display(Name = " امتیاز ")]
        public int? Rate { get; set; }


        [Display(Name = " شماره شبا ")]
        [ValidIranShebaNumber(ErrorMessage = "شماره شبا را درست وارد کنید")]
        public string ShebaNumber { get; set; }


        public string LastSeen { get; set; }


        //navigation prop   
        public ICollection<GiftCart> GiftCarts { get; set; }
        //public ICollection<GiftCart> GiftCart_Copys{ get; set; }
        public ICollection<CompanyCategory> CompanyCategorires { get; set; }
        public ICollection<JobAdvertisement> JobAdvertisements { get; set; }
        //public int JobAdvertId { get; set; }
        //public JobAdvertisement JobAdvertisement { get; set; }
        public ICollection<Factor> Factors { get; set; }
        public ICollection<EmployeeFactor> EmployeeFactors { get; set; }

        public int? PlanId { get; set; }
        public Plan Plan { get; set; }

        public int? EmployeePlanId { get; set; }
        public EmployeePlan EmployeePlan { get; set; }

        public SpecialEmpolyee SpecialEmpolyee { get; set; }

        public ICollection<Payment> Payments { get; set; }


        public int? ResomeId { get; set; }
        public Resome Resome { get; set; }

        public ICollection<JobOpportunity> JobOpportunities { get; set; }

        public ICollection<AdvertismentNotification> AdvertismentNotifications { get; set; }
        public ICollection<MarkedAdver> MarkedAdvers { get; set; }
        public ICollection<Ticket> SenderTickets { get; set; }
        public ICollection<Ticket> ReceiverTickets { get; set; }

        public string ConecctionId { get; set; }

        public User Refrence { get; set; }
        public string RefrenceId { get; set; }

        public ICollection<User> RefrenceUsers { get; set; }

        public List<RefrenceTransation> RefrenceTransations { get; set; }
        public List<RefrenceTransation> RefrenceTransations2 { get; set; }

        public List<RefrenceDepositRequest> RefrenceDepositRequest { get; set; }
        public List<RefrenceDepositRequest> RefrenceDepositRequest2 { get; set; }






        public string Description { get; set; }
        public string Iframe { get; set; }
        public string Province { get; set; }
        public List<Image> Gallery { get; set; }
        public bool IsMain { get; set; }
        public int StorySazCount { get; set; }
    }
}
