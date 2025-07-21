using FUEM.Application.Interfaces.CategoryUseCases;
using FUEM.Application.Interfaces.EventUseCases;
using FUEM.Application.Interfaces.LocationUseCases;
using FUEM.Application.Interfaces.OrganizerUseCases;
using FUEM.Application.Interfaces.RegistrationUseCases;
using FUEM.Application.Interfaces.UserUseCases;
using FUEM.Application.UseCases.CategoryUseCases;
using FUEM.Application.UseCases.EventUseCases;
using FUEM.Application.UseCases.LocationUseCases;
using FUEM.Application.UseCases.OrganizerUseCases;
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
            // Authentication
            builder.Services.AddTransient<ILogin, Login>();
            builder.Services.AddTransient<ISignUp, SignUp>();
            builder.Services.AddTransient<IForgotPassword, ForgotPassword>();

            // Event
            builder.Services.AddTransient<ICreateEvent, CreateEvent>();
            builder.Services.AddTransient<IGetEventForGuest, GetEventForGuest>();
            builder.Services.AddTransient<IProcessEvent, ProcessEvent>();

            //Category
            builder.Services.AddTransient<IGetAllCategories, GetAllCategories>();

            // Location
            builder.Services.AddTransient<IGetAllLocation, GetAllLocation>();

            //Organizer
            builder.Services.AddTransient<IGetAllOrganizers, GetAllOrganizers>();

            return builder;
        }
    }
}
