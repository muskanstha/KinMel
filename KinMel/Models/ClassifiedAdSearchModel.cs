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

        [Range(0,10000000,ErrorMessage = "Maximum range 10 crore")]
        public int? PriceFrom { get; set; }
        
        [Range(1, 100000000, ErrorMessage = "Price must be between 1 and 10 crore")]
        public int? PriceTo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Category { get; set; }
        //public ClassifiedAdSearchModel Category { get; set; }
        public string Condition { get; set; }

        public Boolean PriceNegotiable { get; set; }
        public Boolean Delivery { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsSold { get; set; }
        public bool IsActive { get; set; }
       
        public string City { get; set; }

        public string SortBy { get; set; }
       
        public double DeliveryCharges { get; set; }

        public string WarrantyType { get; set; }
        public string WarrantyPeriod { get; set; }
        public string Title { get; set; }

        //this is our collection of search results
        public List<ClassifiedAd> PropertyResults { get; set; }

        public ClassifiedAdSearchModel()
        {
            
            Condition = "";
            City = "";   
            Price = null;
            PriceFrom = null;
            PriceTo = null;
            Category = "";
            SortBy = "";
            PropertyResults = new List<ClassifiedAd>(); 

        }
    }
}