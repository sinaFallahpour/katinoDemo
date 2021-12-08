using Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Domain
{
    public class MyAsignListDTO
    {
        public int AsignId { get; set; }
        public int AdverId { get; set; }
        public string AdverTitle { get; set; }
        public string CompanyName { get; set; }
        public string AsignDate { get; set; }
        public AsingResomeStatus AsingResomeStatus { get; set; }
    }
    public class ResomeAsignDetail: MyAsignListDTO
    {
        public double CompletedResomePercent{ get; set; }
        public string PhoneNumber { get; set; }
        public string PdfFile{ get; set; }
        public string KatinoPdfFile { get; set; }
    }
}
