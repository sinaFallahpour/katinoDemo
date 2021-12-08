using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Domain
{
    public class ChnageIndexSetting
    {



        public string KhadamatMa { get; set; }
        public string SharayetAkhzNamayande { get; set; }

        public string Linkedin { get; set; }
        public string Telegram { get; set; }
        public string Instagram { get; set; }
        public string whatsApp { get; set; }
        public IFormFile Logo { get; set; }
        public IFormFile Landing_Img { get; set; }
        public IFormFile? Landing_Banner { get; set; }
        public bool ShoudHaveBanner { get; set; }
        public string Landin_Resome_Title { get; set; }
        public string Landin_Resome_Content { get; set; }
        public string AboutUs { get; set; }
        public string Policy { get; set; }
        public string EmployeeHelper { get; set; }
        public string Onlinepaymentgiude { get; set; }
    }
}
