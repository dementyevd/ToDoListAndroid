using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Plugin.LocalNotification;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Models;
using ToDoList.Services;
using ToDoList.Views;

namespace ToDoList.ViewModels
{

    public partial class TaskViewModel : BaseViewModel
    {
        TaskService taskService;

        public ObservableCollection<MyTask> Tasks { get; set; } = new ObservableCollection<MyTask>();
        public ObservableCollection<MyTask> CompletedTasks { get; set; } = new ObservableCollection<MyTask>();
        public ObservableCollection<MyTask> FutureTasks { get; set; } = new ObservableCollection<MyTask>();
        public ObservableCollection<MyTask> TodayTasks { get; set; } = new ObservableCollection<MyTask>();

        public TaskViewModel(TaskService taskService)
        {
            Title = "Список напоминаний";
            this.taskService = taskService;

            Task.Run(async () => await GetTasksAsync());
            LocalNotificationCenter.Current.NotificationActionTapped += Current_NotificationActionTapped;
        }
        private async void Current_NotificationActionTapped(Plugin.LocalNotification.EventArgs.NotificationActionEventArgs e)
        {
            var currNotification = e.Request;
            MyTask modifiedTask = await taskService.GetTaskByIdAsync(currNotification.NotificationId);
            var navParam = new Dictionary<string, object>
            {
                { "modifiedTask", modifiedTask }
            };
            if (e.IsDismissed)
            {
                if (modifiedTask.TaskRepeatInterval == "Никогда")
                    modifiedTask.IsCompleted = true;
                else
                {
                    while(modifiedTask.TaskDate < DateTime.Now)
                    {
                        modifiedTask.TaskDate += taskService.GetRepeatInterval(modifiedTask);
                    }
                }

                await taskService.Update(modifiedTask);
                await GetTasksAsync();

            }
            else if (e.IsTapped)
            {
                await Shell.Current.GoToAsync($"{nameof(AddTaskPage)}", true, navParam);
            }
        }

        [RelayCommand]
        public async Task GetTasksAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;
                var tempTasks = await taskService.GetTasksAsync();

                if (Tasks.Count > 0)
                {
                    Tasks.Clear();
                    CompletedTasks.Clear();
                    FutureTasks.Clear();
                    TodayTasks.Clear();
                }

                foreach (var task in tempTasks)
                {
                    Tasks.Add(task);
                    if (task.IsCompleted)
                        CompletedTasks.Add(task);
                    else
                    {
                        if (task.TaskDate.ToShortDateString() == DateTime.Today.ToShortDateString())
                            TodayTasks.Add(task);
                        else
                            //if (task.TaskDate > DateTime.Today.AddDays(1))
                            FutureTasks.Add(task);
                    }
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Ошибка", $"Ошибка при получении задач: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        async Task GoToAddTaskPageAsync(MyTask modifiedTask)
        {
            await Shell.Current.GoToAsync($"{nameof(AddTaskPage)}", true, new Dictionary<string, object>
            {
                {"modifiedTask", modifiedTask }
            });
        }

        [RelayCommand]
        async Task GoToTasksAsync(ObservableCollection<MyTask> tasks)
        {
            await Shell.Current.GoToAsync($"{nameof(DetailsPage)}", true, new Dictionary<string, object>
            {
                { "tasks", tasks }
            });

        }
    }
}


