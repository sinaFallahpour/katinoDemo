using Domain.DTO.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain
{
    public class AllAdver
    {
        public AllAdver()
        {

        }
        public int Id { get; set; }
        public string Title { get; set; }
        public string City { get; set; }
        public TypeOfCooperation TypeOfCooperation { get; set; }
        public Salary Salary { get; set; }
        public WorkExperience WorkExperience { get; set; }
        public DegreeOfEducation DegreeOfEducation { get; set; }
        public Gender Gender { get; set; }
        public string Military { get; set; }
        public string DescriptionOfJob { get; set; }
        public string FeildOfActivity { get; set; }
        public int FeildOfActivityId { get; set; }
        public string CompanyName { get; set; }
        public string IsImmediate { get; set; }
        public AdverStatus AdverStatus { get; set; }
        public bool IsMarked { get; set; }
        public bool IsStorySaz { get; set; }
        public SpecialEmpolyee SpecialEmpolyee { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public int ResomeColorId { get; set; }
        public bool IsActive { get; set; }

    }

    //public class Company
    //{
    //    public string verificationCode { get; set; }
    //    public string UserName { get; set; }
    //}

    public class AllAdverForAdmin : AllAdver
    {

        //public string verificationCode { get; set; }
        //public string UserName { get; set; }


        public User Company { get; set; }
        public AdverCreatationStatus AdverCreatationStatus { get; set; }
    }
    public class AllAdverForCurrectUser : AllAdverForAdmin
    {
        public string AdminDescription { get; set; }
        public List<AsignStatusWithCount> AsignStatusWithCounts { get; set; }

    }
    public class AdverDetail : AllAdver
    {
        public string CompanyDescription { get; set; }
        public bool IsAsignResomeToThisAdver { get; set; }
    }

    public class AsignStatusWithCount
    {
        public AsingResomeStatus AsingResomeStatus { get; set; }
        public int Count { get; set; }
    }
}
