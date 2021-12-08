using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class UserJobSkill
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }


        //navigation propperties

        public int JobSkillId { get; set; }
        public JobSkill JobSkill { get; set; }

        public int ResomeId { get; set; }
        public Resome Resome { get; set; }
    }
}
