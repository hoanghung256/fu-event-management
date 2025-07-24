using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.Interfaces.ChatUseCases
{
    public interface IGetChat
    {
        Task<List<ChatMessage>> GetAllGroupMessagesAsync(string groupId);

        Task<List<ChatGroup>> GetAllChatGroupsAsync(int id);

        Task<List<ChatGroup>> GetStudentGroupChat(int studentId);

        Task<ChatGroup> GetChatGroupByNameAsync(string groupName);

        Task<ChatGroup> GetChatGroupByEventId(int eventId);

        Task AddGroupMessageAsync(ChatMessage chatMessage);

        Task AddGroupMemberAsync(ChatGroupMember chatGroupMember);

        Task AddGroupAsync(ChatGroup chatGroup);
    }
}
