using Domain.DTO.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Web.Http.Results;
using System.Web.Http.Routing.Constraints;

namespace Domain
{
    public class JobOpportunity
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }



        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [Display(Name = "عنوان شغلی")]
        public string Title { get; set; }

        [Display(Name = "نوع قرارداد ")]
        public WorkExperience WorkExperience { get; set; }

        [Display(Name = "شهر")]
        [MaxLength(100, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
       //var city="sari,neka,qaemshahr,..."
        public string City { get; set; }



        //navigaton prop
        public int CategoyId { get; set; }
        public Category Category { get; set; }

        public string EmployeeId { get; set; }
        public User Employee { get; set; }

    }
}
