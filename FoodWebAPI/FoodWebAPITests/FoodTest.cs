using FoodWebAPI.DB;
using FoodWebAPI.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace FoodWebAPITests
{
    [TestClass]
    public class FoodTest
    {
        FoodWebAPI.Controllers.FoodsController foodsController = new FoodWebAPI.Controllers.FoodsController();

        [TestMethod]
        public async void Get_Food()
        {
            // Act
            IHttpActionResult result = await foodsController.GetFood(3);

            // Assert
            Assert.IsInstanceOfType(result, typeof(Food));
        }

        [TestMethod]
        public async void Get_Food_Error()
        {
            // Act
            IHttpActionResult result = await foodsController.GetFood(100);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Get_All_Food() 
        {
            IQueryable<Food> result = (IQueryable<Food>) foodsController.GetFood();
            Assert.IsTrue(result.Count() >= 1);
        }

        [TestMethod]
        public async void Put_FoodAsync()
        {
            // Act
            Food newFood = new Food();
            newFood.Name = "Köttbullar med mos";
            Food updatedFood = (Food)await foodsController.PutFood(1, newFood);

            // Assert
            Assert.AreEqual("Köttbullar med mos", updatedFood.Name);
        }

        [TestMethod]
        public void Delete_Food()
        {
            // Act
            var deletedFood = foodsController.DeleteFood(3);

            // Assert
            Assert.IsNull(deletedFood);
        }

        [TestMethod]
        public void Post_City()
        {
            // Act
            Food newFood = new Food();
            newFood.Name = "Pizza";
            var newF = foodsController.PostFood(newFood);

            // Assert
            Assert.IsNotNull(newF);

        }
    }
}
