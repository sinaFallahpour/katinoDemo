using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DTO
{
    public class AddCategoryDTO
    {
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public string Name { get; set; }
        public int ParentId { get; set; }
    }
  
}
