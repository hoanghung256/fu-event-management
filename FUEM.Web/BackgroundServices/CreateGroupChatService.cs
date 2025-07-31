
using FUEM.Domain.Entities;
using FUEM.Domain.Interfaces.Repositories;
using FUEM.Infrastructure.Common;
using FUEM.Infrastructure.Persistence;
using FUEM.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Text.RegularExpressions;

namespace FUEM.Web.BackgroundServices
{
    public class CreateGroupChatService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<CreateGroupChatService> _logger;

        public CreateGroupChatService(IServiceProvider serviceProvider, ILogger<CreateGroupChatService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Background service started.");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<FUEMDbContext>();
                    var mongoService = scope.ServiceProvider.GetRequiredService<MongoDBService>();

                    var today = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(7));

                    //Create Group Chat when CollaboratorDeadline is reach
                    var eventList = await dbContext.Events
                        .Where(x => x.CollaboratorRegisterDeadline.Value == today && x.EventCollaborators.Count > 0)
                        .Include(x => x.EventCollaborators)
                        .ThenInclude(x => x.Student)
                        .ToListAsync(stoppingToken);

                    foreach (var eve in eventList)
                    {
                        var chatGroupExist = await mongoService.ChatGroups.Find(s => s.EventId == eve.Id).FirstOrDefaultAsync(stoppingToken);
                        if (chatGroupExist == null)
                        {
                            ChatGroup chatGroup = new ChatGroup()
                            {
                                CreatorId = eve.OrganizerId.Value,
                                IsHidden = false,
                                Name = eve.Fullname,
                                EventId = eve.Id
                            };

                            await mongoService.ChatGroups.InsertOneAsync(chatGroup, cancellationToken: stoppingToken);
                            await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
                        }

                        ChatGroup createdChatGroup = await mongoService.ChatGroups.Find(s => s.EventId == eve.Id).FirstOrDefaultAsync(stoppingToken);
                        foreach (var collaborator in eve.EventCollaborators)
                        {
                            if (createdChatGroup != null)
                            {
                                ChatGroupMember chatGroupMemberExist = await mongoService.ChatGroupMembers.Find(s => s.GroupId == createdChatGroup.Id && s.StudentId == collaborator.StudentId).FirstOrDefaultAsync(stoppingToken);
                                if (chatGroupMemberExist == null)
                                {
                                    ChatGroupMember groupMember = new ChatGroupMember()
                                    {
                                        GroupId = createdChatGroup.Id,
                                        StudentId = collaborator.StudentId,
                                    };
                                    await mongoService.ChatGroupMembers.InsertOneAsync(groupMember, cancellationToken: stoppingToken);
                                    await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
                                }
                            }
                        }
                    }

                    //Hidden event that has past 30 days from the day event start
                    var eventListOver30Days = await dbContext.Events
                        .Where(x => x.DateOfEvent.Value.AddDays(30) == today)
                        .ToListAsync(stoppingToken);

                    foreach (var eve in eventListOver30Days)
                    {
                        var chatGroupExist = await mongoService.ChatGroups.Find(s => s.EventId == eve.Id).FirstOrDefaultAsync(stoppingToken);

                        if (chatGroupExist != null)
                        {
                            var filter = Builders<ChatGroup>.Filter.Eq(g => g.Id, chatGroupExist.Id);

                            var update = Builders<ChatGroup>.Update.Set(g => g.IsHidden, true);

                            var result = await mongoService.ChatGroups.UpdateOneAsync(filter, update, cancellationToken: stoppingToken);

                            Console.WriteLine(result.ModifiedCount);
                            await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred in background service.");
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }

            _logger.LogInformation("Background service stopped.");
        }
    }
}
