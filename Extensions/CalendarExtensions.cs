using System.Text;
using Lab2.Models.DTOs;

namespace Lab2.Helpers
{
    public static class CalendarHelper
    {
        public static string GenerateICalendar(IEnumerable<BookingDto> bookings, string calendarName = "Moje rezerwacje")
        {
            var sb = new StringBuilder();

            sb.AppendLine("BEGIN:VCALENDAR");
            sb.AppendLine("VERSION:2.0");
            sb.AppendLine("PRODID:-//Room Booking System//Lab2//PL");
            sb.AppendLine("CALSCALE:GREGORIAN");
            sb.AppendLine("METHOD:PUBLISH");
            sb.AppendLine($"X-WR-CALNAME:{EscapeText(calendarName)}");
            sb.AppendLine("X-WR-TIMEZONE:Europe/Warsaw");

            foreach (var booking in bookings)
            {
                sb.AppendLine("BEGIN:VEVENT");

                sb.AppendLine($"UID:booking-{booking.BookingId}@roombooking.local");

                sb.AppendLine($"DTSTAMP:{FormatDateTime(DateTime.UtcNow)}");

                sb.AppendLine($"DTSTART:{FormatDateTime(booking.StartTime)}");

                sb.AppendLine($"DTEND:{FormatDateTime(booking.EndTime)}");

                sb.AppendLine($"SUMMARY:{EscapeText($"Rezerwacja: {booking.RoomName}")}");

                var description = $"Rezerwacja sali {booking.RoomName}\\nZarezerwowane przez: {booking.UserName}";
                sb.AppendLine($"DESCRIPTION:{EscapeText(description)}");

                sb.AppendLine($"LOCATION:{EscapeText(booking.RoomName)}");

                sb.AppendLine("STATUS:CONFIRMED");

                sb.AppendLine("CATEGORIES:Rezerwacja sali");

                sb.AppendLine("BEGIN:VALARM");
                sb.AppendLine("TRIGGER:-PT15M");
                sb.AppendLine("ACTION:DISPLAY");
                sb.AppendLine($"DESCRIPTION:Przypomnienie: {EscapeText(booking.RoomName)} za 15 minut");
                sb.AppendLine("END:VALARM");

                sb.AppendLine("END:VEVENT");
            }

            sb.AppendLine("END:VCALENDAR");

            return sb.ToString();
        }

        private static string FormatDateTime(DateTime dateTime)
        {
            var utcDateTime = dateTime.Kind == DateTimeKind.Utc
                ? dateTime
                : dateTime.ToUniversalTime();

            return utcDateTime.ToString("yyyyMMdd'T'HHmmss'Z'");
        }

        private static string EscapeText(string text)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            return text
                .Replace("\\", "\\\\")
                .Replace(",", "\\,")
                .Replace(";", "\\;")
                .Replace("\n", "\\n")
                .Replace("\r", "");
        }
    }
}
