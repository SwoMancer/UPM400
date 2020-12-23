using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using FoodWebAPI;
using FoodWebAPI.Models;
using FoodWebAPI.DB;
using System.Data.Entity;

namespace FoodWebAPI.Containers
{
    public static class FoodBlock
    {
        private static readonly FoodDBEntities db = new FoodDBEntities();

        public static async Task<List<FoodImg>> GetFood()
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

            return mFoods;
        }
        public static async Task<List<FoodImg>> GetFood(Restaurant restaurant)
        {
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

            return mFoods;
        }
        public static async Task<FoodImg> PostFood(Food food)
        {
            db.Food.Add(food);
            FoodImg imgFood = FoodImg.ToFood(food);
            Task imgCallTask = imgFood.getImages();

            await db.SaveChangesAsync();
            await imgCallTask;

            return imgFood;
        }
        public static async Task<FoodImg> PutFood(Food food)
        {
            var item = db.Food.Find(food);
            item.Name = food.Name;

            FoodImg imgFood = FoodImg.ToFood(food);

            await db.SaveChangesAsync();

            return imgFood;
        }
        public static async Task<FoodImg> DeleteFood(Food food)
        {
            FoodImg imgFood = FoodImg.ToFood(food);
            Task imgCallTask = imgFood.getImages();

            db.Food.Remove(food);
            await db.SaveChangesAsync();
            await imgCallTask;

            return imgFood;
        }
        public static async Task<FoodImg> GetFood(int id)
        {
            return FoodImg.ToFood(await db.Food.FindAsync(id));
        }
    }
}