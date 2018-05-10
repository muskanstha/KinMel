using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KinMel.Data;
using KinMel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KinMel.ViewComponents
{
    public class HomePageAdsViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public HomePageAdsViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(string listType)
        {
            var items = await GetLatestAdsAsync(listType);
            return View(items);
        }
        private Task<List<ClassifiedAd>> GetLatestAdsAsync(string listType)
        {
            ViewData["ComponentTitle"] = listType;

            switch (listType)
            {
                case "Latest Ads":
                    return _context.ClassifiedAd.OrderByDescending(c => c.DateCreated).Take(6).ToListAsync();
                case "name":
                    return _context.ClassifiedAd.OrderByDescending(c => c.Title).Take(6).ToListAsync();
                case "Free Ads":
                    return _context.ClassifiedAd.Where(c => c.Price.Equals(0)).OrderByDescending(c => c.DateCreated).Take(6).ToListAsync();
                default:
                    ViewData["ComponentTitle"] = "default";
                    return _context.ClassifiedAd.OrderBy(c => c.DateCreated).Take(6).ToListAsync();

            }
        }
    }
}
