using Domain;
using Domain.DTO.Response;
using Domain.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Katino.Areas.Refrence.Controllers
{
    [Area("Refrence")]
    public class PaymentController : Controller
    {
        private readonly DataContext _context;

        public PaymentController(DataContext context)
        {
            _context = context;
        }
        [HttpGet()]
        public async Task<IActionResult> Index()
        {
            ViewBag.UserList = await _context.Users.Where(x => x.IsActive && x.Role != PublicHelper.ADMINROLE && x.Role != PublicHelper.RefrenceRole)
                .Select(x => new UserList { PhoneNumber = x.PhoneNumber, Fullname = x.Fullname }).ToListAsync();

            return View();
        }





        [HttpPost("Withdrawl")]
        //[AllowAnonymous]
        [Authorize(Roles = PublicHelper.ADMINROLE)]
        public async Task<IActionResult> Withdrawl(WithdrawlDTO model)
        {
            var error = new List<string>();
            var refrenceFromDB = await _context.Users.FirstOrDefaultAsync(c => c.Id == model.RefrenceId);
            if (refrenceFromDB == null)
            {
                error.Add("نماینده یافت نشد");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, true, null));
            }
            if (refrenceFromDB.RefrenceTotalPrice < model.Amount)
            {
                error.Add("مبلغ بیش تر از موجودی است");
                return BadRequest(new ResponseResult(Domain.DTO.Response.StatusCode.BadRequest, error, true, null));
            }


            refrenceFromDB.RefrenceTotalPrice = (decimal)(refrenceFromDB.RefrenceTotalPrice - model.Amount);
            await _context.SaveChangesAsync();

            return Ok(new ResponseResult(Domain.DTO.Response.StatusCode.ok, error, true, refrenceFromDB.RefrenceTotalPrice));
        }





        public class UserList
        {
            public string PhoneNumber { get; set; }
            public string Fullname { get; set; }
        }



        public class WithdrawlDTO
        {
            public string RefrenceId { get; set; }

            [Required(ErrorMessage = "{0} الزامیست")]
            public decimal? Amount { get; set; }
        }
    }
}
