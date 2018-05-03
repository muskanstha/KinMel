using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KinMel.Data;
using KinMel.Models;
using KinMel.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace KinMel.Controllers
{
    [Authorize]

    public class MobilesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MobilesController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: Mobiles
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Mobile.Include(m => m.CreatedByUser).Include(m => m.SubCategory);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Mobiles/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mobile = await _context.Mobile
                .Include(m => m.CreatedByUser)
                .Include(m => m.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (mobile == null)
            {
                return NotFound();
            }

            return View(mobile);
        }

        // GET: Mobiles/Create
        public IActionResult Create()
        {
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("Mobile")), "Id", "Name");
            return View();
        }

        // POST: Mobiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Brand,Model,Color,Storage,Ram,FrontCamera,BackCamera,PhoneOs,ScreenSize,Features,Id,SubCategoryId,Title,Description,Condition,Price,PriceNegotiable,Delivery,IsSold,IsActive,AdDuration,City,Address,UsedFor,DeliveryCharges,WarrantyType,WarrantyPeriod,WarrantyIncludes")] Mobile mobile, List<IFormFile> imageFiles)
        {
            if (ModelState.IsValid)
            {
                long size = imageFiles.Sum(f => f.Length);
                if (size > 0)
                {
                    var currentUserID = _userManager.GetUserId(this.User);
                    mobile.CreatedByUserId = currentUserID;

                    mobile.DateCreated = DateTime.Now;
                    _context.Add(mobile);
                    await _context.SaveChangesAsync();

                    string forSlug = mobile.Id + " " + String.Join(" ", mobile.Title.Split().Take(4));
                    string slug = forSlug.GenerateSlug();

                    mobile.Slug = slug;


                    await BlobStorageHelper.UploadBlobs(slug, imageFiles);

                    mobile.ImageUrls = await BlobStorageHelper.ListBlobsFolder(slug);


                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "ClassifiedAds", new { id = slug });
                }
            }

            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("Mobile")), "Id", "Name");
            return View(mobile);
        }

        // GET: Mobiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mobile = await _context.Mobile.SingleOrDefaultAsync(m => m.Id == id);
            if (mobile == null)
            {
                return NotFound();
            }
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("Mobile")), "Id", "Name");
            return View(mobile);
        }

        //// POST: Mobiles/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Brand,Model,Color,Storage,Ram,FrontCamera,BackCamera,PhoneOs,ScreenSize,Features,Id,SubCategoryId,CreatedByUserId,Title,Description,Condition,Price,PriceNegotiable,Delivery,DateCreated,IsSold,IsActive,Slug,Discriminator")] Mobile mobile)
        //{
        //    if (id != mobile.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(mobile);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!MobileExists(mobile.Id))
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
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", mobile.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", mobile.SubCategoryId);
        //    return View(mobile);
        //}

        //// GET: Mobiles/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var mobile = await _context.Mobile
        //        .Include(m => m.CreatedByUser)
        //        .Include(m => m.SubCategory)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (mobile == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(mobile);
        //}

        //// POST: Mobiles/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var mobile = await _context.Mobile.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.Mobile.Remove(mobile);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool MobileExists(int id)
        //{
        //    return _context.Mobile.Any(e => e.Id == id);
        //}
    }
}
