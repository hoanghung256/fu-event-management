using FUEM.Application.Interfaces.StudentUseCases;
using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.UseCases.StudentUseCases
{
    public class FollowUseCase : IFollowUseCase
    {
        private readonly IStudentRepository _repository;

        public FollowUseCase(IStudentRepository repository)
        {
            _repository = repository;
        }

        public async Task<bool> isUserFollowing(int studentId, int organizerId)
            => await _repository.IsUserFollowingAsync(studentId, organizerId);

        public async Task Follow(int studentId, int organizerId)
            => await _repository.FollowAsync(studentId, organizerId);

        public async Task UnFollow(int studentId, int organizerId)
            => await _repository.UnfollowAsync(studentId, organizerId);
    }
}
