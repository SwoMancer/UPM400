using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodAPI.Models.Vet_ej_om_det_behöves
{
    public class Order_To_Drink
    {
        public int Id { get; set; }

        public int Id_Order { get; set; }
        public int Id_Drink { get; set; }
    }
}