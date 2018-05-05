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
    public class TabletsAndIPadsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TabletsAndIPadsController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: TabletsAndIPads
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.TabletsAndIPads.Include(t => t.CreatedByUser).Include(t => t.SubCategory);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TabletsAndIPads/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tabletsAndIPads = await _context.TabletsAndIPads
                .Include(t => t.CreatedByUser)
                .Include(t => t.SubCategory)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (tabletsAndIPads == null)
            {
                return NotFound();
            }

            return View(tabletsAndIPads);
        }

        // GET: TabletsAndIPads/Create
        public IActionResult Create()
        {
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("Car")), "Id", "Name");
            return View();
        }

        // POST: TabletsAndIPads/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Brand,Model,Color,Storage,Ram,FrontCamera,BackCamera,PhoneOs,ScreenSize,Features,Id,SubCategoryId,Title,Description,Condition,Price,PriceNegotiable,Delivery,IsSold,IsActive,AdDuration,City,Address,UsedFor,DeliveryCharges,WarrantyType,WarrantyPeriod,WarrantyIncludes")] TabletsAndIPads tabletsAndIPads, List<IFormFile> imageFiles)
        {
            if (ModelState.IsValid)
            {
                long size = imageFiles.Sum(f => f.Length);
                if (size > 0)
                {
                    var currentUserId = _userManager.GetUserId(this.User);
                    tabletsAndIPads.CreatedByUserId = currentUserId;

                    tabletsAndIPads.DateCreated = DateTime.Now;
                    _context.Add(tabletsAndIPads);
                    await _context.SaveChangesAsync();

                    string forSlug = tabletsAndIPads.Id + " " + String.Join(" ", tabletsAndIPads.Title.Split().Take(4));
                    string slug = forSlug.GenerateSlug();

                    tabletsAndIPads.Slug = slug;

                    await BlobStorageUploader.UploadBlobs(slug, imageFiles);

                    tabletsAndIPads.ImageUrls = await BlobStorageUploader.ListBlobsFolder(slug);

                    await _context.SaveChangesAsync();
                    return RedirectToAction("Details", "ClassifiedAds", new { id = slug });
                }

            }
            ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>().Where(sc => sc.Category.Name.Equals("Car")), "Id", "Name", tabletsAndIPads.SubCategoryId);
            return View(tabletsAndIPads);
        }

        //// GET: TabletsAndIPads/Edit/5
        //public async Task<IActionResult> Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var tabletsAndIPads = await _context.TabletsAndIPads.SingleOrDefaultAsync(m => m.Id == id);
        //    if (tabletsAndIPads == null)
        //    {
        //        return NotFound();
        //    }
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", tabletsAndIPads.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", tabletsAndIPads.SubCategoryId);
        //    return View(tabletsAndIPads);
        //}

        //// POST: TabletsAndIPads/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(int id, [Bind("Brand,Model,Color,Storage,Ram,FrontCamera,BackCamera,PhoneOs,ScreenSize,Features,Id,SubCategoryId,CreatedByUserId,Title,Description,ImageUrls,Condition,Price,PriceNegotiable,Delivery,DateCreated,IsSold,IsActive,Slug,Discriminator")] TabletsAndIPads tabletsAndIPads)
        //{
        //    if (id != tabletsAndIPads.Id)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(tabletsAndIPads);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!TabletsAndIPadsExists(tabletsAndIPads.Id))
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
        //    ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Id", tabletsAndIPads.CreatedByUserId);
        //    ViewData["SubCategoryId"] = new SelectList(_context.Set<SubCategory>(), "Id", "Id", tabletsAndIPads.SubCategoryId);
        //    return View(tabletsAndIPads);
        //}

        //// GET: TabletsAndIPads/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var tabletsAndIPads = await _context.TabletsAndIPads
        //        .Include(t => t.CreatedByUser)
        //        .Include(t => t.SubCategory)
        //        .SingleOrDefaultAsync(m => m.Id == id);
        //    if (tabletsAndIPads == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(tabletsAndIPads);
        //}

        //// POST: TabletsAndIPads/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var tabletsAndIPads = await _context.TabletsAndIPads.SingleOrDefaultAsync(m => m.Id == id);
        //    _context.TabletsAndIPads.Remove(tabletsAndIPads);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool TabletsAndIPadsExists(int id)
        //{
        //    return _context.TabletsAndIPads.Any(e => e.Id == id);
        //}
    }
}
