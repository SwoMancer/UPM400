using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodAPI.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Popularity { get; set; }
        public City Id_City { get; set; }
    }
}