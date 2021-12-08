using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Katino.Cron_Job
{
    public interface ISendAdvertEmails
    {
        string SendBasedOnJobSkills();

        bool SendEmailNotifications();
    }
}
