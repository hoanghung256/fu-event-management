using System.Globalization;
using System.Security.Claims;
using FUEM.Application.Interfaces.UserUseCases;
using FUEM.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUEM.Web.Controllers
{
    public class StudentController : Controller
    {
        IManageCalendar _manageCalendarUseCase;
        public StudentController(IManageCalendar manageCalendarUseCase)
        {
            _manageCalendarUseCase = manageCalendarUseCase;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Student")]
        public IActionResult StudentCalendar()
        {
            ViewBag.StudentId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> GetRegisteredEvents(string start, string end)
        {
            var studentClaimId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (studentClaimId == null || !int.TryParse(studentClaimId, out int studentId))
            {
                return Unauthorized("User is not authenticated or does not have a valid student ID.");
            }
            DateTime parsedStart;
            DateTime parsedEnd;

            // Bước 1: Tiền xử lý chuỗi để thay thế khoảng trắng bằng dấu '+'
            // Ví dụ: "2025-06-29T00:00:00 07:00" sẽ thành "2025-06-29T00:00:00+07:00"
            string cleanedStart = start.Replace(" ", "+");
            string cleanedEnd = end.Replace(" ", "+");

            // Bước 2: Parse chuỗi đã làm sạch sang DateTime
            // DateTimeStyles.RoundtripKind rất phù hợp cho các chuỗi ISO 8601 có múi giờ
            if (!DateTime.TryParse(cleanedStart, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out parsedStart))
            {
                Console.WriteLine($"Lỗi: Không thể parse chuỗi start date: {start}");
                return BadRequest("Invalid start date format.");
            }

            if (!DateTime.TryParse(cleanedEnd, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out parsedEnd))
            {
                Console.WriteLine($"Lỗi: Không thể parse chuỗi end date: {end}");
                return BadRequest("Invalid end date format.");
            }
            var registeredEvents = await _manageCalendarUseCase.GetRegisteredEventsForStudentAsync(studentId, parsedStart, parsedEnd);


            var calendarEvents = registeredEvents. Select(e =>
            {
                var studentEventGuest = e.EventGuests.FirstOrDefault(eg => eg.GuestId == studentId); // Giả định EventGuest có StudentId

                // Xác định màu sắc dựa trên trạng thái IsAttended
                string eventColor;
                if (studentEventGuest != null && studentEventGuest.IsAttended == true)
                {
                    eventColor = "#28a745"; // Màu xanh lá cây nếu đã tham gia
                }
                else
                {
                    eventColor = "#dc3545"; // Màu đỏ nếu chưa tham gia hoặc không tìm thấy bản ghi
                }

                return new {
                    id = e.Id,
                    title = e.Fullname,
                    start = e.DateOfEvent?.ToDateTime(e.StartTime ?? TimeOnly.MinValue).ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
                    end = e.DateOfEvent?.ToDateTime(e.EndTime ?? TimeOnly.MinValue).ToString("yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture),
                    url = Url.Action("Detail", "EventRegistration", new { id = e.Id }),
                    color = eventColor, // Green if attended, red if not
                extendedProps = new
                {
                    location = e.Location?.LocationName,
                    status = e.Status.ToString(),
                    description = e.Description
                },
                    allDay = !(e.StartTime.HasValue && e.EndTime.HasValue)
                };
            }).ToList();
            return Ok(calendarEvents);
        }
    }
}
