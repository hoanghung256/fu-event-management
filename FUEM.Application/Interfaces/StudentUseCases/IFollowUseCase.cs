using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application.Interfaces.StudentUseCases
{
    public interface IFollowUseCase
    {
        Task<bool> isUserFollowing(int studentId, int organizerId);
        Task Follow(int studentId, int organizerId);
        Task UnFollow(int studentId, int organizerId);
    }
}
