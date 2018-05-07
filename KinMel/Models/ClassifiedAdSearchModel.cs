using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Newtonsoft.Json;

namespace KinMel.Models
{
    public class ClassifiedAdSearchModel
    {
        public int? Id { get; set; }

        public double? Price { get; set; }

        [Required(ErrorMessage = "Please Enter Minimum Price")]
        [Range(1,10000000,ErrorMessage = "Price Must Be Minimum 1")]
        public int? PriceFrom { get; set; }

        [Required(ErrorMessage = "Please Enter Maximum Price")]
        [Range(1, 100000000, ErrorMessage = "Price Must Be Between above 1")]
        public int? PriceTo { get; set; }
        //public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Description { get; set; }
       
        public string Condition { get; set; }

        public Boolean PriceNegotiable { get; set; }
        public Boolean Delivery { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsSold { get; set; }
        public bool IsActive { get; set; }
       
        [DisplayName("Run ad for following days")]
        public int AdDuration { get; set; }

        public string City { get; set; }
        public string Address { get; set; }

        public string UsedFor { get; set; }

        public double DeliveryCharges { get; set; }

        public string WarrantyType { get; set; }
        public string WarrantyPeriod { get; set; }
        public string Title { get; set; }

        //this is our collection of search results
        public List<ClassifiedAd> PropertyResults { get; set; }

        public ClassifiedAdSearchModel()
        {
            //constructor sets up default search parameters for the properties
            //so that they don't need to be specified unless we're
            //performing a filtered search from the user.

            //Title = "";   
            Condition = "";
            City = "";
            //Description = "";
            Price = null;
            PriceFrom = null;
            PriceTo = null;
            //FirstName = "";
            PropertyResults = new List<ClassifiedAd>();   //an empty list by default

        }
    }
}