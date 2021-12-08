using Domain;
using Domain.DTO;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface ICategoryService
    {
        List<ListOfCategoriesForSelect> GetAllCategories();
        List<ListOfCategories> GetAllCategoriesForAdmin();
        Category GetCategoryById(int id);
        Task<(bool isSuccess, List<string>errors)> AddCategory(AddCategoryDTO model);
        Task<(bool isSuccess, List<string> errors)> UpdateCategory(EditCategoryDTO model);
        Task<(bool isSuccess, List<string> errors)> UpdateCategory(Category model);
        Task<(bool isSuccess, List<string> errors)> DisableCategory(int id);
        Task<(bool isSuccess, List<string> errors)> EnableCategory(int id);
        Task<(bool isSuccess, List<string> errors)> DeleteCategory(Category model);
    }
}
