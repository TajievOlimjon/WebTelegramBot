using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiTelegramBot.Services.EntitiesServices;

namespace WebApiTelegramBot
{
    public static class RegisterService
    {
        public static IServiceCollection RegisterServices(this IServiceCollection services)
        {
            services.AddScoped<StudentService>();
            services.AddScoped<GroupService>();
            services.AddScoped<StudentGroupService>();
            services.AddScoped<ProgressBookService>();
            services.AddScoped<TimeTableService>();
            return services;
        }
    }
}
