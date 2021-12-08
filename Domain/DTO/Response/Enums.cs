using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.DTO.Response
{
    public enum StatusCode
    {

        ok = 200,
        BadRequest = 400,
        notFound = 404,
        unAuthorize = 401,
        forbidden = 403,
        An_unhandled_error_occurred = 300,//this error for unhandler
        another = 500,
    }
    //تعداد پرسنل
    public enum NumberOfStaff
    {
        [Display(Name = "بین 2 تا 10 نفر")]
        P2_10,
        [Display(Name = "بین 11 تا 50 نفر")]
        P11_50,
        [Display(Name = "بین 51 تا 200 نفر")]
        P51_200,
        [Display(Name = "بین 201 تا 500 نفر")]
        P201_500,
        [Display(Name = "بین 501 تا 100 نفر")]
        P501_1000,
        [Display(Name = "بیشتر از 1000 نفر")]
        MoreThan1000
    }
    //نوع فعالیت
    public enum TypeOfCooperation
    {
        [Display(Name = "تمام وقت")]
        FullTime = 1,//تمام وقت
        [Display(Name = "پاره وقت")]
        PartTime = 2,//پارت تایم
        [Display(Name = "کارآموزی")]
        Internship = 3,//کارآموزی
        [Display(Name = "دورکاری")]
        Teleworking = 4//دورکاری
    }
    //حقوق
    public enum Salary
    {
        [Display(Name = "کمتر از 1 میلیون")]
        LesThan1Mili = 1,
        [Display(Name = "بین 1 تا 2.5 میلیون")]
        Between1And2_5 = 2,
        [Display(Name = "بین 2.5 تا 3.5 میلیون")]
        Between2_5And3_5 = 3,
        [Display(Name = "بین 3.5 تا 5 میلیون")]
        Between3_5And5 = 4,
        [Display(Name = "بین 5 تا 8 میلیون")]
        Between5And8 = 5,
        [Display(Name = "بیشتر از 8 میلیون")]
        MoreThan8 = 6,
        [Display(Name = "توافقی")]
        Agreement = 7,
        [Display(Name = "طبق قانون کار")]
        AccordingToLaborLaw = 8
    }
    //سابقه کاری
    public enum WorkExperience
    {
        [Display(Name = "مهم نیست")]
        NotImp = 1,
        [Display(Name = "کمتر از 3 سال")]
        LessThan3 = 2,
        [Display(Name = "بین 3 تا 7 سال")]
        Between3And7 = 3,
        [Display(Name = "بیشتر از 7 سال")]
        MoreThan7 = 4
    }
    //مدرک تحصیلی
    public enum DegreeOfEducation
    {
        [Display(Name = "مهم نیست")]
        NotImp = 1,
        [Display(Name = "دیپلم")]
        Diploma = 2,//دیپلم
        [Display(Name = "کاردانی")]
        Associate = 3,//کاردانی
        [Display(Name = "کارشناسی")]
        Bachelor = 4,//کارشناسی
        [Display(Name = "کارشناسی ارشد")]
        Masters = 5,//کارشناسی ارشد
        [Display(Name = "دکترا")]
        PHD = 6//دکترا
    }
    public enum Gender
    {
        [Display(Name = "مهم نیست")]
        NotImp = 1,
        [Display(Name = "مرد")]
        Male = 2,
        [Display(Name = "زن")]
        Female = 3,
        [Display(Name = "سایر")]
        Others = 4
    }
    //وضعیت سربازی

    public enum AdverStatus
    {
        [Display(Name = "فعال")]
        Active = 1,
        [Display(Name = "پیش نویس")]
        Draft = 2,
        [Display(Name = "آرشیو")]
        Archive = 3,
        [Display(Name = "پایان یافته")]
        Finished = 4,
        [Display(Name = "غیرفعال")]
        Disable = 5,
        [Display(Name = "منقضی شده")]
        Expired = 6,
        [Display(Name = "رد شده")]
        AdverCreatationStatusRejected =7,
        [Display(Name = "باطل شده")]
        BoolIsActive,
    }
    public enum AdverCreatationStatus
    {
        [Display(Name = "در حال بررسی")]
        Pending = 1,
        [Display(Name = "پذیرفته شده")]
        Accepted = 3,
        [Display(Name = "رد شده")]
        Rejected = 2,
        [Display(Name = "برگشت خورده")]
        Returned = 4,

    }
    //for use list of enum must create clas of this
    public enum Gateways
    {
        Saman,
        Mellat,
        Parsian,
        Pasargad,
        IranKish,
        Melli,
        AsanPardakht,
        ZarinPal,
        PayIr,
        IdPay,
        ParbadVirtual
    }
    public enum LanguageLevel
    {
        [Display(Name = "مبتدی")]
        Beginner,

        [Display(Name = "متوسط")]
        Intermediate,

        [Display(Name = "حرفه ای")]
        Advanced,

        [Display(Name = "زبان بومی")]
        NativeLanguage
    }
    public enum Senioritylevel
    {
        [Display(Name = "تازه کار")]
        Junior,

        [Display(Name = "متخصص")]
        Expert,

        [Display(Name = " مدیر")]
        Manager,

        [Display(Name = " مدیر ارشد")]
        SeniorManger
    }
    public enum AsingResomeStatus
    {
        [Display(Name = "در انتظار تعیین وضعییت")]
        Pending = 1,
        [Display(Name = "تاییده برای مصاحبه")]
        AcceptedForInterview = 3,
        [Display(Name = "رد شده")]
        Rejected = 2,
        [Display(Name = "استخدام شده")]
        Hired = 4,
    }

    public enum EmploymentStatus
    {
        [Display(Name = "جویای کار")]
        jobSeeker = 1,
        [Display(Name = "شاغل")]
        Employed = 2,
        [Display(Name = "به دنبال شغل بهتر")]
        LookingForBetterJob = 3,

    }
    public enum TicketPriorityStatus
    {
        [Display(Name = "فوری")]
        Immediate = 1,
        [Display(Name = "معمولی")]
        Ordinary = 2,
        [Display(Name = "جهت اطلاع")]
        justForInformation = 3,
    }
    public enum TicketStatus
    {
        [Display(Name = "فرستنده_یوزر_پاسخ_نگرفته")]
        UserSenderNotReply = 1,
        [Display(Name = "فرستنده_ادمین_پاسخ_گرفته")]
        AdminSenderReply = 2,
        [Display(Name = "فرستنده_یوزر_پاسخ_گرفته")]
        UserSenderReply = 3,
        [Display(Name = "فرستنده_ادمین_پاسخ_نگرفته")]
        AdminSenderNotReply = 4,

    }
    public enum PaymnetType
    {
        [Display(Name = "پرداخت آنلاین")]
        Pay,
        [Display(Name = "هدیه")]
        Gift,
        [Display(Name = "برگشت")]
        BackMoney,
        [Display(Name = "تراکنش دستی ادمین")]
        AdminManual,
        
        [Display(Name = "خرید پلن رایگان")]
        FreePlan,


    }
    public enum SpecialEmpolyee
    {
        [Display(Name = "هیچکدام")]
        None,
        [Display(Name = "مددجوی بهزیستی")]
        WelfareClient,
        [Display(Name = "کمیته امداد")]
        AidCommittee,
        [Display(Name = "بسیج سپاه کربلا")]
        BasijOfKarbalaCorps


    }



    public enum RefrenceTransationStatus
    {
        [Description("در انتظار تعیین وضعییت")]
        [Display(Name = "در انتظار تعیین وضعییت")]
        Pending,
        [Description("تایید شده")]
        [Display(Name = "تایید شده")]
        Accepted,
        [Description("رد شده")]
        [Display(Name = "رد شده")]
        Rejected,
    }




    public enum DepositStatus
    {
        [Display(Name = "پرداخت شده")]
        Deposit,
        [Display(Name = "برداشت شده")]
        Withdrawl
    }

    public enum BlogType
    {
        [Display(Name = "کارفرما")]
        Employer,
        [Display(Name = "کارجو")]
        Employee,
        [Display(Name = "عمومی")]
        Users,
        [Display(Name = "اخبار")]
        News
    }
    public enum ReportAdvertStatus
    {
        [Display(Name = "ثبت شده")]
        Saved,
        [Display(Name = "حذف شده")]
        Deleted
    }
    public enum ReportAdvertType
    {
        [Display(Name = " آگهی غیر واقعی ")]
        FalseAdvert,
        [Display(Name = "  صاحب آگهی در دسترس نیست ")]
        EmployerNonExistent,
        [Display(Name = "  عکاسی نامناسب  ")]
        BadImage,
        [Display(Name = "   گروه بندی نامناسب  ")]
        BadCategorizing,
        [Display(Name = "    آگهی پیدا نشد ")]
        NotFount404,
        [Display(Name = " محصول فروخته شده / اجاره رفته  ")]
        AlreadyBooked,
        [Display(Name = "  قیمت نادرست  ")]
        WrongPricing,
        [Display(Name = "   شماره تماس نادرست ")]
        WrongPhoneNumber,
        [Display(Name = "    آگهی نامناسب ")]
        BadAdvert,
        [Display(Name = "  دیگر ")]
        Others
    }
    public enum NotificationType
    {
        Advert,
        Resome,
        Ticket
    }
    public enum EmailNotificationSendTime
    {
        Daily,
        Weekly
    }

}
