using Domain;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Katino.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RefrenceRequestController : Controller
    {


        private readonly DataContext _context;
        private readonly IAccountService _accountService;

        public RefrenceRequestController(DataContext context, IAccountService accountService)
        {
            _context = context;
            _accountService = accountService;
        }




        [HttpGet()]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Index()
        {
            return View();
        }



    }
}
