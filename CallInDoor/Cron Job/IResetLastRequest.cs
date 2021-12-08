using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Katino.Helpers.Cron_Job
{
    public interface IResetLastRequest
    {
        Task<int> Reset();
    }
}
