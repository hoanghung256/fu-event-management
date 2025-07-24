using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;
using FUEM.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FUEM.Infrastructure.Persistence.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly MongoDBService _context;

        public ChatRepository(MongoDBService context)
        {
            _context = context;
        }

        public async Task<List<ChatMessage>> GetAllGroupMessagesAsync(string groupId)
        {
            return await _context.ChatMessages.Find(m => m.GroupId == groupId).ToListAsync();
        }

        public async Task<List<ChatGroupMember>> GetChatGroupMembersAsync(string groupId)
        {
            return await _context.ChatGroupMembers.Find(m => m.GroupId == groupId).ToListAsync();
        }

        public async Task<List<ChatGroup>> GetAllChatGroupsAsync(int id)
        {
            return await _context.ChatGroups.Find(g => g.CreatorId == id).ToListAsync();
        }

        public async Task<List<ChatGroup>> GetStudentGroupChatAsync(int studentId)
        {
            var chatGroup = await _context.ChatGroupMembers.Find(g => g.StudentId == studentId).ToListAsync();
            var groupIds = chatGroup.Select(g => g.GroupId).ToList();
            return await _context.ChatGroups.Find(g => groupIds.Contains(g.Id)).ToListAsync();
        }

        public async Task<ChatGroup> GetChatGroupByNameAsync(string groupName)
        {
            return await _context.ChatGroups.Find(s => s.Name.Equals(groupName)).FirstOrDefaultAsync();
        }

        public async Task<ChatGroup> GetChatGroupByEventIdAsync(int eventId)
        {
            return await _context.ChatGroups.Find(s => s.EventId == eventId).FirstOrDefaultAsync();
        }

        public async Task AddGroupAsync(ChatGroup chatGroup)
        {
            await _context.ChatGroups.InsertOneAsync(chatGroup);
        }

        public async Task AddGroupMemberAsync(ChatGroupMember chatGroupMember)
        {
            await _context.ChatGroupMembers.InsertOneAsync(chatGroupMember);
        }

        public async Task AddGroupMessageAsync(ChatMessage chatMessage)
        {
            await _context.ChatMessages.InsertOneAsync(chatMessage);
        }
    }
}
