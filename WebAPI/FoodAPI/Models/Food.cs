using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodAPI.Models
{
    public class Food
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Price { get; set; }
        public Restaurant Id_Restaurant { get; set; }
    }
}