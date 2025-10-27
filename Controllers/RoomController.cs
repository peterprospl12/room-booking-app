using FluentResults;
using Lab2.Models;
using Lab2.Models.DTOs;
using Lab2.Services;
using Lab2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lab2.Controllers
{
    [Authorize(Roles = nameof(UserRole.Admin))]
    public class RoomController(IRoomService roomService) : Controller
    {
        [HttpGet]
        public IActionResult Manage()
        {
            var viewModel = LoadRoomsViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddRoom(RoomManageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model = LoadRoomsViewModel(model);
                return View("Manage", model);
            }

            var dto = new CreateRoomDto(model.Name, model.Capacity);

            var addResult = roomService.AddRoom(dto);

            if (addResult.IsFailed)
            {
                AddErrorsToModelState(addResult);
                model = LoadRoomsViewModel(model);
                return View("Manage", model);
            }

            TempData["Success"] = "Room has been successfully added";
            return RedirectToAction("Manage");
        }

        [HttpPost]
        public IActionResult DeleteRoom(int id)
        {
            var deleteResult = roomService.DeleteRoom(id);

            if (deleteResult.IsFailed)
            {
                TempData["Error"] = deleteResult.Errors.FirstOrDefault()?.Message ?? "Failed to delete the room";
                return RedirectToAction("Manage");
            }

            TempData["Success"] = "Room has been deleted";
            return RedirectToAction("Manage");
        }

        private RoomManageViewModel LoadRoomsViewModel(RoomManageViewModel? existingModel = null)
        {
            var roomsResult = roomService.GetAllRooms();

            var viewModel = existingModel ?? new RoomManageViewModel();
            
            if (roomsResult.IsFailed)
            {
                viewModel.Rooms = new List<RoomDto>();
                TempData["Error"] = "Failed to retrieve the list of rooms";
            }
            else
            {
                viewModel.Rooms = roomsResult.Value;
            }

            return viewModel;
        }

        private void AddErrorsToModelState<T>(Result<T> result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Message);
            }
        }
    }
}
