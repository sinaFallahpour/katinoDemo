using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Domain.DTO.Account
{
    public class JwtUser
    {
        public string Id { get; set; }
        public string Token { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }

    }
}
