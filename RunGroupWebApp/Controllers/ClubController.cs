using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroupWebApp.Data;
using RunGroupWebApp.Interfaces;
using RunGroupWebApp.Models;

namespace RunGroupWebApp.Controllers
{
    public class ClubController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IClubRepository _clubRepository;
        public ClubController(ApplicationDbContext context, IClubRepository clubRepository) 
        {
            _context = context;
            _clubRepository = clubRepository;
        }
        public async Task<IActionResult> Index()
        {
            var clubs = await _clubRepository.GetAll();
            return View(clubs);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var club = await _clubRepository.GetByIdAsync(id);
            /*
            var club  = _context.Clubs.Include(a => a.Address).FirstOrDefault(c => c.Id == id);//Address mast be included in order to avoid object references errors.
            */
            if (club == null)
            {
                return NotFound();
            }
            return View(club);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Club club)
        {
            if (!ModelState.IsValid)
            {
                return(View(club));
            }
            _clubRepository.AddClub(club);

            return RedirectToAction("Index");
        }
    }
}
