﻿using FUEM.Application.Interfaces.EventUseCases;
using FUEM.Application.Interfaces.UserUseCases;
using FUEM.Application.UseCases.EventUseCases;
using FUEM.Application.UseCases.UserUseCases;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUEM.Application
{
    public static class DependencyInjection
    {
        public static IHostApplicationBuilder AddUseCases(this IHostApplicationBuilder builder)
        {
            builder.Services.AddTransient<ICreateEvent, CreateEvent>();
            builder.Services.AddTransient<IGetEventForGuest, GetEventForGuest>();
            builder.Services.AddTransient<ILogin, Login>();
            return builder;
        }
    }
}
