
using DNTPersianUtils.Core;
using Domain;
using Domain.DTO.Response;
using Domain.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class BlogService : IBlogService
    {
        private readonly IlogService _ilog;
        private readonly DataContext _dataContext;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BlogService(IlogService ilog, DataContext dataContext, IWebHostEnvironment webHostEnvironment)
        {
            _ilog = ilog;
            _dataContext = dataContext;
            _webHostEnvironment = webHostEnvironment;
        }
        public async Task<(bool isSuccess, string error)> AddBlog(BlogDTO model)
        {
            var root = _webHostEnvironment.WebRootPath;

            try
            {
                var uploadResult = FileUploader.UploadFile(model.Pic, root + "/Img/Blog/");
                if (uploadResult.succsseded)
                {

                    var newBlog = new Blog()
                    {
                        UpdateDate = DateTime.Now,
                        CreateDate = DateTime.Now,
                        Content = model.Content,
                        Pic = (model.Pic != null) ? uploadResult.result : "",
                        Title = model.Title,
                        Type = model.Type
                    };
                    await _dataContext.Blogs.AddAsync(newBlog);
                    await _dataContext.SaveChangesAsync();

                    return (true, null);
                }
                else return (false, uploadResult.result);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "AddBlog", "Blog");

                return (false, "مشکلی رخ داده است");

            }
        }

        public async Task<(bool isSuccess, string error)> DeleteBlog(int id)
        {
            var root = _webHostEnvironment.WebRootPath;

            try
            {
                var deletedBlog = await _dataContext.Blogs.FindAsync(id);
                if (!string.IsNullOrEmpty(deletedBlog.Pic)) FileUploader.DeleteFile(root + "/Img/Blog/", deletedBlog.Pic);
                _dataContext.Blogs.Remove(deletedBlog);
                await _dataContext.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "DeleteBlog", "Blog");

              
                return (false, "مشکلی رخ داده است");
            }
        }

        public async Task<(bool isSuccess, string error, List<AllBlogsDTO> result)> GetAllBlog()
        {
            var model =  _dataContext.Blogs.OrderByDescending(x => x.CreateDate).AsNoTracking()
                .Select(x => new AllBlogsDTO()
                {
                    Content = x.Content,
                    Id = x.Id,
                    UploadPic = x.Pic,
                    Title = x.Title,
                    UpdateDate = x.UpdateDate.ToLongPersianDateString(false),
                    Pic=null,
                    BlogType = x.Type.GetDisplayAttributeFrom(),
                    Type = x.Type
                }).AsQueryable();
            var finalModel = await model.ToListAsync();
            if (finalModel.Count > 0)
            {
                return (true, "", finalModel);
            }
            else return (false, "اطلاعاتی یافت نشد",null);
        }

        public async Task<(bool isSuccess, string error, AllBlogsDTO result)> GetBlogById(int id)
        {
            var finalModel =await _dataContext.Blogs.OrderByDescending(x => x.CreateDate).AsNoTracking()
                .Where(x=>x.Id==id)
                 .Select(x => new AllBlogsDTO()
                 {
                     Content = x.Content,
                     Id = x.Id,
                     UploadPic = x.Pic,
                     Title = x.Title,
                     UpdateDate = x.UpdateDate.ToShortPersianDateTimeString(true),
                     Pic = null,
                     BlogType = x.Type.GetDisplayAttributeFrom(),
                     Type = x.Type
                 }).AsQueryable().FirstOrDefaultAsync();
            if (finalModel!=null)
            {
                return (true, "", finalModel);
            }
            else return (false, "اطلاعاتی یافت نشد", null);
        }

        public async Task<(bool isSuccess, string error, List<AllBlogsDTO> result)> GetTop10BlogForIndex(BlogType? type)
        {
            var query = _dataContext.Blogs.AsQueryable();
            if(type != null)
            {
                query = query.Where(x => x.Type == type).AsQueryable();
            }
            var model = query.OrderByDescending(x => x.CreateDate).AsNoTracking()
                .Select(x => new AllBlogsDTO()
                {
                    Content = x.Content,
                    Id = x.Id,
                    UploadPic = x.Pic,
                    Title = x.Title,
                    UpdateDate = x.UpdateDate.ToLongPersianDateString(true),
                    Pic = null,
                    BlogType = x.Type.GetDisplayAttributeFrom(),
                    Type = x.Type
                }).AsQueryable().Take(10);
            var finalModel = await model.ToListAsync();
            if (finalModel.Count > 0)
            {
                return (true, "", finalModel);
            }
            else return (false, "اطلاعاتی یافت نشد", null);
        }

        public async Task<(bool isSuccess, string error)> UpdateBlog(EditBlogDTO model)
        {
            var root = _webHostEnvironment.WebRootPath;

            try
            {
                var editBlog = await _dataContext.Blogs.FindAsync(model.Id);
                var pic = editBlog.Pic;
                if (model.Pic != null)
                {
                    var uploadResult = FileUploader.UploadFile(model.Pic, root + "/Img/Blog/");
                    if (uploadResult.succsseded)
                    {
                        pic = (model.Pic != null)?uploadResult.result : "";
                        if (!string.IsNullOrEmpty(editBlog.Pic)) FileUploader.DeleteFile(root + "/Img/Blog/", editBlog.Pic);

                    }
                    else
                    {
                        return (false, uploadResult.result);
                    }

                }
                editBlog.Pic = pic;
                editBlog.Content = model.Content;
                editBlog.Title = model.Title;
                editBlog.Type = model.Type;
                editBlog.UpdateDate = DateTime.Now;
               
                 _dataContext.Blogs.Update(editBlog);
                await _dataContext.SaveChangesAsync();

                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "UpdateBlog", "Blog");
                return (false, "مشکلی رخ داده است");

            }
        }
    }
}
