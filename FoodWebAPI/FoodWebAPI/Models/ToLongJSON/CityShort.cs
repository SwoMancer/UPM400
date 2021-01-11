using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodWebAPI.Models.ToLongJSON
{
    public class CityShort
    {
        public int Id { get; set; }
        public int Name { get; set; }
        public List<RestaurantShort> Restaurant { get; set; }
    }
}