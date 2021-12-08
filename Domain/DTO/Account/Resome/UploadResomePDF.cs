using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class UploadResomePDF
    {
        //public int AdverId { get; set; }
        public IFormFile PDF { get; set; }
    }
}
