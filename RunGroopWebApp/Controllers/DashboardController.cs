using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RunGroopWebApp.Data;
using RunGroopWebApp.Interfaces;
using RunGroopWebApp.Models;
using RunGroopWebApp.ViewModels;

namespace RunGroopWebApp.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IPhotoService _photoService;
        public DashboardController(IDashboardRepository dashboardRepository, IHttpContextAccessor contextAccessor, IPhotoService photoService)
        {
            _dashboardRepository = dashboardRepository;
            _contextAccessor = contextAccessor;
            _photoService = photoService;
        }

        private void MapUserEdit(AppUser user, EditUserDashboardViewModel editUserVM, ImageUploadResult photoResult)
        {
            user.Id = editUserVM.Id;
            user.Pace = editUserVM.Pace;
            user.Mileage = editUserVM.Mileage;
            user.ProfileImageUrl = photoResult.Url.ToString();
            user.State = editUserVM.State;
            user.City = editUserVM.City;
        }

        public async Task<IActionResult> Index()
        {
            var userRaces = await _dashboardRepository.GetAllUserRaces();
            var userClubs = await _dashboardRepository.GetAllUserClubs();
            var dashboardViewModel = new DashboardViewModel {
                Races = userRaces,
                Clubs = userClubs
            };

            return View(dashboardViewModel);
        }

        public async Task<IActionResult> EditUserProfile()
        {
            var CurrentUserID = _contextAccessor.HttpContext.User.GetUserId();
            var user = await _dashboardRepository.GetUserById(CurrentUserID);
            var editUserViewModel = new EditUserDashboardViewModel
            {
                Id = CurrentUserID,
                Pace = user.Pace,
                Mileage = user.Mileage,
                ProfileImageUrl = user.ProfileImageUrl,
                City = user.City,
                State = user.State
            };
            if (user == null) { return View("Error"); }

            return View(editUserViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> EditUserProfile(EditUserDashboardViewModel editUserVM)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edut the user profile.");
                return View("EditUserProfile", editUserVM);
            }
            var user = await _dashboardRepository.GetByIdNoTracking(editUserVM.Id);//I get it as "No Tracking" because I alreary have an instance of an entity that manipulates the user table.

            if(string.IsNullOrEmpty(user.ProfileImageUrl))
            {
                var photoResult = await _photoService.AddPhotoAsync(editUserVM.Image);

                MapUserEdit(user, editUserVM, photoResult);
                _dashboardRepository.Update(user);

                return RedirectToAction("Index");
            }
            else
            {
                try
                {
                    await _photoService.DeletePhotoAsync(user.ProfileImageUrl);
                    var photoResult = await _photoService.AddPhotoAsync(editUserVM.Image);

                    MapUserEdit(user, editUserVM, photoResult);
                    _dashboardRepository.Update(user);

                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Could not delete the photo.");
                    return View(editUserVM);
                }
                
            }

        }
    }
}
