﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KinMel.Data;
using KinMel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace KinMel.Controllers
{
    [Authorize]
    public class HelpAndServicesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public HelpAndServicesController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: HelpAndServices
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.HelpAndServices.Include(h => h.CreatedByUser).Include(h => h.SubCategory);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: HelpAndServices/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var helpAndServices = await _context.HelpAndServices
                .Include(h => h.CreatedByUser)
                .Include(h => h.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (helpAndServices == null)
            {
                return NotFound();
            }

            return View(helpAndServices);
        }

        // GET: HelpAndServices/Create
        public IActionResult Create()
        {
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("HelpAndServices")), "Id", "Name");
            return View();
        }

        // POST: HelpAndServices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SubCategoryId,Title,Description,Condition,Price,PriceNegotiable,Delivery,IsSold,IsActive,AdDuration,City,Address,UsedFor,DeliveryCharges,WarrantyType,WarrantyPeriod,WarrantyIncludes")] HelpAndServices helpAndServices, List<IFormFile> imageFiles)
        {
            if (ModelState.IsValid)
            {
                _context.Add(helpAndServices);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("HelpAndServices")), "Id", "Name", helpAndServices.SubCategoryId);
            return View(helpAndServices);
        }

        //// GET: HelpAndServices/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var helpAndServices = await _context.HelpAndServices.SingleOrDefaultAsync(m => m.Id == id);
        //    if (helpAndServices == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", helpAndServices.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", helpAndServices.SubCategoryId);
        //    return View(helpAndServices);
        //}

        //// POST: HelpAndServices/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Id,SubCategoryId,CreatedByUserId,Title,Description,ImageUrls,Condition,Price,PriceNegotiable,Delivery,DateCreated,IsSold,IsActive,Slug,Discriminator")] HelpAndServices helpAndServices)
        //{
        //    if (id != helpAndServices.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(helpAndServices);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!HelpAndServicesExists(helpAndServices.Id))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", helpAndServices.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", helpAndServices.SubCategoryId);
        //    return View(helpAndServices);
        //}

        //// GET: HelpAndServices/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var helpAndServices = await _context.HelpAndServices
        //        .Include(h => h.CreatedByUser)
        //        .Include(h => h.SubCategory)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (helpAndServices == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(helpAndServices);
        //}

        //// POST: HelpAndServices/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var helpAndServices = await _context.HelpAndServices.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.HelpAndServices.Remove(helpAndServices);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool HelpAndServicesExists(int id)
        //{
        //    return _context.HelpAndServices.Any(e => e.Id == id);
        //}
    }
}