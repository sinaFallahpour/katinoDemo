using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Domain;
using Domain.Entities;
using Hangfire.Annotations;
using Domain.Utilities;
using Domain.DTO.Response;
using DNTPersianUtils.Core;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.SignalR;
using Katino.Hubs;
using Katino.Config.Extentions;
using Microsoft.AspNetCore.Authorization;
using Service.Interfaces.Account;
using Service.Interfaces.JwtManager;
using System.Diagnostics;
using Service;
using DNTPersianUtils.Core.IranCities;

namespace Katino.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AccountController : Controller
    {
        private readonly DataContext _context;
        private readonly IHubContext<ChatHub> _hub;
        private readonly IAccountService _accountService;
        private readonly IJwtManager _jwtManager;
        private readonly IPlanService _planService;


        public AccountController(DataContext context, IHubContext<ChatHub> hub, IAccountService accountService, IJwtManager jwtManager, IPlanService planService)
        {
            _context = context;
            _hub = hub;
            _accountService = accountService;
            _jwtManager = jwtManager;
            _planService = planService;
        }
        [HttpGet()]
        [Route("Admin/Login")]
        public async Task<IActionResult> Login()
        {
            return View();
        }




        [HttpGet()]
        [Route("Admin/SiteVisit")]
        public async Task<IActionResult> SiteVisit()
        {
            var items = await _context.SiteVisits.ToListAsync();
            var data = items.GroupBy(g => g.CreatedAt.Date)
                .Select(s => new
                {
                    date = (s.Key.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds),
                    value = s.Count()
                });
            return new JsonResult(data);
        }


        [HttpGet()]
        [Route("Admin/Dashboard")]
        public async Task<IActionResult> Dashboard()
        {

            var ticketCount = await _context.Tickets.CountAsync();
            ViewBag.EmployeeCount = await _context.Users.Where(x => x.Role == PublicHelper.EmployeeRole).CountAsync();
            ViewBag.EmployerCount = await _context.Users.Where(x => x.Role == PublicHelper.EmployerRole).CountAsync();
            ViewBag.TicketCount = ticketCount;
            ViewBag.AdverCount = await _context.JobAdvertisements.CountAsync();
            ViewBag.OrdersCount = await _context.Payments.CountAsync();
            ViewBag.FactorsCount = await _context.Factors.CountAsync();

            if (ticketCount < 1)
            {
                ViewBag.TicketResponsePersent = 0;
                ViewBag.AvaregeOfTicketResponse = 0;
            }
            else
            {
                //for ticket Response
                var allTicketCount = ticketCount;
                var responseTicketCount = await _context.Tickets.Where(x => !string.IsNullOrEmpty(x.Answer) && x.AnswerDate != null).CountAsync();
                double ticketResponsePersent = Convert.ToDouble(responseTicketCount) / Convert.ToDouble(allTicketCount);
                ViewBag.TicketResponsePersent = Convert.ToInt16(ticketResponsePersent * 100);

                var allResponseMinute = await _context.Tickets.Where(x => x.AnswerDate != null).Select(x => (x.AnswerDate.Value - x.CreateDate).TotalHours)
                    .ToListAsync();
                var avaregeOfTicketResponse = allResponseMinute.Sum() == 0 ? 0 : allResponseMinute.Sum() / allResponseMinute.Count();
                ViewBag.AvaregeOfTicketResponse = Convert.ToInt16(avaregeOfTicketResponse);
            }
            return View();
        }
        [HttpGet()]
        [Route("GetDataForCircleChart")]
        [Authorize(Roles = PublicHelper.ADMINROLE)]
        public async Task<IActionResult> GetDataForCircleChart()
        {
            var oneYearsAgo = DateTime.Now.AddYears(-1);

            var finalModel = await (from p in _context.Factors.Include(x => x.Plan)
                                    where p.Date > oneYearsAgo
                                    group p by p.Plan.Title into g
                                    select new BestSellingPlansForCircleChart
                                    {
                                        Name = g.Key,
                                        y = g.Count()
                                    }).OrderByDescending(x => x.y)
                                .Take(5).ToListAsync();

            return new JsonResult(finalModel);
        }

        [HttpGet()]
        [Route("GetDataForLineChart")]
        [Authorize(Roles = PublicHelper.ADMINROLE)]
        public async Task<IActionResult> GetDataForLineChart()
        {
            var oneMonthAgo = DateTime.Now.AddMonths(-1);

            try
            {
                var finalModel = (from p in _context.JobAdvertisements
                                  where p.CreateAt > oneMonthAgo && p.AdverCreatationStatus == AdverCreatationStatus.Accepted
                                  select new BestSellingPlansForLineChart
                                  {
                                      Lable = p.CreateAt.ToShortPersianDateString(true),
                                      Data = p.Id,

                                  }).ToList()
                              .GroupBy(x => x.Lable)
                              .Select(x => new BestSellingPlansForLineChart()
                              {
                                  Lable = x.Key,
                                  Data = x.Count()
                              }).ToList();
                var model = new BestSellingPlansForLine()
                {
                    Data = finalModel.Select(x => x.Data).ToList(),
                    Lable = finalModel.Select(x => x.Lable).ToList(),
                };
                return new JsonResult(model);
            }
            catch (Exception ex)
            {

                return new JsonResult(ex.Message);

            }
        }




        [HttpGet()]
        [Authorize(Roles = PublicHelper.ADMINROLE)]
        [Route("GetDataForLineChartForComin")]
        public async Task<IActionResult> GetDataForLineChartForComin()
        {
            var lastYear = DateTime.Now.AddYears(-1);

            try
            {
                var finalModel = (from p in _context.Factors
                                  where p.Date > lastYear
                                  select new BestSellingPlansForLineChart
                                  {
                                      Lable = p.Date.ToShortPersianDateString(true).Substring(5, 2).GetMonthName(),
                                      Data = Convert.ToInt32(p.Price),

                                  }).ToList()
                              .GroupBy(x => x.Lable)
                              .Select(x => new BestSellingPlansForLineChart()
                              {
                                  Lable = x.Key,
                                  Data = x.ToList().Sum(x => x.Data)
                              }).ToList();
                var model = new BestSellingPlansForLine()
                {
                    Data = finalModel.Select(x => x.Data).ToList(),
                    Lable = finalModel.Select(x => x.Lable).ToList(),
                };
                return new JsonResult(model);
            }
            catch (Exception ex)
            {

                return new JsonResult(ex.Message);

            }
        }

        [HttpGet()]
        [Route("GetDataForColumnChart")]
        [Authorize(Roles = PublicHelper.ADMINROLE)]
        public async Task<IActionResult> GetDataForColumnChart()
        {
            var lastYear = DateTime.Now.AddYears(-1);

            try
            {
                var employeeCount = (from p in _context.Users
                                     where p.RegisterationDate > lastYear && p.Role == PublicHelper.EmployeeRole
                                     select new GetUserCountForChart
                                     {
                                         Lable = p.RegisterationDate.ToShortPersianDateString(true).Substring(5, 2).GetMonthName(),
                                         Data = p.Id,

                                     }).ToList()
                              .GroupBy(x => x.Lable)
                              .Select(x => new BestSellingPlansForLineChart()
                              {
                                  Lable = x.Key,
                                  Data = x.Count()
                              }).ToList();

                var employerCount = (from p in _context.Users
                                     where p.RegisterationDate > lastYear && p.Role == PublicHelper.EmployerRole
                                     select new GetUserCountForChart
                                     {
                                         Lable = p.RegisterationDate.ToShortPersianDateString(true).Substring(5, 2).GetMonthName(),
                                         Data = p.Id,

                                     }).ToList()
                             .GroupBy(x => x.Lable)
                             .Select(x => new BestSellingPlansForLineChart()
                             {
                                 Lable = x.Key,
                                 Data = x.Count()
                             }).ToList();

                var adverCount = (from p in _context.JobAdvertisements
                                  where p.CreateAt > lastYear && p.AdverCreatationStatus == AdverCreatationStatus.Accepted
                                  select new BestSellingPlansForLineChart
                                  {
                                      Lable = p.CreateAt.ToShortPersianDateString(true).Substring(5, 2).GetMonthName(),
                                      Data = p.Id,

                                  }).ToList()
                          .GroupBy(x => x.Lable)
                          .Select(x => new BestSellingPlansForLineChart()
                          {
                              Lable = x.Key,
                              Data = x.Count()
                          }).ToList();


                var model = new InfoCountForColumnChart()
                {
                    Lable = adverCount.Select(x => x.Lable).ToList(),
                    AdverCount = adverCount.Select(x => x.Data).ToList(),
                    EmployeeCount = employeeCount.Select(x => x.Data).ToList(),
                    EmployerCount = employerCount.Select(x => x.Data).ToList(),
                };
                return new JsonResult(model);
            }
            catch (Exception ex)
            {

                return new JsonResult(ex.Message);

            }
        }

        [HttpGet()]
        [Route("Admin/Companies")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Index()
        {
            ViewBag.Plans = await _planService.GetAllPlanForCompany();
            return View();
        }

        [HttpGet()]
        [Route("Admin/Users")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Users()
        {
            return View();
        }


        [HttpGet()]
        [Route("Admin/Agent")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> Agent()
        {
            /////#warning :next line should be delete 
            ViewBag.Plans = await _planService.GetAllPlanForCompany();
            //var cities = Iran.Cities.OrderBy(x => x.ProvinceName).ThenBy(x => x.CountyName).Select(
            //                                x => x.ProvinceName + "-" + x.CountyName
            //                                                    ).ToList();
            //ViewBag.Cities = cities.Distinct().ToList();

            ViewBag.Provinces = Iran.Provinces.OrderBy(x => x.ProvinceName).Select(x => x.ProvinceName).ToList();
            return View();
        }  
        [HttpPost()]
        [Route("Admin/GetAgentById")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> GetAgentById(string id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            return Json( new { user.Description, user.Iframe });
        }
        [HttpGet()]
        [Route("Admin/GetProvinceCities")]
        public IActionResult GetProvinceCities(string provinceName)
        {
            var cities = Iran.Cities.Where(x => x.ProvinceName == provinceName).Select(x => x.CityName).OrderBy(x => x).ToList();
            return new JsonResult(cities);
        }
        [HttpGet()]
        [Route("Admin/Admins")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Admin()
        {
            return View();
        }



        [HttpGet()]
        [Route("GetUserInfoForForceLogin")]
        [ApiExplorerSettings(IgnoreApi = true)]

        [ClaimsAuthorize]
        [Authorize(Roles = PublicHelper.ADMINROLE)]
        public async Task<IActionResult> GetUserInfoForForceLogin(string phoneNumber)
        {
            var user = await _accountService.FindUserByPhonenumber(phoneNumber);
            user.verificationCodeExpireTime = DateTime.Now.AddSeconds(30);
            _context.Users.Update(user);
            await _context.SaveChangesAsync();

            //if(user==null) return new JsonResult()

            return new JsonResult(new { phoneNumber = user.PhoneNumber, verificationCode = user.verificationCode, role = user.Role });
        }

    }
}
