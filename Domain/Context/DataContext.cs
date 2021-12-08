using DNTPersianUtils.Core;
using DNTPersianUtils.Core;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class DataContext : IdentityDbContext<User>
    {


        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<SiteVisit> SiteVisits { get; set; }
        public DbSet<RefrenceTransation> RefrenceTransations { get; set; }
        public DbSet<RefrenceDepositRequest> RefrenceDepositRequest { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<GiftCart> GiftCarts { get; set; }

        public DbSet<Category> Categories { get; set; }
        public DbSet<CompanyCategory> CompanyCategories { get; set; }
        public DbSet<JobAdvertisement> JobAdvertisements { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Factor> Factors { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<JobSkill> JobSkills { get; set; }
        public DbSet<Languag> Languags { get; set; }
        public DbSet<UserWorkExperience> UserWorkExperiences { get; set; }
        public DbSet<EducationalBackground> EducationalBackgrounds { get; set; }
        public DbSet<UserLanguage> UserLanguage { get; set; }
        public DbSet<UserJobSkill> UserJobSkills { get; set; }
        public DbSet<UserJobPreferences> UserJobPreferences { get; set; }
        public DbSet<UserJobPreferenceCategory> UserJobPreferenceCategories { get; set; }
        public DbSet<Resome> Resomes { get; set; }
        public DbSet<JobOpportunity> JobOpportunity { get; set; }
        public DbSet<MarkedAdver> MarkedAdvers { get; set; }
        public DbSet<AdvertismentNotification> AdvertismentNotifications { get; set; }
        public DbSet<AsignResome> AsignResomes { get; set; }
        public DbSet<UserJobShortDescription> UserJobShortDescription { get; set; }
        public DbSet<MarkAsignResome> MarkAsignResomes { get; set; }
        public DbSet<CommentAsignResome> CommentAsignResomes { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<FrequentlyAskedQuestion> FrequentlyAskedQuestions { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Blog> Blogs { get; set; }

        public DbSet<EmployeePlan> EmployeePlans { get; set; }
        public DbSet<EmployeePayment> EmployeePayments { get; set; }
        public DbSet<EmployeeFactor> EmployeeFactors { get; set; }
        public DbSet<EmployeeTransaction> EmployeeTransactions { get; set; }
        public DbSet<ContactUsMessage> ContactUsMessages { get; set; }
        public DbSet<ReportAdvert> ReportAdverts { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<StorySizeNotification> StorySizeNotifications { get; set; }
        public DbSet<ResomeColors> ResomeColors { get; set; }
        public DbSet<Services> Services { get; set; }
        public DbSet<EmailNotification> EmailNotifications { get; set; }



        //
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);



            #region RefrenceTrqansaction

            builder.Entity<RefrenceTransation>()
               .HasOne<User>(x => x.User)
               .WithMany(y => y.RefrenceTransations)
               .HasForeignKey(g => g.UserId);


            builder.Entity<RefrenceTransation>()
               .HasOne<User>(x => x.Refrence)
               .WithMany(y => y.RefrenceTransations2)
               .HasForeignKey(g => g.RefrenceId);

            #endregion




            #region RefrenceDepositRequest
            //builder.Entity<RefrenceDepositRequest>()
            //   .HasOne<User>(x => x.User)
            //   .WithMany(y => y.RefrenceDepositRequest)
            //   .HasForeignKey(g => g.UserId);


            builder.Entity<RefrenceDepositRequest>()
               .HasOne<User>(x => x.Refrence)
               .WithMany(y => y.RefrenceDepositRequest2)
               .HasForeignKey(g => g.RefrenceId);
            #endregion 



            #region user factor and plan

            builder.Entity<EmployeeFactor>()
                .HasOne<EmployeePlan>(x => x.EmployeePlan)
                .WithMany(y => y.EmployeeFactors)
                .HasForeignKey(g => g.EmployeePlanId);



            builder.Entity<EmployeePayment>()
                .HasOne<EmployeePlan>(x => x.EmployeePlan)
                .WithMany(y => y.EmployeePayments)
                .HasForeignKey(g => g.EmployeePlanId);




            builder.Entity<User>()
                .HasOne<EmployeePlan>(x => x.EmployeePlan)
                .WithMany(y => y.Employees)
                .HasForeignKey(g => g.EmployeePlanId);


            builder.Entity<EmployeeTransaction>()
              .HasOne<EmployeePayment>(x => x.EmployeePayment)
              .WithMany(y => y.EmployeeTransactions)
              .HasForeignKey(g => g.EmployeePaymentId);


            #endregion

            #region giftcart
            //giftcart and user
            builder.Entity<GiftCart>()
                .HasOne<User>(x => x.Employer)
                .WithMany(y => y.GiftCarts)
                .HasForeignKey(g => g.EmployerId);
            //giftcart index
            builder.Entity<GiftCart>()
                .HasIndex(u => u.GiftCode);

            #endregion






            #region Category
            //category Self Rel
            builder.Entity<Category>()
                .HasOne<Category>(x => x.Parent)
                .WithMany(u => u.Childs)
                .HasForeignKey(i => i.CategoryId);

            //category Index
            builder.Entity<Category>()
                .HasIndex(i => i.Name).IsUnique(true);

            #endregion

            #region Ticket
            builder.Entity<Ticket>()
               .HasOne<User>(x => x.Sender)
               .WithMany(y => y.SenderTickets)
               .HasForeignKey(g => g.SenderId);

            builder.Entity<Ticket>()
            .HasOne<User>(x => x.Receive)
            .WithMany(y => y.ReceiverTickets)
            .HasForeignKey(g => g.ReceiverId);
            #endregion

            #region User
            //User Index
            builder.Entity<User>()
                .HasIndex(i => i.CompanyPersianName).IsUnique(true);
            builder.Entity<User>()
                .HasIndex(i => i.EmergencPhone);
            builder.Entity<User>()
                .HasIndex(i => i.Fullname);
            builder.Entity<User>()
                .HasIndex(u => u.City);

            builder.Entity<User>()
               .HasIndex(i => i.CompanyEngName).IsUnique(true);


            //relation between Company and Plan

            builder.Entity<User>()
                .HasOne<Plan>(x => x.Plan)
                .WithMany(x => x.Companies)
                .HasForeignKey(x => x.PlanId);




            #endregion

            #region CompanyCategory
            //category and companycategory
            builder.Entity<CompanyCategory>()
                .HasOne<Category>(x => x.Category)
                .WithMany(u => u.CompanyCategorires)
                .HasForeignKey(i => i.CategoryId);

            //category and companycategory
            builder.Entity<CompanyCategory>()
                .HasOne<User>(x => x.Company)
                .WithMany(u => u.CompanyCategorires)
                .HasForeignKey(i => i.CompanyId);

            #endregion


            #region JobAdvertisement
            //relation between Adver and Plan

            builder.Entity<JobAdvertisement>()
                .HasOne<Plan>(x => x.Plan)
                .WithMany(x => x.JobAdvertisements)
                .HasForeignKey(x => x.PlanId);
            //realationShip JobAdvertisement and Category
            builder.Entity<JobAdvertisement>()
                .HasOne<Category>(x => x.Category)
                .WithMany(i => i.JobAdvertisements)
                .HasForeignKey(u => u.CategoryId);
            //realationShip JobAdvertisement and Company
            builder.Entity<JobAdvertisement>()
                .HasOne<User>(x => x.Company)
                .WithMany(i => i.JobAdvertisements)
                .HasForeignKey(u => u.CompanyId);

            //realationShip EmployeeAdver and user Employee



            //JobAdvertisement Index

            builder.Entity<JobAdvertisement>()
                .HasIndex(x => x.Title);

            builder.Entity<JobAdvertisement>()
             .HasIndex(x => x.City);

            builder.Entity<JobAdvertisement>()
             .HasIndex(x => x.CategoryId);


            builder.Entity<JobAdvertisement>()
                .HasIndex(x => x.TypeOfCooperation);

            builder.Entity<JobAdvertisement>()
                .HasIndex(x => x.AdverStatus);



            builder.Entity<JobAdvertisement>()
             .HasIndex(x => new
             {
                 x.Title,
                 x.City
             });

            builder.Entity<JobAdvertisement>()
             .HasIndex(x => new
             {
                 x.Title,
                 x.City,
                 x.TypeOfCooperation
             });



            builder.Entity<JobAdvertisement>()
             .HasIndex(x => x.ExpireTime);
            #endregion

            #region Plan

            //Index For Plan

            //builder.Entity<Plan>()
            //    .HasIndex(i => i.Title).IsUnique();

            builder.Entity<Plan>()
              .HasIndex(i => i.AdverExpireTime);

            #endregion


            #region Factor


            builder.Entity<Factor>()
                .HasOne<User>(x => x.Company)
                .WithMany(x => x.Factors)
                .HasForeignKey(x => x.CompanyId);


            builder.Entity<Factor>()
                .HasOne<Plan>(x => x.Plan)
                .WithMany(x => x.Factors)
                .HasForeignKey(x => x.PlanId);



            //factor Index

            builder.Entity<Factor>()
                .HasIndex(x => x.Date);

            builder.Entity<Factor>()
               .HasIndex(x => x.IsImmediately);



            //miltyColumn Index For incearese Performance
            builder.Entity<Factor>()
              .HasIndex(x => new { x.CompanyId, x.PlanId });

            builder.Entity<Factor>()
             .HasIndex(x => new { x.CompanyId, x.IsImmediately });

            #endregion

            #region Log
            builder.Entity<Log>()
           .HasIndex(x => x.Date);

            builder.Entity<Log>()
            .HasIndex(x => x.MethodName);

            builder.Entity<Log>()
          .HasIndex(x => x.TableName);

            builder.Entity<Log>()
         .HasIndex(x => x.ExceptionMessage);

            #endregion



            #region PAYMENT

            //rel between payment and plan
            builder.Entity<Payment>()
               .HasOne<Plan>(x => x.Plan)
               .WithMany(x => x.Payments)
               .HasForeignKey(x => x.PlanId);


            //relation between User And Payment
            builder.Entity<Payment>()
                .HasOne<User>(x => x.User)
                .WithMany(x => x.Payments)
                .HasForeignKey(x => x.UserId);

            //index for Payment
            builder.Entity<Payment>()
                .HasIndex(x => x.InvoiceKey).IsUnique(true);

            builder.Entity<Payment>()
            .HasIndex(x => x.TransactionCode).IsUnique(true);


            builder.Entity<Payment>()
          .HasIndex(x => x.Date);


            #endregion



            #region Transaction


            //relation between User And Transaction


            builder.Entity<Transaction>()
          .HasIndex(x => x.Amount);


            //rel between payment and transaction
            builder.Entity<Transaction>()
               .HasOne<Payment>(x => x.Payment)
               .WithMany(x => x.Transactions)
               .HasForeignKey(x => x.PaymentId);


            #endregion


            #region JobSkill


            //jobskill
            //builder.Entity<JobSkill>()
            //.HasOne<Category>(x => x.Category)
            //.WithMany(x => x.JobSkills)
            //.HasForeignKey(x => x.CategoryId);



            builder.Entity<JobSkill>()
                .HasIndex(x => x.Name);


            #endregion

            #region UserWorkExperience


            //UserWorkExperience


            builder.Entity<UserWorkExperience>()
                .HasIndex(x => x.WorkTitle);

            builder.Entity<UserWorkExperience>()
              .HasIndex(x => x.IsActive);

            builder.Entity<UserWorkExperience>()
             .HasIndex(x => x.CreateDate);



            builder.Entity<UserWorkExperience>()
                        .HasOne<Resome>(x => x.Resome)
                        .WithMany(x => x.UserWorkExperiences)
                        .HasForeignKey(x => x.ResomeId);


            #endregion



            #region EducationalBackground

            //EducationalBackground

            builder.Entity<EducationalBackground>()
                .HasIndex(x => x.FieldOfStudy);

            builder.Entity<EducationalBackground>()
              .HasIndex(x => x.IsActive);

            builder.Entity<EducationalBackground>()
             .HasIndex(x => x.CreateDate);

            builder.Entity<EducationalBackground>()
                       .HasOne<Resome>(x => x.Resome)
                       .WithMany(x => x.EducationalBackgrounds)
                       .HasForeignKey(x => x.ResomeId);



            #endregion

            #region Language

            //Language


            builder.Entity<Domain.Languag>()
                .HasIndex(x => x.Name);

            builder.Entity<Languag>()
             .HasIndex(x => x.CreateDate);



            #endregion


            #region UserLangage

            //Language and UserLanguage
            builder.Entity<UserLanguage>()
                          .HasOne<Languag>(x => x.Languag)
                          .WithMany(x => x.UserLanguages)
                          .HasForeignKey(x => x.LanguageId);



            builder.Entity<UserLanguage>()
             .HasIndex(x => x.CreateDate);

            builder.Entity<UserLanguage>()
                       .HasOne<Resome>(x => x.Resome)
                       .WithMany(x => x.UserLanguages)
                       .HasForeignKey(x => x.ResomeId);


            #endregion

            #region UserJobSkill

            builder.Entity<UserJobSkill>()
                        .HasOne<JobSkill>(x => x.JobSkill)
                        .WithMany(x => x.UserJobSkills)
                        .HasForeignKey(x => x.JobSkillId);

            builder.Entity<UserJobSkill>()
                        .HasOne<Resome>(x => x.Resome)
                        .WithMany(x => x.UserJobSkills)
                        .HasForeignKey(x => x.ResomeId);


            builder.Entity<UserJobSkill>()
             .HasIndex(x => x.CreateDate);

            #endregion


            #region UserJobPreference




            builder.Entity<UserJobPreferences>()
             .HasIndex(x => x.CreateDate);


            builder.Entity<UserJobPreferences>()
             .HasIndex(x => x.City);


            builder.Entity<UserJobPreferences>()
             .HasIndex(x => x.Salary);


            builder.Entity<UserJobPreferences>()
             .HasIndex(x => x.Insurance);



            builder.Entity<UserJobPreferences>()
             .HasIndex(x => new { x.City, x.Salary });


            #endregion

            #region UserJobPreferenceCategory

            builder.Entity<UserJobPreferenceCategory>()
                    .HasOne<Category>(x => x.Category)
                    .WithMany(x => x.UserJobPreferenceCategories)
                    .HasForeignKey(x => x.CategoryId);

            builder.Entity<UserJobPreferenceCategory>()
                .HasOne<UserJobPreferences>(x => x.UserJobPreferences)
                .WithMany(x => x.UserJobPreferenceCategories)
                .HasForeignKey(x => x.UserJobPreferenceId);
            #endregion


            //   
            #region Resome

            builder.Entity<User>()
            .HasOne<Resome>(x => x.Resome)
            .WithOne(x => x.Employee)
            .HasForeignKey<Resome>(x => x.EmployeeId);



            builder.Entity<Resome>()
            .HasOne<User>(x => x.Employee)
            .WithOne(x => x.Resome)
            .HasForeignKey<User>(x => x.ResomeId);




            builder.Entity<Resome>()
            .HasOne<UserJobPreferences>(x => x.UserJobPreferences)
            .WithOne(x => x.Resome)
            .HasForeignKey<UserJobPreferences>(x => x.ResomeId);


            builder.Entity<UserJobPreferences>()
            .HasOne<Resome>(x => x.Resome)
             .WithOne(x => x.UserJobPreferences)
            .HasForeignKey<Resome>(x => x.UserJobPreferencesId);



            #endregion

            #region JobOpportunity

            builder.Entity<JobOpportunity>()
            .HasOne<Category>(x => x.Category)
            .WithMany(x => x.JobOpportunities)
            .HasForeignKey(x => x.CategoyId);

            builder.Entity<JobOpportunity>()
            .HasOne<User>(x => x.Employee)
            .WithMany(x => x.JobOpportunities)
            .HasForeignKey(x => x.EmployeeId);
            #endregion

            #region MarkedAdver

            builder.Entity<MarkedAdver>()
            .HasOne<JobAdvertisement>(x => x.JobAdvertisement)
            .WithMany(x => x.MarkedAdvers)
            .HasForeignKey(x => x.AdverId);

            builder.Entity<MarkedAdver>()
            .HasOne<User>(x => x.User)
            .WithMany(x => x.MarkedAdvers)
            .HasForeignKey(x => x.UserId);
            #endregion
            #region AdvertismentNotification

            builder.Entity<AdvertismentNotification>()
                .HasOne<JobAdvertisement>(x => x.JobAdvertisement)
                .WithMany(x => x.AdvertismentNotifications)
                .HasForeignKey(x => x.JobAdvertisementId);


            builder.Entity<AdvertismentNotification>()
            .HasOne<User>(x => x.User)
                .WithMany(x => x.AdvertismentNotifications)
                .HasForeignKey(x => x.UserId);

            #endregion
            #region AsignResome

            builder.Entity<AsignResome>()
            .HasOne<Resome>(x => x.Resome)
                .WithMany(x => x.AsignResomes)
                .HasForeignKey(x => x.ResomeId);

            builder.Entity<AsignResome>()
           .HasOne<JobAdvertisement>(x => x.JobAdvertisement)
               .WithMany(x => x.AsignResomes)
               .HasForeignKey(x => x.JobAdvertisementId);
            #endregion
            #region UserJobShortDescription
            builder.Entity<UserJobShortDescription>()
         .HasOne<Resome>(x => x.Resome)
             .WithMany(x => x.UserJobShortDescriptions)
             .HasForeignKey(x => x.ResomeId);
            #endregion


            #region MarkAsignResome
            builder.Entity<MarkAsignResome>()
         .HasOne<AsignResome>(x => x.AsignResome)
             .WithMany(x => x.MarkAsignResomes)
             .HasForeignKey(x => x.AsignResomeId);
            #endregion
            #region LikeAsignResome
            builder.Entity<CommentAsignResome>()
         .HasOne<AsignResome>(x => x.AsignResome)
             .WithMany(x => x.CommentAsignResomes)
             .HasForeignKey(x => x.AsignResomeId);
            #endregion
        }

    }
}
