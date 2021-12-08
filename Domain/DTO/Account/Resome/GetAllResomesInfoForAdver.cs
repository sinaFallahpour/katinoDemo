using Domain.DTO.Response;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace Domain
{
    public class GetAllResomesInfoForAdver
    {
        public int Id { get; set; }
        public int AsignResomeId { get; set; }
        public int ResomeId { get; set; }
        public string FullName { get; set; }
        public string JobTitle { get; set; }
        public string UserAvatar { get; set; }
        public AsingResomeStatus  AsingResomeStatus{ get; set; }
        public string Date { get; set; }
        public bool HasComment { get; set; }
        public bool IsMark { get; set; }
        public Gender Gender { get; set; }
        public string City { get; set; }
        public Senioritylevel Senioritylevel { get; set; }
        public SpecialEmpolyee? SpecialEmpolyee { get; set; }


    }
    public class GetAllResomesInfoForAdverForAdmin
    {
        public int Id { get; set; }
        public int AsignResomeId { get; set; }
        public int ResomeId { get; set; }
        public User Employee { get; set; }
        public User Company { get; set; }
        public JobAdvertisement JobAdvertisement { get; set; }
        public Resome Resome { get; set; }
        public string FullName { get; set; }
        public string JobTitle { get; set; }
        public string UserAvatar { get; set; }
        public AsingResomeStatus  AsingResomeStatus{ get; set; }
        public string Date { get; set; }
        public bool HasComment { get; set; }
        public bool IsMark { get; set; }
        public Gender Gender { get; set; }
        public string City { get; set; }
        public Senioritylevel Senioritylevel { get; set; }
        public SpecialEmpolyee? SpecialEmpolyee { get; set; }


    }
    public class FilterAsingResome{
        public int AdverId { get; set; }
        public string SeacrchKey { get; set; }
        public bool? HasComment{ get; set; }
        public bool? IsMarked { get; set; }
        public List<AsingResomeStatus> AsingResomeStatuses { get; set; }
        public List<Gender> Genders { get; set; }
        public List<string> Cities { get; set; }
        public List<Senioritylevel> Seniorityleveles { get; set; }
        public List<SpecialEmpolyee> SpecialEmpolyees{ get; set; }


    }
}
