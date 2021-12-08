using DNTPersianUtils.Core;
using DNTPersianUtils.Core.IranCities;
using Domain;
using Domain.DTO.Response;
using Domain.Migrations;
using Domain.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Operators;
using Service.Interfaces.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class TicketService : ITicketService
    {
        private readonly DataContext _dataContext;
        private readonly IlogService _ilog;
        private readonly IAccountService _accountService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ISendSmsService _sendSmsService;

        public TicketService(DataContext dataContext, IlogService ilog, IAccountService accountService,
            IWebHostEnvironment webHostEnvironment,ISendSmsService sendSmsService)
        {
            _dataContext = dataContext;
            _ilog = ilog;
            _accountService = accountService;
            _webHostEnvironment = webHostEnvironment;
            _sendSmsService = sendSmsService;
        }

        public async Task<(bool isSuccess, string error)> CreateTicket(CreateTiket model)
        {
            var root = _webHostEnvironment.WebRootPath;
            string senderFile = "";
            (bool succsseded, string result) uploadResult;
            if (model.File != null)
            {
                uploadResult = FileUploader.UploadFile(model.File, root + "/Img/Ticket/");
                if (!uploadResult.succsseded)
                {
                    return (false, uploadResult.result);
                }
                else senderFile = uploadResult.result;
            }

            var user = await _accountService.GetCurrectUser();

            if (user == null || !user.IsActive)
            {
                return (false, "کابر نامعتبر");
            }
            try
            {

                if (user.Role == PublicHelper.ADMINROLE)
                {
                    var users = await _dataContext.Users.Where(x => x.IsActive && x.Role != PublicHelper.ADMINROLE).ToListAsync();
                    if (model.UserId[0] == "all")
                    {
                        model.UserId = await _dataContext.Users.Where(x => x.IsActive && x.Role != PublicHelper.ADMINROLE).Select(x => x.Id).ToListAsync();
                    }
                    foreach (var item in model.UserId)
                    {
                        var newTicket = new Ticket()
                        {
                            Content = model.Content,
                            Subject = model.Subject,
                            SenderFile = senderFile,
                            TicketPriorityStatus = model.TicketPriorityStatus,
                            CreateDate = DateTime.Now,
                            SenderId = user.Id,
                            ReceiverId = item,
                            TicketStatus = TicketStatus.AdminSenderNotReply

                        };
                        var receiver = users.FirstOrDefault(x => x.Id == item);
                        await _sendSmsService.SendForNewTicket(receiver.UserName, model.Subject, newTicket.CreateDate.ToPersianDateTextify());
                        var notif = new AdvertismentNotification
                        {
                            CreateDate = DateTime.Now,
                            User = receiver,
                            UserId = receiver.Id,
                            Type = NotificationType.Ticket
                        };
                        await _dataContext.AdvertismentNotifications.AddAsync(notif);
                        await _dataContext.Tickets.AddAsync(newTicket);

                    }
                }

                else if (user.Role == PublicHelper.RefrenceRole)
                {
                    var provinceName = user.City.Split("-")[0];
                    var cityName = user.City.Split("-")[1];
                    var cityCodes = Iran.Cities
                        .Where(x => x.ProvinceName == provinceName && x.CityName == cityName)
                        .Select(x => x.CityDivisionCode.ToString())
                        .ToList();
                    if (model.UserId[0] == "all")
                    {
                        model.UserId = await _dataContext.Users.Where(x => x.IsActive && x.Role != PublicHelper.ADMINROLE && x.Role != PublicHelper.RefrenceRole
                            && cityCodes.IndexOf(x.City) != -1)
                            .Select(x => x.Id).ToListAsync();
                    }
                    foreach (var item in model.UserId)
                    {
                        var newTicket = new Ticket()
                        {
                            Content = model.Content,
                            Subject = model.Subject,
                            SenderFile = senderFile,
                            TicketPriorityStatus = model.TicketPriorityStatus,
                            CreateDate = DateTime.Now,
                            SenderId = user.Id,
                            ReceiverId = item,
                            TicketStatus = TicketStatus.AdminSenderNotReply

                        };
                        await _dataContext.Tickets.AddAsync(newTicket);

                    }
                }
                else
                {
                    Domain.Entities.User admin = new Domain.Entities.User();
                    if (!string.IsNullOrEmpty(model.City))
                    {
                        var cityCode = int.Parse(model.City);
                        var cityName = Iran.Cities.Where(x => x.CityDivisionCode == cityCode)
                            .Select(x => x.ProvinceName + "-" + x.CountyName).FirstOrDefault();
                        if (!string.IsNullOrEmpty((cityName)))
                            admin = _dataContext.Users.FirstOrDefault(x => x.City == cityName);
                    }

                    admin = await _dataContext.Users.FirstOrDefaultAsync(x => x.Role == "Admin");

                        var newTicket = new Ticket()
                    {
                        Content = model.Content,
                        Subject = model.Subject,
                        SenderFile = senderFile,
                        TicketPriorityStatus = model.TicketPriorityStatus,
                        CreateDate = DateTime.Now,
                        SenderId = user.Id,
                        ReceiverId = admin.Id,
                        TicketStatus = TicketStatus.UserSenderNotReply
                            

                    };
                    await _dataContext.Tickets.AddAsync(newTicket);


                }
                await _dataContext.SaveChangesAsync();

                return (true, "با موفقیت ثبت شده است");

            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "CreateTicket", "Ticket");

                return (false, "مشکلی رخ داده است");

            }
        }


        public async Task<(bool isSuccess, string error)> SenderSeenTicket(List<int> ids)
        {
            var user = await _accountService.GetCurrectUser();
            if (user == null || !user.IsActive)
            {
                return (false, "کابر نامعتبر");
            }

            try
            {
                foreach (var id in ids)
                {
                    var ticket = await _dataContext.Tickets.FindAsync(id);
                    if (ticket.SenderId != user.Id) return (false, "کابر نامعتبر");

                    ticket.IsSenderSeen = true;
                    ticket.SenderSeenDate = DateTime.Now;

                    _dataContext.Tickets.Update(ticket);
                }
                await _dataContext.SaveChangesAsync();

                return (true, "با موفقیت ثبت شده است");

            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "SenderSeenTicket", "Ticket");

                return (false, "مشکلی رخ داده است");

            }
        }

        public async Task<(bool isSuccess, string error)> ReceiverSeenTicket(List<int> ids)
        {

            var user = await _accountService.GetCurrectUser();
            if (user == null || !user.IsActive)
            {
                return (false, "کابر نامعتبر");
            }

            try
            {
                foreach (var id in ids)
                {
                    var ticket = await _dataContext.Tickets.FindAsync(id);
                    if (ticket.ReceiverId != user.Id) return (false, "کابر نامعتبر");

                    ticket.IsReciverSeen = true;
                    ticket.ReceiverSeenDate = DateTime.Now;

                    _dataContext.Tickets.Update(ticket);
                }
                await _dataContext.SaveChangesAsync();

                return (true, "با موفقیت ثبت شده است");

            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "ReceiverSeenTicket", "Ticket");

                return (false, "مشکلی رخ داده است");

            }
        }


        public async Task<(bool isSuccess, string error)> AnswerTicket(TicketAdminAnswer model)
        {
            var root = _webHostEnvironment.WebRootPath;
            string answerFile = "";
            (bool succsseded, string result) uploadResult;
            if (model.AnswerFile != null)
            {
                uploadResult = FileUploader.UploadFile(model.AnswerFile, root + "/Img/Ticket/");
                if (!uploadResult.succsseded)
                {
                    return (false, uploadResult.result);
                }
                else answerFile = uploadResult.result;
            }

            var user = await _accountService.GetCurrectUser();
            if (user == null || !user.IsActive)
            {
                return (false, "کابر نامعتبر");
            }
            try
            {
                var ticket = await _dataContext.Tickets.FindAsync(model.Id);
                if (user.Role == PublicHelper.ADMINROLE || user.Role == PublicHelper.RefrenceRole)
                {
                    ticket.TicketStatus = TicketStatus.AdminSenderReply;

                }
                else
                {
                    if (ticket.ReceiverId != user.Id) return (false, "کابر نامعتبر");
                    ticket.TicketStatus = TicketStatus.UserSenderReply;


                }
                ticket.AnswerDate = DateTime.Now;
                ticket.Answer = model.Answer;
                ticket.ReceiverFile = answerFile;
                ticket.IsReciverSeen = true;

                _dataContext.Tickets.Update(ticket);
                await _dataContext.SaveChangesAsync();

                return (true, "با موفقیت ثبت شده است");

            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "AnswerTicket", "Ticket");

                return (false, "مشکلی رخ داده است");

            }
        }


        public async Task<(TicketNotification model, bool isSuccess, string error)> GetNotifTicket(string RefrenceId = "")
        {
            var user = await _accountService.GetCurrectUser();
            if (user == null || !user.IsActive)
            {
                return (null, false, "کابر نامعتبر");
            }
            try
            {
                var tickets = await _dataContext.Tickets
                    .Include(x => x.Sender)
                    .Include(x => x.Receive)
                    .Where(w => (RefrenceId != "") ? (w.Sender.RefrenceId == RefrenceId) : true)
                    .Where(x => (x.SenderId == user.Id && !string.IsNullOrEmpty(x.Answer) && !x.IsSenderSeen) || (x.ReceiverId == user.Id && !x.IsReciverSeen))
                    .OrderByDescending(x => x.IsReciverSeen)
                    .ThenByDescending(x => x.CreateDate)
                    .ThenBy(x => x.TicketPriorityStatus)
                    .Select(x => new GetAllTicketForAdmin()
                    {
                        Id = x.Id,
                        SenderFile = x.SenderFile,
                        TicketPriorityStatus = x.TicketPriorityStatus,
                        Answer = x.Answer,
                        ReceiverFile = x.ReceiverFile,
                        AnswerDate = (x.AnswerDate != null) ? x.AnswerDate.ToShortPersianDateTimeString(true) : "",
                        Content = x.Content,
                        CreateDate = (x.CreateDate != null) ? x.CreateDate.ToShortPersianDateTimeString(true) : "",
                        Subject = x.Subject,
                        IsSenderSeen = x.IsSenderSeen,
                        SenderFullName = (x.Sender.Role == "Admin") ? "ادمین" : x.Sender.Fullname,
                        ReceiverFullName = (x.Receive.Role == "Admin") ? "ادمین" : x.Receive.Fullname,
                        HasAnswer = ((x.SenderId != user.Id) && string.IsNullOrEmpty(x.Answer)) ? true : false
                    }).ToListAsync();
                if (tickets == null)
                {
                    return (null, false, "");

                }
                else
                {
                    var finalModel = new TicketNotification()
                    {
                        Count = tickets.Count,
                        Tickets = tickets
                    };
                    return (finalModel, true, "");

                }


            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "GetNotifTicket", "Ticket");

                return (null, false, "مشکلی رخ داده است");

            }
        }
        public async Task<(List<GetAllTicketForAdmin> model, bool isSuccess, string error)> GetAllTicketForAdmin(string role)
        {
            var user = await _accountService.GetCurrectUser();
            if (user == null || !user.IsActive)
            {
                return (null, false, "کابر نامعتبر");
            }
            try
            {
                var model = _dataContext.Tickets
                    .Include(x => x.Sender)
                    .Include(x => x.Receive)
                    .Where(x => x.Sender.Role == PublicHelper.ADMINROLE || x.Receive.Role == PublicHelper.ADMINROLE)
                    .OrderByDescending(x => x.CreateDate)
                    .Select(x => new GetAllTicketForAdmin()
                    {
                        Id = x.Id,
                        SenderFile = x.SenderFile,
                        TicketPriorityStatus = x.TicketPriorityStatus,
                        Answer = x.Answer,
                        ReceiverFile = x.ReceiverFile,
                        AnswerDate = (x.AnswerDate != null) ? x.AnswerDate.ToShortPersianDateTimeString(true) : "",
                        Content = x.Content,
                        CreateDate = (x.CreateDate != null) ? x.CreateDate.ToShortPersianDateTimeString(true) : "",
                        Subject = x.Subject,
                        IsSenderSeen = x.IsSenderSeen,
                        SenderFullName = (x.Sender.Role == "Admin") ? "ادمین" : x.Sender.Fullname,
                        ReceiverFullName = (x.Receive.Role == "Admin") ? "ادمین" : x.Receive.Fullname,
                        HasAnswer = ((x.SenderId != user.Id) && string.IsNullOrEmpty(x.Answer)) ? true : false,
                        SenderId = (x.SenderId != user.Id) ? x.SenderId : "",
                        RecieverRole = x.Receive.Role,
                        SenderRole = x.Sender.Role
                    }).AsNoTracking().AsQueryable();

                if (!string.IsNullOrEmpty(role))
                {
                    if (role == PublicHelper.EmployeeRole)
                        model = model.Where(x => x.SenderRole == PublicHelper.EmployeeRole || x.RecieverRole == PublicHelper.EmployeeRole);
                    else if (role == PublicHelper.EmployerRole)
                        model = model.Where(x => x.SenderRole == PublicHelper.EmployerRole || x.RecieverRole == PublicHelper.EmployerRole);

                }
                var tickets = await model.ToListAsync();
                if (tickets == null)
                {
                    return (null, false, "");

                }
                else
                {
                    return (tickets, true, "");

                }


            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "GetAllTicketForAdmin", "Ticket");

                return (null, false, "مشکلی رخ داده است");

            }
        }
        public async Task<(List<GetAllTicketForCurrectUser> model, bool isSuccess, string error)> GetAllTicketForUser()
        {
            var user = await _accountService.GetCurrectUser();
            if (user == null || !user.IsActive)
            {
                return (null, false, "کابر نامعتبر");
            }
            try
            {
                var tickets = await _dataContext.Tickets
                    .Where(x => x.SenderId == user.Id || x.ReceiverId == user.Id)
                    .Include(x => x.Sender)
                    .Include(x => x.Receive)
                    .OrderByDescending(x => x.CreateDate)
                    .ThenBy(x => x.TicketPriorityStatus)
                    .Select(x => new GetAllTicketForCurrectUser()
                    {
                        Id = x.Id,
                        TicketPriorityStatus = x.TicketPriorityStatus,
                        Answer = x.Answer,
                        Content = x.Content,
                        CreateDate = (x.CreateDate != null) ? x.CreateDate.ToShortPersianDateTimeString(true) : "",
                        Subject = x.Subject,

                        SenderFullName = (x.Sender.Role == PublicHelper.ADMINROLE) ?
                        "ادمین" : (x.Sender.Role == PublicHelper.RefrenceRole) ?
                        $"نماینده شهر {x.Sender.City}" : x.Sender.Fullname,

                        ReceiverFullName = (x.Receive.Role == PublicHelper.ADMINROLE) ?
                        "ادمین" : (x.Receive.Role == PublicHelper.RefrenceRole) ?
                        $"نماینده شهر {x.Receive.City}" : x.Receive.Fullname,

                        HasAnswer = ((x.SenderId != user.Id) && string.IsNullOrEmpty(x.Answer)) ? true : false
                    }).ToListAsync();
                if (tickets == null)
                {
                    return (null, false, "");

                }
                else
                {
                    return (tickets, true, "");

                }


            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "GetAllTicketForUser", "Ticket");

                return (null, false, "مشکلی رخ داده است");

            }
        }

        public async Task<(TicketDetailsForUser model, bool isSuccess, string error)> GetTicketInfo(int id)
        {
            var user = await _accountService.GetCurrectUser();
            if (user == null || !user.IsActive)
            {
                return (null, false, "کابر نامعتبر");
            }
            try
            {
                var tickets = await _dataContext.Tickets.
                    Where(x => x.Id == id)
                    .Where(x => x.ReceiverId == user.Id || x.SenderId == user.Id)
                    .Select(x => new TicketDetailsForUser()
                    {
                        Id = x.Id,
                        ReceiverFile = "Img/Ticket/" + x.ReceiverFile,
                        TicketPriorityStatus = x.TicketPriorityStatus,
                        Answer = x.Answer,
                        SenderFile = "Img/Ticket/" + x.SenderFile,
                        AnswerDate = (x.AnswerDate != null) ? x.AnswerDate.ToShortPersianDateTimeString(true) : "",
                        Content = x.Content,
                        CreateDate = (x.CreateDate != null) ? x.CreateDate.ToShortPersianDateTimeString(true) : "",
                        Subject = x.Subject,
                        IsSenderSeen = x.IsSenderSeen,
                        SenderFullName = (x.Sender.Role == PublicHelper.ADMINROLE) ?
                        "ادمین" : (x.Sender.Role == PublicHelper.RefrenceRole) ?
                        $"نماینده شهر {x.Sender.City}" : x.Sender.Fullname,
                        ReceiverFullName = (x.Receive.Role == PublicHelper.ADMINROLE) ?
                        "ادمین" : (x.Receive.Role == PublicHelper.RefrenceRole) ?
                        $"نماینده شهر {x.Receive.City}" : x.Receive.Fullname,
                        HasAnswer = ((x.SenderId != user.Id) && string.IsNullOrEmpty(x.Answer)) ? true : false
                    }).FirstOrDefaultAsync();
                if (tickets == null)
                {
                    return (null, false, "");

                }
                else
                {
                    return (tickets, true, "");

                }


            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "GetTicketByIdForUser", "Ticket");

                return (null, false, "مشکلی رخ داده است");

            }
        }

        public async Task<(List<GetAllTicketForAdmin> model, bool isSuccess, string error)> GetAllTicketForRefrence(string role)

        {
            var user = await _accountService.GetCurrectUser();
            if (user == null || !user.IsActive)
            {
                return (null, false, "کابر نامعتبر");
            }
            try
            {
                var model = _dataContext.Tickets
                    //.Where(x => x.SenderId == user.Id || x.ReceiverId == user.Id)
                    .Include(x => x.Sender)
                    .Include(x => x.Receive)
                    .Where(x => x.Sender.City == user.City || x.Receive.City == user.City)
                    .OrderBy(x => x.TicketStatus)
                    .ThenByDescending(x => x.CreateDate)
                    .Select(x => new GetAllTicketForAdmin()
                    {
                        Id = x.Id,
                        SenderFile = x.SenderFile,
                        TicketPriorityStatus = x.TicketPriorityStatus,
                        Answer = x.Answer,
                        ReceiverFile = x.ReceiverFile,
                        AnswerDate = (x.AnswerDate != null) ? x.AnswerDate.ToShortPersianDateTimeString(true) : "",
                        Content = x.Content,
                        CreateDate = (x.CreateDate != null) ? x.CreateDate.ToShortPersianDateTimeString(true) : "",
                        Subject = x.Subject,
                        IsSenderSeen = x.IsSenderSeen,
                        SenderFullName = (x.Sender.Role == PublicHelper.RefrenceRole) ? "ادمین" : x.Sender.Fullname,
                        ReceiverFullName = (x.Receive.Role == PublicHelper.RefrenceRole) ? "ادمین" : x.Receive.Fullname,
                        HasAnswer = ((x.SenderId != user.Id) && string.IsNullOrEmpty(x.Answer)) ? true : false,
                        SenderId = (x.SenderId != user.Id) ? x.SenderId : "",
                        RecieverRole = x.Receive.Role,
                        SenderRole = x.Sender.Role
                    }).AsNoTracking().AsQueryable();

                if (!string.IsNullOrEmpty(role))
                {
                    if (role == PublicHelper.EmployeeRole)
                        model = model.Where(x => x.SenderRole == PublicHelper.EmployeeRole || x.RecieverRole == PublicHelper.EmployeeRole);
                    else if (role == PublicHelper.EmployerRole)
                        model = model.Where(x => x.SenderRole == PublicHelper.EmployerRole || x.RecieverRole == PublicHelper.EmployerRole);

                }
                var tickets = await model.ToListAsync();
                if (tickets == null)
                {
                    return (null, false, "");

                }
                else
                {
                    return (tickets, true, "");

                }


            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "GetAllTicketForRefrence", "Ticket");

                return (null, false, "مشکلی رخ داده است");

            }
        }

    }
}




//Message = "The INSERT statement conflicted with the FOREIGN KEY constraint \"FK_Tickets_AspNetUsers_ReceiverId\". The conflict occurred in database \"KaatinoSina\", table \"dbo.AspNetUsers\", column 'Id'.\r\nThe statement has been terminated."