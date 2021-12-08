using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface ISendSmsService
    {
        Task<(bool isSuccess, string error)> SendMessage(string To, string data, string pattern);
        Task<(bool isSuccess, string error)> SendVerificationCode(string name, string phoneNumber, string code);
        Task<(bool isSuccess, string error)> SendGiftCodeCode(string name, string phoneNumber, string code);
        Task<(bool isSuccess, string error)> SendNewGiftCode(string name, string phoneNumber, string code);
        Task<(bool isSuccess, string error)> SendMessageForAcceptAdver(string name, string phoneNumber);
        Task<(bool isSuccess, string error)> SendMessageForRejectAdver(string name, string phoneNumber);
        Task<(bool isSuccess, string error)> SendMessageForReturnAdver(string name, string phoneNumber);
        Task<(bool isSuccess, string error)> SendMessageForPendindAdver(string name, string phoneNumber);
        Task<(bool isSuccess, string error)> SendMessageForBackMoney(string phoneNumber, string trackingNumber);
        Task<(bool isSuccess, string error)> SendConfirmationStatusToEmployee(string name, string phoneNumber, string JobAdvertisementTitle,int JobAdvertisementId);
        Task<(bool isSuccess, string error)> SendNewResomeComeInToEmployer(string name, string phoneNumber, string JobAdvertisementTitle);
        Task<(bool isSuccess, string error)> SendForNewTicket(string phoneNumber, string title, string date);
        Task<(bool isSuccess, string error)> SendForCreateGiftCode(string phoneNumber, string percent, string days,string code);



    }
}
