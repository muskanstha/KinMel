using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using KinMel.Data;
using KinMel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace KinMel.Controllers
{
    [Authorize]
    public class AnswersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AnswersController(ApplicationDbContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Answers
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Answer.Include(a => a.Category).Include(a => a.CreatedBy);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Answers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answer
                .Include(a => a.Category)
                .Include(a => a.CreatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        //// GET: Answers/Create
        //public IActionResult Create()
        //{
        //    ViewData["QuestionId"] = new SelectList(_context.Question, "Id", "Id");
        //    ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id");
        //    return View();
        //}

        // POST: Answers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Text,QuestionId")] Answer answer)
        {
            if (ModelState.IsValid)
            {

                answer.DateCreated = DateTimeOffset.UtcNow;

                string currentUserId = _userManager.GetUserId(User);
                answer.CreatedById = currentUserId;

                _context.Add(answer);
                await _context.SaveChangesAsync();
                var question = await _context.Question.Include(q => q.ClassifiedAd).FirstOrDefaultAsync(q => q.Id.Equals(answer.QuestionId));
                return RedirectToAction("Details", "ClassifiedAds", new { id = question.ClassifiedAd.Slug });
            }
      
            return View(answer);
        }

        // GET: Answers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answer.FindAsync(id);
            if (answer == null)
            {
                return NotFound();
            }
            ViewData["QuestionId"] = new SelectList(_context.Question, "Id", "Id", answer.QuestionId);
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id", answer.CreatedById);
            return View(answer);
        }

        // POST: Answers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text,QuestionId,CreatedById,DateCreated")] Answer answer)
        {
            if (id != answer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(answer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AnswerExists(answer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["QuestionId"] = new SelectList(_context.Question, "Id", "Id", answer.QuestionId);
            ViewData["CreatedById"] = new SelectList(_context.Users, "Id", "Id", answer.CreatedById);
            return View(answer);
        }

        // GET: Answers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var answer = await _context.Answer
                .Include(a => a.Category)
                .Include(a => a.CreatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (answer == null)
            {
                return NotFound();
            }

            return View(answer);
        }

        // POST: Answers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var answer = await _context.Answer.FindAsync(id);
            _context.Answer.Remove(answer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AnswerExists(int id)
        {
            return _context.Answer.Any(e => e.Id == id);
        }
    }
}
