using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace KinMel.Models
{
    public class ClassifiedAdSearchModel
    {
        public int? Id { get; set; }
        public double Price { get; set; }
        public int? PriceFrom { get; set; }
        public int? PriceTo { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        private string _imageUrls;
        public string ImageUrls
        {
            get => _imageUrls;
            set => _imageUrls = value;
        }
        public List<string> ImageUrlList
        {
            get
            {
                if (this._imageUrls == null)
                {
                    return new List<string>() { "/images/NoImage.svg" };
                }
                return JsonConvert.DeserializeObject<List<string>>(this._imageUrls);
            }
        }

        public string Condition { get; set; }
      
        public Boolean PriceNegotiable { get; set; }
        public Boolean Delivery { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsSold { get; set; }
        public bool IsActive { get; set; }
        public string Slug { get; set; }
        public string Discriminator { get; set; }

        [DisplayName("Run ad for following days")]
        public int AdDuration { get; set; }

        public string City { get; set; }
        public string Address { get; set; }

        public string UsedFor { get; set; }

        public double DeliveryCharges { get; set; }

        public string WarrantyType { get; set; }
        public string WarrantyPeriod { get; set; }
    }
}