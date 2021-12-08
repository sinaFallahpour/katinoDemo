using DNTPersianUtils.Core;
using Domain;
using Domain.DTO.Response;
using Domain.Entities;
using Domain.Utilities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Parbad;
using Service.Interfaces.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Service
{
    public class PaymentService : IPaymentService
    {
        private readonly IOnlinePayment _onlinePayment;
        private readonly DataContext _dataContext;
        private readonly IAccountService _accountService;
        private readonly IlogService _ilog;
        private readonly ITicketService _ticketService;
        private readonly ISendSmsService _smsService;

        public PaymentService(IOnlinePayment onlinePayment, DataContext dataContext, IAccountService accountService,
            IlogService ilog, ITicketService ticketService, ISendSmsService smsService)
        {
            _onlinePayment = onlinePayment;
            _dataContext = dataContext;
            _accountService = accountService;
            _ilog = ilog;
            _ticketService = ticketService;
            _smsService = smsService;
        }

        public async Task<PreFactor> PreFactor(int planId)
        {
            var plan = await _dataContext.Plans.FindAsync(planId);

            var prefactor = new PreFactor()
            {

                Date = DateTime.Now.ToShortPersianDateString(),
                PurePrice = plan.Price,
                Discount = plan.Price * Convert.ToDecimal(plan.Discount) / 100,
                Tax = plan.Price * PublicHelper.Tax / 100,
                FinalPrice = plan.Price - (plan.Price * Convert.ToDecimal(plan.Discount) / 100) + plan.Price * PublicHelper.Tax / 100

            };
            return prefactor;

        }
        public async Task<List<ListOfOrder>> GetLastOfOrders()
        {
            var user = await _accountService.GetCurrectUser();
            var lastOfOrder = _dataContext.Payments
                .Include(x => x.Plan)
                .Where(x => x.UserId == user.Id /*&& x.IsSucceed*/ /*&& x.ErrorDescription != null*/)
                .OrderByDescending(x => x.Date)
                .Select(x => new ListOfOrder
                {
                    Date = x.Date.ToShortPersianDateTimeString(true),
                    OrderId = x.Id,
                    PlanName = x.Plan.Title,
                    Price = x.Amount + x.Discount,
                    PriceWithTax = x.FinallyAmountWithTax,
                    OrderType = (x.FinallyAmountWithTax == 0) ? "هدیه" : "پرداخت آنلاین"
                }).AsNoTracking().AsQueryable();

            var lastOfOrderList = await lastOfOrder.ToListAsync();
            var lastFactorsForBackMoney = _dataContext.Factors.Include(x => x.Plan)
                                                                    .Where(x => x.CompanyId == user.Id && x.PaymnetType == PaymnetType.BackMoney)
                                                                    .Select(x => new ListOfOrder
                                                                    {
                                                                        Date = x.Date.ToShortPersianDateTimeString(true),
                                                                        OrderId = x.Id,
                                                                        OrderType = "بازگشت پول",
                                                                        PlanName = x.Plan.Title,
                                                                        Price = x.Price,
                                                                        PriceWithTax = x.Price,
                                                                    }).AsNoTracking().AsQueryable();

            //merg 
            var lastFactorsForBackMoneyList = await lastFactorsForBackMoney.ToListAsync();
            lastOfOrderList.AddRange(lastFactorsForBackMoneyList);

            return lastOfOrderList;

        }

        public async Task<OrderDetails> OrderDetails(int orderId)
        {
            var user = await _accountService.GetCurrectUser();
            var orderDetails = await _dataContext.Payments
                .Include(x => x.Plan)
                .Where(x => x.UserId == user.Id &&/* x.IsSucceed &&*/ x.Id == orderId)
                .Select(x => new OrderDetails
                {
                    Date = x.Date.ToShortPersianDateTimeString(true),
                    OrderId = x.Id,
                    PlanName = x.Plan.Title,
                    Price = x.Amount + x.Discount,
                    Discount = x.Discount,
                    PriceWithDiscount = x.Amount,
                    PayTo = PublicHelper.CompanyName,
                    PriceWithTax = x.FinallyAmountWithTax,
                    Issuccess = x.IsSucceed ? "موفق" : "نا موفق",
                    OrderType = (x.FinallyAmountWithTax == x.Amount) ? "هدیه" : "پرداخت آنلاین"
                }).AsNoTracking().FirstOrDefaultAsync();

            if (orderDetails == null)
            {
                orderDetails = await _dataContext.Factors
            .Include(x => x.Plan)
            .Where(x => x.CompanyId == user.Id && x.Id == orderId)
            .Select(x => new OrderDetails
            {
                Date = x.Date.ToShortPersianDateTimeString(true),
                OrderId = x.Id,
                PlanName = x.Plan.Title,
                Price = x.Price,
                Discount = 0,
                PriceWithDiscount = 0,
                PayTo = PublicHelper.CompanyName,
                PriceWithTax = x.Price,
                Issuccess = "موفق",
                OrderType = "برگشت پول",
            }).AsNoTracking().FirstOrDefaultAsync();

                if (orderDetails == null)
                {
                    return null;
                }
            }
            return orderDetails;

        }

        public async Task<(bool isSuccess, string message, double? discount)> CheckGiftCode(string giftCode)
        {
            var user = await _accountService.GetCurrectUser();

            var giftCart = await _dataContext.GiftCarts.FirstOrDefaultAsync(x => x.GiftCode == giftCode && x.EmployerId == user.Id);

            if (giftCart == null)
            {
                giftCart = await _dataContext.GiftCarts.FirstOrDefaultAsync(x => x.GiftCode == giftCode);
                if (giftCart == null)
                {
                    return (false, "کد وارد شده معتبر نیست", null);

                }
            }

            if (string.IsNullOrEmpty(giftCart.EmployerId))
            {
                if (giftCart.IsUse)
                {
                    return (false, "شما قبلا از این کد استفاده کردین", null);
                }
                if (giftCart.ExpireTime < DateTime.Now)
                {
                    return (false, "تاریخ مصرف این کد به پایان رسیده است", null);
                }
            }
            else
            {
                if (user == null && !user.IsActive || !user.IsActive)
                {
                    return (false, "کاربر نامعتبر", null);
                }
                if (giftCart.EmployerId != user.Id)
                {
                    return (false, "کد وارد شده معتبر نیست", null);
                }

                if (giftCart.IsUse)
                {
                    return (false, "شما قبلا از این کد استفاده کردین", null);
                }
                if (giftCart.ExpireTime < DateTime.Now)
                {
                    return (false, "تاریخ مصرف این کد به پایان رسیده است", null);
                }
            }

            return (true, "", giftCart.Discount);


        }

        public async Task<(bool isSuccess, string message, MoneyBackDTO model)> GetUserInfoForMoneyBack(string phoneNumber)
        {

            try
            {
                var user = await _accountService.FindUserByPhonenumber(phoneNumber);

                if (user == null) return (false, "کابری یافت نشد", null);
                if (user.ShebaNumber == null) return (false, "اطلاعات شبا کاربر تکمیل نشده است", null);
                var lastFactor = await _dataContext.Factors.Include(x => x.Plan)
                                                            .Where(x => x.CompanyId == user.Id && !x.IsImmediately && !x.IsBackMOney)
                                                           .OrderByDescending(x => x.Date)
                                                           .FirstOrDefaultAsync();
                if (lastFactor == null) return (false, "این کابر هیچ تراکنش مالی برای بازگشت پول ندارد  ", null);
                var advers = await _dataContext.JobAdvertisements.Include(x => x.AsignResomes)
                                                .Where(x => x.CompanyId == user.Id && x.AdverStatus != AdverStatus.Draft &&
                                                x.AdverCreatationStatus == AdverCreatationStatus.Accepted && x.AsignResomes.Count > 0
                                                && x.PlanId == lastFactor.PlanId && x.CreateAt > lastFactor.Date)
                                                 .ToListAsync();
                if (advers.Count > 0) return (false, "کاربر با آخرین خرید آگهی ثبت کرده است و قابلیت بازگشت پول ندارد", null);


                var model = new MoneyBackDTO()
                {
                    PlanName = lastFactor.Plan.Title,
                    Price = Convert.ToDouble(lastFactor.Price),
                    ShebaNumber = user.ShebaNumber,

                    PlanId = lastFactor.PlanId.Value,
                    CompanyId = lastFactor.CompanyId,
                    IsImmediately = lastFactor.IsImmediately,
                    FactorId = lastFactor.Id
                };
                return (true, "", model);

            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "GetUserInfoForMoneyBack", "Payment");

                var err = "مشکلی رخ داده است";
                return (false, err, null);

            }
        }

        public async Task<(bool isSuccess, string message)> MoneyBack(MoneyBackDTO model)
        {
            try
            {
                var lastFactor = await _dataContext.Factors.FindAsync(model.FactorId);
                lastFactor.IsBackMOney = true;
                var factor = new Factor()
                {
                    IsImmediately = model.IsImmediately,
                    CompanyId = model.CompanyId,
                    Date = DateTime.Now,
                    PlanId = model.PlanId,
                    Price = Convert.ToDecimal(model.Price),
                    TrackingCode = model.TrackingCode,
                    IsBackMOney = true,
                    PaymnetType = PaymnetType.BackMoney,

                };
                var userId = new List<string>();
                userId.Add(model.CompanyId);

                var user = await _dataContext.Users.FindAsync(model.CompanyId);
                user.PlanId = null;
                user.Plan = null;
                _dataContext.Users.Update(user);
                await _dataContext.Factors.AddAsync(factor);
                await _dataContext.SaveChangesAsync();
                var ticket = new CreateTiket()
                {
                    Subject = "بازگشت پول",
                    Content = $"کابر گرامی مبلغ {model.Price} به حساب شما واریز شده است" +
                    $"شماره پیگیری : {model.TrackingCode}",
                    TicketPriorityStatus = TicketPriorityStatus.Immediate,
                    UserId = userId,

                };

                await _ticketService.CreateTicket(ticket);


                //send sms to user
                await _smsService.SendMessageForBackMoney(user.PhoneNumber, model.TrackingCode);
                //
                return (true, "");
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "MoneyBack", "Payment");

                var err = "مشکلی رخ داده است";
                return (false, err);

            }

        }

        public async Task<List<OrderListForAdmin>> GetLastOfOrdersForAdmin(string RefrenceId = "")
        {
            try
            {
                var lastOfOrder = _dataContext.Payments
               .Include(x => x.User)
              .Include(x => x.Plan)
              //.Where(x => x.ErrorDescription != null)
              .Where(w => (RefrenceId != "") ? w.User.RefrenceId == RefrenceId : true)
              .OrderByDescending(x => x.Date)
              .Select(x => new OrderListForAdmin
              {
                  RefrenceUserName = x.RefrenceTransations.Refrence.UserName,
                  RefrenceAmount = x.RefrenceTransations.Amount,

                  Date = x.Date.ToShortPersianDateTimeString(true),
                  OrderId = x.Id,
                  PlanName = x.Plan.Title,
                  Price = x.Amount + x.Discount,
                  PriceWithTax = x.FinallyAmountWithTax,
                  OrderType = (x.FinallyAmountWithTax == 0) ? "هدیه" : "پرداخت آنلاین",
                  CompanyName = x.User.CompanyPersianName,
                  Issuccess = (x.IsSucceed) ? "موفق" : "ناموفق",
                  Discount = x.Discount,
                  PriceWithDiscount = x.Amount

              }).AsNoTracking().AsQueryable();
                var finalModel = await lastOfOrder.ToListAsync(); ;

                var lastFactorsForBackMoney = _dataContext.Factors
                    .Include(x => x.Plan).Include(x => x.Company)
                    .Where(x => x.PaymnetType == PaymnetType.BackMoney)
                    .Where(w => (RefrenceId != "") ? w.Company.RefrenceId == RefrenceId : true)
                    .Select(x => new OrderListForAdmin
                    {
                        Date = x.Date.ToShortPersianDateTimeString(true),
                        OrderId = x.Id,
                        OrderType = "بازگشت پول",
                        PlanName = x.Plan.Title,
                        Price = x.Price,
                        PriceWithTax = x.Price,
                        Issuccess = "موفق",
                        CompanyName = x.Company.CompanyPersianName
                    }).AsNoTracking().AsQueryable();

                //merg 

                var lastFactorsForBackMoneyList = await lastFactorsForBackMoney.ToListAsync();
                lastFactorsForBackMoneyList.AddRange(finalModel);

                return lastFactorsForBackMoneyList;


                return finalModel;
            }
            catch (Exception ex)
            {

                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "GetLastOfOrdersForAdmin", "Payment");

                return (null);
            }
        }
    }
}
