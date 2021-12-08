using Domain.DTO.Response;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class BlogDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public IFormFile Pic { get; set; }
        public BlogType Type { get; set; }
        public string BlogType { get; set; }
    }
    public class EditBlogDTO : BlogDTO
    {
        public int Id { get; set; }
    }
    public class AllBlogsDTO : EditBlogDTO
    {
        public string UpdateDate { get; set; }
        public string UploadPic { get; set; }

    }
}
