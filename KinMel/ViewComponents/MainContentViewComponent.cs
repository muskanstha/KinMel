using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using KinMel.Data;
using KinMel.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KinMel.ViewComponents
{
    public class MainContentViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public MainContentViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<IViewComponentResult> InvokeAsync(string sortOrder, string category, ClassifiedAdSearchModel searchModel)
        {
            
            if (searchModel != null)
            {
                //var searchItems = await GetFilteredNewAsync(searchModel.SortBy, searchModel.Category);
                var searchItems = await GetFilteredNewAsync("random String", searchModel);
                return View(searchItems);
            }
            else if(!String.IsNullOrWhiteSpace(category))
            {
                var categoryItems = await GetSortedAdsAsync(sortOrder, category);
                return View(categoryItems);
            }

            var items = await GetSortedAdsAsync(sortOrder);
            return View(items);
        }


        private Task<List<ClassifiedAd>> GetSortedAdsAsync(string sortOrder)
        {
            ViewData["DateSortParm"] = sortOrder == "date_desc" ? "Date" : "date_desc";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            var classifiedAd = from c in _context.ClassifiedAd
                               select c;
            switch (sortOrder)
            {
                case "Price":
                    classifiedAd = classifiedAd.OrderBy(c => c.Price);
                    break;
                case "price_desc":
                    classifiedAd = classifiedAd.OrderByDescending(c => c.Price);
                    break;
                case "date_desc":
                    classifiedAd = classifiedAd.OrderBy(c => c.DateCreated);
                    break;
                case "Date":
                    classifiedAd = classifiedAd.OrderByDescending(c => c.DateCreated);
                    break;
                default:
                    classifiedAd = classifiedAd.OrderByDescending(c => c.DateCreated);
                    break;
            }
            return classifiedAd.AsNoTracking().Include(c => c.CreatedByUser).Include(c => c.SubCategory).ToListAsync();
        }
        private Task<List<ClassifiedAd>> GetSortedAdsAsync(string sortOrder, string category)
        {
            ViewData["DateSortParm"] = sortOrder == "date_desc" ? "Date" : "date_desc";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            var classifiedAd = from c in _context.ClassifiedAd
                               select c;
            switch (sortOrder)
            {
                case "Price":
                    classifiedAd = classifiedAd.Where(c => c.Discriminator.Equals(category)).OrderBy(c => c.Price);
                    break;
                case "price_desc":
                    classifiedAd = classifiedAd.Where(c => c.Discriminator.Equals(category)).OrderByDescending(c => c.Price);
                    break;
                case "date_desc":
                    classifiedAd = classifiedAd.Where(c => c.Discriminator.Equals(category)).OrderBy(c => c.DateCreated);
                    break;
                case "Date":
                    classifiedAd = classifiedAd.Where(c => c.Discriminator.Equals(category)).OrderByDescending(c => c.DateCreated);
                    break;
                default:
                    classifiedAd = classifiedAd.Where(c => c.Discriminator.Equals(category)).OrderByDescending(c => c.DateCreated);
                    break;
            }
            return classifiedAd.AsNoTracking().Include(c => c.CreatedByUser).Include(c => c.SubCategory).ToListAsync();
        }
        //private Task<List<ClassifiedAd>> GetFilteredAsync(ClassifiedAdSearchModel searchModel)
        //{
        //    var classifiedAd = from c in _context.ClassifiedAd
        //                     select c;

        //    //city
        //    if (searchModel.City != null && searchModel.PriceFrom == null && searchModel.PriceTo == null && searchModel.Condition == null)
        //    {
        //        classifiedAd = classifiedAd.Where(k => k.City == searchModel.City);
        //    }
        //    //condition
        //    if (searchModel.Condition != null && searchModel.City == null && searchModel.PriceFrom == null && searchModel.PriceTo == null)
        //    {
        //        classifiedAd = classifiedAd.Where(k => k.Condition == searchModel.Condition);
        //    }

        //    //price
        //    if (searchModel.PriceFrom != null && searchModel.PriceTo != null && searchModel.City == null && searchModel.Condition == null)
        //    {
        //        classifiedAd = classifiedAd.Where(k => k.Price >= searchModel.PriceFrom & k.Price <= searchModel.PriceTo);

        //    }

        //    //sabai
        //    if (searchModel.Condition != null && searchModel.City != null && searchModel.PriceFrom != null && searchModel.PriceTo != null)
        //    {
        //        classifiedAd = classifiedAd.Where(k => k.Condition == searchModel.Condition & k.City == searchModel.City & k.Price >= searchModel.PriceFrom & k.Price <= searchModel.PriceTo);
        //    }

        //    //city ra price
        //    if (searchModel.City != null && searchModel.PriceFrom != null && searchModel.PriceTo != null)
        //    {
        //        classifiedAd = classifiedAd.Where(k => k.Price >= searchModel.PriceFrom & k.Price <= searchModel.PriceTo & k.City == searchModel.City);
        //    }

        //    //city ra condition
        //    if (searchModel.City != null && searchModel.Condition != null && searchModel.PriceFrom == null && searchModel.PriceTo == null)
        //    {
        //        classifiedAd = classifiedAd.Where(k => k.Condition == searchModel.Condition & k.City == searchModel.City);
        //    }

        //    //price ra condition
        //    if (searchModel.PriceFrom != null && searchModel.PriceTo != null && searchModel.Condition != null)
        //    {
        //        classifiedAd = classifiedAd.Where(k => k.Condition == searchModel.Condition & k.Price >= searchModel.PriceFrom & k.Price <= searchModel.PriceTo);
        //    }
        //    //categories
        //    if (searchModel.Category != null)
        //    {
        //        classifiedAd = classifiedAd.Where(k => k.Category == searchModel.Category);
        //    }

        //    return classifiedAd.Include(c => c.CreatedByUser).Include(c => c.SubCategory).ToListAsync();
        //}

        //private Task<List<ClassifiedAd>> GetFilteredNewAsync(string sortOrder, ClassifiedAdSearchModel searchModel)
        private Task<List<ClassifiedAd>> GetFilteredNewAsync(string sortOrder, ClassifiedAdSearchModel searchModel)
        {
            var classifiedAd = from c in _context.ClassifiedAd
                               select c;
            //city
            if (!String.IsNullOrWhiteSpace(searchModel.City))
            {
                classifiedAd = classifiedAd.Where(k => k.City == searchModel.City);
            }
            //condition
            if (!String.IsNullOrWhiteSpace(searchModel.Condition))
            {
                classifiedAd = classifiedAd.Where(k => k.Condition == searchModel.Condition);
            }

            //price
            if (searchModel.PriceFrom != null)
            {
                classifiedAd = classifiedAd.Where(k => k.Price >= searchModel.PriceFrom);
            }
            //price
            if (searchModel.PriceTo != null)
            {
                classifiedAd = classifiedAd.Where(k => k.Price <= searchModel.PriceTo);
            }

            //Category
            if (!String.IsNullOrWhiteSpace(searchModel.Category))
            {
                classifiedAd = classifiedAd.Include(c => c.SubCategory).ThenInclude(c => c.Category).Where(k => k.SubCategory.Category.Name == searchModel.Category);
            }

            if (searchModel.SortBy != null)
            {
                ViewData["DateSortParm"] = searchModel.SortBy == "date_desc" ? "Date" : "date_desc";
                ViewData["PriceSortParm"] = searchModel.SortBy == "Price" ? "price_desc" : "Price";
               
                switch (searchModel.SortBy)
                {
                    case "Price":
                        classifiedAd = classifiedAd.OrderBy(c => c.Price);
                        break;
                    case "price_desc":
                        classifiedAd = classifiedAd.OrderByDescending(c => c.Price);
                        break;
                    case "date_desc":
                        classifiedAd = classifiedAd.OrderBy(c => c.DateCreated);
                        break;
                    case "Date":
                        classifiedAd = classifiedAd.OrderByDescending(c => c.DateCreated);
                        break;
                    default:
                        classifiedAd = classifiedAd.OrderByDescending(c => c.DateCreated);
                        break;
                }
                //return classifiedAd.AsNoTracking().Include(c => c.CreatedByUser).Include(c => c.SubCategory).ToListAsync();
            }

            //ViewData["DateSortParm"] = sortOrder == "date_desc" ? "Date" : "date_desc";
            //ViewData["PriceSortParm"] = sortOrder == "Price" ? "price_desc" : "Price";
            //switch (sortOrder)
            //{
            //    case "Price":
            //        classifiedAd = classifiedAd.OrderBy(c => c.Price);
            //        break;
            //    case "price_desc":
            //        classifiedAd = classifiedAd.OrderByDescending(c => c.Price);
            //        break;
            //    case "date_desc":
            //        classifiedAd = classifiedAd.OrderBy(c => c.DateCreated);
            //        break;
            //    case "Date":
            //        classifiedAd = classifiedAd.OrderByDescending(c => c.DateCreated);
            //        break;
            //    default:
            //        classifiedAd = classifiedAd.OrderByDescending(c => c.DateCreated);
            //        break;
            //}
            return classifiedAd.Include(c => c.CreatedByUser).Include(c => c.SubCategory).ToListAsync();
        }
    }
}
