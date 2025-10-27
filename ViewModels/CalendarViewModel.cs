using Lab2.Models.DTOs;
using Lab2.Extensions; 

namespace Lab2.ViewModels
{
    public class CalendarViewModel
    {
        public DateTime SelectedDate { get; set; } = DateTime.Today;
        public IEnumerable<RoomDto> Rooms { get; set; } = new List<RoomDto>();
        public IEnumerable<BookingDto> Bookings { get; set; } = new List<BookingDto>();
        public CalendarViewMode ViewMode { get; set; } = CalendarViewMode.Week;

        public DateTime PeriodStart => ViewMode == CalendarViewMode.Day
            ? SelectedDate.Date
            : SelectedDate.GetWeekStart();

        public DateTime PeriodEnd => ViewMode == CalendarViewMode.Day
            ? SelectedDate.Date.AddDays(1)
            : SelectedDate.GetWeekEnd();

        public DateTime WeekStart => SelectedDate.GetWeekStart();
        public DateTime WeekEnd => SelectedDate.GetWeekEnd();

        public int[] WorkingHours { get; set; } = Enumerable.Range(8, 12).ToArray();
    }

    public enum CalendarViewMode
    {
        Day,
        Week
    }
}
