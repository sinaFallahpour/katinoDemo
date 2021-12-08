using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Domain;
using Domain.DTO.Response;
using Domain.Entities;
using Domain.Utilities;
using Katino.Config.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Parbad;
using Parbad.AspNetCore;
using Service;
using Service.Interfaces.Account;

namespace Katino.Controllers
{
    [Route("api/[controller]")]

    public class EmployeePaymentController : ControllerBase
    {
        private readonly IOnlinePayment _onlinePayment;
        private readonly DataContext _dataContext;
        private readonly IAccountService _accountService;
        private readonly IlogService _ilog;
        private readonly IPaymentService _paymentService;
        private readonly IHttpClientFactory _ClientFactory;
        private readonly IGiftCodeService _giftCodeService;

        public EmployeePaymentController(IOnlinePayment onlinePayment
            , DataContext dataContext,
            IAccountService accountService,
            IlogService ilog,
           IPaymentService paymentService
            , IHttpClientFactory httpClientFactory,
           IGiftCodeService giftCodeService)
        {
            _onlinePayment = onlinePayment;
            _dataContext = dataContext;
            _accountService = accountService;
            _ilog = ilog;
            _paymentService = paymentService;
            _ClientFactory = httpClientFactory;
            _giftCodeService = giftCodeService;
        }


        #region Pay
        [HttpPost("EmployeepaymentPlan")]
        [ClaimsAuthorize]
        [Authorize(Roles = PublicHelper.EmployeeRole)]

        public async Task<IActionResult> Pay(int planId)
        {
            //chech giftcart
            var error = new List<string>();

            var plan = await _dataContext.EmployeePlans.FindAsync(planId);
            if (plan == null)
            {
                error.Add("طرح انتخابی شما یافت نشد.");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, false, null));
            }
            var user = await _accountService.GetCurrectUser();
            if (user == null || !user.IsActive)
            {
                error.Add("کاربر نامعتبر");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.unAuthorize, error, false, null));

            }
            if (user.EmployeePlanId != null)
            {
                error.Add("کاربر گرامی شما در حال حاضر طرح فعال دارید");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, false, null));

            }
            var callbackUrl = Url.Action("Verify", "EmployeePayment", new { plan = planId }, Request.Scheme);
            var price = plan.Price;




            var payment = new EmployeePayment
            {
                Amount = price,
                Date = DateTime.Now,
                EmployeeId = user.Id,
                EmployeePlanId = plan.Id,


            };

            var result = await _onlinePayment.RequestAsync(invoice =>
            {
                invoice
                    .SetTrackingNumber(DateTime.Now.Ticks)
                    .SetAmount(payment.Amount)
                    .SetCallbackUrl(callbackUrl)
                    .SetGateway(Gateways.Parsian.ToString());
            });

            // save result in db
            payment.TrackingNumber = result.TrackingNumber.ToString();

            if (result.IsSucceed)
            {
                payment.ErrorDescription = result.Message;
                _dataContext.Update(payment);
                await _dataContext.SaveChangesAsync();
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, result));

            }
            else
            {
                payment.ErrorDescription = result.Message;
                _dataContext.Update(payment);
                await _dataContext.SaveChangesAsync();
                error.Add("خطای اتصال به درگاه");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true,
                    new
                    {
                        ErrorDescription = payment.ErrorDescription,
                        price = payment.Amount,
                        username = payment.Employee.Fullname,
                        date = payment.Date.ToPersianDateString()
                    }));

            }
        }



        #endregion
        #region Verify
        [AllowAnonymous]
        [HttpPost("EmployeeVerify")]
        public async Task<IActionResult> Verify()
        {
            var error = new List<string>();

            try
            {
                var invoice = await _onlinePayment.FetchAsync();


                var result = await _onlinePayment.VerifyAsync(invoice);
                var payment = _dataContext.EmployeePayments.FirstOrDefault(x => x.TrackingNumber == result.TrackingNumber.ToString());
                var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == payment.EmployeeId);
                if (user == null)
                {
                    error.Add("کاربر نامعتبر");
                    return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.unAuthorize, error, false, null));

                }


                if (result.Amount == payment.Amount && result.IsSucceed == true)
                {
                    user.EmployeePlanId = payment.EmployeePlanId;
                    _dataContext.Users.Update(user);
                    payment.IsSucceed = true;
                    payment.TransactionCode = result.TransactionCode;
                    payment.ErrorDescription = result.Message;
                    _dataContext.Update(payment);


                    var transaction = new EmployeeTransaction()
                    {
                        Amount = payment.Amount,
                        DateTime = DateTime.Now,
                        Information = result.Message,
                        EmployeePaymentId = payment.Id,
                        TransactionCode = result.TransactionCode,

                    };
                    await _dataContext.EmployeeTransactions.AddAsync(transaction);

                    var factor = new EmployeeFactor()
                    {
                        EmployeeId = user.Id,
                        EmployeePlanId = payment.EmployeePlanId,
                        Date = DateTime.Now,
                        Price = payment.Amount,
                        IsBackMOney = false,
                        PaymnetType = (payment.Amount == 0) ? PaymnetType.Gift : PaymnetType.Pay,
                        TrackingCode = result.TransactionCode,
                    };
                    await _dataContext.EmployeeFactors.AddAsync(factor);
                    await _dataContext.SaveChangesAsync();
                    error.Add("پرداخت موفق");
                    error.Add("کد رهگیری : " + payment.TrackingNumber);
                    //return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, true));
                    return Redirect("http://katino.ir/employee/payment/success?trackingnumber=" + payment.TrackingNumber);


                }
                else
                {
                    payment.IsSucceed = false;
                    payment.TransactionCode = result.TransactionCode;
                    payment.ErrorDescription = result.Message;
                    _dataContext.Update(payment);
                    _dataContext.Update(payment);
                    await _dataContext.SaveChangesAsync();

                    error.Add("پرداخت ناموفق");
                    error.Add(payment.ErrorDescription);
                    //return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, false, null));
                    return Redirect("http://katino.ir/employee/payment/failure?trackingnumber=" + payment.TrackingNumber);

                }
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "Verify", "EmployeePayment", User.Identity.Name);

                error.Add("مشکلی رخ داده اس ");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, false, null));

            }
            // save result in db

        }
        #endregion





    }
}
