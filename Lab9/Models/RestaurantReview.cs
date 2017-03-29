using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab9.Models
{
    public class RestaurantReview
    {
        public RestaurantReview()
        {
            //default constructor still needed
        }
        /// <summary>
        /// Creates JSON-ready object for xml
        /// </summary>
        public RestaurantReview(RestaurantsRestaurant restaurant)
        {
            Name = restaurant.Name;
            Address address = new Address()
            {
                City = restaurant.RestaurantAddress.City,
                PostalCode = restaurant.RestaurantAddress.PostalCode,
                Province = restaurant.RestaurantAddress.Province.ToString("G"),
                Street = restaurant.RestaurantAddress.Address
            };
            Address = address;
            Summary = restaurant.Summary;
            Rating = restaurant.Rating;
        }
        public string Name { get; set; }
        public Address Address { get; set; }
        public string Summary { get; set; }
        public int Rating { get; set; }
    }
    public class Address
    {
     public string Street { get; set; }
     public string City { get; set; }
     public string Province { get; set; }
     public string PostalCode { get; set; }
    }
}
