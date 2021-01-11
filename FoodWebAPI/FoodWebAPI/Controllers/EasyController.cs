using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using FoodWebAPI.DB;
using FoodWebAPI.Models;

namespace FoodWebAPI.Controllers
{
    public class EasyController : ApiController
    {
        #region props
        private FoodDBEntities db = new FoodDBEntities();
        private Object lockObject = new object();
        #endregion
        #region city

        #region get
        [ResponseType(typeof(Answer))]
        [Route("easy/city/get/all")]
        public async Task<IHttpActionResult> GetAll()
        {
            try
            {
                List<DB.City> cities = db.City.ToList();

                return Ok(Answer.Complete(cities));

            }
            catch (Exception ex)
            {
                return Ok(Answer.Error(ex));
            }
        }
        [ResponseType(typeof(Answer))]
        [Route("easy/city/get/onebyname")]
        public async Task<IHttpActionResult> GetOneByName(string name)
        {
            try
            {
                name = name.Trim().ToLower();

                List<DB.City> cities = await db.City.Where(n => n.Name == name).ToListAsync();
                

                if (cities.Count <= 0)
                    return Ok(Answer.Error("Kan ej hitta någon med namn: " + name));

                City city = cities.FirstOrDefault();
                city.Order.Clear();
                //city.Restaurant.Clear();

                List<Restaurant> restaurants = new List<Restaurant>();
                for (int r = 0; r < city.Restaurant.Count; r++)
                {
                    Restaurant restaurant = city.Restaurant.ToList()[r];

                    List<Food> foods = new List<Food>();
                    for (int f = 0; f < restaurant.Food.Count; f++)
                    {
                        Food food = restaurant.Food.ToList()[f];

                        List<Ingredient_To_Food> ingredients_To_Foods = new List<Ingredient_To_Food>();
                        for (int fi = 0; fi < food.Ingredient_To_Food.Count(); fi++)
                        {
                            Ingredient_To_Food ingredient_To_Food = food.Ingredient_To_Food.ToList()[fi];

                            ingredient_To_Food.Food = null;
                            ingredient_To_Food.Ingredient.Ingredient_To_Food = null;

                            ingredients_To_Foods.Add(ingredient_To_Food);
                        }
                        food.Ingredient_To_Food.Clear();
                        food.Order_To_Food.Clear();
                        food.Restaurant = null;

                        food.Ingredient_To_Food = ingredients_To_Foods;

                        foods.Add(food);
                    }
                    restaurant.City = null;
                    restaurant.Food.Clear();

                    restaurant.Food = foods;

                    restaurants.Add(restaurant);
                }

                city.Restaurant.Clear();
                city.Restaurant = restaurants;

                return Ok(Answer.Complete(city));
            }
            catch (Exception ex)
            {
                return Ok(Answer.Error(ex));
            }
        }
        [ResponseType(typeof(Answer))]
        [Route("easy/city/get/onebyid")]
        public async Task<IHttpActionResult> GetOneById(int id)
        {
            try
            {
                List<DB.City> cities = await db.City.Where(n => n.Id == id).ToListAsync();

                if (cities.Count <= 0)
                    return Ok(Answer.Error("Kan ej hitta någon med id: " + id));

                return Ok(Answer.Complete(cities.FirstOrDefault()));
            }
            catch (Exception ex)
            {
                return Ok(Answer.Error(ex));
            }
        }
        #endregion
        #endregion
        #region restaurant
        [ResponseType(typeof(Answer))]
        [Route("easy/restaurant/add/one")]
        public async Task<IHttpActionResult> AddOneRestaurant(Models.EasyInputs.EasyRestaurant restaurant)
        {
            try
            {
                if (restaurant is null)
                {
                    return Ok(Answer.Error("EasyRestaurant post vara något fel med. Prova igen med andra värden."));
                }


                List<Restaurant> restaurants = await db.Restaurant
                    .Where(n => n.Name == restaurant.Name)
                    .Where(i => i.Id_City == restaurant.Id_City)
                    .ToListAsync();


                if (restaurants is null || restaurants.Count <= 0)
                {
                    db.Restaurant.Add(restaurant.ToDBRestaurant());
                    await db.SaveChangesAsync();
                }
                else
                {
                    return Ok(Answer.Complete(restaurants.FirstOrDefault()));
                }


                restaurants = await db.Restaurant
                    .Where(n => n.Name == restaurant.Name)
                    .Where(i => i.Id_City == restaurant.Id_City)
                    .ToListAsync();


                return Ok(Answer.Complete(restaurants.FirstOrDefault()));
            }
            catch (Exception ex)
            {
                return Ok(Answer.Error(ex));
            }
        }
        #endregion
        #region food
        [ResponseType(typeof(Answer))]
        [Route("easy/food/add/one")]
        public async Task<IHttpActionResult> AddOneFood(Models.EasyInputs.EasyFoodAndIngredients easyFood)
        {
            try
            {
                if (easyFood is null)
                {
                    return Ok(Answer.Error("Något är fel med värderna som har sänts in. Prova att göra om det med nya värden. Det är fellet kan även komma om encoding är ej satt, om det är fallet prova att ange UTF-8"));
                }
                //Start värden
                List<int> IngredientIds = new List<int>();
                //List<Task<Answer>> answersIngredientTasks = new List<Task<Answer>>();
                //List<Task<Answer>> answerIngredientTasks = new List<Task<Answer>>();

                //Lägg till food som task
                //Task<Answer> foodTask = await AddFood(easyFood);

                #region gammal kod
                /*
                //Lägg till alla ing task
                foreach (string ingredientName in easyFood.Ingredients)
                    answersIngredientTasks.Add(AddIngredient(ingredientName));

                //Ser till att alla finns och inga problem finns
                foreach (Task<Answer> answerTask in answersIngredientTasks)
                {
                    Answer answer = await answerTask;
                    if (answer.IsASuccess)
                    {
                        IngredientIds.Add((int)answer.Json);
                    }
                    else
                    {
                        return Ok(answer);
                    }
                }
                */
                #endregion

                foreach (string ingredientName in easyFood.NameOfIngredients)
                {
                    Answer answer = await AddIngredient(ingredientName);
                    if (answer.IsASuccess)
                    {
                        IngredientIds.Add((int)answer.Json);
                    }
                    else
                    {
                        return Ok(answer);
                    }
                }

                Answer foodAnswer = await AddFood(easyFood);
                if (!foodAnswer.IsASuccess)
                {
                    return Ok(foodAnswer);
                }

                //Om här så har inga fel kommit fram. Går att gå vidare...

                int foodId = (int)foodAnswer.Json;

                #region gammal kod
                /*
                foreach (int IngredientId in IngredientIds)
                    answerIngredientTasks.Add(AddBondFoodAndIngredient(foodId, IngredientId));

                foreach (Task<Answer> IngredientTask in answerIngredientTasks)
                {
                    Answer answer = await IngredientTask;
                    if (!answer.IsASuccess)
                    {
                        //om något går fel med Ingredient
                        return Ok(answer);
                    }
                }
                */
                #endregion

                foreach (int IngredientId in IngredientIds)
                {
                    Answer answer = await AddBondFoodAndIngredient(foodId, IngredientId);
                    if (!answer.IsASuccess)
                    {
                        //om något går fel med Ingredient
                        return Ok(answer);
                    }
                }

                Food food = await db.Food.FindAsync(foodId);

                return Ok(Answer.Complete(food));
            }
            catch (Exception ex)
            {
                return Ok(Answer.Error(ex));
            }
        }
        #endregion
        #region Order
        [ResponseType(typeof(Answer))]
        [Route("easy/order/add/one")]
        public async Task<IHttpActionResult> AddOneOrder(Models.EasyInputs.EasyOrderAndFoodsIdsAndDrinksIds orderFoodIDs)
        {
            return Ok(await AddOrderAndItsFoods(orderFoodIDs));
        }
        #endregion
        #region Private method
        #region Order
        private async Task<Answer> AddOrderAndItsFoods(Models.EasyInputs.EasyOrderAndFoodsIdsAndDrinksIds orderFoodIDs)
        {
            try
            {
                //Lägg till order
                Answer orderAnswer = await AddOrder(orderFoodIDs.ToEasyOrder());
                if (!orderAnswer.IsASuccess)
                    return orderAnswer;

                Order order = (Order)orderAnswer.Json;

                //Lägg till bonds mellan order och foods
                foreach (int foodId in orderFoodIDs.Foods_Ids)
                {
                    Answer answerFoodBond = await AddBondOrderAndFood(foodId, order.Id, "");
                    if (!answerFoodBond.IsASuccess)
                        return answerFoodBond;
                }

                //Lägg till bonds mellan order och drinks
                foreach (int drinkId in orderFoodIDs.Drinks_Ids)
                {
                    Answer answerDrinkBond = await AddBondOrderAndDrink(drinkId, order.Id);
                    if (!answerDrinkBond.IsASuccess)
                        return answerDrinkBond;
                }

                return Answer.Complete(await db.Order.FindAsync(order.Id));
            }
            catch (Exception ex)
            {
                return Answer.Error(ex);
            }
        }
        private async Task<Answer> AddOrder(Models.EasyInputs.EasyOrder order)
        {
            try
            {
                Order inputOrder = order.ToDBOrder();


                db.Order.Add(inputOrder);
                await db.SaveChangesAsync();

                return Answer.Complete(db.Order.LastOrDefault());
            }
            catch (Exception ex)
            {
                return Answer.Error(ex);
            }
        }
        #endregion
        #region bonds
        private async Task<Answer> AddBondOrderAndDrink(int idDrink, int idOrder)
        {
            try
            {
                Order_To_Drink order_To_Drink = new Order_To_Drink();

                order_To_Drink.Id_Drink = idDrink;
                order_To_Drink.Id_Order = idOrder;

                db.Order_To_Drink.Add(order_To_Drink);
                await db.SaveChangesAsync();

                order_To_Drink = db.Order_To_Drink.Last();

                return Answer.Complete(order_To_Drink);
            }
            catch (Exception ex)
            {
                return Answer.Error(ex);
            }
        }
        private async Task<Answer> AddBondOrderAndFood(int idFood, int idOrder, string customerMessage)
        {
            try
            {
                Order_To_Food order_To_Food = new Order_To_Food();

                order_To_Food.CustomerMessage = customerMessage;
                order_To_Food.Id_Food = idFood;
                order_To_Food.Id_Order = idOrder;

                db.Order_To_Food.Add(order_To_Food);
                await db.SaveChangesAsync();

                order_To_Food = db.Order_To_Food.Last();

                return Answer.Complete(order_To_Food);
            }
            catch (Exception ex)
            {
                return Answer.Error(ex);
            }
        }
            private async Task<Answer> AddBondFoodAndIngredient(int idFood, int idIngredient)
        {
            Food dbFood = new Food();
            Ingredient dbIngredient = new Ingredient();
            Ingredient_To_Food dbIngredient_To_Food = new Ingredient_To_Food();

            Task<Food> taskFood = db.Food.FindAsync(idFood);
            Task<Ingredient> taskIngredient = db.Ingredient.FindAsync(idIngredient);

            dbIngredient_To_Food.Id_Food = idFood;
            dbIngredient_To_Food.Id_Ingredient = idIngredient;

            try
            {
                dbFood = await taskFood;
                if (dbFood is null)
                {
                    return Answer.Error("Kan ej finna food med det här id: " + idFood);
                }

                dbIngredient = await taskIngredient;
                if (dbIngredient is null)
                {
                    return Answer.Error("Kan ej finna ingredient med det här id: " + idIngredient);
                }
            }
            catch (Exception ex)
            {
                return Answer.Error(ex);
            }

            try
            {
                dbIngredient_To_Food.Id_Food = dbFood.Id;
                dbIngredient_To_Food.Id_Ingredient = dbIngredient.Id;

                db.Ingredient_To_Food.Add(dbIngredient_To_Food);
                await db.SaveChangesAsync();

                List<Ingredient_To_Food> dbIngredient_To_FoodId = new List<Ingredient_To_Food>();


                dbIngredient_To_FoodId = await db.Ingredient_To_Food
                    .Where(idf => idf.Id_Food == dbFood.Id)
                    .Where(idi => idi.Id_Ingredient == dbIngredient.Id)
                    .ToListAsync();


                return Answer.Complete(dbIngredient_To_FoodId.FirstOrDefault().Id);
            }
            catch (Exception ex)
            {
                return Answer.Error(ex);
            }

        }

        #endregion
        #region food
        private async Task<Answer> AddFood(Models.EasyInputs.EasyFoodAndIngredients easyFood)
        {
            try
            {
                if (easyFood is null)
                {
                    return Answer.Error("easyFood post vara något fel med. Prova igen med andra värden.");
                }

                List<Food> foods = new List<Food>();


                foods = await db.Food
                .Where(n => n.Name == easyFood.Name)
                .Where(i => i.Price == easyFood.Price)
                .Where(r => r.Id_Restaurant == easyFood.Id_Restaurant)
                .ToListAsync();


                if (foods is null || foods.Count <= 0)
                {
                    db.Food.Add(easyFood.ToDBFood());
                    await db.SaveChangesAsync();
                }
                else
                {
                    return Answer.Complete(foods.FirstOrDefault().Id);
                }

                foods = await db.Food
                .Where(n => n.Name == easyFood.Name)
                .Where(i => i.Price == easyFood.Price)
                .Where(r => r.Id_Restaurant == easyFood.Id_Restaurant)
                .ToListAsync();


                return Answer.Complete(foods.FirstOrDefault().Id);
            }
            catch (Exception ex)
            {
                return Answer.Error(ex);
            }

        }
        #endregion
        #region ingredient
        private async Task<Answer> AddIngredient(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                return Answer.Error("Ingredient name kan ej vara null, tom eller bara white space");
            }

            try
            {
                List<DB.Ingredient> ingredients = new List<Ingredient>();


                ingredients = await db.Ingredient
                .Where(n => n.Name == name)
                .ToListAsync();


                if (ingredients is null || ingredients.Count <= 0)
                {
                    Ingredient dbIngredient = new Ingredient();

                    dbIngredient.Name = name;

                    db.Ingredient.Add(dbIngredient);
                    await db.SaveChangesAsync();


                    ingredients = await db.Ingredient
                    .Where(n => n.Name == name)
                    .ToListAsync();


                    return Answer.Complete(ingredients.FirstOrDefault().Id);
                }
                else
                {
                    return Answer.Complete(ingredients.FirstOrDefault().Id);
                }
            }

            catch (Exception ex)
            {
                ErrorLog log = new ErrorLog();

                log.exception = ex;
                log.messages = "name: " + name + ".";

                return Answer.Error(log);
            }

        }
        #endregion
        #endregion
    }
}
