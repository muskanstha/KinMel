using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using KinMel.Controllers;
using KinMel.Controllers.Categories;
using KinMel.Data;
using KinMel.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Moq;
using Xunit;

namespace XUnitTestProject
{
    public class XUnitTest
    {
        public IFormFile image;
        public List<IFormFile> imageFiles;
        public ClassifiedAdCreateViewModel _createVM;
        public Car _car;
        public string num;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        //instantiate Act
        private HomeController _homeController;
        private ManageController _manageController;
        private CarsController _carsController;
        private ClassifiedAdsController _classifiedAdsController;

        private ClassifiedAdSearchModel sm;
        //private ClassifiedAdsController _adsController;

        public XUnitTest()
        {
            _homeController = new HomeController();
            //_adsController = new ClassifiedAdsController( _context);
            _classifiedAdsController = new ClassifiedAdsController(_context);
            sm = new ClassifiedAdSearchModel() { SortBy = "Latest Ads" };
            _createVM = new ClassifiedAdCreateViewModel();
          
        }


        [Fact]
        public void Test_HomePageIndexView()
        {
            //Act
            var result = _homeController.Index() as ViewResult;
            //System.Console.WriteLine(result);
            //Assert
            //Assert.Equal("Index", result.ViewName);
            Assert.True(result.ViewName == "Index");
        }

        [Fact]
        public void Test_HomePageResultType()
        {
            IActionResult result = _homeController.Index();

            Assert.IsType<ViewResult>(result);
        }

        //[Fact]
        //public void TestModelData()
        //{
        //   // sut = new KinMel.Controllers.ClassifiedAdsController(_context);

        //    IActionResult result = sut.Index(sm);

        //    ViewResult viewResult = Assert.IsType<ViewResult>(result);

        //    ClassifiedAdSearchModel model = Assert.IsType<ClassifiedAdSearchModel>(viewResult.Model);

        //    Assert.True(model.SortBy.GetType() == num.GetType());


        //}

        [Fact]
        public void TestDetailsRedirect()
        {

            //var result = (RedirectToRouteResult)sut.Details("1");
            //Assert.AreEqual("Index", result.Values["action"]);

        }

        //[Fact]
        //public void TestAdsCreate()
        //{
        //    //ClassifiedAdCreateViewModel cm = new ClassifiedAdCreateViewModel();
        //    //cm.CategoryName = "Cars";
        //    _car = new Car()
        //    {
        //         Type = "Test",
        //         Brand = "",
        //         Color = "Test",
        //         Condition =   "USED"
        //    };
        //    // Model,ModelYear,Color,TotalKm,FuelType,Features,DoorsNo,Id,SubCategoryId,Title,Description,Condition,Price,PriceNegotiable,Delivery,IsSold,IsActive,Engine,Mileage,Transmission,RegisteredDistrict,LotNo,AdDuration,City,Address,UsedFor,DeliveryCharges,WarrantyType,WarrantyPeriod,WarrantyIncludes

        //    // Act

        //    var result = _carsController.Create(_car, imageFiles, image);

        //    //Assert
        //    var feedback = _context.ClassifiedAd.First();
        //    Assert.True(feedback.Discriminator == result);

        //}
    }
}
