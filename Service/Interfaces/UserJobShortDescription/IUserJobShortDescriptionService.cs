using Domain;
using Domain.DTO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IUserJobShortDescriptionService

    {
        Task<(LoadUserJobShortDescriptionDTO finalModel, string errors)> LoadUserJobShortDescription();
        Task<(bool isSuccess, string errors)> UpdateUserJobShortDescription(EditUserJobShortDescriptionDTO model);
    }
}
