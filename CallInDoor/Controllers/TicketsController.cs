using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Domain.DTO;
using Domain.DTO.Response;
using Domain.Utilities;
using Katino.Config.Extentions;
using Katino.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Service;
using Service.Interfaces.Account;

namespace Katino.Controllers
{
    [Route("api/[controller]")]
    [ClaimsAuthorize]
    [Authorize(Roles = PublicHelper.EmployerRole + "," + PublicHelper.EmployeeRole + "," + PublicHelper.ADMINROLE + "," + PublicHelper.RefrenceRole)]

    public class TicketsController : ControllerBase
    {
        private readonly DataContext _dataContext;
        private readonly IAccountService _accountService;
        private readonly ITicketService _ticketService;
        private readonly IHubContext<ChatHub> _hub;

        public TicketsController(DataContext dataContext, IAccountService accountService, ITicketService ticketService, IHubContext<ChatHub> hub)
        {
            _dataContext = dataContext;
            _accountService = accountService;
            _ticketService = ticketService;
            _hub = hub;
        }



        [HttpPost("CreateTicket")]
        public async Task<ActionResult> CreateTicket([FromForm] CreateTiket model)
        {
            var message = new List<string>();
            var result = await _ticketService.CreateTicket(model);
            if (result.isSuccess)
            {
                var user = _dataContext.Users.FirstOrDefault(x => x.Role == PublicHelper.ADMINROLE);
                var currectUser = await _accountService.GetCurrectUser();
                if (currectUser.Role != PublicHelper.ADMINROLE)
                {
                    var msg = currectUser.Fullname + " : " + model.Subject;
                    await _hub.Clients.Client(user.ConecctionId).SendAsync("NOtif", msg);

                }




                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, true));
            }
            else
            {
                message.Add(result.error);
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, false, null));
            }
        }



        [HttpPost("SenderSeenTicket")]
        public async Task<ActionResult> SenderSeenTicket([FromBody] listOfIds model)
        {
            var message = new List<string>();
            var result = await _ticketService.SenderSeenTicket(model.ids);
            if (result.isSuccess)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, true));
            }
            else
            {
                message.Add(result.error);
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, false, null));
            }
        }

        [HttpPost("ReceiverSeenTicket")]
        public async Task<ActionResult> ReceiverSeenTicket([FromBody] listOfIds model)
        {
            var message = new List<string>();
            var result = await _ticketService.ReceiverSeenTicket(model.ids);
            if (result.isSuccess)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, true));
            }
            else
            {
                message.Add(result.error);
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, false, null));
            }
        }



        [HttpPost("AnswerTicket")]
        public async Task<ActionResult> AnswerTicket([FromForm] TicketAdminAnswer model)
        {
            var message = new List<string>();
            var result = await _ticketService.AnswerTicket(model);
            if (result.isSuccess)
            {
                var user = _dataContext.Users.FirstOrDefault(x => x.Role == PublicHelper.ADMINROLE);
                var currectUser = await _accountService.GetCurrectUser();
                if (currectUser.Role != PublicHelper.ADMINROLE)
                {
                    var msg = currectUser.Fullname + " : " + model.Answer;
                    await _hub.Clients.Client(user.ConecctionId).SendAsync("NOtif", msg);

                }

                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, true));
            }
            else
            {
                message.Add(result.error);
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, false, null));
            }
        }









        [HttpGet("GetTicketInfo")]
        public async Task<ActionResult> GetTicketInfo(int id)
        {
            var message = new List<string>();
            var result = await _ticketService.GetTicketInfo(id);
            if (result.isSuccess)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result.model));
            }
            else
            {
                message.Add(result.error);
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, false, null));
            }
        }



        [HttpGet("GetNotifTicket")]
        public async Task<ActionResult> GetNotifTicket()
        {
            var message = new List<string>();
            var result = await _ticketService.GetNotifTicket();
            if (result.isSuccess)
            {
                message.Add("با موفقیت انجام شد");
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result.model));
            }
            else
            {
                message.Add(result.error);
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, false, null));
            }
        }




        [HttpGet("GetAllTicketForAdmin")]
        [AllowAnonymous]
        [Authorize(Roles = PublicHelper.ADMINROLE + "," + PublicHelper.RefrenceRole)]
        [ClaimsAuthorize]
        public async Task<ActionResult> GetAllTicketForAdmin(string role)
        {
            var message = new List<string>();

            var cu = await _accountService.GetCurrectUser();
            var result = cu.Role == PublicHelper.RefrenceRole ?
                //await _ticketService.GetAllTicketForRefrence(role) :
                await _ticketService.GetAllTicketForAdmin(role) :
                await _ticketService.GetAllTicketForAdmin(role);

            if (result.isSuccess)
            {
                message.Add("با موفقیت انجام شد");
                return new JsonResult(result.model);
                //return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result.model));
            }
            else
            {
                message.Add(result.error);
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, false, null));
            }
        }


        [HttpGet("GetAllTicketForRefrence")]
        [AllowAnonymous]
        [Authorize(Roles = PublicHelper.ADMINROLE + "," + PublicHelper.RefrenceRole)]
        [ClaimsAuthorize]
        public async Task<ActionResult> GetAllTicketForRefrence(string role)
        {
            var message = new List<string>();


            var result = await _ticketService.GetAllTicketForRefrence (role);

            if (result.isSuccess)
            {
                message.Add("با موفقیت انجام شد");
                return new JsonResult(result.model);
                //return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result.model));
            }
            else
            {
                message.Add(result.error);
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, false, null));
            }
        }
        


        [HttpGet("GetNotifTicketForAdmin")]
        public async Task<ActionResult> GetNotifTicketForAdmin()
        {
            var message = new List<string>();
            var result = await _ticketService.GetNotifTicket();
            if (result.isSuccess)
            {
                message.Add("با موفقیت انجام شد");
                //return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result.model));
                return new JsonResult(result.model.Tickets);

            }
            else
            {
                message.Add(result.error);
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, false, null));
            }
        }


        [HttpGet("GetNotifTicketCountForAdmin")]
        public async Task<ActionResult> GetNotifTicketCountForAdmin()
        {
            var message = new List<string>();

            var cu = await _accountService.GetCurrectUser();
            var result = cu.Role == PublicHelper.RefrenceRole ?
                await _ticketService.GetNotifTicket(cu.Id) :
                await _ticketService.GetNotifTicket();

            if (result.isSuccess)
            {
                message.Add("با موفقیت انجام شد");
                //return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result.model));
                return new JsonResult(result.model.Count);

            }
            else
            {
                message.Add(result.error);
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, false, null));
            }
        }


        [HttpGet("GetAllTicketForUser")]
        [AllowAnonymous]
        [Authorize(Roles = PublicHelper.EmployerRole + "," + PublicHelper.EmployeeRole)]
        [ClaimsAuthorize]
        public async Task<ActionResult> GetAllTicketForUser()
        {
            var message = new List<string>();
            var result = await _ticketService.GetAllTicketForUser();
            if (result.isSuccess)
            {
                message.Add("با موفقیت انجام شد");
                //return new JsonResult(result.model);
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, result.model));
            }
            else
            {
                message.Add(result.error);
                return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, false, null));
            }
        }
        [HttpGet("GetUnSeenTicketCount")]
        [Authorize(Roles = PublicHelper.EmployerRole + "," + PublicHelper.EmployeeRole)]
        [ClaimsAuthorize]
        public async Task<IActionResult> GetUnSeenTicketCount()
        {
            var message = new List<string>();
            message.Add("با موفقیت انجام شد");
            var user = await _accountService.GetCurrectUser();
            var tickets = await _dataContext.Tickets.Where(x => x.ReceiverId == user.Id && x.IsReciverSeen == false).CountAsync();
            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, message, true, tickets));
        }

    }

    public class listOfIds
    {
        public List<int> ids { get; set; }
    }
}
