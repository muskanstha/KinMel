using System;
using KinMel.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace XUnitTestProject
{
    public class XUnitTest
    {
        //instantiate
        private HomeController _homeController;
        public XUnitTest()
        {
            _homeController = new HomeController();
        }
        [Fact]
        public void Test_HomePageIndexView()
        {
            var result = _homeController.Index() as ViewResult;
            System.Console.WriteLine(result);
            Assert.Equal("Index", result.ViewName);
        }
    }
}
