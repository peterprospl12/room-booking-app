using Lab2.Extensions;
using Lab2.Models;
using Lab2.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Lab2.Controllers
{
    public class HomeController(IRepository repository) : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")] 
        public IActionResult Init([FromQuery] string confirm)
        {
            if (confirm != "yes-i-am-sure")
            {
                return BadRequest("No confirmation");
            }

            if (!User.IsAdmin())
            {
                return Forbid();
            }

            try
            {
                var users = new[]
                {
                    new User
                    {
                        Name = "Jan Kowalski",
                        Email = "jan.kowalski@example.com",
                        PasswordHash = HashExtensions.ToSha256Hash("password123"),
                        Role = UserRole.User
                    },
                    new User
                    {
                        Name = "Anna Nowak",
                        Email = "anna.nowak@example.com",
                        PasswordHash = HashExtensions.ToSha256Hash("password123"),
                        Role = UserRole.User
                    },
                    new User
                    {
                        Name = "Zbyszek Papka",
                        Email = "zbyszek.papka@example.com",
                        PasswordHash = HashExtensions.ToSha256Hash("password123"),
                        Role = UserRole.User
                    },
                    new User
                    {
                        Name = "Patryk Gdynia",
                        Email = "patryk.gdynia@example.com",
                        PasswordHash = HashExtensions.ToSha256Hash("password123"),
                        Role = UserRole.User
                    }
                };

                var createdUsers = users.
                    Where(user => repository.GetUserByEmail(user.Email) == null)
                    .Count(user => repository.AddUser(user));

                var rooms = new[]
                {
                    new Room { Name = "Sala Konferencyjna A", Capacity = 20 },
                    new Room { Name = "Sala Szkoleniowa B", Capacity = 15 },
                    new Room { Name = "Sala Warsztatowa C", Capacity = 10 },
                    new Room { Name = "Sala Executive D", Capacity = 8 }
                };

                var createdRooms = rooms.
                    Where(room => repository.GetRoomByName(room.Name) == null).
                    Count(repository.AddRoom);

                return Ok(new
                {
                    success = true,
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
