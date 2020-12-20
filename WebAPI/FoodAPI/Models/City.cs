using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace FoodAPI.Models
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<int> PK_Order_Ids { get; set; }
        public List<int> PK_Restaurant_Ids { get; set; }

        public City() { }
        public City(DB.City dbCity) 
        {
            this.Id = dbCity.Id;
            this.Name = dbCity.Name;

            List<int> ids = new List<int>();
            if (dbCity.Restaurant.Count >= 1)
            {
                foreach (DB.Restaurant restaurant in dbCity.Restaurant)
                {
                    ids.Add(restaurant.Id);
                }
                this.PK_Restaurant_Ids = ids;
            }
            else
            {
                this.PK_Restaurant_Ids = new List<int>();
            }

            ids = new List<int>();
            if (dbCity.Order.Count >= 1)
            {
                foreach (DB.Order order in dbCity.Order)
                {
                    ids.Add(order.Id);
                }
                this.PK_Order_Ids = ids;
            }
            else
            {
                this.PK_Order_Ids = new List<int>();
            }
        }

        public static City ToCity(DB.City dbCity)
        {
            City city = new City();
            city.Id = dbCity.Id;
            city.Name = dbCity.Name;

            List<int> ids = new List<int>();
            if (dbCity.Restaurant.Count >= 1)
            {
                foreach (DB.Restaurant restaurant in dbCity.Restaurant)
                {
                    ids.Add(restaurant.Id);
                }
                city.PK_Restaurant_Ids = ids;
            }
            else
            {
                city.PK_Restaurant_Ids = new List<int>();
            }

            ids = new List<int>();
            if (dbCity.Order.Count >= 1)
            {
                foreach (DB.Order order in dbCity.Order)
                {
                    ids.Add(order.Id);
                }
                city.PK_Order_Ids = ids;
            }
            else
            {
                city.PK_Order_Ids = new List<int>();
            }

            return city;
        }
        public static List<City> ToCityList(DbSet<DB.City> dbSetCitys)
        {
            List<City> citys = new List<City>();

            foreach (DB.City dbCity in dbSetCitys)
            {
                citys.Add(new City(dbCity));
            }
            return citys;
        }

        public static DB.City ReverseCity(City city)
        {
            DB.City dbCity = new DB.City();
            dbCity.Id = city.Id;
            dbCity.Name = city.Name;

            return dbCity;
        }
    }
}