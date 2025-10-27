using System.ComponentModel.DataAnnotations;
using Lab2.Models.DTOs;

namespace Lab2.ViewModels
{
    public class RoomManageViewModel
    {
        public IEnumerable<RoomDto> Rooms { get; set; } = new List<RoomDto>();

        [Required(ErrorMessage = "Room name is required")]
        [StringLength(100, ErrorMessage = "Room name cannot be longer than 100 characters")]
        [Display(Name = "Room name")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Room capacity is required")]
        [Range(1, 1000, ErrorMessage = "Room capacity must be between 1 and 1000")]
        [Display(Name = "Room capacity")]
        public int Capacity { get; set; }
    }
}
