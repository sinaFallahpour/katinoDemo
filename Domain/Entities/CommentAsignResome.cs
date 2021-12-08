using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
   public class CommentAsignResome
    {
        public int Id { get; set; }
        public DateTime Date{ get; set; }
        public string Comment { get; set; }
        public int AsignResomeId { get; set; }
        public AsignResome AsignResome { get; set; }
    }
}
