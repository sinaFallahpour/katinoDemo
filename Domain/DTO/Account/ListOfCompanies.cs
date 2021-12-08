using Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class ListOfCompanies
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string CompanyPersianName { get; set; }
        public string CompanyEngName { get; set; }
        public string ManagementFullName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Url { get; set; }
        public List<string> FiledOfActivity { get; set; }
        public NumberOfStaff NumberOfStaff { get; set; }
        public bool IsActive { get; set; }
        public string CreateDate { get; set; }
        public string Image { get; set; }
        public string Mobile { get; set; }
        public string phoneNumber { get; set; }
        public int Rate { get; set; }
        public string Description { get; set; }
        public string Desc { get; set; }
        public List<string> Gallery { get; set; }
    }
    public class CompanyDetails
    {
        public ListOfCompanies Company { get; set; }
        public List<AllAdver> ActiveAdver { get; set; }
        public List<AllAdver> DeactiveAdver { get; set; }
    }
}
