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
        FoodWebAPI.Controllers.FoodsController foodsController = new FoodWebAPI.Controllers.FoodsController(); // Skapar en ny variabel av typen FoodsController från projektet FoodWebAPI

        [TestMethod]
        public async void Get_Food()
        {
            // Act
            IHttpActionResult result = await foodsController.GetFood(3); // Hämtar ett objekt med Id: 3

            // Assert
            Assert.IsInstanceOfType(result, typeof(Food));
        }

        [TestMethod]
        public async void Get_Food_Error()
        {
            // Act
            IHttpActionResult result = await foodsController.GetFood(100); // Försöker hämta ett objekt med Id: 100 vilket inte finns. Detta ska resultera i ett error

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void Get_All_Food() 
        {
            IQueryable<Food> result = (IQueryable<Food>) foodsController.GetFood(); // Hämtar en lista med alla Food
            Assert.IsTrue(result.Count() >= 1); // Om det är fler eller lika med 1 så innebär det att det gick att hämta alla objekt
        }

        [TestMethod]
        public async void Put_FoodAsync()
        {
            //Arrange 
            string Name = "";

            // Act
            Food newFood = new Food(); // Skapar en ny Food
            newFood.Name = "Köttbullar med mos";
            Food updatedFood = (Food) await foodsController.PutFood(1, newFood);

            IQueryable<Food> result = (IQueryable<Food>)foodsController.GetFood(); // Hämtar en lista med alla Food

            foreach (var item in result) // Loopar igenom listan och ser om det finns en Food med Id: 1
            {
                if (item.Id == 1)
                {
                    Name = item.Name;
                }
            }

            // Assert
            Assert.AreEqual("Köttbullar med mos", Name); // Om det finns en Food med Id: 1 så kollar vi om namnet för det objektet har ändrats
        }

        [TestMethod]
        public void Delete_Food()
        {
            // Arrange
            int f_Id = 0;

            int newF_Id = 0;

            // Act
            Food newFood = new Food(); // Skapar en ny Food
            newFood.Name = "Pizza";
            var newF = foodsController.PostFood(newFood);

            IQueryable<Food> result = (IQueryable<Food>)foodsController.GetFood(); // Hämtar en lista med alla Food
            
            foreach (var item in result)  // Loopar igenom listan och ser om det finns en Food med namnet Pizza
            {
                if (item.Name == "Pizza")
                {
                    f_Id = item.Id;      
                }
            }

            var deletedFood = foodsController.DeleteFood(f_Id); // Om objektet hittas så skickas objektets Id till delete funktionen i controllern

            IQueryable<Food> newResult = (IQueryable<Food>)foodsController.GetFood(); // Hämtar en lista med alla Food igen

            foreach (var item in newResult) // Loopar igenom listan och ser om det finns en Food med samma Id som det objektet vi precis skapade
            {
                if (item.Id == f_Id)
                {
                    newF_Id = item.Id;
                }
                else
                {
                    newF_Id = 0;
                }
            }

            // Assert
            Assert.IsTrue(newF_Id < 1); // Om objektet har ett Id mindre än 1 så innebär det att det gick att ta bort objektet
        }

        [TestMethod]
        public void Post_City()
        {
            // Arrange
            int f_count = 0;

            // Act
            Food newFood = new Food(); // Skapar en ny Food
            newFood.Name = "Pizza";
            var newF = foodsController.PostFood(newFood);

            IQueryable<Food> result = (IQueryable<Food>)foodsController.GetFood(); // Hämtar en lista av alla Food

            foreach (var item in result) // Loopar igenom listan och ser om det finns ett objekt med namnet Pizza. Om det gör det så ökar f_count med 1
            {
                if (item.Name == "Pizza")
                {
                    f_count += 1;
                }
            }

            // Assert
            Assert.IsTrue(f_count > 0); // Om f_count är större än 0 så innebär det att det gick att lägga till objektet

        }
    }
}
