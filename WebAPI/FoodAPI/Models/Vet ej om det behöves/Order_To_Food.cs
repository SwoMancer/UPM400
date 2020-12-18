using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodAPI.Models.Vet_ej_om_det_behöves
{
    public class Order_To_Food
    {
        public int Id { get; set; }
        public string CustomerMessage { get; set; }

        public int Id_Order { get; set; }
        public int Id_Food { get; set; }
    }
}