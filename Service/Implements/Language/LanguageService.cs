using DNTPersianUtils.Core;
using Domain;
using Domain.DTO;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class LanguageService : ILanguageService
    {
        private readonly DataContext _Context;
        private readonly IlogService _ilog;

        public LanguageService(DataContext dataContext, IlogService ilog)
        {
            this._Context = dataContext;
            _ilog = ilog;
        }
        public async Task<List<ListOfLanguagesForSelect>> GetAllLanguags()
        {
            var languages = _Context.Languags.Where(x=>x.IsActive).OrderByDescending(x => x.CreateDate)
                .ThenByDescending(x => x.IsActive).Select(x => new ListOfLanguagesForSelect
                {
                    Id = x.Id,
                    Name = x.Name
                }).AsNoTracking().AsQueryable();
            return await languages.ToListAsync();
        }

        public async Task<List<ListOfLanguagesForAdmin>> GetAllLanguagsForAdmin()
        {
            var languages = _Context.Languags.OrderByDescending(x => x.CreateDate)
                .ThenByDescending(x => x.IsActive).Select(x => new ListOfLanguagesForAdmin
                {
                    Id = x.Id,
                    Name = x.Name
                    ,IsActive=x.IsActive,
                    UpdateDate=x.UpdateDate.ToShortPersianDateTimeString(true)
                }).AsNoTracking().AsQueryable();
            return await languages.ToListAsync();
        }

        public async Task<Languag> GetLanguagById(int id)
        {
            return await _Context.Languags.FindAsync(id);
        }
        public async Task<(bool isSuccess, List<string> errors)> AddLanguag(AddLanguageDTO model)
        {
            try
            {
                var languageModel = new Languag()
                {
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Name = model.Name,
                    IsActive=true
                };
                await _Context.Languags.AddAsync(languageModel);
                await _Context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "AddLanguag", "Languag");

                var err = new List<string>();
                err.Add("مشکلی رخ داده است");
                return (false, err);

            }
        }

        public async Task<(bool isSuccess, List<string> errors)> DisableLanguag(int id)
        {
            try
            {
                var disabledLan = _Context.Languags.Find(id);
                disabledLan.IsActive = false;
                disabledLan.UpdateDate = DateTime.Now;
                _Context.Languags.Update(disabledLan);
                await _Context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "DisableLanguag", "Languag");

                var err = new List<string>();
                err.Add("مشکلی رخ داده است");
                return (false, err);
            }
        }
        public async Task<(bool isSuccess, List<string> errors)> EnableLanguag(int id)
        {
            try
            {
                var disabledLan = _Context.Languags.Find(id);
                disabledLan.IsActive = true;
                disabledLan.UpdateDate = DateTime.Now;

                _Context.Languags.Update(disabledLan);
                await _Context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "EnableLanguag", "Languag");

                var err = new List<string>();
                err.Add("مشکلی رخ داده است");
                return (false, err);
            }
        }

        public async Task<(bool isSuccess, List<string> errors)> UpdateLanguag(UpdateLanguageDTO model)
        {
            try
            {
                var editModel = await GetLanguagById(model.Id);
                editModel.UpdateDate = DateTime.Now;
                editModel.Name = model.Name;
                _Context.Languags.Update(editModel);
                await _Context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "UpdateLanguag", "Languag");
                var err = new List<string>();
                err.Add("مشکلی رخ داده است");
                return (false, err);

            }
        }

    }
}
