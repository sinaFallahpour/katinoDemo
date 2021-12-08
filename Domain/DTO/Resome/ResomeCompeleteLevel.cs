using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class ResomeCompeleteLevel
    {
        public double CompeletePercent { get; set; }
        public double aboutMe { get; set; } 
        public double checkUserInfo { get; set; } 
        public double UserJobSkillId { get; set; } 
        public double UserWorkExperienceId { get; set; } 
        public double EducationalBackgroundId { get; set; } 
        public double UserLanguageId { get; set; } 
        public double UserJobPreferencesId { get; set; } 

        public double GetCompeletePercent()
        {
            double percent = 0;
            if (this.aboutMe==0) percent += 6;
            if (this.checkUserInfo == 0) percent += 20;
            if (this.UserJobPreferencesId == 0) percent += 20;
            if (this.UserJobSkillId == 0) percent += 12;
            if (this.UserWorkExperienceId == 0) percent += 15;
            if (this.EducationalBackgroundId == 0) percent += 15;
            if (this.UserLanguageId == 0) percent += 12;
            return percent;
        }

    }
}
