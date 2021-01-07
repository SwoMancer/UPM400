using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using FoodWebAPI.DB;
using FoodWebAPI.Models;

namespace FoodWebAPI.Controllers
{
    public class BondsController : ApiController
    {
        private FoodDBEntities db = new FoodDBEntities();
        [Route("bonds/Order_To_Food")]
        public Answer OrToFo(int idOrder, int idFood)
        {
            try
            {
                Order_To_Food to = new Order_To_Food();
                to.Id_Food = idFood;
                to.Id_Order = idOrder;

                db.Order_To_Food.Add(to);
                db.SaveChanges();

                return Answer.Complete("Allt är bra");
            }
            catch (Exception ex)
            {
                return Answer.Error(ex.Message);
            }
        }
        [Route("bonds/Order_To_Drink")]
        public Answer OrToDr(int idOrder, int idDrink)
        {
            try
            {
                Order_To_Drink to = new Order_To_Drink();
                to.Id_Drink = idDrink;
                to.Id_Order = idOrder;

                db.Order_To_Drink.Add(to);
                db.SaveChanges();

                return Answer.Complete("Allt är bra");
            }
            catch (Exception ex)
            {
                return Answer.Error(ex.Message);
            }
        }
        [Route("bonds/Ingredient_To_Food")]
        public Answer InToFo(int idFood, int idIngredient)
        {
            try
            {
                Ingredient_To_Food to = new Ingredient_To_Food();
                to.Id_Food = idFood;
                to.Id_Ingredient = idIngredient;

                db.Ingredient_To_Food.Add(to);
                db.SaveChanges();

                return Answer.Complete("Allt är bra");
            }
            catch (Exception ex)
            {
                return Answer.Error(ex.Message);
            }
        }
    }
}
