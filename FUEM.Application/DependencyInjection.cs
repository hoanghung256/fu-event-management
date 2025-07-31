using FUEM.Application.Interfaces.CategoryUseCases;
using FUEM.Application.Interfaces.ChatUseCases;
using FUEM.Application.Interfaces.EventUseCases;
using FUEM.Application.Interfaces.LocationUseCases;
using FUEM.Application.Interfaces.NotificationUseCases;
using FUEM.Application.Interfaces.OrganizerUseCases;
using FUEM.Application.Interfaces.RegistrationUseCases;
using FUEM.Application.Interfaces.StudentUseCases;
using FUEM.Application.Interfaces.UserUseCases;
using FUEM.Application.UseCases.CategoryUseCases;
using FUEM.Application.UseCases.ChatUseCases;
using FUEM.Application.UseCases.EventUseCases;
using FUEM.Application.UseCases.LocationUseCases;
using FUEM.Application.UseCases.NotificationUseCases;
using FUEM.Application.UseCases.OrganizerUseCases;
using FUEM.Application.UseCases.RegistrationUseCases;
using FUEM.Application.UseCases.StudentUseCases;
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
            builder.Services.AddTransient<IGetOrganizedEvents, GetOrganizedEvents>();
            builder.Services.AddTransient<IRegisterIntoEvent, RegisterIntoEvent>();
            builder.Services.AddTransient<IProcessEvent, ProcessEvent>();
            builder.Services.AddTransient<IGetRecentEvents, GetRecentEvents>();
            builder.Services.AddTransient<IGetEvent, GetEvent>();

            //Category
            builder.Services.AddTransient<IGetAllCategories, GetAllCategories>();

            // Location
            builder.Services.AddTransient<IGetAllLocation, GetAllLocation>();

            //Organizer
            builder.Services.AddTransient<IGetAllOrganizers, GetAllOrganizers>();
            builder.Services.AddTransient<IGetOrganizer, GetOrganizer>();
            builder.Services.AddTransient<IEditOrganizer, EditOrganizer>();

            //Student
            builder.Services.AddTransient<IFollowUseCase, FollowUseCase>();
            builder.Services.AddTransient<IGetStudent, GetStudent>();
            builder.Services.AddTransient<ICheckInUseCase, CheckInUseCase>();
            builder.Services.AddTransient<IManageCalendar, ManageCalendar>();

            //Chat
            builder.Services.AddTransient<IGetChat, GetChat>();

            //Admin
            builder.Services.AddTransient<ICompareEventUseCase, CompareEventUseCase>();

            //AdminNotifications
            builder.Services.AddTransient<IAdminNotification, AdminNotification>();

            //StudentNotifications
            builder.Services.AddTransient<IStudentNotification, StudentNotification>();
            builder.Services.AddScoped<IGetAttendedEvents, GetAttendedEvents>();
            builder.Services.AddScoped<IFeedback, FeedbackService>();
            //ClubNotifications
            builder.Services.AddTransient<IClubNotification, ClubNotification>();

            return builder;
        }
    }
}
