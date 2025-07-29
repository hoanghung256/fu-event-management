using FUEM.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Domain.Interfaces.Repositories
{
    public interface IChatRepository
    {
        Task<List<ChatGroup>> GetAllChatGroupsAsync(int id);
        Task<List<ChatMessage>> GetAllGroupMessagesAsync(string groupId, int skip = 0, int take = 5);
        Task<List<ChatGroup>> GetStudentGroupChatAsync(int studentId);
        Task<ChatGroup> GetChatGroupByNameAsync(string groupName);
        Task<ChatGroup> GetChatGroupByEventIdAsync(int eventId);
        Task AddGroupAsync(ChatGroup chatGroup);
        Task AddGroupMemberAsync(ChatGroupMember chatGroupMember);
        Task AddGroupMessageAsync(ChatMessage chatMessage);
    }
}
