using System;
using System.Collections.Generic;
using System.Text;
using DNTPersianUtils.Core;

namespace Domain
{
    public class AllEmployeeForAdmin
    {
        public string Id { get; set; }
        public string Logo { get; set; }
        public string FullName { get; set; }
        public string PhoneNUmber { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public int AsignCount { get; set; }
        public string IsMarried { get; set; }
        public string Birthday { get; set; }
        public string Military { get; set; }
        public string ExemptionExpirestionDate { get; set; }
        public string ExemptionExpirestionRecieveDate { get; set; }
        public bool IsActive { get; set; }
        public string StaticPhoneNumber { get; set; }
        public string ShebaNumber { get; set; }
        public bool AcceptRule { get; set; }
        public DateTime RegisterationDate { get; set; }
        public string RegisterationDateJalali => RegisterationDate.ToFriendlyPersianDateTextify();
        public string LastSeen { get; set; }
        public string Province { get; set; }
        public string Iframe { get; set; }
        public string Description { get; set; }
        public List<string> Gallery { get; set; }
        public bool IsMain { get; set; }
        public double Percent { get; set; }
    }
}
