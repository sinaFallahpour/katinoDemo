using Domain;
using Domain.DTO.Response;
using Microsoft.EntityFrameworkCore;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Katino.Config.Extentions
{
    public class HangfireUpdateJobAdvertisment : IHangfireUpdateJobAdvertisment
    {
        private readonly DataContext _dataContext;
        private readonly IlogService _ilog;

        public HangfireUpdateJobAdvertisment(DataContext dataContext,IlogService ilog)
        {
            _dataContext = dataContext;
            _ilog = ilog;
        }
        public async  Task CheckExpire()
        {
            try
            {
                //expire
                var expiredAdverList = await _dataContext.JobAdvertisements
                               .Where(x => x.AdverStatus == AdverStatus.Active && x.ExpireTime < DateTime.Now).ToListAsync();

                foreach (var item in expiredAdverList)
                {
                    item.AdverStatus = AdverStatus.Expired;
                }
                _dataContext.JobAdvertisements.UpdateRange(expiredAdverList);


                await _dataContext.SaveChangesAsync();
                Console.WriteLine($"Update Successfully");
            }
            catch (Exception ex)
            {
              await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "CheckExpire", "HangfireUpdateJobAdvertisment");

                Console.WriteLine(ex.Message);

            }
        }
    }
}
