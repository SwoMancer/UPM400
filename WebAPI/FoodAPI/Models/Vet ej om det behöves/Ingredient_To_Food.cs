using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FoodAPI.Models.Vet_ej_om_det_behöves
{
    public class Ingredient_To_Food
    {
        public int Id { get; set; }

        public int Id_Food { get; set; }
        public int Id_Ingredient { get; set; }
    }
}