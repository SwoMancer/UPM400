using FlickerAPIClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodWebAPITests
{
    [TestClass]
    public class APITest
    {
       [TestMethod]
        public void GetAllImages()
        {
            Task task = GetAllImagesAsync();
            task.Wait();
        }

    
        private async Task<string[]> GetAllImagesAsync()
        {
            Answer AllImages = await API.GetAll(new string[] { "Hamburger" });
            string[] AllImg = (string[]) AllImages.Json;

            return AllImg;
        }
    }
}
