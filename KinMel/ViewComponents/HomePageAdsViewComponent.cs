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
            var items = await GetAdsAsync(listType);
            return View(items);
        }
        private Task<List<ClassifiedAd>> GetAdsAsync(string listType)
        {
            ViewData["ComponentTitle"] = listType;

            switch (listType)
            {
                case "Latest Ads":
                    return _context.ClassifiedAd.Include(c => c.CreatedByUser).OrderByDescending(c => c.DateCreated).Take(10).ToListAsync();
                case "Popular Ads":
                    return _context.ClassifiedAd.Include(c => c.CreatedByUser).OrderByDescending(c => c.Questions.Count).Take(10).ToListAsync();
                case "Free Ads":
                    return _context.ClassifiedAd.Include(c => c.CreatedByUser).Where(c => c.Price.Equals(0)).OrderByDescending(c => c.DateCreated).Take(10).ToListAsync();
                default:
                    ViewData["ComponentTitle"] = "default";
                    return _context.ClassifiedAd.Include(c => c.CreatedByUser).OrderBy(c => c.DateCreated).Take(10).ToListAsync();

            }
        }
    }
}
