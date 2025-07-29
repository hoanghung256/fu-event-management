using FUEM.Domain.Interfaces.Repositories;
using FUEM.Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using FUEM.Infrastructure.Common;

namespace FUEM.Infrastructure.Persistence
{
    public static class DependencyInjection
    {
        public static IHostApplicationBuilder AddRepositories(this IHostApplicationBuilder builder)
        {
            builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
            builder.Services.AddTransient<IChatRepository, ChatRepository>();
            builder.Services.AddTransient<IEventCollaboratorRepository, EventCollaboratorRepository>();
            builder.Services.AddTransient<IEventGuestRepository, EventGuestRepository>();
            builder.Services.AddTransient<IEventImageRepository, EventImageRepository>();
            builder.Services.AddTransient<IEventRepository, EventRepository>();
            builder.Services.AddTransient<IFeedbackRepository, FeedbackRepository>();
            builder.Services.AddTransient<IFileRepository, FileRepository>();
            builder.Services.AddTransient<IFollowRepository, FollowRepository>();
            builder.Services.AddTransient<ILocationRepository, LocationRepository>();
            //builder.Services.AddTransient<INotificationReceiverRepository, NoticationReceiverRepository>();
            //builder.Services.AddTransient<INotificationReceiverRepository, NotificationRepository>();
            builder.Services.AddTransient<IOrganizerRepository, OrganizerRepository>();
            builder.Services.AddTransient<IStudentRepository, StudentRepository>();

            builder.Services.AddTransient<FirebaseStorageService>();
            builder.Services.AddTransient<MongoDBService>();
            builder.Services.AddTransient<PayOSService>();

            return builder;
        }
    }
}
