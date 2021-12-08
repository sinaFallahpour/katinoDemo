using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Katino.Areas.Refrence.ViewModel
{
    public class RequestDTO
    {
        [Required(ErrorMessage = "{0} الزامیست")]
        public decimal? Amount { get; set; }


        [Required(ErrorMessage = "{0} الزامیست")]
        public string RefrenceDescription { get; set; }
    }
}
