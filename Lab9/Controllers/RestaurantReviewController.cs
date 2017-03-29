using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Lab9.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Xml.Serialization;
using System.Collections;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
// Fix this for RESTFUL api!
namespace Lab9.Controllers
{
    public class RestaurantReviewController : Controller
    {
        private const string RESTAURANTXMLPATH = "xml/restaurant_reviews.xml";

        private readonly IHostingEnvironment _env;

        public RestaurantReviewController(IHostingEnvironment env)
        {
            _env = env;
        }
        // /restaurantreview/GetRestaurantNames
        [HttpGet]
        public IEnumerable<RestaurantReview> GetRestaurantNames()
        {
            var allRestaurants = GetXMLToList();
            return allRestaurants.Select(restaurant => new RestaurantReview(restaurant));
        }

        // /restaurantreview/GetRestaurantByName/name
        [HttpGet]
        public IActionResult GetRestaurantByName(string id)
        {
            //read xml into an object
            var allRestaurants = GetXMLToList();

            var selectedRestaurant = allRestaurants
                .Select(restaurant => new RestaurantReview(restaurant))
                .Single(restaurant => restaurant.Name == id);

            return new ObjectResult(selectedRestaurant);
        }
        [HttpPost]
        public IActionResult SaveRestaurant(RestaurantReview restaurant)
        {
            var allRestaurants = GetXMLToList();
            var restaurantToSave = allRestaurants.SingleOrDefault(r => restaurant.Name == r.Name);
            restaurantToSave.Summary = restaurant.Summary;
            restaurantToSave.Rating = restaurant.Rating;

            if (SaveToXML(allRestaurants))
                return new ObjectResult(new { Message = "Successfully saved data to: restaurant_reviews.xml" });
            else
                return new ObjectResult(new { Message = "Failed to save to xml." });
        }
      
        protected List<RestaurantsRestaurant> GetXMLToList(string path = RESTAURANTXMLPATH)
        {
            path = Path.Combine(_env.WebRootPath, path);
            using (var fs = new FileStream(path, FileMode.Open))
            {
                var xmlSerializer = new XmlSerializer(typeof(Restaurants));
                return ((Restaurants)xmlSerializer.Deserialize(fs)).Restaurant.ToList();
            }
        }
        protected bool SaveToXML(List<RestaurantsRestaurant> restos, string path = RESTAURANTXMLPATH)
        {
            path = Path.Combine(_env.WebRootPath, path);
            bool success = false;
            RestaurantsRestaurant[] restaurantsProperty = restos.ToArray();
            Restaurants restaurants = new Restaurants()
            {
                Restaurant = restaurantsProperty
            };
            try
            {
                using (var fs = new FileStream(path, FileMode.Create))
                {
                    var xmlSerializer = new XmlSerializer(typeof(Restaurants));
                    xmlSerializer.Serialize(fs, restaurants);
                    success = true;
                }
            }
            catch
            {
                // to make sure it's caught is all
            }
            return success;

        }

    }
}
