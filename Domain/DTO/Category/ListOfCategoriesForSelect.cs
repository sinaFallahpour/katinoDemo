using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class ListOfCategoriesForSelect
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class ListOfCategories:ListOfCategoriesForSelect
    {
        public string ParentName { get; set; }
        public bool IsActive { get; set; }
        public string UpdateDate { get; set; }
    }
}
