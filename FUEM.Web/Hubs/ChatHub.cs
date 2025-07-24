using FirebaseAdmin.Messaging;
using FUEM.Domain.Entities;
using FUEM.Domain.Enums;
using FUEM.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace FUEM.Web.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatRepository _chatUseCase;
        private readonly IOrganizerRepository _organizerRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IEventRepository _eventRepository;

        public ChatHub(IChatRepository chatUseCase, IOrganizerRepository organizerRepository, IStudentRepository studentRepository, IEventRepository eventRepository)
        {
            _chatUseCase = chatUseCase;
            _organizerRepository = organizerRepository;
            _studentRepository = studentRepository;
            _eventRepository = eventRepository;
        }

        //public override async Task OnConnectedAsync()
        //{
        //    int? userId = Context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier) != null ? int.Parse(Context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value) : null;
        //    if (userId == null)
        //    {
        //        await Groups.AddToGroupAsync(Context.ConnectionId, "GeneralGroup");
        //    }
        //    else
        //    {
        //        if (Context.User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value == Role.Club.ToString() || Context.User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value == Role.Admin.ToString())
        //        {
        //            //var org = await _organizerRepository.GetOrganizerByIdAsync(userId.Value);
        //            var chatGroupList = await _chatUseCase.GetAllChatGroupsAsync(userId.Value);
        //            foreach (var item in chatGroupList)
        //            {
        //                await Groups.AddToGroupAsync(userId.ToString(), item.Name);
        //            }
        //        }
        //        else
        //        {
        //            var email = Context.User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
        //            var stu = await _studentRepository.GetStudentByEmailAsync(email);
        //        }
        //    }
        //    await base.OnConnectedAsync();
        //}

        public async Task JoinGroup(string groupName, string eventName)
        {
            Claim id = Context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            Claim role = Context.User.FindFirst(System.Security.Claims.ClaimTypes.Role);
            int? userId = id != null ? int.Parse(id.Value) : null;
            Event? eve = await _eventRepository.GetEventByNameAsync(eventName);
            ChatGroup groupChat = await _chatUseCase.GetChatGroupByEventIdAsync(eve.Id);
            Student student = null;
            Organizer organizer = null;
            if (userId != null)
            {
                if (role.Value == Role.Club.ToString() || role.Value == Role.Admin.ToString())
                {
                    organizer = await _organizerRepository.GetOrganizerByIdAsync(userId.Value);
                }
                else if (role.Value == Role.Student.ToString())
                {
                    student = await _studentRepository.GetStudentByIdAsync(userId.Value);
                }
                await Groups.AddToGroupAsync(Context.ConnectionId, groupChat.Name);
            }
        }

        public async Task LeaveGroup(string groupName)
        {
            Claim id = Context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            Claim role = Context.User.FindFirst(System.Security.Claims.ClaimTypes.Role);
            int? userId = id != null ? int.Parse(id.Value) : null;
            Student student = null;
            Organizer organizer = null;
            if (userId != null)
            {
                if (role.Value == Role.Club.ToString() || role.Value == Role.Admin.ToString())
                {
                    organizer = await _organizerRepository.GetOrganizerByIdAsync(userId.Value);
                }
                else if (role.Value == Role.Student.ToString())
                {
                    student = await _studentRepository.GetStudentByIdAsync(userId.Value);
                }
                await Groups.RemoveFromGroupAsync(role.Value == Role.Student.ToString() ? student.Fullname : organizer.Acronym, groupName);
                //await Clients.Group(groupName).SendAsync("ReceiveMessage", "System", $"{role.Value == Role.Student.ToString() ? student.Fullname : organizer.Acronym} has left the group {groupName}.");
            }
        }

        public async Task SendMessage(string groupName, string eventName, string message)
        {
            Claim id = Context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            Claim role = Context.User.FindFirst(System.Security.Claims.ClaimTypes.Role);
            int? userId = id != null ? int.Parse(id.Value) : null;
            Event? eve = await _eventRepository.GetEventByNameAsync(eventName);
            ChatGroup groupChat = await _chatUseCase.GetChatGroupByEventIdAsync(eve.Id);
            if (userId == null)
            {
                //var group = await _chatUseCase.GetChatGroupByNameAsync(groupName);
                //var chatMessage = new ChatMessage()
                //{
                //    GroupId = groupChat.Id,
                //    Content = message,
                //    SenderOrganizerId = 9
                //};
                //var org = await _organizerRepository.GetOrganizerByIdAsync(9);
                //await _chatUseCase.AddGroupMessageAsync(chatMessage);
                //await Clients.Group(groupName).SendAsync("ReceiveMessage", org.Acronym, message);
            }
            else
            {
                ChatMessage chatMessage = new ChatMessage();
                Student student = null;
                Organizer organizer = null;
                if (role.Value == Role.Club.ToString() || role.Value == Role.Admin.ToString())
                {
                    chatMessage.Content = message;
                    chatMessage.SenderOrganizerId = userId.Value;
                    chatMessage.GroupId = groupChat.Id;
                    organizer = await _organizerRepository.GetOrganizerByIdAsync(userId.Value);
                }
                else if (role.Value == Role.Student.ToString())
                {
                    chatMessage.Content = message;
                    chatMessage.SenderStudentId = userId.Value;
                    chatMessage.GroupId = groupChat.Id;
                    student = await _studentRepository.GetStudentByIdAsync(userId.Value);
                }
                await _chatUseCase.AddGroupMessageAsync(chatMessage);
                await Clients.Group(groupChat.Name).SendAsync("ReceiveMessage", role.Value == Role.Student.ToString() ? student.Fullname : organizer.Acronym, message);
            }
        }
    }
}
