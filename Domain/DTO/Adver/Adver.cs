using Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class Adver
    {
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} باید حداقل {1} کاراکتر باشد")]
        public string Title { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "{0} باید حداقل {1} کاراکتر باشد")]
        public string City { get; set; }

        public TypeOfCooperation TypeOfCooperation { get; set; }
        public Salary Salary { get; set; }
        public WorkExperience WorkExperience { get; set; }
        public DegreeOfEducation DegreeOfEducation { get; set; }
        public Gender Gender { get; set; }
        public string Military { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(1000, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public string DescriptionOfJob { get; set; }
        public SpecialEmpolyee SpecialEmpolyee { get; set; }




        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        //[MaxLength(1000, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        //[Display(Name = "ساعت کاری")]
        //public string WorkTime { get; set; }


        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        //[MaxLength(1000, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        //[Display(Name ="شماره ثابت")]
        //public string StaticNumber { get; set; }

        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(1000, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        [Display(Name ="شماره موبایل")]
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

    }
    public class AddAdverDTO : Adver
    {
        //[Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int fieldOfActivity { get; set; }
        public string StorySaz { get; set; }


    }
    public class LoadAdverDTO : Adver
    {
        public int FieldOfActivity { get; set; }
    }

    public class EditAdverDTO : Adver
    {
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Id { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int fieldOfActivity { get; set; }
    }
}
