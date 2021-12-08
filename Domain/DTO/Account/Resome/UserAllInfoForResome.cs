using Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class UserAllInfoForResome
    {
        public int ResomeId { get; set; }
        public UserPersonalInfoForResome UserPersonalInfoForResome { get; set; }
        public string AbouteMe { get; set; }
        public List<ListOfUserJobSkill> UserJobSkill { get; set; }
        public List<UserWorkExperienceDTO> UserWorkExperience { get; set; }
        public List<UpdateEducationalBackgroundDTO> UserEducationalBackground { get; set; }
        public List<ListOfUserLanguage> ListOfUserLanguage { get; set; }
        public UserJobPreferences UserJobPreference { get; set; }
    }
    public class UserPersonalInfoForResome
    {
        public string FullName { get; set; }
        public string JobTitle { get; set; }
        public EmploymentStatus? EmploymentStatus { get; set; }
        public List<string> LastCompanies { get; set; }
        public List<string> LastDegreeOfEducation { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public bool? IsMarried { get; set; }
        public string BirthYear { get; set; }
        public Gender Gender { get; set; }
        public string Military { get; set; }

        [Display(Name = " تاریخ پایان معافیت")]
        public string ExemptionExpirestionDate { get; set; }

        [Display(Name = " تاریخ دریافت کارت خدمت ")]
        public string ExemptionExpirestionRecieveDate { get; set; }
    }
}
