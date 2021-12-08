using Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class EditUserLanguageDTO
    {
        public int Id { get; set; }
        public LanguageLevel LanguageLevel { get; set; }
        public int LanguageId { get; set; }

    }
}
