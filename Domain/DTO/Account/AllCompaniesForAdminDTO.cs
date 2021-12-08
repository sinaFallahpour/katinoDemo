using System;
using System.Collections.Generic;
using System.Text;
using DNTPersianUtils.Core;

namespace Domain
{
    public class AllCompaniesForAdminDTO
    {
        public string Id { get; set; }
        public string Logo { get; set; }
        public string CompanyPersianName { get; set; }
        public string CompanyEngName { get; set; }
        public string ManagerFullName { get; set; }
        public string CompanyPhone { get; set; }
        public string ManagerPhoneNUmber { get; set; }
        public string ManagerEmail { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public int ActiveAdversCount { get; set; }
        public int DeactiveAdversCount { get; set; }
        public string SumOfBuying { get; set; }
        public string Rate { get; set; }
        public bool IsActive { get; set; }
        public string LastSeen { get; set; }
        public string RegisterationDateJalali => RegisterationDate.ToFriendlyPersianDateTextify();
        public int? PlanId { get; set; }
        public DateTime RegisterationDate { get; set; }

    }
}
