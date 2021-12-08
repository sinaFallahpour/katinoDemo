export const APIs = {
    getAllJobCategories: "https://panel.katinojob.ir/api/Categories/GetAllCategories2",
    getAllCities: "https://panel.katinojob.ir/api/Account/GetCities",
    subscribeInGetNewAdvers : "https://panel.katinojob.ir/api/EmailNotifications/SaveEmailNotificationSetting",
    getPersonalInfo : "https://panel.katinojob.ir/api/Account/LoadEmployeePersonalInformation"
};

export const salaries = {
    selectionType: "single",
    list: [
        { value: 1, label: "کمتر از 1 میلیون" },
        { value: 2, label: "بین 1 تا 2.5 میلیون" },
        { value: 3, label: "بین 2.5 تا 3.5 میلیون" },
        { value: 4, label: "بین 3.5 تا 5 میلیون" },
        { value: 5, label: "بین 5 تا 8 میلیون" },
        { value: 6, label: "بیشتر از 8 میلیون" },
        { value: 7, label: "توافقی" },
        { value: 8, label: "طبق قانون کار" },
    ]
};

// public enum Salary
//     {
//         [Display(Name = "کمتر از 1 میلیون")]
//         LesThan1Mili = 1,
//         [Display(Name = "بین 1 تا 2.5 میلیون")]
//         Between1And2_5 = 2,
//         [Display(Name = "بین 2.5 تا 3.5 میلیون")]
//         Between2_5And3_5 = 3,
//         [Display(Name = "بین 3.5 تا 5 میلیون")]
//         Between3_5And5 = 4,
//         [Display(Name = "بین 5 تا 8 میلیون")]
//         Between5And8 = 5,
//         [Display(Name = "بیشتر از 8 میلیون")]
//         MoreThan8 = 6,
//         [Display(Name = "توافقی")]
//         Agreement = 7,
//         [Display(Name = "طبق قانون کار")]
//         AccordingToLaborLaw = 8
//     }


export const typesOfCooperation = {
    selectionType: "single",
    list: [
        { value: 1, label: "تمام وقت" },
        { value: 2, label: "پاره وقت" },
        { value: 3, label: "کارآموزی" },
        { value: 4, label: "دورکاری" },
    ]
};


export const senioritylevels = {
    selectionType: "single",
    list: [

        { value: 0, label: "مهم نیست" },
        { value: 1, label: "کمتر از سه سال" },
        { value: 2, label: "بین 3 تا 7 سال" },
        { value: 3, label: "بیشتر از 7 سال" },
    ]
};

export const jobPreferenceItemKeys = [
    { value: "salary", label: 'میزان حقوق' },
    { value: "senioritylevel", label: 'سابقه کار' },
    { value: "typeOfCooperation", label: 'نوع همکاری' },
    { value: "city", label: 'شهر' },
    { value: "promotion", label: 'ارتقا شغلی' },
    { value: "insurance", label: 'بیمه' },
    { value: "educationCourses", label: 'مدرک تحصیلی', },
    { value: "flexibleWorkingTime", label: 'انعطاف پذیر بودن ساعت کاری' },
    { value: "hasMeel", label: 'همراه با وعده غذایی' },
    { value: "transportationService", label: 'همراه با سرویس رفت و آمد' },
    { value: "categoryForJobPrefence", label: 'دسته شغلی' }
];

export const cities = {
    selectionType: "single",
    list: []
};


export const jobCategories = {
    list: [],
    selectionType: "multi"
};
