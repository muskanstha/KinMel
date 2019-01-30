using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using KinMel.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace KinMel.Models
{
    public class ClassifiedAd
    {
        public int Id { get; set; }

        public int SubCategoryId { get; set; }
        public virtual SubCategory SubCategory { get; set; }
        //public string Category { get; set; }
        public string CreatedByUserId { get; set; }
        public virtual ApplicationUser CreatedByUser { get; set; }

        [Required]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        private string _primaryImageUrl;

        public string PrimaryImageUrl
        {
            get
            {
                if (String.IsNullOrWhiteSpace(this._primaryImageUrl))
                {
                    return "/images/NoImage.svg";
                }
                return this._primaryImageUrl;
            }
            set => _primaryImageUrl = value;
        }

        private string _imageUrls;

        public string ImageUrls
        {
            get => _imageUrls; set => _imageUrls = value;
        }
        public List<string> ImageUrlList
        {
            get
            {
                if (this._imageUrls == null)
                {
                    return new List<string>() { };
                }
                return JsonConvert.DeserializeObject<List<string>>(this._imageUrls).OrderByDescending(url => url).ToList();
            }
        }
        [Required]
        public string Condition { get; set; }
        [Required]
        public double Price { get; set; }
        public Boolean PriceNegotiable { get; set; }
        public string UsedFor { get; set; }

        [DisplayName("Posted on")]
        [DisplayFormat(DataFormatString = "{0:dd-MMMM-yyyy hh:mm}", ApplyFormatInEditMode = true)]
        public DateTimeOffset DateCreated { get; set; }

        [NotMapped]
        public string DateCreatedRelative => this.DateCreated.GetRelativeDate();

        public bool IsSold { get; set; }
        public bool IsActive { get; set; }

        public string Slug { get; set; }
        public string Discriminator { get; set; }

        [DisplayName("Run ad for following days")]
        public int AdDuration { get; set; }

        [Required]
        public string City { get; set; }
        public string Address { get; set; }
        [DisplayFormat(NullDisplayText = "-")]

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Boolean Delivery { get; set; }
        public double? DeliveryCharges { get; set; }

        public string WarrantyType { get; set; }
        public string WarrantyPeriod { get; set; }
        public string WarrantyIncludes { get; set; }

        [NotMapped]
        private List<string> FeaturesList { get; set; }
        public string Features { get; set; }

        public virtual ICollection<Question> Questions { get; set; }

    }

    public class Car : ClassifiedAd
    {
        public string Type { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }//khasai use hudaina jasto lagxa
        public int? ModelYear { get; set; }//MadeYear
        public string Color { get; set; }
        public int? TotalKm { get; set; }//TravelledKm
        public string FuelType { get; set; }
        public int? DoorsNo { get; set; }

        public int? Engine { get; set; }//Engine(CC)number
        public int? Mileage { get; set; }//Mileage(km/l)

        public string Transmission { get; set; }//Gear System dropdown
        public string RegisteredDistrict { get; set; }//string dropdown
        public string LotNo { get; set; } //number or numbertext?

    }
    public class Motorcycle : ClassifiedAd
    {
        public string Type { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int? ModelYear { get; set; }
        public string Color { get; set; }
        public int? TotalKm { get; set; }//travelledKm

        public int? Engine { get; set; }//Engine(CC)
        public int? Mileage { get; set; }//Mileage(km/l)

        public string RegisteredDistrict { get; set; }//string dropdown
        public string LotNo { get; set; } //number or numbertext?
    }

    public class Mobile : ClassifiedAd
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public double? Storage { get; set; }//internal storage
        public int? Ram { get; set; }
        public double? FrontCamera { get; set; }
        public double? BackCamera { get; set; }
        public string PhoneOs { get; set; }
        public double? ScreenSize { get; set; }
    }

    public class TabletsAndIPads : ClassifiedAd
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public double? Storage { get; set; }//internal storage
        public double? Ram { get; set; }
        public double? FrontCamera { get; set; }
        public double? BackCamera { get; set; }
        public string PhoneOs { get; set; }
        public string ScreenSize { get; set; }
    }


    public class Computer : ClassifiedAd
    {
        public string Type { get; set; }//dropdown(desktop, laptop,2in1)
        public string Processor { get; set; }
        public string ProcessorGeneration { get; set; }
        public double? Ram { get; set; }
        public double? GraphicsCard { get; set; }
        public double? HDD { get; set; }
        public double? SSD { get; set; }
        public string ScreenType { get; set; }
        public double? ScreenSize { get; set; }
        public double? Battery { get; set; }
    }

    public class RealEstate : ClassifiedAd
    {
        public string PropertyType { get; set; }//dropdown
        public string LandSize { get; set; }//(aana/dhur/m2)
        public int? Floors { get; set; }
        public int? TotalRooms { get; set; }
        public string Furnishing { get; set; }//dropdown-full semi none
    }

    public class Jobs : ClassifiedAd
    {
        [Required]
        public double? Salary { get; set; }
        [Required]
        public string WorkingDays { get; set; }
        public string ContractFor { get; set; }
    }

    public class BeautyAndHealth : ClassifiedAd { }

    public class BooksAndLearning : ClassifiedAd
    {
        [Required]
        public string Author { get; set; }
        public string Isbn { get; set; }

    }

    public class Electronics : ClassifiedAd { }

    public class Furnitures : ClassifiedAd { }

    public class Camera : ClassifiedAd { }

    public class MusicInstruments : ClassifiedAd { }

    public class PetsAndPetCare : ClassifiedAd { }

    public class SportsAndFitness : ClassifiedAd { }


    public class ToysAndGames : ClassifiedAd { }

    public class TravelAndTours : ClassifiedAd { }

    public class HelpAndServices : ClassifiedAd { }

    public class MobileAccessories : ClassifiedAd { }

    public class ComputerParts : ClassifiedAd { }

    public class ApparelsAndAccessories : ClassifiedAd { }

    public class VehiclesParts : ClassifiedAd { }



    public class ClassifiedAdCreateViewModel
    {
        public string CategoryName { get; set; }
    }
}
