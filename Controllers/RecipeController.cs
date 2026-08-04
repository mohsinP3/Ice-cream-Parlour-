using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Ice_Cream_Parlour_Eproject.Data;
using Ice_Cream_Parlour_Eproject.Models;

namespace Ice_Cream_Parlour_Eproject.Controllers
{
    public class RecipeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecipeController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ===== RECIPE LIST (All Recipes) =====
        public async Task<IActionResult> Index()
        {
            var recipes = await _context.Recipes.ToListAsync();
            return View(recipes);
        }

        // ===== RECIPE DETAIL - Registered Users Only =====
        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null) return NotFound();
            return View(recipe);
        }

        // ===== FREE RECIPES - Public =====
        public async Task<IActionResult> FreeRecipes()
        {
            var recipes = await _context.Recipes.Where(r => r.IsFree).ToListAsync();
            return View(recipes);
        }

        // ===== RECIPE STATIC PAGE =====
        public IActionResult Recipe()
        {
            return View();
        }
    }
}