using Domain;
using Domain.Entities;
using Domain.Utilities;
using Microsoft.EntityFrameworkCore;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Katino.Cron_Job
{
    public class SendAdvertEmails : ISendAdvertEmails
    {
        private readonly DataContext _context;
        private readonly IMailService _mailService;
        public SendAdvertEmails(DataContext context, IMailService mailService)
        {
            _context = context;
            _mailService = mailService;
        }
        public string SendBasedOnJobSkills()
        {
            var users = _context.Users.Where(x => x.Role == PublicHelper.EmployeeRole && !string.IsNullOrWhiteSpace(x.Email)).Include(x => x.Resome).ThenInclude(x => x.UserJobSkills).ThenInclude(x => x.JobSkill).ThenInclude(x => x.Category).ToList();
            var adverts = _context.JobAdvertisements.AsNoTracking().ToList();
            var list = new List<CronJobVM>();
            foreach (var item in users)
            {
                var vm = item.Resome.UserJobSkills.Select(x => new CronJobVM
                {
                    C = x.JobSkill.Category.Id,
                    User = item
                }).FirstOrDefault();
                list.Add(vm);
            }
            foreach (var item in list)
            {
                var advertsToSend = adverts.Where(x => x.CategoryId == item.C).OrderByDescending(x => x.CreateAt).Take(3).ToList();
                _mailService.SimpleSend(item.User.Email, "", "");
            }
            return "Ok";
        }

        public bool SendEmailNotifications()
        {
            var emailNotificationData = _context.EmailNotifications.AsNoTracking().ToList();
            var advers = _context.JobAdvertisements.ToList();
            foreach (var item in emailNotificationData)
            {
                advers = advers.Where(x => item.CategoryIds.Contains(x.CategoryId.ToString())).ToList();

                if (!string.IsNullOrWhiteSpace(item.KeyWord))
                    advers = advers.Where(x => x.Title.Contains(item.KeyWord)).ToList();

                if (item.TypeOfCooperation != null)
                    advers = advers.Where(x => x.TypeOfCooperation == item.TypeOfCooperation).ToList();

                if (!string.IsNullOrWhiteSpace(item.Cities))
                    advers = advers.Where(x => x.City.Contains(item.Cities)).ToList();

                if(item.EmailNotificationSendTime == Domain.DTO.Response.EmailNotificationSendTime.Weekly)
                {
                    var now = DateTime.Now;
                    var dayOfWeek=  now.DayOfWeek;
                    if(dayOfWeek == DayOfWeek.Saturday)
                    {
                        _mailService.SimpleSend(item.Email, "", "");
                    }
                }
                else
                {
                    _mailService.SimpleSend(item.Email, "", "");
                }
            }
            return true;

        }

        public class CronJobVM
        {
            public User User { get; set; }
            public int C { get; set; }
        }
    }
}
