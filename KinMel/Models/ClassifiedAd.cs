using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace KinMel.Models
{
    public class ClassifiedAd
    {
        public int Id { get; set; }

        public int SubCategoryId { get; set; }
        public virtual SubCategory SubCategory { get; set; }

        public string CreatedByUserId { get; set;}
        public virtual ApplicationUser CreatedByUser { get; set; }

        [Required]
        public string Title { get; set; }

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
                    return new List<string>(){"/images/NoImage.svg"};
                }
                return JsonConvert.DeserializeObject<List<string>>(this._imageUrls);
            }
        }
        public string Condition { get; set; }

        //public string AdditionalFields { get; set; }

        public double Price { get; set; }

        public Boolean PriceNegotiable { get; set; }

        public Boolean Delivery { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

        public bool IsSold { get; set; }

        public bool IsActive { get; set; }

        public string Slug { get; set; }

        public string Discriminator { get; set; }
    }

    public class Car : ClassifiedAd
    {
        public string Type { get; set; }

        public string Brand { get; set; }
        public string ModelNo { get; set; }
        public int ModelYear { get; set; }

        public string Color { get; set; }
        public int TotalKm { get; set; }

        public string FuelType { get; set; }

        [NotMapped]
        public List<String> FeatureList { get; set; }

        public string Features { get; set; }

        public int DoorsNo { get; set; }
    }

    public class Mobile : ClassifiedAd
    {
        public string Brand { get; set; }
        public string ModelNo { get; set; }
        public string Color { get; set; }

        public string Storage { get; set; }
        public string Ram { get; set; }
        public string FrontCamera { get; set; }
        public string BackCamera { get; set; }
        public string PhoneOs { get; set; }
        public string ScreenSize { get;set; }

        [NotMapped]
        public List<String> FeatureList { get; set; }
        public string Features { get; set; }
    }

    public class ClassifiedAdCreateViewModel
    {
        public string CategoryName { get; set; }
    }
}
