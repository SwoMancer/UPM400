using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FoodAPI.Models
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreationTime { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerAdress { get; set; }
        public string CustomerZIP { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public City Id_City { get; set; }
    }
}