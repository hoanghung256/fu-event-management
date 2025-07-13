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
            builder.Services.AddTransient<IEventRepository, EventRepository>();
            builder.Services.AddTransient<IStudentRepository, StudentRepository>();
            builder.Services.AddTransient<IOrganizerRepository, OrganizerRepository>();
            builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();

            builder.Services.AddTransient<FirebaseStorageService>();

            return builder;
        }
    }
}
