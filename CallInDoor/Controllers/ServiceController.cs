//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Domain;
//using Domain.Entities;
//using Domain.DTO.Account;
//using Microsoft.AspNetCore.Authorization;
//using Domain.Utilities;
//using Service.Interfaces.Account;
//using Domain.DTO.Response;
//using Microsoft.Extensions.Localization;
//using Service.Interfaces.ServiceType;

//namespace Katino.Controllers
//{
//    [Route("api/[controller]")]
//    //[ApiController]
//    public class ServiceController : BaseControlle
//    {
//        private readonly DataContext _context;
//        private readonly IAccountService _accountService;
//        private readonly IServiceService _servicetypeService;

//        private IStringLocalizer<ShareResource> _localizerShared;

//        public ServiceController(DataContext context,
//             IStringLocalizer<ShareResource> localizerShared,
//             IAccountService accountService,
//              IServiceService servicetypeService
//            )
//        {
//            _context = context;
//            _accountService = accountService;
//            _localizerShared = localizerShared;
//            _servicetypeService = servicetypeService;
//        }






//        // GET: api/ServiceType
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<ServiceTBL>>> GetServiceTypeTBL()
//        {
//            return await _context.ServiceTBL.ToListAsync();
//        }







//        // GET: api/ServiceType/5
//        [HttpGet("{id}")]
//        public async Task<ActionResult<ServiceTBL>> GetServiceTypeTBL(int id)
//        {
//            var serviceTypeTBL = await _context.ServiceTBL.FindAsync(id);

//            if (serviceTypeTBL == null)
//            {
//                return NotFound();
//            }

//            return serviceTypeTBL;
//        }

//        // PUT: api/ServiceType/5
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for
//        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutServiceTypeTBL(int id, ServiceTBL serviceTypeTBL)
//        {
//            if (id != serviceTypeTBL.Id)
//            {
//                return BadRequest();
//            }

//            _context.Entry(serviceTypeTBL).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!ServiceTypeTBLExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return NoContent();
//        }

//        // POST: api/ServiceType
//        // To protect from overposting attacks, enable the specific properties you want to bind to, for
//        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
//        [HttpPost]
//        public async Task<ActionResult<ServiceTBL>> PostServiceTypeTBL(ServiceTBL serviceTypeTBL)
//        {
//            _context.ServiceTBL.Add(serviceTypeTBL);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction("GetServiceTypeTBL", new { id = serviceTypeTBL.Id }, serviceTypeTBL);
//        }

//        // DELETE: api/ServiceType/5
//        [HttpDelete("{id}")]
//        public async Task<ActionResult<ServiceTBL>> DeleteServiceTypeTBL(int id)
//        {
//            var serviceTypeTBL = await _context.ServiceTBL.FindAsync(id);
//            if (serviceTypeTBL == null)
//            {
//                return NotFound();
//            }

//            _context.ServiceTBL.Remove(serviceTypeTBL);
//            await _context.SaveChangesAsync();

//            return serviceTypeTBL;
//        }

//        private bool ServiceTypeTBLExists(int id)
//        {
//            return _context.ServiceTBL.Any(e => e.Id == id);
//        }
//    }
//}
