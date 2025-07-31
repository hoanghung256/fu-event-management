using FUEM.Application.Interfaces.ChatUseCases;
using FUEM.Application.Interfaces.OrganizerUseCases;
using FUEM.Application.Interfaces.StudentUseCases;
using FUEM.Domain.Entities;
using FUEM.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using MimeKit;

namespace FUEM.Web.Controllers
{
    public class ChatController : Controller
    {
        private readonly IGetChat _getChatUseCase;
        private readonly IGetStudent _getStudentUseCase;
        private readonly IGetOrganizer _getOrganizerUseCase;
        private readonly IGetEvent _getEventUseCase;
        private readonly IServiceProvider _serviceProvider;

        public ChatController(IGetChat getChatUseCase, IGetStudent getStudentUseCase, IGetOrganizer getOrganizerUseCase, IGetEvent getEventUseCase, IServiceProvider serviceProvider)
        {
            _getChatUseCase = getChatUseCase;
            _getStudentUseCase = getStudentUseCase;
            _getOrganizerUseCase = getOrganizerUseCase;
            _getEventUseCase = getEventUseCase;
            _serviceProvider = serviceProvider;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserGroups()
        {
            int? userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier) != null ? int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value) : null;
            var role = User.FindFirst(System.Security.Claims.ClaimTypes.Role).Value;
            List<ChatGroup> groups = new List<ChatGroup>();
            if (userId != null && (role == Role.Club.ToString() || role == Role.Admin.ToString()))
            {
                groups = await _getChatUseCase.GetAllChatGroupsAsync(userId.Value);
            }
            else if (userId != null && role == Role.Student.ToString())
            {
                groups = await _getChatUseCase.GetStudentGroupChat(userId.Value);
            }
            //else
            //{
            //    groups = await _getChatUseCase.GetAllChatGroupsAsync(9);
            //}
            //var groupTasks = groups.Select(async g => new { name = g.Name, eventName = await _getEventUseCase.GetEventById(g.EventId) }).ToList();
            //var group = await Task.WhenAll(groupTasks);
            var groupTasks = groups.Select(async group =>
            {
                string eventName = null;

                if (group.EventId != null)
                {
                    using var scope = _serviceProvider.CreateScope();
                    var eventUseCase = scope.ServiceProvider.GetRequiredService<IGetEvent>();
                    var ev = await eventUseCase.GetEventById(group.EventId);
                    eventName = ev?.Fullname;
                }

                return new
                {
                    eventName,
                    name = group.Name
                };
            });

            var groupList = await Task.WhenAll(groupTasks);

            return Json(groupList);
        }

        [HttpGet]
        public async Task<IActionResult> GetGroupMessages(string groupName, string eventName, int skip = 0, int take = 5)
        {
            int? userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier) != null ? int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value) : null;
            Event? eve = await _getEventUseCase.GetEventByName(eventName);
            ChatGroup groupChat = await _getChatUseCase.GetChatGroupByEventId(eve.Id);
            List<ChatMessage> messages = await _getChatUseCase.GetAllGroupMessagesAsync(groupChat.Id, skip, take);
            var messageTasks = messages.Select(async g =>
            {
                using var scope = _serviceProvider.CreateScope();

                var organizerUseCase = scope.ServiceProvider.GetRequiredService<IGetOrganizer>();
                var studentUseCase = scope.ServiceProvider.GetRequiredService<IGetStudent>();

                string user = null;

                if (g.SenderOrganizerId != null)
                {
                    var organizer = await organizerUseCase.GetOrganizerByIdAsync(g.SenderOrganizerId.Value);
                    user = organizer?.Acronym;
                }
                else if (g.SenderStudentId != null)
                {
                    var student = await studentUseCase.GetStudentById(g.SenderStudentId.Value);
                    user = student?.Fullname;
                }

                return new
                {
                    user,
                    text = g.Content,
                    sentAt = g.SentAt
                };
            }).ToList();
            var message = await Task.WhenAll(messageTasks);
            //var messageList = new List<object>();
            //if (userId != null)
            //{
            //    foreach (var g in messages)
            //    {
            //        string user = null;

            //        if (g.SenderOrganizerId != null)
            //        {
            //            var organizer = await _getOrganizerUseCase.GetOrganizerByIdAsync(g.SenderOrganizerId.Value);
            //            user = organizer.Acronym;
            //        }
            //        else if (g.SenderStudentId != null)
            //        {
            //            var student = await _getStudentUseCase.GetStudentById(g.SenderStudentId.Value);
            //            user = student.Fullname;
            //        }

            //        messageList.Add(new
            //        {
            //            user,
            //            text = g.Content
            //        });
            //    }
            //}
            return Json(message);
        }

        public async Task<IActionResult> Index()
        {
            //var messages = await _getChatUseCase.GetGroupMessagesAsync(groupId);
            //var userId = _chatDao.GetCurrentUserId();
            //return View(messages);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            //await _getChatUseCase.AddGroupAsync(new Domain.Entities.ChatGroup()
            //{
            //    CreatorId = 1,
            //    EventId = 1,
            //    IsHidden = false,
            //    Name = "Test"
            //});
            return RedirectToAction("Index");
        }
    }
}
