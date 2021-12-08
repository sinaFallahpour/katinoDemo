using Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class UserLanguage
    {
        public int Id { get; set; }
        public LanguageLevel LanguageLevel { get; set; }

        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }


        //navigation properties

        public int LanguageId { get; set; }
        public Languag Languag { get; set; }


        public int ResomeId { get; set; }
        public Resome Resome { get; set; }
    }
}
