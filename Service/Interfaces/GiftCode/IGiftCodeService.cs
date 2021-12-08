using Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IGiftCodeService
    {
        Task<(bool isSuccess, string message)> CreateGiftCode(CreateGiftCode model);
        Task<(bool isSuccess, string message)> DeleteGiftCode( int Id);
        Task<(bool isSuccess, string message, List<AllGiftCodeForAdmin> result)> GetAllGiftCodeForAdmin();

    }
}
