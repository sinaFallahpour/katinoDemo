using Domain.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class EditUserJobPreferencesDTO:AddUserJobPreferencesDTO
    {
        public int Id { get; set; }


    }
}
