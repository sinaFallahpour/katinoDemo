using Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class Blog
    {
        public int Id { get; set; }
        public DateTime  CreateDate{ get; set; }
        public DateTime  UpdateDate { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Pic { get; set; }
        public BlogType Type { get; set; }
    }
}
