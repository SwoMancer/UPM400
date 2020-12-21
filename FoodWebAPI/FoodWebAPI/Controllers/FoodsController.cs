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

namespace FoodWebAPI.Controllers
{
    public class FoodsController : ApiController
    {
        private FoodDBEntities db = new FoodDBEntities();

        // GET: /Foods
        public IQueryable<Food> GetFood()
        {
            return db.Food;
        }

        // GET: /Foods/5
        [ResponseType(typeof(Food))]
        public async Task<IHttpActionResult> GetFood(int id)
        {
            Food food = await db.Food.FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }

            return Ok(food);
        }

        // PUT: /Foods/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFood(int id, Food food)
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
        [ResponseType(typeof(Food))]
        public async Task<IHttpActionResult> PostFood(Food food)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Food.Add(food);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = food.Id }, food);
        }

        // DELETE: /Foods/5
        [ResponseType(typeof(Food))]
        public async Task<IHttpActionResult> DeleteFood(int id)
        {
            Food food = await db.Food.FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }

            db.Food.Remove(food);
            await db.SaveChangesAsync();

            return Ok(food);
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