using FUEM.Application.Interfaces.ChatUseCases;
using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;
using FUEM.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.UseCases.ChatUseCases
{
    public class GetChat : IGetChat
    {
        private readonly IChatRepository _repository;

        public GetChat(IChatRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<ChatMessage>> GetAllGroupMessagesAsync(string groupId, int skip, int take)
           => await _repository.GetAllGroupMessagesAsync(groupId, skip, take);

        public async Task<List<ChatGroup>> GetAllChatGroupsAsync(int id)
           => await _repository.GetAllChatGroupsAsync(id);

        public async Task<List<ChatGroupMember>> GetChatGroupMembers(string groupId)
            => await _repository.GetChatGroupMembersAsync(groupId);

        public async Task<List<ChatGroup>> GetStudentGroupChat(int studentId)
            => await _repository.GetStudentGroupChatAsync(studentId);

        public async Task<ChatGroup> GetChatGroupByNameAsync(string groupName)
            => await _repository.GetChatGroupByNameAsync(groupName);

        public async Task<ChatGroup> GetChatGroupByEventId(int eventId)
            => await _repository.GetChatGroupByEventIdAsync(eventId);

        public async Task AddGroupMessageAsync(ChatMessage chatMessage)
            => await _repository.AddGroupMessageAsync(chatMessage);

        public async Task AddGroupMemberAsync(ChatGroupMember chatGroupMember)
            => await _repository.AddGroupMemberAsync(chatGroupMember);

        public async Task AddGroupAsync(ChatGroup chatGroup)
            => await _repository.AddGroupAsync(chatGroup);
    }
}
