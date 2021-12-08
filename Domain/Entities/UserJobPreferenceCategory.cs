using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class UserJobPreferenceCategory
    {
        public int Id { get; set; }

        //navigation properties
        public int CategoryId { get; set; }
        public Category  Category { get; set; }

        public int UserJobPreferenceId { get; set; }
        public UserJobPreferences UserJobPreferences { get; set; }
    }
}
