using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class ListOfUserJobPreferences : EditUserJobPreferencesDTO
    {
        public List<CategoryForJobPrefence> CategoryForJobPrefence { get; set; }

    }
}
