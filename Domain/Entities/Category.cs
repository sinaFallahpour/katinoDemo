using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        
        public string Name { get; set; }
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
        //navigation prop
        public int? CategoryId { get; set; }
        public Category Parent { get; set; }
        public ICollection<Category> Childs { get; set; }
        public ICollection<CompanyCategory> CompanyCategorires { get; set; }
        public ICollection<JobAdvertisement> JobAdvertisements { get; set; }
        public ICollection<JobSkill> JobSkills { get; set; }
        public ICollection<UserJobPreferenceCategory> UserJobPreferenceCategories { get; set; }
        public ICollection<JobOpportunity> JobOpportunities { get; set; }

        public bool IsActive { get; set; }


    }
}
