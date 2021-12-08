using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class JobSkill
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(50, ErrorMessage = "{0} باید حداکثر {1} کاراکتر باشد")]
        public string Name { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }


        //navigation prop   
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public bool IsActive { get; set; }
        public ICollection<UserJobSkill> UserJobSkills { get; set; }

    }
}
