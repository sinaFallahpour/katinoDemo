using Domain.DTO.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DTO
{
    public class AddUserLanguageDTO
    {
        public LanguageLevel LanguageLevel { get; set; }
        public int LanguageId { get; set; }
    }
  
}
