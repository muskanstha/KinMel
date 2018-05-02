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
      //Deepak sir adds
        //public string AdDuration { get; set; }
        //public string Location { get; set; }
        //public string UsedFor { get; set; }
        //public double DeliveryCharges { get; set; }
        //public string WarrantyType { get; set; }
        //public int WarrentyPeriod { get; set; }
        //public string WarrantyIncludes { get; set; }
        
}

    public class Car : ClassifiedAd
    {
        public string Type { get; set; }

        public string Brand { get; set; }
        public string ModelNo { get; set; }//khasai use hudaina jasto lagxa
        public int ModelYear { get; set; }//MadeYear

        public string Color { get; set; }
        public int TotalKm { get; set; }//TravelledKm

        public string FuelType { get; set; }

        [NotMapped]
        public List<String> FeatureList { get; set; }

        public string Features { get; set; }

        public int DoorsNo { get; set; }
       // public string RegisteredDistrict { get; set; }//string dropdown
       // public string LotNo { get; set; } //number or numbertext?
       // public int Engine { get; set; }//Engine(CC)number
       //public string Transmission { get; set; }//Gear System dropdown
    }

    public class Mobile : ClassifiedAd
    {
        public string Brand { get; set; }
        public string ModelNo { get; set; }
        public string Color { get; set; }

        public string Storage { get; set; }//internal storage
        public string Ram { get; set; }
        public string FrontCamera { get; set; }
        public string BackCamera { get; set; }
        public string PhoneOs { get; set; }
        public string ScreenSize { get;set; }

        public string Features { get; set; }
    }

    public class Motorcycle : ClassifiedAd
    {
        public string Brand { get; set; }
        public string ModelNo { get; set; }
        public string Color { get; set; }
        public int Engine { get; set; }//Engine(CC)
        public int Mileage { get; set; }//Mileage(km/l)
        public string TotalKm { get; set; }//travelledKm
        public DateTime MadeYear { get; set; }

        public string Features { get; set; }
    }
    //public class RealState : ClassifiedAd
    //{
    //    public string PropertyType { get; set; }//dropdown
    //    public string LandSize { get; set; }//(aana/dhur/m2)
    //    public int Floors { get; set; }
    //    public int TotalRooms { get; set; }
    //    public string Furnishing { get; set; }//dropdown-full semi none

    //    public string Features { get; set; }

    //}
    //public class Computer:ClassifiedAd
    //{
    //  public string Type { get; set; }//dropdown(desktop, laptop,2in1)
    //  public string Processor { get; set; }
    //  public string ProcessorGeneration { get; set; }
    //  public int Ram { get; set; }
    //  public int VideoCard { get; set; }
    //  public int HDD { get; set; }
    //  public int SSD { get; set; }
    //  public string ScreenType { get; set; }
    //  public int ScreenSize { get; set; }
    //  public double Battery { get; set; }

    //  [NotMapped]
    //  public List<String> FeatureList { get; set; }
    //  public string Features { get; set; }
    //}

    //public class Jobs
    //{
    //  public int Salary { get; set; }
    //  public int WorkingDays { get; set; }
    //  public int ContractFor { get; set; }
    //}

    //public class BeautyAndHealth{}

    //public class BooksAndLearing
    //{
    //  public string Author { get; set; }
    //  public int ISBN { get; set; }

    //}
    //public class Electronics{}

    //public class Furnitures{}

    //public class Camera{}

    //public class MusicInstruments{}

    //public class PetsAndPetCare{}

    //public class SportsAndFitness{}

    //public class TabletsAndIPads:Mobile{//esto garda hunxa?
    //}

    //public class ToysAndGames{}

    //public class TravelAndTours{}

    //public class Services{}

    //public class MobileAccessories{}

    //public class ComputerParts{}

    //public class ApparelsAndAccessories{}

    //public class VehiclesParts{}

    public class ClassifiedAdCreateViewModel
    {
        public string CategoryName { get; set; }
    }
}
