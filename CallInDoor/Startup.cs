using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Domain;
using Domain.Entities;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Katino.Models;
using Katino.Config.Extentions;
using Katino.Config;
using Katino.Config.Middleware;
using Domain.Utilities;
using Service;
using Service.Interfaces.JwtManager;
using Service.Interfaces.Account;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using Hangfire;
using Hangfire.MemoryStorage;
using Parbad.Builder;
using Service.Interfaces.UserWorkExperience;
using Katino.Hubs;
using ElmahCore;
using ElmahCore.Mvc;
using Service.Implements.GenerateImage;
using Service.Interfaces.GenerateImage;
using Katino.Cron_Job;

namespace Katino
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //services.AddLocalization();

            services.AddDbContext<DataContext>(opt =>
            {
                opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            //cors
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
            });

            //setting  of identity
            services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;

                options.User.RequireUniqueEmail = false;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ 0123456789@#+-اآبپتثجچحخدذرزژسشصضطظعغفقکگلمنوهیئيك";

                //confirm Phone Number
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedPhoneNumber = true;
            })
            .AddRoleManager<RoleManager<IdentityRole>>()
            .AddSignInManager<SignInManager<User>>()
            //این توکن میسازه باید باشه برا چنج پسورد و.کانفیرم ایمیل
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<DataContext>();


            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure jwt authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            services.AddOurAuthentication(appSettings);
            services.AddAuthorization(option =>
            {
                option.AddPolicy("IsActive", policy => policy.RequireClaim("IsActive", "TRUE"));
            });

            services.AddElmah(options =>
            {
                options.Path = "/elmah";
            });

            //swagger config
            services.AddOurSwaager();

            services.AddControllersWithViews();
            services.AddRazorPages();
            //autoMapper
            services.AddAutoMapper(typeof(AllAdver));
            //hangfire
            services.AddHangfire(config =>
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseDefaultTypeSerializer()
                .UseMemoryStorage()
            );
            services.AddHangfireServer();



 
            services.AddParbad()
                .ConfigureGateways(gateways =>
                {
                    gateways
                        .AddParsian()
                        .WithAccounts(accounts =>
                        {
                            accounts.AddInMemory(account =>
                            {
                                account.LoginAccount = "your LoginAccount";
                            });
                        });

                 
                })
                .ConfigureHttpContext(builder => builder.UseDefaultAspNetCore())
                .ConfigureStorage(builder => builder.UseMemoryCache());


            services.AddSignalR();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(10);
            });
            //
            services.AddMvc(options =>
            {
                options.EnableEndpointRouting = false;
                options.Filters.Add(new ModelStateCheckFilter());

            })
                 .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();




            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IJwtManager, JwtManager>();
            //services.AddScoped<ICommonService, CommonService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IAdverService, AdverService>();
            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IJobSkillService, JobSkillService>();
            services.AddScoped<IUserWorkExperienceService, UserWorkExperienceService>();
            services.AddScoped<IEducationalBackgroundService, EducationalBackgroundServic>();
            services.AddScoped<ILanguageService, LanguageService>();
            services.AddScoped<IUserLanguageService, UserLanguageService>();
            services.AddScoped<IUserJobSkillService, UserJobSkillService>();
            services.AddScoped<IUserJobPreferenceService, UserJobPreferenceService>();
            services.AddScoped<IResomeService, ResomeService>();
            services.AddScoped<IJobOpportunityService, JobOpportunityService>();
            services.AddScoped<IUserJobShortDescriptionService, UserJobShortDescriptionService>();
            services.AddScoped<IlogService, LogService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISendSmsService, SendSmsService>();
            services.AddScoped<ISettingService, SettingService>();
            services.AddScoped<IAnswerQuestionService, AnswerQuestionService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IGiftCodeService, GiftCodeService>();
            services.AddScoped<IBlogService, BlogService>();
            services.AddScoped<IGenerateImageService, GenerateImageService>();
            services.AddScoped<IHangfireUpdateJobAdvertisment, HangfireUpdateJobAdvertisment>();
            services.AddScoped<IMailService, MailService>();
            //add newtonsoftJson with this config for Loop error Handling
            services.AddControllers()
                .AddNewtonsoftJson(option =>
                option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app
            , IWebHostEnvironment env,
            IBackgroundJobClient backgroundJobClient,
            IRecurringJobManager recurringJobManager,
            IServiceProvider serviceProvider)
        {
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            //app.UseDeveloperExceptionPage();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseMiddleware<ErrorWrappingMiddleware2>();
            }

            /*.........................swagger..........................*/
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("v1/swagger.json", "v1");
            });



            app.UseHttpsRedirection();
            app.UseStaticFiles();


            ////2 زبانه
            //var supportedCultures = new List<CultureInfo>()
            //{
            //    new CultureInfo(PublicHelper.persianCultureName),
            //};
            //var options = new RequestLocalizationOptions()
            //{
            //    DefaultRequestCulture = new RequestCulture(PublicHelper.persianCultureName),
            //    SupportedCultures = supportedCultures,
            //    SupportedUICultures = supportedCultures,
            //    RequestCultureProviders = new List<IRequestCultureProvider>()
            //    {
            //        new QueryStringRequestCultureProvider(),
            //        new CookieRequestCultureProvider(),
            //        new AcceptLanguageHeaderRequestCultureProvider(),
            //    }
            //};
            //app.UseRequestLocalization(options);


            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseElmah();


            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "areas",
                  template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
                );
            });
            app.UseSignalR(routs =>
            {
                routs.MapHub<ChatHub>("/chathub");
            });

            //hangfire
            app.UseHangfireDashboard();
            recurringJobManager.AddOrUpdate(
                "CheckExpire",
                () => serviceProvider.GetService<IHangfireUpdateJobAdvertisment>().CheckExpire(),
                "1 0 * * * "
                );

            recurringJobManager.AddOrUpdate(
               "Send Mail For Employee",
               () => serviceProvider.GetService<IJobOpportunityService>().GetSeggustAdverForUser(),
               //"0 19 * * *"
               "0 16 * * *"
               );

            //recurringJobManager.AddOrUpdate(
            //   "Daily Email And Texts",
            //   () => serviceProvider.GetService<ISendAdvertEmails>().Send(),
            //   "0 8 * * *"
            //   );
            //use parbad virtual 
            //app.UseParbadVirtualGatewayWhenDeveloping();    

        }
    }
}
