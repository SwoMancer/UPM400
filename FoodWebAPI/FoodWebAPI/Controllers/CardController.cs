using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FoodWebAPI.Models;

namespace FoodWebAPI.Controllers
{
    public class CardController : ApiController
    {
        // GET api/values/5
        public Answer Post(Card cardInfo)
        {
            return cardInfo.IsCardValid();
        }
    }
}
