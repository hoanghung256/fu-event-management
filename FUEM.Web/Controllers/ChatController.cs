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

        public ChatController(IGetChat getChatUseCase, IGetStudent getStudentUseCase, IGetOrganizer getOrganizerUseCase, IGetEvent getEventUseCase)
        {
            _getChatUseCase = getChatUseCase;
            _getStudentUseCase = getStudentUseCase;
            _getOrganizerUseCase = getOrganizerUseCase;
            _getEventUseCase = getEventUseCase;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserGroups()
        {
            int? userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier) != null ? int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value) : null;
            List<ChatGroup> groups = new List<ChatGroup>();
            if (userId != null && (User.FindFirst(System.Security.Claims.ClaimTypes.Role).Value == Role.Club.ToString() || User.FindFirst(System.Security.Claims.ClaimTypes.Role).Value == Role.Admin.ToString()))
            {
                groups = await _getChatUseCase.GetAllChatGroupsAsync(userId.Value);
            }
            else if (userId != null && User.FindFirst(System.Security.Claims.ClaimTypes.Role).Value == Role.Student.ToString())
            {
                groups = await _getChatUseCase.GetStudentGroupChat(userId.Value);
            }
            //else
            //{
            //    groups = await _getChatUseCase.GetAllChatGroupsAsync(9);
            //}
            //var groupTasks = groups.Select(async g => new { name = g.Name, eventName = await _getEventUseCase.GetEventById(g.EventId) }).ToList();
            //var group = await Task.WhenAll(groupTasks);
            var groupList = new List<object>();
            foreach (var item in groups)
            {
                string eventName = null;

                if (item.EventId != null)
                {
                    //Console.WriteLine(item.EventId);
                    var eve = await _getEventUseCase.GetEventById(item.EventId);
                    eventName = eve.Fullname;
                }

                groupList.Add(new
                {
                    eventName,
                    name = item.Name
                });
            }
            return Json(groupList);
        }

        [HttpGet]
        public async Task<IActionResult> GetGroupMessages(string groupName, string eventName)
        {
            int? userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier) != null ? int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value) : null;
            //List<ChatMessage> messages = new List<ChatMessage>();
            //if (User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier) != null)
            //{
            //Console.WriteLine("Group Name: " + groupName);
            Event? eve = await _getEventUseCase.GetEventByName(eventName);
            ChatGroup groupChat = await _getChatUseCase.GetChatGroupByEventId(eve.Id);
            List<ChatMessage> messages = await _getChatUseCase.GetAllGroupMessagesAsync(groupChat.Id);
            //var messageTasks = messages.Select(async g => new
            //{
            //    //user = g.SenderOrganizerId ?? g.SenderStudentId,
            //    user = g.SenderOrganizerId != null ? (await _getOrganizerUseCase.GetOrganizerByIdAsync(g.SenderOrganizerId.Value)).Acronym : g.SenderStudentId != null ? (await _getStudentUseCase.GetStudentById(g.SenderStudentId.Value)).Fullname : null,
            //    text = g.Content,
            //}).ToList();
            //var message = await Task.WhenAll(messageTasks);
            var messageList = new List<object>();
            if (userId != null)
            {
                foreach (var g in messages)
                {
                    string user = null;

                    if (g.SenderOrganizerId != null)
                    {
                        var organizer = await _getOrganizerUseCase.GetOrganizerByIdAsync(g.SenderOrganizerId.Value);
                        user = organizer.Acronym;
                    }
                    else if (g.SenderStudentId != null)
                    {
                        var student = await _getStudentUseCase.GetStudentById(g.SenderStudentId.Value);
                        user = student.Fullname;
                    }

                    messageList.Add(new
                    {
                        user,
                        text = g.Content
                    });
                }
            }
            //}
            return Json(messageList);
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
