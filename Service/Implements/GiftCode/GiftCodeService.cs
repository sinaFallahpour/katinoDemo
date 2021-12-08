using DNTPersianUtils.Core;
using Domain;
using Domain.Entities;
using Domain.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class GiftCodeService : IGiftCodeService
    {
        private readonly DataContext _dataContext;
        private readonly IlogService _ilog;
        private readonly ISendSmsService _sendSmsService;

        public GiftCodeService(DataContext dataContext, IlogService ilog,ISendSmsService sendSmsService)
        {
            _dataContext = dataContext;
            _ilog = ilog;
            _sendSmsService = sendSmsService;
        }
        public async Task<(bool isSuccess, string message)> CreateGiftCode(CreateGiftCode model)
        {
            if (model.EmployerId == null || model.EmployerId.Count <= 0) return (false, "کابری انتخاب نکرده اید");
            try
            {
                if (model.EmployerId[0] == "all")
                {
                    var users = await _dataContext.Users.Where(x => x.Role == PublicHelper.EmployerRole && x.IsActive).Select(x=> new
                    {
                        x.Id,
                        x.UserName
                    }).ToListAsync();
                    foreach (var item in users)
                    {
                        var newGiftCode = new GiftCart()
                        {
                            CreateAt = DateTime.Now,
                            Discount = model.Discount,
                            EmployerId = item.Id,
                            GiftCode = model.GiftCode,
                            IsUse = false,
                            ExpireTime = DateTime.Now.AddDays(model.ExpireTime)
                        };
                        await _dataContext.GiftCarts.AddAsync(newGiftCode);
                        await _sendSmsService.SendForCreateGiftCode(item.UserName, model.Discount.ToString(), model.ExpireTime.ToString(), model.GiftCode);
                    }
                    await _dataContext.SaveChangesAsync();

                }
                else
                {
                    var users = await _dataContext.Users.Where(x => x.Role == PublicHelper.EmployerRole && x.IsActive).Select(x => new
                    {
                        x.Id,
                        x.UserName
                    }).ToListAsync();
                    foreach (var id in model.EmployerId)
                    {
                        var item = users.FirstOrDefault(x => x.Id == id);
                        var newGiftCode = new GiftCart()
                        {
                            CreateAt = DateTime.Now,
                            Discount = model.Discount,
                            EmployerId = id,
                            GiftCode = model.GiftCode,
                            IsUse = false,
                            ExpireTime = DateTime.Now.AddDays(model.ExpireTime)
                        };
                        await _dataContext.GiftCarts.AddAsync(newGiftCode);
                        await _sendSmsService.SendForCreateGiftCode(item.UserName, model.Discount.ToString(), model.ExpireTime.ToString(), model.GiftCode);
                    }
                    await _dataContext.SaveChangesAsync();
                }
             
                return (true, "");
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "CreateGiftCode", "GiftCode");
                return (false, "خطایی رخ داده است");

            }
        }

        public async Task<(bool isSuccess, string message)> DeleteGiftCode(int Id)
        {
            try
            {
                var giftcarrd = await _dataContext.GiftCarts.FindAsync(Id);

                if (giftcarrd != null)
                {
                    _dataContext.GiftCarts.Remove(giftcarrd);
                    await _dataContext.SaveChangesAsync();
                    return (true, "");
                }
                else
                {
                    return (false, "کد تخفیف انتخابی شما یافت نشد");

                }


            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "DeleteGiftCode", "GiftCode");
                return (false, "خطایی رخ داده است");

            }
        }

        public async Task<(bool isSuccess, string message, List<AllGiftCodeForAdmin> result)> GetAllGiftCodeForAdmin()
        {
            try
            {
                var giftcarrd = await _dataContext.GiftCarts.Include(x => x.Employer)
                    .OrderByDescending(x=>x.CreateAt)
                    .Select(x => new AllGiftCodeForAdmin()
                    {
                        Id = x.Id,
                        CreateDate = x.CreateAt.ToShortPersianDateString(true),
                        Discount = x.Discount,
                        GiftCode = x.GiftCode,
                        UseDate = x.UseAt.ToShortPersianDateString(true),
                        ComoanyName = x.Employer.CompanyPersianName
                    }).AsNoTracking().ToListAsync();

                await _dataContext.SaveChangesAsync();
                return (true, "",giftcarrd);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "GetAllGiftCodeForAdmin", "GiftCode");
                return (false, "خطایی رخ داده است",null);

            }

        }
    }
}
