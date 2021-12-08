using Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class ListOfUserLanguage
    {
        public int Id { get; set; }
        public LanguageLevel LanguageLevel { get; set; }
        public string LanguageName { get; set; }
    }
}
