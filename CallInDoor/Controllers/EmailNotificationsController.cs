using DNTPersianUtils.Core.IranCities;
using Domain;
using Domain.DTO.EmailNotifications;
using Domain.DTO.Response;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace Katino.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    //[ApiController]
    public class EmailNotificationsController : ControllerBase
    {
        private readonly DataContext _context;
        public EmailNotificationsController(DataContext dataContext)
        {
            _context = dataContext;
        }
        [Microsoft.AspNetCore.Mvc.HttpPost("SaveEmailNotificationSetting")]
        [AllowAnonymous]
        public async Task<IActionResult> SaveEmailNotificationSetting(EmailNotificationDTO input)
        {
            var message = new List<string>();
            if (string.IsNullOrWhiteSpace(input.CategoryIds))
            {
                message.Add(" لطفا حداقل 1 دسته بندی شغلی را انتخاب کنید");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, message, false, null));
            }
            var emailNotification = new EmailNotification();
            emailNotification.CategoryIds = input.CategoryIds;

            var cities = "";
            var allCities = Iran.Cities.ToList();
            foreach (var item in input.Cities.Split(","))
            {
                var city = allCities.FirstOrDefault(x => x.CityDivisionCode.ToString() == item);
                cities += city.ProvinceName + "، " + city.CityName;
            }
            emailNotification.Cities = cities;
            emailNotification.Email = input.Email;
            emailNotification.KeyWord = input.KeyWord;
            emailNotification.TypeOfCooperation = input.TypeOfCooperation;
            emailNotification.Date = DateTime.Now;
            emailNotification.ShouldSend = true;
            await _context.EmailNotifications.AddAsync(emailNotification);
            await _context.SaveChangesAsync();
            message.Add("با موفقیت انجام شد");
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, true));
        }
    }
}
