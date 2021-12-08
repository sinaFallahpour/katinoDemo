using Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class ListOfEmployees
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string PersianName { get; set; }
        public string City { get; set; }
        //public List<string> FiledOfActivity { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public string CreateDate { get; set; }
        public string Mobile { get; set; }


    }
}
