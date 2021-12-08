using DNTPersianUtils.Core;
using Domain;
using Domain.DTO;
using Domain.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class SettingService : ISettingService
    {
        private readonly DataContext _dataContext;
        private readonly IlogService _ilog;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SettingService(DataContext dataContext, IlogService ilog, IWebHostEnvironment webHostEnvironment)
        {
            _dataContext = dataContext;
            _ilog = ilog;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<(bool isSuccess, string error)> ChnageIndexSetting(ChnageIndexSetting model)
        {
            try
            {
                var root = _webHostEnvironment.WebRootPath;

                var allSetting = _dataContext.Settings.AsNoTracking().AsQueryable();
                foreach (var item in allSetting)
                {
                    if (model.Logo != null)
                    {
                        if (item.Key == "Logo") item.Value = FileUploader.UploadImageHighQuality(model.Logo, root + "/Img/Setting/").result;
                    }
                    if (model.Landing_Img != null)
                    {
                        if (item.Key == "Landing_Img") item.Value = FileUploader.UploadImageHighQuality(model.Landing_Img, root + "/Img/Setting/").result;
                    }


                    if (model.Landing_Banner != null)
                    {
                        if (item.Key == "Landing_Banner")
                        {
                            if (model.ShoudHaveBanner == false)
                            {
                                item.Value = "#";
                            }
                            else
                            {
                                item.Value = FileUploader.UploadImageHighQuality(model.Landing_Banner, root + "/Img/Setting/").result;
                            }
                        }
                    }


                    if (item.Key == "Linkedin") item.Value = model.Linkedin;
                    if (item.Key == "telegram") item.Value = model.Telegram;
                    if (item.Key == "Instagram") item.Value = model.Instagram;
                    if (item.Key == "whatsApp") item.Value = model.whatsApp;
                    if (item.Key == "Landin_Resome_Content") item.Value = model.Landin_Resome_Content;
                    if (item.Key == "Landin_Resome_Title") item.Value = model.Landin_Resome_Title;
                    if (item.Key == "KhadamatMa") item.Value = model.KhadamatMa;
                    if (item.Key == "SharayetAkhzNamayande") item.Value = model.SharayetAkhzNamayande;

                    if (item.Key == "AboutUs") item.Value = model.AboutUs;
                    if (item.Key == "onlinepaymentgiude") item.Value = model.Onlinepaymentgiude;
                    if (item.Key == "Policy") item.Value = model.Policy;
                    if (item.Key == "EmployeeHelper") item.Value = model.EmployeeHelper;
                    _dataContext.Settings.Update(item);

                }
                await _dataContext.SaveChangesAsync();

                return (true, "با موفقیت ثبت شده است");

            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "ChnageIndexSetting", "Setting");

                return (false, "مشکلی رخ داده است");

            }
        }

        public async Task<(bool isSuccess, string error)> CreateContactUs(ContactUsDTO model)
        {
            try
            {
                var contactUsMessage = new ContactUsMessage()
                {
                    Comment = model.Comment,
                    CreateAt = DateTime.Now,
                    Email = model.Email,
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber
                };
                await _dataContext.ContactUsMessages.AddAsync(contactUsMessage);
                await _dataContext.SaveChangesAsync();
                return (true, "");

            }
            catch (Exception ex)
            {

                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "CreateContactUs", "Setting");

                return (false, "مشکلی رخ داده است");

            }
        }

        public async Task<(bool isSuccess, string error)> CreateSetting(CreateSettingDTO model)
        {

            try
            {
                var newSetting = new Setting()
                {
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    Key = model.Key,
                    Value = model.Value,
                    Title = model.Title
                };
                await _dataContext.Settings.AddAsync(newSetting);
                await _dataContext.SaveChangesAsync();

                return (true, "با موفقیت ثبت شده است");

            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "CreateSetting", "Setting");

                return (false, "مشکلی رخ داده است");

            }

        }

        public async Task<(bool isSuccess, string error)> DeleteSetting(int id)
        {
            try
            {
                var deletedSetting = await _dataContext.Settings.FindAsync(id);
                _dataContext.Settings.Remove(deletedSetting);
                await _dataContext.SaveChangesAsync();

                return (true, "با موفقیت ثبت شده است");

            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "DeleteSetting", "Setting");

                return (false, "مشکلی رخ داده است");

            }
        }

        public async Task<(bool isSuccess, string error)> EditSetting(EditSettingDTO model)
        {
            try
            {
                var editSetting = await _dataContext.Settings.FindAsync(model.Id);
                editSetting.Key = model.Key;
                editSetting.Value = model.Value;
                editSetting.Title = model.Title;
                editSetting.UpdatedAt = DateTime.Now;
                _dataContext.Settings.Update(editSetting);
                await _dataContext.SaveChangesAsync();

                return (true, "با موفقیت ثبت شده است");

            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "EditSetting", "Setting");

                return (false, "مشکلی رخ داده است");

            }
        }



        public async Task<(bool isSuccess, string error, List<ContactUsDTO> model)> GetAllContactUs()
        {
            try
            {
                var contactUs = await _dataContext.ContactUsMessages
                    .OrderByDescending(x => x.CreateAt)
                    .Select(x => new ContactUsDTO
                    {

                        Comment = x.Comment,
                        Email = x.Email,
                        FullName = x.FullName,
                        Id = x.Id,
                        PhoneNumber = x.PhoneNumber,
                        Date = x.CreateAt.ToFriendlyPersianDateTextify(true, true, true)
                    }).ToListAsync();
                return (true, "", contactUs);
            }
            catch (Exception ex)
            {

                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "GetAllContactUs", "Setting");

                return (false, "مشکلی رخ داده است", null);

            }
        }



        public async Task<(List<EditSettingDTO> model, bool isSuccess, string error)> GetAllInfoForIndex()
        {
            var finalModel = await _dataContext.Settings
                .Where(x => x.Key == "Linkedin" || x.Key == "Twitter" || x.Key == "telegram" || x.Key == "gmail" || x.Key == "MainCompanyAddress" || x.Key == "MainCompanyCompany"
                || x.Key == "Instagram" || x.Key == "whatsapp" ||
                x.Key == "Logo" || x.Key == "Landin_Resome_Content" || x.Key == "Landin_Resome_Title"
                || x.Key == "Landing_Img" || x.Key == "Landing_Banner" || x.Key == "AboutUs" || x.Key == "Policy" || x.Key == "EmployeeHelper"
                || x.Key == "KhadamatMa" || x.Key == "onlinepaymentgiude" || x.Key == "SharayetAkhzNamayande") 
                .Select(x => new EditSettingDTO
                {
                    Id = x.Id,
                    Key = x.Key,
                    Title = x.Title,
                    Value = x.Value
                }).ToListAsync();
            if (finalModel == null) return (null, false, "هیچ اطلاعاتی یافت نشد");
            else return (finalModel, true, "");
        }




        public async Task<(List<EditSettingDTO> model, bool isSuccess, string error)> GetAllSetting()
        {
            var finalModel = await _dataContext.Settings
              .Select(x => new EditSettingDTO
              {
                  Id = x.Id,
                  Key = x.Key,
                  Title = x.Title,
                  Value = x.Value
              }).ToListAsync();
            if (finalModel == null) return (null, false, "هیچ اطلاعاتی یافت نشد");
            else return (finalModel, true, "");
        }

        public async Task<(bool isSuccess, string error, string content)> GetContactUsMessage(int id)
        {
            try
            {
                var contactUs = await _dataContext.ContactUsMessages.FindAsync(id);
                return (true, "", contactUs == null ? "" : contactUs.Comment);
            }
            catch (Exception ex)
            {


                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "GetAllContactUs", "Setting");

                return (false, "مشکلی رخ داده است", null);
            }
        }

        public async Task<(bool isSuccess, string error, string content)> GetEmployeeHelper()
        {
            try
            {
                var employeeHelper = await _dataContext.Settings.FirstOrDefaultAsync(
                    x => x.Key.Equals("EmployeeHelper")
                    );
                return (true, "", employeeHelper == null ? "" : employeeHelper.Value);
            }
            catch (Exception ex)
            {


                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "GetEmployeeHelper", "Setting");

                return (false, "مشکلی رخ داده است", null);
            }
        }

        public async Task<(bool isSuccess, string error, string content)> GetPolicy()
        {
            try
            {
                var policy = await _dataContext.Settings.FirstOrDefaultAsync(
                    x => x.Key.Equals("Policy")
                    );
                return (true, "", policy == null ? "" : policy.Value);
            }
            catch (Exception ex)
            {


                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "GetPolicy", "Setting");

                return (false, "مشکلی رخ داده است", null);
            }
        }
        public async Task<(List<EditSettingDTO> model, bool isSuccess, string error)> GetAllInfoForAbouteUs()
        {
            throw new NotImplementedException();
        }
        public async Task<(bool isSuccess, string error, string content)> GetAboutUs()
        {
            try
            {
                var AboutUs = await _dataContext.Settings.FirstOrDefaultAsync(
                    x => x.Key.Equals("AboutUs")
                    );
                return (true, "", AboutUs == null ? "" : AboutUs.Value);
            }
            catch (Exception ex)
            {


                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "GetAboutUs", "Setting");

                return (false, "مشکلی رخ داده است", null);
            }
        }

        public async Task<(bool isSuccess, string error, string content)> GetKhadamatMa()
        {
            try
            {
                var item = await _dataContext.Settings.FirstOrDefaultAsync(
                    x => x.Key.Equals("KhadamatMa")
                    );
                return (true, "", item == null ? "" : item.Value);
            }
            catch (Exception ex)
            {


                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "GetAboutUs", "Setting");

                return (false, "مشکلی رخ داده است", null);
            }
        }

        public async Task<(bool isSuccess, string error, string content)> GetSharayetAkhzNamayande()
        {
            try
            {
                var item = await _dataContext.Settings.FirstOrDefaultAsync(
                    x => x.Key.Equals("SharayetAkhzNamayande")
                    );
                return (true, "", item == null ? "" : item.Value);
            }
            catch (Exception ex)
            {


                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "GetAboutUs", "Setting");

                return (false, "مشکلی رخ داده است", null);
            }
        }
        //if (item.Key == "") item.Value = model.AboutUs;
        //       if (item.Key == "Policy") item.Value = model.Policy;

        public async Task<(EditSettingDTO model, bool isSuccess, string error)> GetSettingById(int id)
        {
            var finalModel = await _dataContext.Settings
                .Where(x => x.Id == id)
                .Select(x => new EditSettingDTO
                {
                    Id = x.Id,
                    Key = x.Key,
                    Title = x.Title,
                    Value = x.Value
                }).FirstOrDefaultAsync();
            if (finalModel == null) return (null, false, "هیچ اطلاعاتی یافت نشد");
            else return (finalModel, true, "");
        }
    }
}
