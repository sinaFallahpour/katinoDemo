using Domain;
using Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
   public interface IBlogService
    {
        Task<(bool isSuccess, string error, AllBlogsDTO result)> GetBlogById(int id);
        Task<(bool isSuccess, string error, List<AllBlogsDTO> result)> GetAllBlog();
        Task<(bool isSuccess, string error, List<AllBlogsDTO> result)> GetTop10BlogForIndex(BlogType? type);
        Task<(bool isSuccess, string error)> AddBlog(BlogDTO model);
        Task<(bool isSuccess, string error)> UpdateBlog(EditBlogDTO model);
        Task<(bool isSuccess, string error)> DeleteBlog(int id);
    }
}
