using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public class CityWithCountDTO
    {
        public string CityName { get; set; }
        public int Count { get; set; }
    }
    public class GetAllResomesInfoWitCount{
        public Dictionary<string, int> Model { get; set; }
        public List<CityWithCountDTO> City { get; set; }
    }
}
