using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using FoodWebAPI.DB;
using FoodWebAPI.Models;

namespace FoodWebAPI.Controllers
{
    public class FoodsController : ApiController
    {
        private FoodDBEntities db = new FoodDBEntities();

        // GET: /Foods
        [ResponseType(typeof(List<FoodImg>))]
        public async Task<IHttpActionResult> GetFood()
        {
            IQueryable<Food> foods = db.Food;
            List<FoodImg> mFoods = FoodImg.ToFoodList(foods.ToList());
            List<Task> tasks = new List<Task>();

            //Hittar alla bilder
            for (int i = 0; i < mFoods.Count; i++)
            {
                Task task = mFoods[i].getImages();
                tasks.Add(task);
            }

            //Väntar på det...

            for (int i = 0; i < tasks.Count; i++)
            {
                await tasks[i];
            }

            return Ok(mFoods);
        }

        // GET: /Foods
        [ResponseType(typeof(List<FoodImg>))]
        public async Task<IHttpActionResult> GetFood(Restaurant restaurant)
        {
            if (db.Restaurant.Contains(restaurant))
                return Ok(new List<FoodImg>());

            List<Food> foods = await db.Food.Where(r => r.Id_Restaurant == restaurant.Id).ToListAsync();
            List<FoodImg> mFoods = FoodImg.ToFoodList(foods);
            List<Task> tasks = new List<Task>();

            //Hittar alla bilder
            for (int i = 0; i < mFoods.Count; i++)
            {
                Task task = mFoods[i].getImages();
                tasks.Add(task);
            }

            //Väntar på det...

            for (int i = 0; i < tasks.Count; i++)
            {
                await tasks[i];
            }

            return Ok(mFoods);
        }

        // GET: /Foods/5
        [ResponseType(typeof(FoodImg))]
        public async Task<IHttpActionResult> GetFood(int id)
        {
            FoodImg food = FoodImg.ToFood(await db.Food.FindAsync(id));
            if (food == null)
            {
                return NotFound();
            }

            await food.getImages();

            return Ok(FoodImg.ToFood(food));
        }

        // PUT: /Foods/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFood(int id, DB.Food food)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != food.Id)
            {
                return BadRequest();
            }

            db.Entry(food).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FoodExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: /Foods
        [ResponseType(typeof(FoodImg))]
        public async Task<IHttpActionResult> PostFood(Food food)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Food.Add(food);
            FoodImg imgFood = FoodImg.ToFood(food);
            Task imgCallTask = imgFood.getImages();

            await db.SaveChangesAsync();
            await imgCallTask;

            return CreatedAtRoute("DefaultApi", new { id = food.Id }, imgFood);
        }

        // DELETE: /Foods/5
        [ResponseType(typeof(FoodImg))]
        public async Task<IHttpActionResult> DeleteFood(int id)
        {
            Food food = await db.Food.FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }

            FoodImg imgFood = FoodImg.ToFood(food);
            Task imgCallTask = imgFood.getImages();

            db.Food.Remove(food);
            await db.SaveChangesAsync();
            await imgCallTask;

            return Ok(FoodImg.ToFood(imgFood));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FoodExists(int id)
        {
            return db.Food.Count(e => e.Id == id) > 0;
        }
    }
}