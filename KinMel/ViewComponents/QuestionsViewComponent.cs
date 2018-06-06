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
    public class QuestionsViewComponent:ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public QuestionsViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var items = await GetQuestions(id);
            return View(items);
        }
        private async Task<List<Question>> GetQuestions(int id)
        {
            return await _context.Question.AsNoTracking().Where(q => q.ClassifiedAdId.Equals(id)).Include(q => q.Answers).ThenInclude(a => a.CreatedBy).Include(q => q.CreatedBy).OrderByDescending(q => q.DateCreated).ToListAsync();
        }
    }
}
