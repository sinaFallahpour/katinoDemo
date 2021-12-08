using AutoMapper;
using DNTPersianUtils.Core.IranCities;
using Domain;
using Domain.DTO.Response;
using Domain.Entities;
using Domain.Utilities;
using MailKit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Service.Interfaces.Account;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AdverService : IAdverService
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly IlogService _ilog;
        private readonly IEmailService _emailService;
        private readonly ISendSmsService _sendSmsService;

        public AdverService(DataContext dataContext, IMapper mapper,
            IAccountService accountService, IlogService ilog, IEmailService emailService, ISendSmsService sendSmsService)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _accountService = accountService;
            _ilog = ilog;
            _emailService = emailService;
            _sendSmsService = sendSmsService;
        }
        public async Task<JobAdvertisement> GetAdverById(int id)
        {

            return await _dataContext.JobAdvertisements.Include(x => x.Company).Include(i => i.Category).FirstOrDefaultAsync(u => u.Id == id);

        }
        public async Task<(bool isSuccess, List<string> errors)> AddAdver(AddAdverDTO model)
        {
            try
            {
                var err = new List<string>();


                var company = await _accountService.GetCurrectUser();
                if (company == null)
                {
                    err.Add("کابر نامعتبر");

                    return (false, err);
                }


                var adverCountForThisUser = await _dataContext.JobAdvertisements
                    .Where(x => x.CompanyId == company.Id).CountAsync();
                //firstAdver
                //if (adverCountForThisUser <= 0)
                //{
                //    var firstAdverPlan = await _dataContext.Plans.Where(x => x.Title == "اولین آگهی").FirstOrDefaultAsync();
                //    var adver = _mapper.Map<AddAdverDTO, JobAdvertisement>(model);
                //    adver.Company = company;
                //    adver.CompanyId = company.Id;
                //    adver.ExpireTime = DateTime.Now.AddDays(firstAdverPlan.AdverExpireTime);
                //    adver.Plan = firstAdverPlan;
                //    adver.PlanId = firstAdverPlan.Id;
                //    adver.CreateAt = DateTime.Now;
                //    adver.UpdateAt = DateTime.Now;
                //    adver.AdverStatus = AdverStatus.Active;
                //    //adver.IsActive = false;
                //    adver.AdverCreatationStatus = AdverCreatationStatus.Pending;


                //    //set category

                //    var category = await _dataContext.Categories.FindAsync(int.Parse(model.fieldOfActivity));
                //    if (category != null)
                //    {
                //        adver.Category = category;
                //        adver.CategoryId = category.Id;
                //    }

                //    await _dataContext.JobAdvertisements.AddAsync(adver);
                //    await _dataContext.SaveChangesAsync();
                //    return (true, null);
                //}
                //
                //else
                //{





                //check can create
                //var result = await CanCreateAdver(company, false);
                //if (!result.isSuccess)
                //{
                //    foreach (var er in result.errors)
                //    {
                //        err.Add(er);
                //    }
                //    return (false, err);
                //}
                var userPlan = await _dataContext.Plans.FindAsync(company.PlanId);

                var adver = _mapper.Map<AddAdverDTO, JobAdvertisement>(model);
                adver.Military = string.IsNullOrWhiteSpace(adver.Military) ? "مهم نیست" : adver.Military;
                adver.Company = company;
                adver.CompanyId = company.Id;
                adver.ExpireTime = DateTime.Now.AddDays(2);
                //adver.ExpireTime = DateTime.Now.AddDays(userPlan?.AdverExpireTime);
                adver.Plan = userPlan;
                adver.PlanId = userPlan?.Id;
                adver.CreateAt = DateTime.Now;
                adver.UpdateAt = DateTime.Now;
                adver.AdverStatus = AdverStatus.Active;
                //adver.IsStorySaz = model.StorySaz == "0" ? false : true;
                adver.AdverCreatationStatus = AdverCreatationStatus.Pending;
                adver.Address = model.Address;





                //set category
                var category = await _dataContext.Categories.FindAsync(model.fieldOfActivity);
                if (category != null)
                {
                    adver.Category = category;
                    adver.CategoryId = category.Id;
                }

                await _dataContext.JobAdvertisements.AddAsync(adver);
                await _dataContext.SaveChangesAsync();
                return (true, null);
                //}

            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "AddAdver", "Adver");

                var err = new List<string>();
                err.Add("مشکلی رخ داده است");
                return (false, err);

            }
        }
        public async Task<(bool isSuccess, List<string> errors)> SaveToDraft(AddAdverDTO model)
        {
            var err = new List<string>();
            try
            {
                var company = await _accountService.GetCurrectUser();
                if (company == null)
                {
                    err.Add("کابر نامعتبر");

                    return (false, err);
                }
                var adver = _mapper.Map<AddAdverDTO, JobAdvertisement>(model);
                adver.Company = company;
                adver.CompanyId = company.Id;
                adver.CreateAt = DateTime.Now;
                adver.UpdateAt = DateTime.Now;
                adver.AdverStatus = AdverStatus.Draft;
                adver.AdverCreatationStatus = AdverCreatationStatus.Pending;

                adver.IsActive = false;

                //set category

                var category = await _dataContext.Categories.FindAsync(model.fieldOfActivity);
                if (category != null)
                {
                    adver.Category = category;
                    adver.CategoryId = category.Id;
                }

                await _dataContext.JobAdvertisements.AddAsync(adver);
                await _dataContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().FullName.ToString(), "SaveToDraft", "Adver");

                err.Add("مشکلی رخ داده است");
                return (false, err);

            }
        }
        public async Task<(bool isSuccess, List<string> errors)> AddAdverFromDraft(int adverId)
        {
            try
            {
                var err = new List<string>();


                var company = await _accountService.GetCurrectUser();
                if (company == null)
                {
                    err.Add("کابر نامعتبر");

                    return (false, err);
                }

                var adverCountForThisUser = await _dataContext.JobAdvertisements
                    .Where(x => x.CompanyId == company.Id).CountAsync();
                //firstAdver
                if (adverCountForThisUser <= 0)
                {
                    var firstAdverPlan = await _dataContext.Plans
                        .Where(x => x.Title == "اولین آگهی").FirstOrDefaultAsync();
                    var adver = await GetAdverById(adverId);
                    adver.ExpireTime = DateTime.Now.AddDays(firstAdverPlan.AdverExpireTime);
                    adver.Plan = firstAdverPlan;
                    adver.PlanId = firstAdverPlan.Id;
                    adver.CreateAt = DateTime.Now;
                    adver.UpdateAt = DateTime.Now;
                    adver.AdverStatus = AdverStatus.Active;
                    adver.AdverCreatationStatus = AdverCreatationStatus.Pending;
                    await _dataContext.JobAdvertisements.AddAsync(adver);
                    await _dataContext.SaveChangesAsync();
                    return (true, null);
                }
                //
                else
                {
                    //check can create
                    var result = await CanCreateAdver(company, false);
                    if (!result.isSuccess)
                    {
                        foreach (var er in result.errors)
                        {
                            err.Add(er);
                        }
                        return (false, err);
                    }
                    var userPlan = await _dataContext.Plans.FindAsync(company.PlanId);


                    var adver = await GetAdverById(adverId);
                    if (adver.CompanyId != company.Id)
                    {
                        err.Add("کاربر گرامی شما قابلیت ویرایش این آگهی را ندارید ");
                        return (false, err);

                    }
                    if (adver.AdverStatus != AdverStatus.Draft)
                    {
                        err.Add("کاربر گرامی شما قابلیت ویرایش این آگهی را ندارید ");
                        return (false, err);

                    }
                    if (adver.AdverCreatationStatus != AdverCreatationStatus.Pending)
                    {
                        err.Add("کاربر گرامی شما قابلیت ویرایش این آگهی را ندارید ");
                        return (false, err);

                    }

                    adver.ExpireTime = DateTime.Now.AddDays(userPlan.AdverExpireTime);
                    adver.Plan = userPlan;
                    adver.PlanId = userPlan.Id;
                    adver.CreateAt = DateTime.Now;
                    adver.UpdateAt = DateTime.Now;
                    adver.AdverStatus = AdverStatus.Active;
                    adver.AdverCreatationStatus = AdverCreatationStatus.Pending;
                    //adver.IsActive = false;

                    _dataContext.JobAdvertisements.Update(adver);
                    await _dataContext.SaveChangesAsync();
                    return (true, null);
                }

            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "AddAdverFromDraft", "Adver");

                var err = new List<string>();
                err.Add("مشکلی رخ داده است");
                return (false, err);

            }
        }

        public async Task<(bool isSuccess, List<string> errors)> ArchiveAdver(int id)
        {
            try
            {
                var adver = await _dataContext.JobAdvertisements.FindAsync(id);
                adver.UpdateAt = DateTime.Now;
                adver.AdverStatus = AdverStatus.Archive;

                _dataContext.JobAdvertisements.Update(adver);
                await _dataContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "ArchiveAdver", "Adver");

                var err = new List<string>();
                err.Add("مشکلی رخ داده است");
                return (false, err);

            }
        }
        public async Task<(bool isSuccess, List<string> errors)> FinishedAdver(int id)
        {
            try
            {
                var adver = await _dataContext.JobAdvertisements.FindAsync(id);
                adver.UpdateAt = DateTime.Now;
                adver.AdverStatus = AdverStatus.Finished;

                _dataContext.JobAdvertisements.Update(adver);
                await _dataContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "FinishedAdver", "Adver");

                var err = new List<string>();
                err.Add("مشکلی رخ داده است");
                return (false, err);

            }
        }
        public async Task<(bool isSuccess, List<string> errors)> DisableAdver(int id)
        {
            try
            {
                var adver = await _dataContext.JobAdvertisements.FindAsync(id);
                adver.UpdateAt = DateTime.Now;
                adver.AdverStatus = AdverStatus.Disable;

                _dataContext.JobAdvertisements.Update(adver);
                await _dataContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "DisableAdver", "Adver");

                var err = new List<string>();
                err.Add("مشکلی رخ داده است");
                return (false, err);

            }
        }

        public async Task<(bool isSuccess, List<string> errors)> EnableAdver(int id)
        {
            try
            {
                var adver = await _dataContext.JobAdvertisements.FindAsync(id);
                adver.UpdateAt = DateTime.Now;
                adver.AdverStatus = AdverStatus.Active;
                //Todo
                adver.IsActive = true;

                _dataContext.JobAdvertisements.Update(adver);
                await _dataContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "EnableAdver", "Adver");

                var err = new List<string>();
                err.Add("مشکلی رخ داده است");
                return (false, err);

            }
        }

        public async Task<(bool isSuccess, List<string> errors)> EnableAgainAdver(int id)
        {
            var err = new List<string>();
            try
            {
                var adver = await _dataContext.JobAdvertisements.FindAsync(id);
                if (adver.AdverStatus == AdverStatus.Active)
                {
                    err.Add("آگهی انتخاب شده فعال میباشد");
                    return (false, err);
                }
                if (adver.AdverStatus != AdverStatus.Expired)
                {
                    err.Add("آگهی انتخاب شده قابلیت فعال سازی ندارد");
                    return (false, err);
                }
                adver.UpdateAt = DateTime.Now;
                adver.AdverStatus = AdverStatus.Active;
                adver.IsActive = false;

                adver.ExpireTime = DateTime.Now.AddMonths(1);

                _dataContext.JobAdvertisements.Update(adver);
                await _dataContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "EnableAgainAdver", "Adver");


                err.Add("مشکلی رخ داده است");
                return (false, err);

            }
        }
        public async Task<AdverStatus> GetAdverStatus(int id)
        {
            var adver = await _dataContext.JobAdvertisements.FindAsync(id);
            if (adver != null)
                return adver.AdverStatus;
            else
                return AdverStatus.Expired;
        }
        public async Task<LoadAdverDTO> LoadAdver(int id)
        {
            var adver = await GetAdverById(id);
            var feildOfActivity = adver.Category.Id;
            var finalModel = _mapper.Map<JobAdvertisement, LoadAdverDTO>(adver);
            finalModel.FieldOfActivity = feildOfActivity;

            return finalModel;
        }

        public async Task<(bool isSuccess, List<string> errors)> EditAdver(EditAdverDTO model)
        {
            try
            {
                var lastAdver = await _dataContext.JobAdvertisements.AsNoTracking().FirstOrDefaultAsync(x => x.Id == model.Id);
                var err = new List<string>();

                var company = await _accountService.GetCurrectUser();

                if (company == null)
                {
                    err.Add("کابر نامعتبر");
                    return (false, err);

                }
                if (lastAdver.CompanyId != company.Id)
                {
                    err.Add("کاربر گرامی شما قابلیت ویرایش این آگهی را ندارید ");
                    return (false, err);

                }
                if (lastAdver.AdverCreatationStatus == AdverCreatationStatus.Pending ||
                    lastAdver.AdverCreatationStatus == AdverCreatationStatus.Returned)
                {


                    var adver = _mapper.Map<EditAdverDTO, JobAdvertisement>(model);
                    adver.UpdateAt = DateTime.Now;
                    adver.AdverStatus = lastAdver.AdverStatus;
                    adver.AdverCreatationStatus = AdverCreatationStatus.Pending;
                    //adver.AdminDescription = null;  
                    adver.CompanyId = company.Id;
                    //set category

                    var category = await _dataContext.Categories.FindAsync(model.fieldOfActivity);
                    if (category != null)
                    {
                        adver.Category = category;
                        adver.CategoryId = category.Id;
                    }


                    _dataContext.JobAdvertisements.Update(adver);
                    await _dataContext.SaveChangesAsync();
                    return (true, null);


                }
                else
                {
                    err.Add("کاربر گرامی شما قابلیت ویرایش این آگهی را ندارید ");
                    return (false, err);
                }


            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "EditAdver", "Adver");

                var err = new List<string>();
                err.Add("مشکلی رخ داده است");
                return (false, err);

            }
        }

        public async Task<bool> IsActiveAdver(int id)
        {
            var adver = await GetAdverById(id);
            if (adver != null && adver.AdverStatus == AdverStatus.Active)
                return true;
            else
                return false;
        }



        public async Task<AdverDetail> AdverDetails(int id)
        {
            var user = await _accountService.GetCurrectUser();
            var listOfIds = new List<int>();
            if (user != null)
            {
                listOfIds = await _dataContext.MarkedAdvers.Where(x => x.UserId == user.Id).Select(x => x.AdverId).ToListAsync();

            }
            var adver = await GetAdverById(id);

            adver.Visit++;
            _dataContext.JobAdvertisements.Update(adver);
            await _dataContext.SaveChangesAsync();

            adver.VisitThisAdver++;
            _dataContext.JobAdvertisements.Update(adver);
            await _dataContext.SaveChangesAsync();
            var feildOfActivity = adver.Category.Name;
            var feildOfActivityId = adver.Category.Id;
            var finalModel = _mapper.Map<JobAdvertisement, AdverDetail>(adver);
            finalModel.FeildOfActivity = feildOfActivity;
            finalModel.FeildOfActivityId = feildOfActivityId;
            if (listOfIds.Count > 0)
            {

                finalModel.IsMarked = (listOfIds.IndexOf(finalModel.Id) == -1) ? false : true;

            }
            if (user != null)
            {
                var isUserAsignToThisAdver = await _dataContext.AsignResomes.AnyAsync(x => x.ResomeId == user.ResomeId && x.JobAdvertisementId == adver.Id);
                finalModel.IsAsignResomeToThisAdver = isUserAsignToThisAdver;
            }

            return finalModel;
        }

        public async Task<(bool isSuccess, List<string> errors)> SetAddverImmediate(int adverId)
        {
            var err = new List<string>();
            try
            {
                var adver = await _dataContext.JobAdvertisements.FindAsync(adverId);
                if (adver.AdverStatus != AdverStatus.Active || adver.AdverCreatationStatus != AdverCreatationStatus.Accepted)
                {
                    err.Add("امکان ثبت آگهی فوری وجود ندارد");
                    return (false, err);
                }
                if (adver.IsImmediate)
                {
                    err.Add("این آگهی فوری میباشد");
                    return (false, err);
                }
                var company = await _accountService.GetCurrectUser();

                var result = await CanCreateAdver(company, true);
                if (!result.isSuccess)
                {
                    foreach (var er in result.errors)
                    {
                        err.Add(er);
                    }
                    return (false, err);
                }
                adver.UpdateAt = DateTime.Now;
                adver.IsImmediate = true;

                _dataContext.JobAdvertisements.Update(adver);
                await _dataContext.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "SetAddverImmediate", "Adver");

                err.Add("مشکلی رخ داده است");
                return (false, err);

            }
        }


        public async Task<AllResult<List<AllAdverForAdmin>>> GetAllAdverForAdmin(int page = 0, int pageSize = PublicHelper.PageSize, string RefrenceId = "")
        {
            if (page == 0)
            {
                page = 1;
            }

            var adver = _dataContext.JobAdvertisements
                .Where(x => x.AdverStatus != AdverStatus.Draft)
                .Include(c => c.Company)
                .Include(x => x.Category)
                .Include(i => i.Company)
                .OrderByDescending(x => x.CreateAt)
                .ThenByDescending(x => x.AdverCreatationStatus)
                .ThenByDescending(x => x.IsImmediate)
                .AsQueryable().AsNoTracking();

            if (RefrenceId != "")
            {
                adver = adver.Where(w => w.Company.RefrenceId == RefrenceId);
            }
            var pageCount = Math.Ceiling(Convert.ToDouble(Convert.ToDouble(adver.Count()) / Convert.ToDouble(pageSize)));
            var adverList = adver
                 .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var finalModel = _mapper.Map<List<JobAdvertisement>, List<AllAdverForAdmin>>(await adverList.ToListAsync());
            return new AllResult<List<AllAdverForAdmin>>()
            {
                PageCount = pageCount,
                ListOfData = finalModel
            };



        }

        public async Task<AllResult<List<AllAdverForAdmin>>> GetAllAdverByFilteringForAdmin(bool? isImmediatley, AdverStatus? adverStatus, AdverCreatationStatus? adverCreatationStatus, int page,int adverCities, int pageSize = PublicHelper.PageSize, string RefrenceId = "")
        {
            if (page == 0)
            {
                page = 1;
            }
            var adver = _dataContext.JobAdvertisements
                .Include(x => x.Category).Include(i => i.Company)
                .OrderByDescending(x => x.CreateAt)
                 .ThenByDescending(x => x.AdverCreatationStatus)
                .ThenByDescending(x => x.IsImmediate)
                .ThenByDescending(x => x.UpdateAt)
                .AsQueryable().AsNoTracking();
            if (RefrenceId != "")
            {
                adver = adver.Where(w => w.Company.RefrenceId == RefrenceId);
            }
            if (isImmediatley != null)
            {
                adver = adver.Where(x => x.IsImmediate == isImmediatley);
            }
            if (adverStatus != null)
            {
                adver = adver.Where(x => x.AdverStatus == adverStatus);
            }
            if (adverCreatationStatus != null)
            {
                adver = adver.Where(x => x.AdverCreatationStatus == adverCreatationStatus);
            }
            if (adverCities != 0)
            {
                var city = Iran.Cities.FirstOrDefault(x => x.CityDivisionCode == adverCities);
                var provinceCity = city.ProvinceName + "، " + city.CityName;
                adver = adver.Where(x => x.City == provinceCity);
            }
            var pageCount = Math.Ceiling(Convert.ToDouble(Convert.ToDouble(adver.Count()) / Convert.ToDouble(pageSize)));
            var adverList = adver
                 .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var finalModel = _mapper.Map<List<JobAdvertisement>, List<AllAdverForAdmin>>(await adverList.ToListAsync());
            return new AllResult<List<AllAdverForAdmin>>()
            {
                PageCount = pageCount,
                ListOfData = finalModel
            };

        }

        public async Task<List<AllAdverForCurrectUser>> GetAllAdverByStatusForCurrectUser(AdverStatus adverStatus)
        {
            var user = await _accountService.GetCurrectUser();
            var listOfIds = new List<int>();
            if (user != null)
            {
                listOfIds = await _dataContext.MarkedAdvers.Where(x => x.UserId == user.Id).Select(x => x.AdverId).ToListAsync();

            }
            var company = await _accountService.GetCurrectUser();
            if (company == null)
            {
                return null;
            }

            var adverList = _dataContext.JobAdvertisements
                .Where(x => x.CompanyId == company.Id)
                .Include(x => x.Category)
                .Include(i => i.Company)
                .Include(o => o.AsignResomes)
                .OrderByDescending(x => x.IsImmediate)
                .ThenByDescending(x => x.UpdateAt).AsQueryable().AsNoTracking();

            if (adverStatus != AdverStatus.AdverCreatationStatusRejected && adverStatus != AdverStatus.BoolIsActive)
            {
                adverList = adverList.Where(x => x.AdverStatus == adverStatus).AsQueryable();
            }
            else if (adverStatus == AdverStatus.AdverCreatationStatusRejected)
            {
                adverList = adverList.Where(x => x.AdverCreatationStatus == AdverCreatationStatus.Rejected).AsQueryable();
            }
            else if (adverStatus == AdverStatus.BoolIsActive)
            {
                adverList = adverList.Where(x => x.IsActive == true).AsQueryable();
            }

            var finalModel = _mapper.Map<List<JobAdvertisement>, List<AllAdverForCurrectUser>>(await adverList.ToListAsync());
            foreach (var item in adverList)
            {
                var asingStatuses = new List<AsignStatusWithCount>();
                foreach (var asing in item.AsignResomes)
                {
                    var asingStatuse = new AsignStatusWithCount();
                    asingStatuse.AsingResomeStatus = asing.AsingResomeStatus;
                    asingStatuse.Count = 1;
                    if (asingStatuses.Any(x => x.AsingResomeStatus == asing.AsingResomeStatus))
                    {
                        var asignedResome = asingStatuses.FirstOrDefault(x => x.AsingResomeStatus == asing.AsingResomeStatus);
                        asignedResome.Count++;
                    }
                    else
                    {
                        asingStatuses.Add(asingStatuse);
                    }

                }
                finalModel.FirstOrDefault(x => x.Id == item.Id).AsignStatusWithCounts = asingStatuses;

            }
            if (listOfIds.Count > 0)
            {
                foreach (var item in finalModel)
                {
                    item.IsMarked = (listOfIds.IndexOf(item.Id) == -1) ? false : true;
                }
            }
            return finalModel;
        }

        public async Task<List<AllAdverForCurrectUser>> SearchAdverForCurrectUser(string key)
        {
            var user = await _accountService.GetCurrectUser();
            var listOfIds = new List<int>();
            if (user != null)
            {
                listOfIds = await _dataContext.MarkedAdvers.Where(x => x.UserId == user.Id).Select(x => x.AdverId).ToListAsync();

            }
            var company = await _accountService.GetCurrectUser();
            if (company == null)
            {
                return null;
            }

            var adverList = _dataContext.JobAdvertisements
                .Where(x => x.CompanyId == company.Id)
                .Include(x => x.Category)
                .Include(i => i.Company)
                .Include(o => o.AsignResomes)
                .OrderByDescending(x => x.IsImmediate)
                .ThenByDescending(x => x.UpdateAt).AsQueryable().AsNoTracking();


            if (!string.IsNullOrEmpty(key))
            {
                adverList = adverList.Where(c => c.Title.Contains(key) || c.DescriptionOfJob.Contains(key));

            }
            var finalModel = _mapper.Map<List<JobAdvertisement>, List<AllAdverForCurrectUser>>(await adverList.ToListAsync());
            foreach (var item in adverList)
            {
                var asingStatuses = new List<AsignStatusWithCount>();
                foreach (var asing in item.AsignResomes)
                {
                    var asingStatuse = new AsignStatusWithCount();
                    asingStatuse.AsingResomeStatus = asing.AsingResomeStatus;
                    asingStatuse.Count = 1;
                    if (asingStatuses.Any(x => x.AsingResomeStatus == asing.AsingResomeStatus))
                    {
                        var asignedResome = asingStatuses.FirstOrDefault(x => x.AsingResomeStatus == asing.AsingResomeStatus);
                        asignedResome.Count++;
                    }
                    else
                    {
                        asingStatuses.Add(asingStatuse);
                    }

                }
                finalModel.FirstOrDefault(x => x.Id == item.Id).AsignStatusWithCounts = asingStatuses;

            }
            if (listOfIds.Count > 0)
            {
                foreach (var item in finalModel)
                {
                    item.IsMarked = (listOfIds.IndexOf(item.Id) == -1) ? false : true;
                }
            }
            return finalModel;
        }

        public async Task<AllResult<List<AllAdver>>> GetAllMarkedAdverForUser(int page = 0, int pageSize = PublicHelper.PageSize)
        {
            var user = await _accountService.GetCurrectUser();
            var listOfIds = new List<int>();
            if (user != null)
            {
                listOfIds = await _dataContext.MarkedAdvers.Where(x => x.UserId == user.Id).Select(x => x.AdverId).ToListAsync();

            }
            if (page == 0)
            {
                page = 1;
            }
            var adver = _dataContext.MarkedAdvers
                .Where(x => x.UserId == user.Id && x.JobAdvertisement.AdverStatus == AdverStatus.Active
                && x.JobAdvertisement.AdverCreatationStatus == AdverCreatationStatus.Accepted)
                .Include(x => x.JobAdvertisement)
                     .ThenInclude(x => x.Company)
                .Include(x => x.JobAdvertisement)
                     .ThenInclude(x => x.Category)
                 .OrderByDescending(x => x.CreatedDate)
                 .ThenByDescending(x => x.JobAdvertisement.IsImmediate)
                 .Select(x => x.JobAdvertisement)
                .AsQueryable().AsNoTracking();

            var pageCount = Math.Ceiling(Convert.ToDouble(Convert.ToDouble(adver.Count()) / Convert.ToDouble(pageSize)));
            var adverList = adver
                 .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var finalModel = _mapper.Map<List<JobAdvertisement>, List<AllAdver>>(await adverList.ToListAsync());
            if (listOfIds.Count > 0)
            {
                foreach (var item in finalModel)
                {
                    item.IsMarked = (listOfIds.IndexOf(item.Id) == -1) ? false : true;
                }
            }
            return new AllResult<List<AllAdver>>()
            {
                PageCount = pageCount,
                ListOfData = finalModel
            };
        }

        public async Task<List<AllAdverForCurrectUser>> GetAllAdverForCurrectUser()
        {
            var company = await _accountService.GetCurrectUser();
            if (company == null)
            {
                return null;
            }

            var adverList = _dataContext.JobAdvertisements
                .Where(x => x.CompanyId == company.Id)
                .Include(x => x.Category)
                .Include(i => i.Company)
                .Include(o => o.AsignResomes)
                .OrderByDescending(x => x.CreateAt)
                .ThenByDescending(x => x.UpdateAt).AsQueryable().AsNoTracking();

            var finalModel = _mapper.Map<List<JobAdvertisement>, List<AllAdverForCurrectUser>>(await adverList.ToListAsync());
            var d = adverList.Where(x => x.IsActive).ToList();
            foreach (var item in adverList)
            {
                var asingStatuses = new List<AsignStatusWithCount>();
                foreach (var asing in item.AsignResomes)
                {
                    var asingStatuse = new AsignStatusWithCount();
                    asingStatuse.AsingResomeStatus = asing.AsingResomeStatus;
                    asingStatuse.Count = 1;
                    if (asingStatuses.Any(x => x.AsingResomeStatus == asing.AsingResomeStatus))
                    {
                        var asignedResome = asingStatuses.FirstOrDefault(x => x.AsingResomeStatus == asing.AsingResomeStatus);
                        asignedResome.Count++;
                    }
                    else
                    {
                        asingStatuses.Add(asingStatuse);
                    }

                }
                finalModel.FirstOrDefault(x => x.Id == item.Id).AsignStatusWithCounts = asingStatuses;

            }
            return finalModel;
        }

        public async Task<AllResult<List<AllAdver>>> GetAllAdverForIndex(int page = 0, int pageSize = PublicHelper.PageSize)
        {
            if (page == 0)
            {
                page = 1;
            }
            var user = await _accountService.GetCurrectUser();
            var listOfIds = new List<int>();
            if (user != null)
            {
                listOfIds = await _dataContext.MarkedAdvers.Where(x => x.UserId == user.Id).Select(x => x.AdverId).ToListAsync();

            }

            var adver = _dataContext.JobAdvertisements
             .Where(x => x.AdverStatus == AdverStatus.Active && x.AdverCreatationStatus == AdverCreatationStatus.Accepted).Include(x => x.Category)
             .Include(i => i.Company)
              .OrderByDescending(x => x.UpdateAt)
              .ThenByDescending(x => x.IsImmediate)

             .AsQueryable().AsNoTracking();

            var pageCount = Math.Ceiling(Convert.ToDouble(Convert.ToDouble(adver.Count()) / Convert.ToDouble(pageSize)));
            var adverList = adver
                 .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var finalModel = _mapper.Map<List<JobAdvertisement>, List<AllAdver>>(await adverList.ToListAsync());

            if (listOfIds.Count > 0)
            {
                foreach (var item in finalModel)
                {
                    item.IsMarked = (listOfIds.IndexOf(item.Id) == -1) ? false : true;
                }
            }

            return new AllResult<List<AllAdver>>()
            {
                PageCount = pageCount,
                ListOfData = finalModel
            };


        }

        public async Task<AllResult<List<AllAdver>>> SearchAdver(string key, string city, int page = 0, int pageSize = PublicHelper.PageSize)
        {
            var user = await _accountService.GetCurrectUser();
            var listOfIds = new List<int>();
            if (user != null)
            {
                listOfIds = await _dataContext.MarkedAdvers.Where(x => x.UserId == user.Id).Select(x => x.AdverId).ToListAsync();

            }
            if (page == 0)
            {
                page = 1;
            }
            var adver = _dataContext.JobAdvertisements
                .Where(x => x.AdverStatus == AdverStatus.Active && x.AdverCreatationStatus == AdverCreatationStatus.Accepted)
                .Include(x => x.Category).Include(i => i.Company)
                .OrderByDescending(x => x.IsImmediate)
                .ThenByDescending(x => x.UpdateAt)
                .AsQueryable().AsNoTracking();

            if (!string.IsNullOrEmpty(city))
            {
                adver = adver.Where(x => x.City.Contains(city)).AsNoTracking().AsQueryable();
            }
            if (!string.IsNullOrEmpty(key))
            {
                adver = adver.Where(x => x.Title.Contains(key) || x.Category.Name.Contains(key) || x.DescriptionOfJob.Contains(key)

                ).AsNoTracking().AsQueryable();
            }











            var pageCount = Math.Ceiling(Convert.ToDouble(Convert.ToDouble(adver.Count()) / Convert.ToDouble(pageSize)));
            var adverList = adver
                 .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var finalModel = _mapper.Map<List<JobAdvertisement>, List<AllAdver>>(await adverList.ToListAsync());
            if (listOfIds.Count > 0)
            {
                foreach (var item in finalModel)
                {
                    item.IsMarked = (listOfIds.IndexOf(item.Id) == -1) ? false : true;
                }
            }
            return new AllResult<List<AllAdver>>()
            {
                PageCount = pageCount,
                ListOfData = finalModel
            };
        }
        public async Task<AllResult<List<AllAdver>>> GetLastAdversForIndex(int page, int pageSize = 10)
        {
            var user = await _accountService.GetCurrectUser();
            var listOfIds = new List<int>();
            if (user != null)
            {
                listOfIds = await _dataContext.MarkedAdvers.Where(x => x.UserId == user.Id).Select(x => x.AdverId).ToListAsync();

            }
            if (page == 0)
            {
                page = 1;
            }
            var adver = _dataContext.JobAdvertisements
                .Where(x => x.AdverStatus == AdverStatus.Active && x.IsImmediate == false && x.AdverCreatationStatus == AdverCreatationStatus.Accepted)
                .Include(x => x.Category)
                .Include(i => i.Company)
                .OrderByDescending(x => x.UpdateAt)
                .AsQueryable().AsNoTracking();




            var pageCount = Math.Ceiling(Convert.ToDouble(Convert.ToDouble(adver.Count()) / Convert.ToDouble(pageSize)));
            var adverList = adver
                 .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var finalModel = _mapper.Map<List<JobAdvertisement>, List<AllAdver>>(await adverList.ToListAsync());
            if (listOfIds.Count > 0)
            {
                foreach (var item in finalModel)
                {
                    item.IsMarked = (listOfIds.IndexOf(item.Id) == -1) ? false : true;
                }
            }


            return new AllResult<List<AllAdver>>()
            {
                PageCount = pageCount,
                ListOfData = finalModel
            };
        }

        public async Task<AllResult<List<AllAdver>>> GetLastImmediatelyAdversForIndex(int page, int pageSize = 10)
        {
            var user = await _accountService.GetCurrectUser();
            var listOfIds = new List<int>();
            if (user != null)
            {
                listOfIds = await _dataContext.MarkedAdvers.Where(x => x.UserId == user.Id).Select(x => x.AdverId).ToListAsync();

            }
            if (page == 0)
            {
                page = 1;
            }
            var adver = _dataContext.JobAdvertisements
                .Where(x => x.AdverStatus == AdverStatus.Active && x.IsImmediate && x.AdverCreatationStatus == AdverCreatationStatus.Accepted)
                .Include(x => x.Category)
                .Include(i => i.Company)
                .OrderByDescending(x => x.UpdateAt)
                .AsQueryable().AsNoTracking();

            var pageCount = Math.Ceiling(Convert.ToDouble(Convert.ToDouble(adver.Count()) / Convert.ToDouble(pageSize)));
            var adverList = adver
                 .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var finalModel = _mapper.Map<List<JobAdvertisement>, List<AllAdver>>(await adverList.ToListAsync());
            if (listOfIds.Count > 0)
            {
                foreach (var item in finalModel)
                {
                    item.IsMarked = (listOfIds.IndexOf(item.Id) == -1) ? false : true;
                }
            }
            return new AllResult<List<AllAdver>>()
            {
                PageCount = pageCount,
                ListOfData = finalModel
            };

        }
        public async Task<AllResult<List<AllAdver>>> FilterAdver(FilterAdverDTO model, int page = 0, int pageSize = PublicHelper.PageSize)
        {

            var user = await _accountService.GetCurrectUser();
            var listOfIds = new List<int>();
            if (user != null)
            {
                listOfIds = await _dataContext.MarkedAdvers.Where(x => x.UserId == user.Id).Select(x => x.AdverId).ToListAsync();

            }
            if (page == 0)
            {
                page = 1;
            }
            var adver = _dataContext.JobAdvertisements
                 .Where(x => x.AdverStatus == AdverStatus.Active && x.AdverCreatationStatus == AdverCreatationStatus.Accepted)
                 .Include(x => x.Category).ThenInclude(x => x.Parent).Include(i => i.Company)
                 .OrderByDescending(x => x.IsImmediate)
                .ThenByDescending(x => x.UpdateAt)
                 .AsQueryable().AsNoTracking();

            if (!string.IsNullOrEmpty(model.Key))
            {
                adver = adver.Where(x => x.Title.Contains(model.Key) ||
                x.DescriptionOfJob.Contains(model.Key)
                || x.City.Contains(model.Key)
                || x.Id.ToString().Equals(model.Key)
                ).AsQueryable().AsNoTracking();
            }
            if (!string.IsNullOrEmpty(model.Code))
            {
                model.Code = ToEnglishNumber(model.Code);
                adver = adver.Where(x => x.Id.ToString().Contains(model.Code)).AsQueryable().AsNoTracking();
            }
            var list = new List<JobAdvertisement>();

            if (!string.IsNullOrEmpty(model.Category))
            {
                //adver = adver.Where(x => x.Category.Name.Contains(model.Category)).AsQueryable().AsNoTracking();

                adver = adver.Where(x => x.Category.Name.Contains(model.Category) || x.Category.Parent.Name.Contains(model.Category)).AsQueryable().AsNoTracking();
                //var data = adver.ToList();
                //foreach (var item in data)
                //{
                //    var children = item.Category.Childs.Select(x => x.).ToList();
                //    if()
                //}
            }
            if (!string.IsNullOrEmpty(model.City))
            {
                var cities = Iran.Cities.ToList();
                var city = cities.FirstOrDefault(x => x.CityDivisionCode.ToString() == model.City);
                var finalCity = city.ProvinceName + "، " + city.CityName;
                adver = adver.Where(x => x.City == finalCity).AsQueryable().AsNoTracking();
            }
            if (model.TypeOfCooperation != null)
            {
                adver = adver.Where(x => x.TypeOfCooperation == model.TypeOfCooperation).AsQueryable().AsNoTracking();
            }
            if (model.WorkExperience != null)
            {
                adver = adver.Where(x => x.WorkExperience == model.WorkExperience).AsQueryable().AsNoTracking();
            }
            if (model.Salary != null)
            {
                adver = adver.Where(x => x.Salary == model.Salary).AsQueryable().AsNoTracking();
            }
            if (model.SpecialEmpolyee != null)
            {
                adver = adver.Where(x => x.SpecialEmpolyee == model.SpecialEmpolyee).AsQueryable().AsNoTracking();
            }

            var pageCount = Math.Ceiling(Convert.ToDouble(Convert.ToDouble(adver.Count()) / Convert.ToDouble(pageSize)));
            var adverList = adver
                 .Skip((page - 1) * pageSize)
                .Take(pageSize).OrderByDescending(x => x.SpecialEmpolyee);

            var finalModel = _mapper.Map<List<JobAdvertisement>, List<AllAdver>>(await adverList.ToListAsync());
            if (listOfIds.Count > 0)
            {
                foreach (var item in finalModel)
                {
                    item.IsMarked = (listOfIds.IndexOf(item.Id) == -1) ? false : true;
                }
            }


            //



            return new AllResult<List<AllAdver>>()
            {
                PageCount = pageCount,
                ListOfData = finalModel
            };
        }



        /// <summary>
        /// تبدیل اعداد با کاراکتر های فارسی به کاراکتر های انگلیسی
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string ToEnglishNumber(string input)
        {
            string[] persian = new string[10] { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };
            string[] english = new string[10] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };

            for (int i = 0; i < 10; i++)
            {
                input = input.Replace(persian[i], english[i]);
            }
            return input;
        }


        public async Task<(bool isSuccess, List<string> errors)> CanCreateAdver(User company, bool isImadeiate)
        {
            var userUsePlanCount = 0;
            var userUsePlanForImmediateAdverCount = 0;
            var remainingDate = 0.0;
            var userPlan = await _dataContext.Plans.FindAsync(company.PlanId);


            try
            {
                var err = new List<string>();
                if (isImadeiate)
                {
                    userUsePlanForImmediateAdverCount = _dataContext.JobAdvertisements
                           .Where(x => x.PlanId == company.PlanId && x.CompanyId == company.Id
                           && x.IsImmediate)
                           .AsQueryable().AsNoTracking().Count();


                    var userShopThisPlan = _dataContext.Factors
                        .Where(x => x.PlanId == company.PlanId && x.IsBackMOney == false &&
                        x.CompanyId == company.Id)
                        .AsQueryable().AsNoTracking().Count();
                    //get user shops count for this plan in facto and get adver count in plan info and difrent of that and user use adver in adver table meaning reaminadvercount for this user
                    var remainImmediateAdverCount = (userShopThisPlan * userPlan.ImmediateAdverCount) - userUsePlanForImmediateAdverCount;

                    if (remainImmediateAdverCount <= 0)
                    {
                        var userShopImmediatelyInFactor = _dataContext.Factors
                            .Where(x => x.IsImmediately && x.CompanyId == company.Id && x.IsBackMOney == false)
                            .Count();
                        remainImmediateAdverCount = ((userShopThisPlan * userPlan.ImmediateAdverCount) + userShopImmediatelyInFactor) - userUsePlanForImmediateAdverCount;


                        if (remainImmediateAdverCount <= 0)
                        {

                            err.Add("شما از تمامی آگهی های فوری خود استفاده کرده اید");
                            return (false, err);
                        }
                    }
                    return (true, null);

                }
                else
                {

                    if (company == null)
                    {
                        err.Add("کابر نامعتبر");

                        return (false, err);
                    }

                    if (company.PlanId == null)
                    {
                        err.Add("برای ایجاد آگهی ابتدا  طرح تعرفه خود را خریداری کنید");

                        return (false, err);
                    }
                    var userAdvers = _dataContext.JobAdvertisements
                          .Where(x => x.PlanId == company.PlanId && x.CompanyId == company.Id)
                          .AsQueryable().AsNoTracking();

                    userUsePlanCount = userAdvers.Count();


                    var userShopThisPlan = _dataContext.Factors
                        .Where(x => x.PlanId == company.PlanId && x.CompanyId == company.Id && x.IsBackMOney == false)
                        .AsQueryable().AsNoTracking().Count();
                    //get user shops count for this plan in facto and get adver count in plan info and difrent of that and user use adver in adver table meaning reaminadvercount for this user
                    var remainAdverCount = (userShopThisPlan * userPlan.AdverCount) - userUsePlanCount;

                    if (remainAdverCount <= 0)
                    {
                        //set compani plan ID ==null
                        company.PlanId = null;
                        _dataContext.Users.Update(company);
                        await _dataContext.SaveChangesAsync();

                        //end
                        err.Add("شمااز تمامی آگهی های این طرح استفاده کرده اید،لظفا برای خرید طرح اقدام کنید");

                        return (false, err);
                    }


                    var userLastShopThisPlan = await _dataContext.Factors
                        .Where(x => x.PlanId == company.PlanId && x.CompanyId == company.Id && x.IsBackMOney == false)
                        .Select(x => x.Date)
                        .OrderBy(x => x.Date)
                        .LastOrDefaultAsync();

                    remainingDate = (userLastShopThisPlan.AddDays(userPlan.Duration) - DateTime.Now).TotalDays;
                    if (remainingDate <= 0)
                    {
                        //set compani plan ID ==null
                        company.PlanId = null;
                        _dataContext.Users.Update(company);
                        await _dataContext.SaveChangesAsync();

                        //end
                        err.Add("فرصت استفاده از این طرح به پایان رسیده است");
                        return (false, err);
                    }

                    return (true, null);

                }


            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "CanCreateAdver", "Adver");

                var err = new List<string>();
                err.Add("مشکلی رخ داده است");
                return (false, err);

            }

        }

        public async Task<(bool isSuccess, string error)> MarkAdver(int adverId)
        {
            try
            {
                var user = await _accountService.GetCurrectUser();
                if (user == null || !user.IsActive)
                {
                    return (false, "کاربر نامعتبر");
                }
                var adver = await GetAdverById(adverId);
                if (adver == null)
                {
                    return (false, "آگهی انتخابی شما یافت نشد");
                }
                if (_dataContext.MarkedAdvers.Any(x => x.AdverId == adverId && x.UserId == user.Id))
                {
                    return (false, "قبلا به آگهی های منتخب شما اضافه شده است");

                }
                var markedAdver = new MarkedAdver()
                {
                    AdverId = adverId,
                    UserId = user.Id,
                    UpdatedDate = DateTime.Now,
                    CreatedDate = DateTime.Now
                };
                await _dataContext.MarkedAdvers.AddAsync(markedAdver);
                await _dataContext.SaveChangesAsync();
                return (true, "با موفقیت ثبت شده است");

            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "MarkAdver", "Adver");

                return (false, "مشکلی رخ داده است");

            }

        }

        public async Task<(bool isSuccess, string error)> UnMarkAdver(int adverId)
        {
            try
            {
                var user = await _accountService.GetCurrectUser();
                if (user == null || !user.IsActive)
                {
                    return (false, "کاربر نامعتبر");
                }
                var adver = await GetAdverById(adverId);
                if (adver == null)
                {
                    return (false, "آگهی انتخابی شما یافت نشد");
                }
                if (!_dataContext.MarkedAdvers.Any(x => x.AdverId == adverId && x.UserId == user.Id))
                {
                    return (false, "آگهی انتخابی شما در لیست آکهی های منتخب یافت نشد");

                }
                var markedAdver = await _dataContext.MarkedAdvers.FirstOrDefaultAsync(x => x.AdverId == adverId && x.UserId == user.Id);
                _dataContext.MarkedAdvers.Remove(markedAdver);
                await _dataContext.SaveChangesAsync();
                return (true, "با موفقیت ثبت شده است");

            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "UnMarkAdver", "Adver");

                return (false, "مشکلی رخ داده است");

            }

        }

        public async Task<UserHasPlanForCreateAdver> GetUserPlan()
        {
            var userUsePlanCount = 0;
            var remainingDate = 0.0;
            var planForOneAdver = await _dataContext.Plans.FirstOrDefaultAsync(x => x.Title == "تک آگهی");
            try
            {
                var company = await _accountService.GetCurrectUser();
                if (company == null)
                {
                    return new UserHasPlanForCreateAdver()
                    {

                        AllPlanFor = new AllPlanForCompany()
                        {
                            Id = planForOneAdver.Id,
                            Title = planForOneAdver.Title,
                            AdverCount = planForOneAdver.AdverCount,
                            AdverExpireTime = planForOneAdver.AdverExpireTime,
                            Content = planForOneAdver.Content,
                            Discount = planForOneAdver.Discount,
                            Duration = planForOneAdver.Duration,
                            ImmediateAdverCount = planForOneAdver.ImmediateAdverCount,
                            IsUseResomeManegement = planForOneAdver.IsUseResomeManegement,
                            Logo = planForOneAdver.Logo,
                            Price = planForOneAdver.Price
                        },
                        HasPlan = false
                    };
                }
                if (company.PlanId == null)
                {
                    return new UserHasPlanForCreateAdver()
                    {
                        AllPlanFor = new AllPlanForCompany()
                        {
                            Id = planForOneAdver.Id,

                            Title = planForOneAdver.Title,
                            AdverCount = planForOneAdver.AdverCount,
                            AdverExpireTime = planForOneAdver.AdverExpireTime,
                            Content = planForOneAdver.Content,
                            Discount = planForOneAdver.Discount,
                            Duration = planForOneAdver.Duration,
                            ImmediateAdverCount = planForOneAdver.ImmediateAdverCount,
                            IsUseResomeManegement = planForOneAdver.IsUseResomeManegement,
                            Logo = planForOneAdver.Logo,
                            Price = planForOneAdver.Price
                        },
                        HasPlan = false
                    };
                }
                var userPlan = await _dataContext.Plans.FindAsync(company.PlanId);


                var userLastShopThisPlan = await _dataContext.Factors
                    .Where(x => x.PlanId == company.PlanId && x.CompanyId == company.Id)
                    .OrderByDescending(x => x.Date)
                    .FirstOrDefaultAsync();

                ////sina zadd
                if (userLastShopThisPlan.PaymnetType == PaymnetType.AdminManual)
                {
                    return new UserHasPlanForCreateAdver()
                    {
                        AllPlanFor = new AllPlanForCompany()
                        {
                            Id = planForOneAdver.Id,
                            Title = planForOneAdver.Title,
                            AdverCount = planForOneAdver.AdverCount,
                            AdverExpireTime = planForOneAdver.AdverExpireTime,
                            Content = planForOneAdver.Content,
                            Discount = planForOneAdver.Discount,
                            Duration = planForOneAdver.Duration,
                            ImmediateAdverCount = planForOneAdver.ImmediateAdverCount,
                            IsUseResomeManegement = planForOneAdver.IsUseResomeManegement,
                            Logo = planForOneAdver.Logo,
                            Price = planForOneAdver.Price
                        },
                        HasPlan = true
                    };
                }


                var userAdvers = _dataContext.JobAdvertisements
                  .Where(x => x.PlanId == company.PlanId && x.CompanyId == company.Id)
                  .AsQueryable().AsNoTracking();

                userUsePlanCount = userAdvers.Count();


                var userShopThisPlan = _dataContext.Factors
                    .Where(x => x.PlanId == company.PlanId && x.CompanyId == company.Id)
                    .AsQueryable().AsNoTracking().Count();
                //get user shops count for this plan in facto and get adver count in plan info and difrent of that and user use adver in adver table meaning reaminadvercount for this user
                var remainAdverCount = (userShopThisPlan * userPlan.AdverCount) - userUsePlanCount;

                if (remainAdverCount <= 0)
                {
                    //set compani plan ID ==null
                    company.PlanId = null;
                    _dataContext.Users.Update(company);
                    await _dataContext.SaveChangesAsync();

                    //end
                    return new UserHasPlanForCreateAdver()
                    {
                        AllPlanFor = new AllPlanForCompany()
                        {
                            Id = planForOneAdver.Id,

                            Title = planForOneAdver.Title,
                            AdverCount = planForOneAdver.AdverCount,
                            AdverExpireTime = planForOneAdver.AdverExpireTime,
                            Content = planForOneAdver.Content,
                            Discount = planForOneAdver.Discount,
                            Duration = planForOneAdver.Duration,
                            ImmediateAdverCount = planForOneAdver.ImmediateAdverCount,
                            IsUseResomeManegement = planForOneAdver.IsUseResomeManegement,
                            Logo = planForOneAdver.Logo,
                            Price = planForOneAdver.Price
                        },
                        HasPlan = false
                    };
                }


                //var userLastShopThisPlan = await _dataContext.Factors
                //    .Where(x => x.PlanId == company.PlanId && x.CompanyId == company.Id)
                //    .OrderByDescending(x => x.Date)
                //    .FirstOrDefaultAsync();

                remainingDate = (userLastShopThisPlan.Date.AddDays(userPlan.Duration) - DateTime.Now).TotalDays;
                if (remainingDate <= 0)
                {
                    //set compani plan ID ==null
                    company.PlanId = null;
                    _dataContext.Users.Update(company);
                    await _dataContext.SaveChangesAsync();

                    //end
                    return new UserHasPlanForCreateAdver()
                    {
                        AllPlanFor = new AllPlanForCompany()
                        {
                            Id = planForOneAdver.Id,
                            Title = planForOneAdver.Title,
                            AdverCount = planForOneAdver.AdverCount,
                            AdverExpireTime = planForOneAdver.AdverExpireTime,
                            Content = planForOneAdver.Content,
                            Discount = planForOneAdver.Discount,
                            Duration = planForOneAdver.Duration,
                            ImmediateAdverCount = planForOneAdver.ImmediateAdverCount,
                            IsUseResomeManegement = planForOneAdver.IsUseResomeManegement,
                            Logo = planForOneAdver.Logo,
                            Price = planForOneAdver.Price
                        },
                        HasPlan = false
                    };
                }

                return new UserHasPlanForCreateAdver()
                {
                    AllPlanFor = null,
                    HasPlan = true
                };
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "GetUserPlan", "Adver");

                return new UserHasPlanForCreateAdver()
                {
                    AllPlanFor = null,
                    HasPlan = false
                };

            }

        }



        public async Task<(bool isSuccess, string error)> ChangeAdverCreateStatus(AdminDescriptionForAdver model)
        {
            try
            {
                var user = await _accountService.GetCurrectUser();
                if (user == null || !user.IsActive)
                {
                    return (false, "کاربر نامعتبر");
                }
                var adver = await GetAdverById(model.AdverId);
                var phoneNumber = adver.PhoneNumber;
                var email = adver.Email;
                if (adver == null)
                {
                    return (false, "آگهی انتخابی شما یافت نشد");
                }
                var company = await _dataContext.Users.FindAsync(adver.CompanyId);
                var companyName = company.CompanyPersianName;
                var companyPhoneNumber = company.PhoneNumber;

                switch (model.AdverCreatationStatus)
                {
                    case AdverCreatationStatus.Accepted:
                        adver.AdminDescription = string.IsNullOrEmpty(model.AdminDescription) ? "توسط ادمین تایید شد" : model.AdminDescription;
                        await _sendSmsService.SendMessageForAcceptAdver(companyName, companyPhoneNumber);
                        await _sendSmsService.SendMessageForAcceptAdver(companyName, phoneNumber);
                        await _emailService.SendEmail(email, "تایید آگهی", $"آگهی{adver.Title} توسط تیم فنی تاییید شده است");

                        break;
                    case AdverCreatationStatus.Pending:
                        adver.AdminDescription = string.IsNullOrEmpty(model.AdminDescription) ? "در حال بررسی توسط تیم ارزیابی لطفا منتظر بمانید" : model.AdminDescription;
                        await _sendSmsService.SendMessageForPendindAdver(companyName, companyPhoneNumber);
                        await _sendSmsService.SendMessageForPendindAdver(companyName, phoneNumber);
                        await _emailService.SendEmail(email, "  در حال ارزیابی توسط تیم فنی", $"آگهی{adver.Title} توسط تیم فنی در خال ارزیابی است");

                        break;
                    case AdverCreatationStatus.Rejected:
                        if (string.IsNullOrEmpty(model.AdminDescription))
                        {
                            return (false, "دلیل رد شدن آگهی را وارد کنید");
                        }
                        adver.AdminDescription = model.AdminDescription;
                        await _sendSmsService.SendMessageForRejectAdver(companyName, companyPhoneNumber);
                        await _sendSmsService.SendMessageForRejectAdver(companyName, phoneNumber);
                        await _emailService.SendEmail(email, "رد شدن آگهی", $"آگهی{adver.Title} شما توسط تیم پشتیبانی رد شده است");

                        break;
                    case AdverCreatationStatus.Returned:
                        if (string.IsNullOrEmpty(model.AdminDescription))
                        {
                            return (false, "دلیل برگشت خوردن آگهی را وارد کنید");
                        }
                        adver.AdminDescription = model.AdminDescription;
                        await _sendSmsService.SendMessageForReturnAdver(companyName, companyPhoneNumber);
                        await _sendSmsService.SendMessageForReturnAdver(companyName, phoneNumber);
                        await _emailService.SendEmail(email, "برگشت پول", $"آگهی {adver.Title}شما باز گشت پول شده است");


                        break;
                    default:
                        break;
                }
                adver.AdverCreatationStatus = model.AdverCreatationStatus;
                _dataContext.JobAdvertisements.Update(adver);
                //add in adver notification
                var adverNotif = new AdvertismentNotification()
                {
                    JobAdvertisementId = adver.Id,
                    CreateDate = DateTime.Now,
                    UserId = company.Id
                };
                await _dataContext.AdvertismentNotifications.AddAsync(adverNotif);
                await _dataContext.SaveChangesAsync();
                return (true, "با موفقیت ثبت شده است");

            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "ChangeAdverCreateStatus", "Adver");

                return (false, "مشکلی رخ داده است");

            }
        }

        public async Task<(AdverNotification adverNotification, string error)> GetAdverNotificationForUser()
        {
            var listOfNotif = new List<Advernotif>();
            try
            {
                var user = await _accountService.GetCurrectUser();
                if (user == null || !user.IsActive)
                {
                    return (null, "کاربر نامعتبر");
                }
                var advers = await _dataContext.AdvertismentNotifications.Where(x => x.UserId == user.Id && !x.IsSeen)
                      .OrderByDescending(x => x.CreateDate)
                      .ThenBy(x => x.IsSeen)
                        .Select(x => new { x.JobAdvertisementId, x.Id, x.Type, x.ResomeId }).ToListAsync();

                foreach (var item in advers)
                {
                    var adver = await _dataContext.JobAdvertisements.FindAsync(item.JobAdvertisementId);
                    var advernotif = new Advernotif();
                    advernotif.Type = item.Type;
                    if (item.Type == NotificationType.Advert)
                        advernotif.Description = adver.AdminDescription;
                    else if (item.Type == NotificationType.Resome)
                        advernotif.Description = "رزومه جدید برای این آگهی ارسال شده";
                    else if (item.Type == NotificationType.Ticket)
                        advernotif.Description = "تیکت جدید";

                    advernotif.AdminDescription = adver.AdminDescription;
                    advernotif.Id = item.Id;
                    advernotif.Title = adver.Title;
                    advernotif.ResomeId = item.ResomeId;
                    listOfNotif.Add(advernotif);
                }

                var finalResult = new AdverNotification()
                {
                    Advernotifs = listOfNotif,
                    NotificationCount = listOfNotif.Count
                };
                return (finalResult, "با موفقیت ثبت شده است");

            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "GetAdverNotificationForUser", "Adver");

                return (null, "مشکلی رخ داده است");

            }
        }

        public async Task<(AdverNotification adverNotification, string error)> GetAllAdverNotificationForUser()
        {
            var listOfNotif = new List<Advernotif>();
            try
            {
                var user = await _accountService.GetCurrectUser();
                if (user == null || !user.IsActive)
                {
                    return (null, "کاربر نامعتبر");
                }
                var advers = await _dataContext.AdvertismentNotifications.Where(x => x.UserId == user.Id)
                      .OrderBy(x => x.IsSeen)
                      .ThenByDescending(x => x.CreateDate)
                      .Select(x => new { x.JobAdvertisementId, x.Id }).ToListAsync();

                foreach (var id in advers)
                {
                    var adver = await _dataContext.JobAdvertisements.FindAsync(id.JobAdvertisementId);
                    var advernotif = new Advernotif()
                    {

                        AdminDescription = adver.AdminDescription,
                        AdverCreatationStatus = adver.AdverCreatationStatus,
                        Title = adver.Title + " - #" + adver.Id.ToString(),
                        AdverId = adver.Id,
                        Id = id.Id
                    };
                    listOfNotif.Add(advernotif);
                }


                var finalResult = new AdverNotification()
                {
                    Advernotifs = listOfNotif,
                    NotificationCount = listOfNotif.Count
                };
                return (finalResult, "با موفقیت ثبت شده است");

            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "GetAdverNotificationForUser", "Adver");

                return (null, "مشکلی رخ داده است");

            }
        }

        public async Task<(bool isSuccess, string error)> SeenAdverNotification(List<int> adversIds)
        {
            try
            {
                foreach (var id in adversIds)
                {
                    var adverNotif = await _dataContext.AdvertismentNotifications.FindAsync(id);
                    adverNotif.IsSeen = true;
                    _dataContext.AdvertismentNotifications.Update(adverNotif);
                }
                await _dataContext.SaveChangesAsync();
                return (true, "با موفقیت ثبت شده است");

            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "SeenAdverNotification", "Adver");

                return (false, "مشکلی رخ داده است");

            }
        }

        public async Task<List<AllAdver>> TheSameAdver(int adverId)
        {
            try
            {

                var user = await _accountService.GetCurrectUser();
                var listOfIds = new List<int>();
                if (user != null)
                {
                    listOfIds = await _dataContext.MarkedAdvers.Where(x => x.UserId == user.Id).Select(x => x.AdverId).ToListAsync();

                }


                var adverFirst = await _dataContext.JobAdvertisements
                                      .Include(x => x.Category).Include(i => i.Company)
                                      .Where(x => x.AdverStatus == AdverStatus.Active && x.Id != adverId &&
                                      x.AdverCreatationStatus == AdverCreatationStatus.Accepted)
                                      .OrderByDescending(x => x.UpdateAt)
                                      .ThenByDescending(x => x.IsImmediate)
                                      .AsNoTracking().ToListAsync();
                var adver = adverFirst;
                var finalAdver = new List<JobAdvertisement>();

                var sameAdver = await _dataContext.JobAdvertisements.FindAsync(adverId);
                if (sameAdver != null)
                {

                    adver = adverFirst.Where(x => x.CategoryId == sameAdver.CategoryId && x.City == sameAdver.City).ToList();
                    //finalAdver/*.*/
                    if (adver.Count < 10)
                    {

                        var newCity = GetSameCity(sameAdver.City);
                        adver.AddRange(adverFirst.Where(x => x.CategoryId == sameAdver.CategoryId && newCity.IndexOf(x.City) != -1));
                        if (adver.Count < 10)
                        {
                            adver.AddRange(adverFirst.Where(x => x.CategoryId == sameAdver.CategoryId));

                            if (adver.Count < 10)
                            {
                                adver.AddRange(adverFirst);
                            }
                        }

                    }

                }





                adver = adver.Distinct().ToList();
                //adver.Reverse();
                var adverList = adver.Take(10).ToList();


                var finalModel = _mapper.Map<List<JobAdvertisement>, List<AllAdver>>(adverList);
                if (listOfIds.Count > 0)
                {
                    foreach (var item in finalModel)
                    {
                        item.IsMarked = (listOfIds.IndexOf(item.Id) == -1) ? false : true;
                    }
                }

                return finalModel;

            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task<AllResult<List<AllAdver>>> SuggestionAdverForUser(int page, int pageSize = 10)
        {

            try
            {
                if (page == 0)
                {
                    page = 1;
                }
                var user = await _accountService.GetCurrectUser();
                var listOfIds = new List<int>();
                if (user != null)
                {
                    listOfIds = await _dataContext.MarkedAdvers.Where(x => x.UserId == user.Id).Select(x => x.AdverId).ToListAsync();

                }
                else
                {
                    return null;
                }

                var adverFirst = await _dataContext.JobAdvertisements
                                      .Include(x => x.Category).Include(i => i.Company)
                                      .Where(x => x.AdverStatus == AdverStatus.Active && x.AdverCreatationStatus == AdverCreatationStatus.Accepted)
                                      .OrderByDescending(x => x.UpdateAt)
                                      .ThenByDescending(x => x.IsImmediate)
                                      .AsNoTracking().ToListAsync();
                var adver = adverFirst;
                var finalAdver = new List<JobAdvertisement>();

                var userJobPreference = await _dataContext.UserJobPreferences.Include(x => x.UserJobPreferenceCategories)
                    .FirstOrDefaultAsync(x => x.ResomeId == user.ResomeId);
                if (userJobPreference != null)
                {
                    var categoryList = userJobPreference.UserJobPreferenceCategories.Select(x => x.CategoryId).ToList();

                    adver = adverFirst.Where(x => categoryList.IndexOf(x.CategoryId) != -1 && x.City == userJobPreference.City).ToList();
                    //finalAdver/*.*/
                    if (adver.Count < 20)
                    {

                        var newCity = GetSameCity(userJobPreference.City);
                        adver.AddRange(adverFirst.Where(x => categoryList.IndexOf(x.CategoryId) != -1 &&
                        newCity.IndexOf(x.City) != -1));
                        if (adver.Count < 20)
                        {
                            adver.AddRange(adverFirst.Where(x => categoryList.IndexOf(x.CategoryId) != -1));

                            if (adver.Count < 20)
                            {
                                adver.AddRange(adverFirst);
                            }
                        }

                    }

                }

                else if (!string.IsNullOrEmpty(user.City))
                {
                    adver = adverFirst.Where(x => x.City == user.City).ToList();

                    if (adver.Count < 20)
                    {
                        var newCity = GetSameCity(user.City);

                        adver.AddRange(adverFirst.Where(x => newCity.IndexOf(x.City) != -1));

                        if (adver.Count < 20)
                        {
                            adver.AddRange(adverFirst);
                        }
                    }

                }




                adver = adver.Distinct().ToList();
                //adver.Reverse();
                var pageCount = Math.Ceiling(Convert.ToDouble(Convert.ToDouble(adver.Count()) / Convert.ToDouble(pageSize)));
                var adverList = adver
                     .Skip((page - 1) * pageSize)
                    .Take(pageSize).ToList();

                var finalModel = _mapper.Map<List<JobAdvertisement>, List<AllAdver>>(adverList);
                if (listOfIds.Count > 0)
                {
                    foreach (var item in finalModel)
                    {
                        item.IsMarked = (listOfIds.IndexOf(item.Id) == -1) ? false : true;
                    }
                }

                return new AllResult<List<AllAdver>>()
                {
                    PageCount = pageCount,
                    ListOfData = finalModel
                };

            }
            catch (Exception ex)
            {

                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "SuggestionAdverForUser", "Adver");

                string path = @"C:\Users\MmdNzhd\Desktop\Log.txt";
                // This text is added only once to the file.
                if (!System.IO.File.Exists(path))
                {
                    // Create a file to write to.
                    using (var sw = System.IO.File.CreateText(path))
                    {
                        sw.WriteLine(ex.Message);
                        sw.WriteLine(ex.StackTrace);
                        sw.WriteLine(ex.InnerException);
                    }
                }
                return new AllResult<List<AllAdver>>()
                {
                    PageCount = 0,
                    ListOfData = null
                };
            }

        }

        public static List<string> GetSameCity(string cityCode)
        {
            var city = new List<string>();
            try
            {
                var isNum = int.Parse(cityCode);
                var province = Iran.Cities.Where(x => x.CityDivisionCode == isNum).Select(x => x.ProvinceName).FirstOrDefault();
                city = Iran.Cities.Where(x => x.ProvinceName == province).Select(x => x.CityDivisionCode.ToString()).ToList();
            }
            catch (Exception ex)
            {
                city = Iran.Cities.Where(x => x.ProvinceName == "مازندران").Select(x => x.CityDivisionCode.ToString()).ToList();
            }
            return city;
        }

        public async Task<AllResult<List<AllAdverForAdmin>>> GetAllAdverByFilteringForRefrence(bool? isImmediatley, AdverStatus? adverStatus, AdverCreatationStatus? adverCreatationStatus, int page, int pageSize = 10, string RefrenceId = "")
        {
            {

                var refrence = await _accountService.GetCurrectUser();
                if (refrence == null) return null;
                if (page == 0)
                {
                    page = 1;
                }
                var adver = _dataContext.JobAdvertisements
                    .Include(x => x.Category).Include(i => i.Company)
                    .Where(x => x.Company.RefrenceId == refrence.Id)
                     .OrderByDescending(x => x.AdverCreatationStatus)
                    .ThenByDescending(x => x.IsImmediate)
                    .ThenByDescending(x => x.UpdateAt)
                    .AsQueryable().AsNoTracking();
                if (RefrenceId != "")
                {
                    adver = adver.Where(w => w.Company.RefrenceId == RefrenceId);
                }
                if (isImmediatley != null)
                {
                    adver = adver.Where(x => x.IsImmediate == isImmediatley);
                }
                if (adverStatus != null)
                {
                    adver = adver.Where(x => x.AdverStatus == adverStatus);
                }
                if (adverCreatationStatus != null)
                {
                    adver = adver.Where(x => x.AdverCreatationStatus == adverCreatationStatus);
                }
                var pageCount = Math.Ceiling(Convert.ToDouble(Convert.ToDouble(adver.Count()) / Convert.ToDouble(pageSize)));
                var adverList = adver
                     .Skip((page - 1) * pageSize)
                    .Take(pageSize);

                var finalModel = _mapper.Map<List<JobAdvertisement>, List<AllAdverForAdmin>>(await adverList.ToListAsync());
                return new AllResult<List<AllAdverForAdmin>>()
                {
                    PageCount = pageCount,
                    ListOfData = finalModel
                };

            }
        }
    }
}