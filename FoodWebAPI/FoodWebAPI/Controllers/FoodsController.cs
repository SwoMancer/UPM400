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
        [ResponseType(typeof(List<Models.FoodImg>))]
        public async Task<IHttpActionResult> GetFood()
        {
            IQueryable<DB.Food> foods = db.Food;
            List<Models.FoodImg> mFoods = Models.FoodImg.ToFoodList(foods.ToList());
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
        [ResponseType(typeof(Models.FoodImg))]
        public async Task<IHttpActionResult> GetFood(int id)
        {
            Models.FoodImg food = Models.FoodImg.ToFood(await db.Food.FindAsync(id));
            if (food == null)
            {
                return NotFound();
            }

            await food.getImages();

            return Ok(Models.FoodImg.ToFood(food));
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
        [ResponseType(typeof(Models.FoodImg))]
        public async Task<IHttpActionResult> PostFood(DB.Food food)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Food.Add(food);
            Models.FoodImg imgFood = Models.FoodImg.ToFood(food);
            Task imgCallTask = imgFood.getImages();

            await db.SaveChangesAsync();
            await imgCallTask;

            return CreatedAtRoute("DefaultApi", new { id = food.Id }, imgFood);
        }

        // DELETE: /Foods/5
        [ResponseType(typeof(Models.FoodImg))]
        public async Task<IHttpActionResult> DeleteFood(int id)
        {
            DB.Food food = await db.Food.FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }

            Models.FoodImg imgFood = Models.FoodImg.ToFood(food);
            Task imgCallTask = imgFood.getImages();

            db.Food.Remove(food);
            await db.SaveChangesAsync();
            await imgCallTask;

            return Ok(Models.FoodImg.ToFood(imgFood));
        }
        /*
        // GET: /Foods
        [ResponseType(typeof(List<FoodImg>))]
        public async Task<IHttpActionResult> GetFood(Restaurant restaurant)
        {
            if (db.Restaurant.Contains(restaurant))
                return Ok(new List<FoodImg>());

            List<DB.Food> foods = await db.Food.Where(r => r.Id_Restaurant == restaurant.Id).ToListAsync();
            List<Models.FoodImg> mFoods = Models.FoodImg.ToFoodList(foods);
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
        */
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