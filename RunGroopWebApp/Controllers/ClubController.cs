using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunGroopWebApp;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers
{
    public class ClubController : Controller
    {
        
        private readonly IClubRepository _clubRepository;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ClubController(IClubRepository clubRepository, IPhotoService photoService, IHttpContextAccessor httpContextAccessor ) 
        {           
            _clubRepository = clubRepository;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IActionResult> Index()
        {
            var clubs = await _clubRepository.GetAll();
            return View(clubs);
        }
        public async Task<IActionResult> Detail(int id)
        {
            var club = await _clubRepository.GetByIdAsync(id);
            
            if (club == null)
            {
                return NotFound();
            }
            return View(club);
        }

        public IActionResult Create()
        {
            //Getting the user ID by using an extension Class that gets the ID of the logged in user.
            var currentUserID = _httpContextAccessor.HttpContext.User.GetUserId();

            //Assign the user ID value to the createClubViewModel object, to have it then in the hidden input of the form form.
            var createClubViewModel = new CreateClubViewModel { AppUserId = currentUserID };

            //Once I have my AppUserId set, I go to the view passing the createClubViewModel object.
            return View(createClubViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel clubVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(clubVM.Image);

                var club = new Club
                {
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = result.Url.ToString(),
                    AppUserId = clubVM.AppUserId,
                    Address = new Address
                    {
                        City = clubVM.Address.City,
                        State = clubVM.Address.State,
                        Street = clubVM.Address.Street,
                    }
                };
                _clubRepository.AddClub(club);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "The club creation has failed.");
            }
            return View(clubVM);   
        }

        public async Task<IActionResult> Edit(int id)
        {
            var club = await _clubRepository.GetByIdAsync(id);
            if (club == null)
            {
                return View("Error");
            }

            var clubVM = new EditClubViewModel
            {
                Title = club.Title,
                Description = club.Description,
                AddressId = club.AddressId,
                Address = club.Address,
                URL = club.Image,
                ClubCategory = club.ClubCategory
            };
            return View(clubVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditClubViewModel clubVM)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club.");
                return View("Error");
            }
            //I must use a method that uses the property "AsNoTracking" in order to avoid errors, because i already have an instance of the DB club entity with the clubVM object.
            var userClub = await _clubRepository.GetByIdAsyncNoTracking(id);

            if (userClub != null)
            {
                try
                {
                    await _photoService.DeletePhotoAsync(userClub.Image);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete the photo.");
                    return View(clubVM);
                }
                var photo = await _photoService.AddPhotoAsync(clubVM.Image);
                var club = new Club
                {
                    Id = id,
                    Title = clubVM.Title,
                    Description = clubVM.Description,
                    Image = photo.Url.ToString(),
                    AddressId = clubVM.AddressId,
                    Address = clubVM.Address
                };

                _clubRepository.UpdateClub(club);
                return RedirectToAction("Index");
            }
            else
            {
                return View(clubVM);
            }            
        }

        public async Task<IActionResult> Delete(int id)
        {
            var clubDetails =  await _clubRepository.GetByIdAsync(id);
            if (clubDetails == null)
            {
                return View("Error");
            }
            return View(clubDetails);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteClub(int id)
        {
            var clubDetails = await _clubRepository.GetByIdAsync(id);
            if (clubDetails == null)
            {
                return View("Error");
            }
            _clubRepository.DeleteClub(clubDetails);
            return RedirectToAction("Index");
        }

    }
}
