using Lab2.Models;
using System.ComponentModel.DataAnnotations;
using Lab2.Models.DTOs;

namespace Lab2.ViewModels
{
    public class RoomManageViewModel
    {
        public IEnumerable<RoomDto> Rooms { get; set; } = new List<RoomDto>();

        [Required(ErrorMessage = "Nazwa sali jest wymagana")]
        [StringLength(100, ErrorMessage = "Nazwa sali nie może być dłuższa niż 100 znaków")]
        [Display(Name = "Nazwa sali")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Pojemność jest wymagana")]
        [Range(1, 1000, ErrorMessage = "Pojemność musi być między 1 a 1000")]
        [Display(Name = "Pojemność")]
        public int Capacity { get; set; }
    }
}
