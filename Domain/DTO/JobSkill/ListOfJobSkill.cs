using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class ListOfJobSkill
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string CategoryName { get; set; }
        public bool IsActive { get; set; }
        public string UpdateDate { get; set; }
    }
}
