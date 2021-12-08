using DNTPersianUtils.Core;
using Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DTO.Account
{
    public class CreateAgentDTO
    {


        public string Id { get; set; }
        [Required(ErrorMessage = "{0} الزامیست")]
        [MaxLength(200, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "{0} الزامیست")]
        [MaxLength(200, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public string StaticPhoneNumber { get; set; }

        [Display(Name = " شماره شبا ")]
        [ValidIranShebaNumber(ErrorMessage = "شماره شبا را درست وارد کنید")]
        public string ShebaNumber { get; set; }

        [Display(Name = "وضعیت تاهل ")]
        public bool IsActive { get; set; }


        //[Display(Name = " جنسیت ")]
        //public Gender Gender { get; set; }

        public bool AcceptRule { get; set; }
        [Required(ErrorMessage = "{0} الزامیست")]
        [Display(Name = "استان ")]
        public string Province { get; set; }
        [Display(Name = "شهر ")]
        [Required(ErrorMessage = "{0} الزامیست")]
        public string City { get; set; }
        public string Iframe { get; set; }
        public string Description { get; set; }

    }




    public class CreateAdmin
    {
        public string Id { get; set; }


        [Required(ErrorMessage = "{0} الزامیست")]
        [MaxLength(200, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public string FullName { get; set; }


        [Required(ErrorMessage = "{0} الزامیست")]
        [MaxLength(200, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public string UserName { get; set; }




        [Display(Name = "وضعیت   ")]
        public bool IsActive { get; set; }


        //[Display(Name = " جنسیت ")]
        //public Gender Gender { get; set; }

        //public bool AcceptRule { get; set; }

    }






   
         


}
