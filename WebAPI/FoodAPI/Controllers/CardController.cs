using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FoodAPI.Models.Call;

namespace FoodAPI.Controllers
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
