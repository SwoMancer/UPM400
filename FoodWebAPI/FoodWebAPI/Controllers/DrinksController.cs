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
    public class DrinksController : ApiController
    {
        private FoodDBEntities db = new FoodDBEntities();

        // GET: api/Drinks
        public IQueryable<Drink> GetDrink()
        {
            return db.Drink;
        }

        // GET: api/Drinks/5
        [ResponseType(typeof(Drink))]
        public async Task<IHttpActionResult> GetDrink(int id)
        {
            Drink drink = await db.Drink.FindAsync(id);
            if (drink == null)
            {
                return NotFound();
            }

            return Ok(drink);
        }

        // PUT: api/Drinks/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutDrink(int id, Drink drink)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != drink.Id)
            {
                return BadRequest();
            }

            db.Entry(drink).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DrinkExists(id))
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

        // POST: api/Drinks
        [ResponseType(typeof(Drink))]
        public async Task<IHttpActionResult> PostDrink(Drink drink)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Drink.Add(drink);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = drink.Id }, drink);
        }

        // DELETE: api/Drinks/5
        [ResponseType(typeof(Drink))]
        public async Task<IHttpActionResult> DeleteDrink(int id)
        {
            Drink drink = await db.Drink.FindAsync(id);
            if (drink == null)
            {
                return NotFound();
            }

            db.Drink.Remove(drink);
            await db.SaveChangesAsync();

            return Ok(drink);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DrinkExists(int id)
        {
            return db.Drink.Count(e => e.Id == id) > 0;
        }
    }
}