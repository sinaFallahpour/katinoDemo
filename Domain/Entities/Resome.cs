using Domain.DTO.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using System.Text;

namespace Domain
{
    public class Resome
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        [MaxLength(800, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public string AboutMe { get; set; }
        /// <summary>
        /// file thats user send from self
        /// </summary>
        public string PDFResome { get; set; }

        /// <summary>
        ///  file thats generate by my site (create resome)
        /// </summary>
        public string KatinoPDFResome { get; set; }

        //navigation Property

        public string EmployeeId { get; set; }
        public User Employee { get; set; }

        public ICollection<UserJobSkill> UserJobSkills { get; set; }


        public ICollection<UserWorkExperience> UserWorkExperiences { get; set; }


        public ICollection<EducationalBackground> EducationalBackgrounds { get; set; }

        public ICollection<UserLanguage> UserLanguages { get; set; }

        public int? UserJobPreferencesId { get; set; }
        public UserJobPreferences UserJobPreferences { get; set; }




        public ICollection<AsignResome> AsignResomes { get; set; }
        public ICollection<UserJobShortDescription> UserJobShortDescriptions { get; set; }
        


    }
}
