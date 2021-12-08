using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class UserShortInfoForResome
    {
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        /// <summary>
        /// return resomeId for show info in new page -front
        /// </summary>
        public int KatinoProfile { get; set; }
        public string UserPdf { get; set; }
        public string Avatar { get; set; }
        public string Date { get; set; }
    }
}
