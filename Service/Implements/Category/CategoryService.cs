using DNTPersianUtils.Core;
using Domain;
using Domain.DTO;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext _Context;
        private readonly IlogService _ilog;

        public CategoryService(DataContext dataContext, IlogService ilog)
        {
            this._Context = dataContext;
            _ilog = ilog;
        }
        List<ListOfCategoriesForSelect> ICategoryService.GetAllCategories()
        {
            return _Context.Categories.Where(x => x.IsActive).OrderBy(x => x.Name).Select(x => new ListOfCategoriesForSelect
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
        }

        Category ICategoryService.GetCategoryById(int id)
        {
            return _Context.Categories.Include(x => x.Parent).FirstOrDefault(x => x.Id == id);
        }
        async Task<(bool isSuccess, List<string> errors)> ICategoryService.AddCategory(AddCategoryDTO model)
        {
            try
            {
                var parent = await _Context.Categories.FindAsync(model.ParentId);
                var newCat = new Category()
                {
                    Name = model.Name,
                    CreateAt = DateTime.Now,
                    UpdateAt = DateTime.Now

                };
                if (parent != null)
                {
                    newCat.Parent = parent;
                    newCat.CategoryId = parent.Id;
                }
                newCat.IsActive = true;
                await _Context.Categories.AddAsync(newCat);
                await _Context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "AddCategory", "Category");

                var err = new List<string>();
                err.Add("مشکلی رخ داده است");
                return (false, err);

            }
        }

        async Task<(bool isSuccess, List<string> errors)> ICategoryService.DisableCategory(int id)
        {
            try
            {
                var disabledCat = _Context.Categories.Find(id);
                disabledCat.IsActive = false;
                disabledCat.UpdateAt = DateTime.Now;
                _Context.Categories.Update(disabledCat);
                await _Context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "DisableCategory", "Category");

                var err = new List<string>();
                err.Add("مشکلی رخ داده است");
                return (false, err);
            }
        }
        async Task<(bool isSuccess, List<string> errors)> ICategoryService.EnableCategory(int id)
        {
            try
            {
                var enabledCat = _Context.Categories.Find(id);
                enabledCat.IsActive = true;
                enabledCat.UpdateAt = DateTime.Now;
                _Context.Categories.Update(enabledCat);
                await _Context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "EnableCategory", "Category");

                var err = new List<string>();
                err.Add("مشکلی رخ داده است");
                return (false, err);
            }
        }
        async Task<(bool isSuccess, List<string> errors)> ICategoryService.DeleteCategory(Category model)
        {
            try
            {
                model.IsActive = false;
                _Context.Categories.Update(model);
                await _Context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "DeleteCategory", "Category");

                var err = new List<string>();
                err.Add("مشکلی رخ داده است");
                return (false, err);

            }
        }
        async Task<(bool isSuccess, List<string> errors)> ICategoryService.UpdateCategory(EditCategoryDTO model)
        {
            try
            {
                var parent = await _Context.Categories.FindAsync(model.ParentId);
                var updatecat = await _Context.Categories.FindAsync(model.Id);
                updatecat.Name = model.Name;
                updatecat.UpdateAt = DateTime.Now;
                if (parent != null)
                {
                    updatecat.Parent = parent;
                    updatecat.CategoryId = parent.Id;
                }
                _Context.Categories.Update(updatecat);
                await _Context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "UpdateCategory", "Category");
                var err = new List<string>();
                err.Add("مشکلی رخ داده است");
                return (false, err);

            }
        }

        public async Task<(bool isSuccess, List<string> errors)> UpdateCategory(Category model)
        {
            try
            {
                _Context.Categories.Update(model);
                await _Context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "UpdateCategory", "Category");


                var err = new List<string>();
                err.Add("مشکلی رخ داده است");
                return (false, err);

            }
        }

        public List<ListOfCategories> GetAllCategoriesForAdmin()
        {
            return _Context.Categories.Include(x => x.Parent)
                .OrderByDescending(x=>x.CreateAt)
                .ThenByDescending(x=>x.IsActive).Select(x => new ListOfCategories
            {
                Id = x.Id,
                Name = x.Name,
                ParentName = x.Parent.Name,
                IsActive = x.IsActive,
                UpdateDate=x.UpdateAt.ToShortPersianDateTimeString(true)
            }).OrderBy(x => x.Name)/*.OrderByDescending(x => x.IsActive)*/.ToList();
        }
    }
}
