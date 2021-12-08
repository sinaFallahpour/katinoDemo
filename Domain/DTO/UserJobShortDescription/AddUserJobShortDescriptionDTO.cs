using Domain.DTO.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DTO
{
    public class AddUserJobShortDescriptionDTO
    {
        public int Id { get; set; }
        public string JobTitle { get; set; }
        public EmploymentStatus EmploymentStatus { get; set; }

    }
    public class LoadUserJobShortDescriptionDTO:AddUserJobShortDescriptionDTO
    {
        public string UserFullName { get; set; }
        public List<string> LastCompanies { get; set; }
        public List<string> LastEducationBackground{ get; set; }
    }

}
