using Domain.DTO.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class AsignResome
    {
        public int Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }

        public AsingResomeStatus AsingResomeStatus { get; set; }

        public string EmployerDescriptioin { get; set; }


        //navigation prop

        public int ResomeId { get; set; }
        public Resome Resome { get; set; }


        public int JobAdvertisementId { get; set; }
        public JobAdvertisement JobAdvertisement { get; set; }

        public ICollection<MarkAsignResome> MarkAsignResomes { get; set; }
        public ICollection<CommentAsignResome> CommentAsignResomes { get; set; }

    }
}
