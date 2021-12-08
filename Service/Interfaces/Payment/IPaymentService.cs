
using Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IPaymentService
    {
        Task<PreFactor> PreFactor(int planId);

        Task<List<ListOfOrder>> GetLastOfOrders();
        Task<List<OrderListForAdmin>> GetLastOfOrdersForAdmin(string RefrenceId = "");
        Task<OrderDetails> OrderDetails(int orderId);
        Task<(bool isSuccess, string message,double? discount)> CheckGiftCode(string giftCode);
        Task<(bool isSuccess, string message,MoneyBackDTO model)> GetUserInfoForMoneyBack(string phoneNumber);
        Task<(bool isSuccess, string message)> MoneyBack(MoneyBackDTO model);


    }
}
