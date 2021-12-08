using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class ListOfLanguagesForSelect
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class ListOfLanguagesForAdmin: ListOfLanguagesForSelect
    {
        public string UpdateDate { get; set; }
        public bool IsActive { get; set; }
    }
}
