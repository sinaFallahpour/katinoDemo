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
    //[Route("api/[controller]")]

    public class PaymentController : ControllerBase
    {
        private readonly IOnlinePayment _onlinePayment;
        private readonly DataContext _dataContext;
        private readonly IAccountService _accountService;
        private readonly IlogService _ilog;
        private readonly IPaymentService _paymentService;
        private readonly IHttpClientFactory _ClientFactory;
        private readonly IGiftCodeService _giftCodeService;

        public PaymentController(IOnlinePayment onlinePayment
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
        [HttpPost("TestPaymentPlan")]
        [ClaimsAuthorize]
        [Authorize(Roles = PublicHelper.EmployeeRole + "," + PublicHelper.EmployerRole)]

        public async Task<IActionResult> TestPaymentPlan(int planId, string giftCode)
        {
            //chech giftcart
            var error = new List<string>();

            var plan = await _dataContext.Plans.FindAsync(planId);
            if (plan == null || !plan.IsActive)
            {
                error.Add("طرح تعرفه مورد نظر یافت نشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.unAuthorize, error, false, null));

            }
            var user = await _accountService.GetCurrectUser();
            if (user == null)
            {
                error.Add("کاربر نامعتبر");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.unAuthorize, error, false, null));

            }
            if (user.PlanId != null)
            {
                error.Add("کاربر گرامی شما در حال حاضر طرح فعال دارید");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, false, null));

            }
            var callbackUrl = Url.Action("Verify", "Payment", new { plan = planId }, Request.Scheme);
            var price = plan.Price - (plan.Price * Convert.ToDecimal(plan.Discount) / 100);
            var discount = plan.Price * Convert.ToDecimal(plan.Discount) / 100;
            var finallyPriceWithTax = (price - discount) + ((price - discount) * PublicHelper.Tax / 100);


            //sum discount in plan and ehrn discount from plan

            var giftCodeResult = await _paymentService.CheckGiftCode(giftCode);

            if (giftCodeResult.isSuccess)
            {
                discount = (price - discount) - ((price - discount) * Convert.ToDecimal(giftCodeResult.discount)) / 100;
                finallyPriceWithTax = (price - discount) + ((price - discount) * PublicHelper.Tax / 100);
            }

            var payment = new Payment
            {
                Amount = price,
                Discount = discount,
                FinallyAmountWithTax = finallyPriceWithTax,
                Date = DateTime.Now,
                UserId = user.Id,
                PlanId = plan.Id,

            };

            var result = await _onlinePayment.RequestAsync(invoice =>
            {
                invoice
                    .SetTrackingNumber(DateTime.Now.Ticks)
                    .SetAmount(payment.FinallyAmountWithTax)
                    .SetCallbackUrl(callbackUrl)
                    .SetGateway(Gateways.ParbadVirtual.ToString());
            });

            // save result in db

            payment.IsSucceed = true;
            payment.TransactionCode = result.TrackingNumber.ToString();
            payment.TrackingNumber = result.TrackingNumber.ToString();
            payment.ErrorDescription = result.Message;
            await _dataContext.Payments.AddAsync(payment);
            await _dataContext.SaveChangesAsync();


            var transaction = new Domain.Transaction()
            {
                FinallyAmountWithTax = result.Amount,
                Amount = payment.Amount,
                DateTime = DateTime.Now,
                Discount = payment.Discount,
                Information = result.Message,
                PaymentId = payment.Id,
                TransactionCode = result.TrackingNumber.ToString()
            };
            await _dataContext.Transactions.AddAsync(transaction);
            await _dataContext.SaveChangesAsync();

            var factor = new Factor()
            {
                CompanyId = user.Id,
                PlanId = planId,
                Date = DateTime.Now,
                Price = payment.FinallyAmountWithTax,
                IsImmediately = false
            };
            await _dataContext.Factors.AddAsync(factor);
            await _dataContext.SaveChangesAsync();

            user.PlanId = planId;
            await _dataContext.SaveChangesAsync();
            try
            {
                await _dataContext.SaveChangesAsync();
                error.Add("پرداخت موفق");
                error.Add("کد رهگیری : " + payment.TrackingNumber);
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, true));
            }
            catch (Exception)
            {

                throw;
            }


        }


        [HttpPost("paymentPlan")]
        [ClaimsAuthorize]
        [Authorize(Roles = PublicHelper.EmployeeRole + "," + PublicHelper.EmployerRole)]
        public async Task<IActionResult> Pay(int planId, string giftCode)
        {

            //chech giftcart
            var error = new List<string>();

            var plan = await _dataContext.Plans.FindAsync(planId);
            if (plan == null)
            {
                error.Add(" طرح انتخابی یافت نشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, false, null));


            }

            var user = await _accountService.GetCurrectUser();
            if (user == null)
            {
                error.Add("کاربر نامعتبر");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.unAuthorize, error, false, null));

            }
            if (user.PlanId != null)
            {
                error.Add("کاربر گرامی شما در حال حاضر طرح فعال دارید");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, false, null));
            }
            var callbackUrl = Url.Action("Verify", "Payment", new { plan = planId }, Request.Scheme);


            // meysam
            var price1 = plan.Price;
            var discountPrice = price1 * Convert.ToDecimal(plan.Discount) / 100;
            var priceWithDiscount = price1 - discountPrice;

            var checkGiftCode = await _paymentService.CheckGiftCode(giftCode);
            decimal giftCodeDiscountPrice = 0;
            if (checkGiftCode.isSuccess)
            {
                giftCodeDiscountPrice = priceWithDiscount * Convert.ToDecimal(checkGiftCode.discount) / 100;
            }
            var priceWithDiscountWithGift = priceWithDiscount - giftCodeDiscountPrice;
            var finalPriceTax = priceWithDiscountWithGift * PublicHelper.Tax / 100;
            var finalPriceWithTaxToIPG = priceWithDiscountWithGift + finalPriceTax;
            // meysam




            //var price = plan.Price - (plan.Price * Convert.ToDecimal(plan.Discount) / 100);
            //var discount = plan.Price * Convert.ToDecimal(plan.Discount) / 100;
            //var finallyPriceWithTax = price + (price * PublicHelper.Tax / 100);


            ////sum discount in plan and ehrn discount from plan

            //var giftCodeResult = await _paymentService.CheckGiftCode(giftCode);

            //if (giftCodeResult.isSuccess)
            //{
            //    discount = price * Convert.ToDecimal(giftCodeResult.discount) / 100;
            //    finallyPriceWithTax = (price - discount) - ((price - discount) * PublicHelper.Tax / 100);
            //}
            //else
            //{

            //    error.Add(giftCodeResult.message);
            //    return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, false, null));
            //}
            var payment = new Payment
            {
                //Amount = price,
                //Discount = discount,
                //FinallyAmountWithTax = finallyPriceWithTax * 10,
                Amount = price1,
                Discount = priceWithDiscountWithGift,
                FinallyAmountWithTax = finalPriceWithTaxToIPG * 10,
                Date = DateTime.Now,
                UserId = user.Id,
                PlanId = plan.Id,
            };
            // if payment.FinallyAmountWithTax < 1 then dargah naro mostaghim verify kon



            if (plan.IsFree)
            {
                var res = user.UsedPlans?.Contains(plan.Id.ToString());
                if (res == true)
                {
                    error.Add("شما از پلن رایگان استفاده کرید");
                    return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.unAuthorize, error, false, null));
                }

                user.UsedPlans = user.UsedPlans + "," + plan.Id;
                //user.UsedPlans()



                //if (user.IsUsedFreePlan)
                //{
                //    error.Add("شما از پلن رایگان استفاده کرید");
                //    return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.unAuthorize, error, false, null));
                //}

                //user.IsUsedFreePlan = true;
                //payment.ErrorDescription = "استفاده از پلن رایگان";
                //payment.er= "استفاده از پلن رایگان";

                _dataContext.Update(payment);
                await _dataContext.SaveChangesAsync();



                //var url = "http://katinojob.ir/employer/payment/success?trackingnumber=" + payment.TrackingNumber;
                var url = "https://katinojob.ir/employer/payment/success?trackingnumber=00000";
                await NewMethod(error, payment, 0, PaymnetType.FreePlan, user, url);

                var response = new
                {
                    url = url,
                };

                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, response));
            }


            if (payment.FinallyAmountWithTax < 1)
            {
                var url = "https://katinojob.ir/employer/payment/success?trackingnumber=00000";
                var paymentType = (payment.Amount == 0) ? PaymnetType.Gift : PaymnetType.Pay;
                await NewMethod(error, payment, payment.Amount, paymentType, user, url);

                var response = new
                {
                    url = url,
                };
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, response));
            }


            var result = await _onlinePayment.RequestAsync(invoice =>
            {
                invoice
                    .SetTrackingNumber(DateTime.Now.Ticks)
                    .SetAmount(payment.FinallyAmountWithTax)
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
                //return result.GatewayTransporter.TransportToGateway();
                //return new JsonResult();
                //error.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, result.GatewayTransporter));
            }
            else
            {
                payment.ErrorDescription = result.Message;
                _dataContext.Update(payment);
                await _dataContext.SaveChangesAsync();
                error.Add("خطا در اتصال به درگاه");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true,
                    new
                    {
                        ErrorDescription = payment.ErrorDescription,
                        price = payment.FinallyAmountWithTax,
                        dicount = payment.Discount,
                        username = payment.User.Fullname,
                        date = payment.Date.ToPersianDateString()
                    }));
            }
        }


        private async Task<IActionResult> NewMethod(List<string> error, Payment payment, decimal price, PaymnetType paymnetType, User user, string url)
        {
            user.PlanId = payment.PlanId;
            _dataContext.Users.Update(user);

            payment.IsSucceed = true;
            payment.TransactionCode = "0";
            payment.ErrorDescription = "خرید پلن رایگان";
            _dataContext.Update(payment);


            var transaction = new Domain.Transaction()
            {
                //FinallyAmountWithTax = result.Amount,
                FinallyAmountWithTax = price,
                Amount = payment.Amount,
                DateTime = DateTime.Now,
                Discount = payment.Discount,
                Information = "خرید پلن رایگان",
                PaymentId = payment.Id,
                TransactionCode = "0"
            };
            await _dataContext.Transactions.AddAsync(transaction);

            var factor = new Factor()
            {
                CompanyId = user.Id,
                PlanId = payment.PlanId,
                Date = DateTime.Now,
                Price = price,
                //Price = payment.FinallyAmountWithTax,
                IsImmediately = false,
                IsBackMOney = false,
                PaymnetType = paymnetType,
                TrackingCode = "0"
            };

            if (user.RefrenceId != null)
            {
                var userFromDB = await _dataContext.Users.Where(c => c.Id == user.RefrenceId).FirstOrDefaultAsync();
                var refrenceTransation = new RefrenceTransation()
                {
                    UserId = user.Id,
                    //Amount = payment.Amount,
                    Amount = price,
                    RefrenceId = user.RefrenceId,
                    Payment = payment,
                };
                userFromDB.RefrenceTotalPrice += payment.Amount;
                await _dataContext.RefrenceTransations.AddAsync(refrenceTransation);
            }

            await _dataContext.Factors.AddAsync(factor);
            await _dataContext.SaveChangesAsync();
            error.Add("پرداخت موفق");
            error.Add("کد رهگیری : " + payment.TrackingNumber);
            //return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, true));
            //return Redirect("http://katinojob.ir/employer/payment/success?trackingnumber=" + payment.TrackingNumber);
            return Redirect(url);
        }




















        [HttpPost("paymentImmediate")]
        [ClaimsAuthorize]
        [Authorize(Roles = PublicHelper.EmployeeRole + "," + PublicHelper.EmployerRole)]
        public async Task<IActionResult> PayImmediate(string giftCode)
        {


            var error = new List<string>();

            var user = await _accountService.GetCurrectUser();
            if (user == null)
            {
                error.Add("کاربر نامعتبر");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.unAuthorize, error, false, null));

            }

            var callbackUrl = Url.Action("Verify", "Payment", new { plan = 0 }, Request.Scheme);
            var pricewithDiscount = PublicHelper.ImmediateAdverPrice
                * Convert.ToDecimal(PublicHelper.ImmediateAdverPriceDiscount) / 100;
            var giftCodeResult = await _paymentService.CheckGiftCode(giftCode);

            if (giftCodeResult.isSuccess)
            {
                pricewithDiscount = (PublicHelper.ImmediateAdverPrice - pricewithDiscount) * Convert.ToDecimal(giftCodeResult.discount) / 100;
                var usedGiftCode = await _dataContext.GiftCarts.FirstOrDefaultAsync(x => x.GiftCode == giftCode);
                if (string.IsNullOrEmpty(usedGiftCode.EmployerId))
                {

                }
                else
                {
                    usedGiftCode.IsUse = true;
                    usedGiftCode.UseAt = DateTime.Now;
                    _dataContext.GiftCarts.Update(usedGiftCode);
                    await _dataContext.SaveChangesAsync();

                }
            }
            //else
            //{

            //    error.Add(giftCodeResult.message);
            //    return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, false, null));
            //}
            var payment = new Payment
            {
                Amount = PublicHelper.ImmediateAdverPrice * 10,
                Discount = pricewithDiscount,
                FinallyAmountWithTax = ((PublicHelper.ImmediateAdverPrice - pricewithDiscount) -
                (PublicHelper.ImmediateAdverPrice - pricewithDiscount) * PublicHelper.Tax / 100) * 10,
                Date = DateTime.Now,
                UserId = user.Id,
                PlanId = null,
                IsImmeditely = true

            };

            var result = await _onlinePayment.RequestAsync(invoice =>
            {
                invoice
                    .SetTrackingNumber(DateTime.Now.Ticks)
                    .SetAmount(payment.FinallyAmountWithTax)
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
                //return result.GatewayTransporter.TransportToGateway();
                //error.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, result));

            }
            else
            {
                payment.ErrorDescription = result.Message;
                _dataContext.Update(payment);
                await _dataContext.SaveChangesAsync();
                error.Add("return to result page ");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true,
                    new
                    {
                        ErrorDescription = payment.ErrorDescription,
                        price = payment.FinallyAmountWithTax,
                        dicount = payment.Discount,
                        username = payment.User.Fullname,
                        date = payment.Date.ToPersianDateString()
                    }));

            }
        }

        #endregion
        #region Verify
        [AllowAnonymous]
        public async Task<IActionResult> Verify()
        {
            var error = new List<string>();
            //error.Add("okkkk");
            //return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, true));

            try
            {
                var invoice = await _onlinePayment.FetchAsync();


                var result = await _onlinePayment.VerifyAsync(invoice);
                var payment = _dataContext.Payments.FirstOrDefault(x => x.TrackingNumber == result.TrackingNumber.ToString());
                var user = await _dataContext.Users.FirstOrDefaultAsync(x => x.Id == payment.UserId);

                if (user == null)
                {
                    error.Add("کاربر نامعتبر");
                    return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.unAuthorize, error, false, null));
                }

                if (payment.IsImmeditely)
                {
                    //var plan = await _dataContext.Plans.FirstOrDefaultAsync(x => x.Id == payment.PlanId);

                    if (result.Amount == payment.FinallyAmountWithTax && result.IsSucceed == true)
                    {

                        payment.IsSucceed = true;
                        payment.TransactionCode = result.TransactionCode;
                        payment.ErrorDescription = result.Message;
                        _dataContext.Update(payment);


                        var transaction = new Domain.Transaction()
                        {
                            FinallyAmountWithTax = result.Amount,
                            Amount = payment.Amount,
                            DateTime = DateTime.Now,
                            Discount = payment.Discount,
                            Information = result.Message,
                            PaymentId = payment.Id,
                            TransactionCode = result.TransactionCode
                        };
                        await _dataContext.Transactions.AddAsync(transaction);

                        var factor = new Factor()
                        {
                            CompanyId = user.Id,
                            PlanId = null,
                            Date = DateTime.Now,
                            Price = payment.FinallyAmountWithTax,
                            IsImmediately = false,
                            IsBackMOney = false,
                            PaymnetType = (payment.Amount == 0) ? PaymnetType.Gift : PaymnetType.Pay,
                            TrackingCode = result.TransactionCode
                        };

                        if (user.RefrenceId != null)
                        {

                            var userFromDB = await _dataContext.Users.Where(c => c.Id == user.RefrenceId).FirstOrDefaultAsync();
                            var refrenceTransation = new RefrenceTransation()
                            {
                                UserId = user.Id,
                                Amount = payment.Amount,
                                RefrenceId = user.RefrenceId,
                                DepositStatus = DepositStatus.Deposit,
                                Payment = payment,
                            };

                            userFromDB.RefrenceTotalPrice += payment.Amount;

                            await _dataContext.RefrenceTransations.AddAsync(refrenceTransation);
                        }

                        await _dataContext.Factors.AddAsync(factor);
                        await _dataContext.SaveChangesAsync();
                        error.Add("پرداخت موفق");
                        error.Add("کد رهگیری : " + payment.TrackingNumber);
                        return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, true));
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
                        return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, false, null));

                    }
                }
                else
                {
                    if (result.Amount == payment.FinallyAmountWithTax && result.IsSucceed == true)
                    {
                        var plan = _dataContext.Plans.FirstOrDefault(x => x.Id == payment.PlanId);
                        user.StorySazCount += plan.StoryCount;
                        user.PlanId = payment.PlanId;
                        _dataContext.Users.Update(user);

                        payment.IsSucceed = true;
                        payment.TransactionCode = result.TransactionCode;
                        payment.ErrorDescription = result.Message;
                        _dataContext.Update(payment);


                        var transaction = new Domain.Transaction()
                        {
                            FinallyAmountWithTax = result.Amount,
                            Amount = payment.Amount,
                            DateTime = DateTime.Now,
                            Discount = payment.Discount,
                            Information = result.Message,
                            PaymentId = payment.Id,
                            TransactionCode = result.TransactionCode
                        };
                        await _dataContext.Transactions.AddAsync(transaction);

                        var factor = new Factor()
                        {
                            CompanyId = user.Id,
                            PlanId = payment.PlanId,
                            Date = DateTime.Now,
                            Price = payment.FinallyAmountWithTax,
                            IsImmediately = false,
                            IsBackMOney = false,
                            PaymnetType = (payment.Amount == 0) ? PaymnetType.Gift : PaymnetType.Pay,
                            TrackingCode = result.TransactionCode
                        };

                        if (user.RefrenceId != null)
                        {
                            var userFromDB = await _dataContext.Users.Where(c => c.Id == user.RefrenceId).FirstOrDefaultAsync();
                            var refrenceTransation = new RefrenceTransation()
                            {
                                UserId = user.Id,
                                Amount = payment.Amount,
                                RefrenceId = user.RefrenceId,
                                Payment = payment,
                            };
                            userFromDB.RefrenceTotalPrice += payment.Amount;
                            await _dataContext.RefrenceTransations.AddAsync(refrenceTransation);
                        }

                        await _dataContext.Factors.AddAsync(factor);
                        await _dataContext.SaveChangesAsync();
                        error.Add("پرداخت موفق");
                        error.Add("کد رهگیری : " + payment.TrackingNumber);
                        //return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, true));
                        return Redirect("http://katinojob.ir/employer/payment/success?trackingnumber=" + payment.TrackingNumber);
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
                        return Redirect("http://katinojob.ir/employer/payment/failure?trackingnumber=" + payment.TrackingNumber);

                        //return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, false, null));

                    }
                }
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "Verify", "Payment", User.Identity.Name);

                error.Add("مشکلی رخ داده است ");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, false, null));

            }
            // save result in db

        }
        #endregion




        #region Verify
        [Authorize(Roles = PublicHelper.EmployerRole)]
        [ClaimsAuthorize]
        [HttpGet("CheckGiftCart")]

        public async Task<IActionResult> CheckGiftCart(string giftCode)
        {
            var error = new List<string>();

            var result = await _paymentService.CheckGiftCode(giftCode);
            if (!result.isSuccess)
            {
                error.Add(result.message);
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, false, null));

            }

            error.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, new { giftDiscount = result.discount }));



        }
        [Authorize(Roles = PublicHelper.ADMINROLE)]
        [ClaimsAuthorize]
        [HttpPost("api/[Controller]/[Action]")]
        public async Task<IActionResult> AdminCreateGiftCode([FromBody] CreateGiftCode model)
        {
            var error = new List<string>();

            var result = await _giftCodeService.CreateGiftCode(model);
            if (!result.isSuccess)
            {
                error.Add(result.message);
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, false, null));

            }

            error.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, true));



        }


        [Authorize(Roles = PublicHelper.ADMINROLE + "," + PublicHelper.RefrenceRole)]
        [ClaimsAuthorize]
        [HttpGet("api/[Controller]/[Action]")]
        public async Task<IActionResult> GetLastOfOrdersForAdmin()
        {
            var error = new List<string>();
            var cu = await _accountService.GetCurrectUser();
            //return new JsonResult(cu);
            var result = cu.Role == PublicHelper.RefrenceRole ?
                await _paymentService.GetLastOfOrdersForAdmin(cu.Id) :
                await _paymentService.GetLastOfOrdersForAdmin();


            //error.Add("با موفقیت انجام شد");
            //return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, true));
            return new JsonResult(result);


        }

        [Authorize(Roles = PublicHelper.ADMINROLE)]
        [ClaimsAuthorize]
        [HttpGet("api/[Controller]/[Action]")]

        public async Task<IActionResult> GetOrderDetailForAdmin(int orderId)
        {
            var error = new List<string>();

            var result = await _paymentService.OrderDetails(orderId);


            error.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, result));



        }




        [Authorize(Roles = PublicHelper.ADMINROLE)]
        [ClaimsAuthorize]
        [HttpPost("api/[Controller]/[Action]")]

        public async Task<IActionResult> GetUserInfoForMoneyBack(string phoneNumber)
        {
            var error = new List<string>();

            var result = await _paymentService.GetUserInfoForMoneyBack(phoneNumber);
            if (result.isSuccess)
            {
                error.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, result.model));
            }
            else
            {
                error.Add(result.message);
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, true, null));
            }

        }




        [Authorize(Roles = PublicHelper.ADMINROLE)]
        [ClaimsAuthorize]
        [HttpPost("api/[Controller]/[Action]")]

        public async Task<IActionResult> MoneyBack([FromBody] MoneyBackDTO model)
        {
            var error = new List<string>();

            var result = await _paymentService.MoneyBack(model);
            if (result.isSuccess)
            {
                error.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, result.isSuccess));
            }
            else
            {
                error.Add(result.message);
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, true, null));
            }

        }
        #endregion
    }
}
