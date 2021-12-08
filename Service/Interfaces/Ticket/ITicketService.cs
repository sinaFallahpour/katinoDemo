using Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface ITicketService
    {
        Task<(bool isSuccess, string error)> CreateTicket(CreateTiket model);
        Task<(bool isSuccess, string error)> SenderSeenTicket(List<int> ids);
        Task<(bool isSuccess, string error)> ReceiverSeenTicket(List<int> ids);
        Task<(bool isSuccess, string error)> AnswerTicket(TicketAdminAnswer model);
        Task<(TicketDetailsForUser model, bool isSuccess, string error)> GetTicketInfo(int id);
        Task<(List<GetAllTicketForAdmin> model, bool isSuccess, string error)> GetAllTicketForAdmin(string role);
        Task<(List<GetAllTicketForAdmin> model, bool isSuccess, string error)> GetAllTicketForRefrence(string role);
        Task<(TicketNotification model, bool isSuccess, string error)> GetNotifTicket(string RefrenceId = "");
        Task<(List<GetAllTicketForCurrectUser> model, bool isSuccess, string error)> GetAllTicketForUser();



    }
}
