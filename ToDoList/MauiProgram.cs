using Microsoft.Extensions.Logging;
using Plugin.LocalNotification;
using ToDoList.Services;
using ToDoList.ViewModels;
using ToDoList.Views;

namespace ToDoList
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseLocalNotification()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<TaskService>();

            builder.Services.AddSingleton<TaskViewModel>();
            builder.Services.AddTransient<AddTaskViewModel>();
            builder.Services.AddTransient<DetailPageViewModel>();

            builder.Services.AddSingleton<MainPage>();
            builder.Services.AddTransient<AddTaskPage>();
            builder.Services.AddTransient<DetailsPage>();
           

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
