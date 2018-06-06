using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KinMel.Controllers;
using KinMel.Data;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.EntityFrameworkCore;

namespace KinMel.Models
{
    public class ClassifiedAdLogic: ClassifiedAdsController
    {
        private ApplicationDbContext _context;


        public ClassifiedAdLogic(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }


        //public IQueryable<ClassifiedAd> GetProducts(ClassifiedAdSearchModel searchModel)
        //{
        //    var result = _context.ClassifiedAd.AsQueryable();
        //    if (searchModel != null)
        //    {
        //        if (searchModel.Id.HasValue)
        //            result = result.Where(x => x.Id == searchModel.Id);
        //        if (!string.IsNullOrEmpty(searchModel.City))
        //            result = result.Where(x => x.Address.Contains(searchModel.Address));
        //        if (searchModel.PriceFrom.HasValue)
        //            result = result.Where(x => x.Price >= searchModel.PriceFrom);
        //        if (searchModel.PriceTo.HasValue)
        //            result = result.Where(x => x.Price <= searchModel.PriceTo);
        //        if (searchModel.Price !=null)
        //            result = result.Where(x => x.Price <= searchModel.Price);
        //    }

        //    return result;
        // "Ae hora? hasta Namaskar :D :p :) "
        //}

        public List<ClassifiedAd> GetAll()
        {
            return _context.ClassifiedAd.ToList();
        }
        
       
    }
}
