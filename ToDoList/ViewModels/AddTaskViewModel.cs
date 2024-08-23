using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Models;
using ToDoList.Services;


namespace ToDoList.ViewModels
{
    [QueryProperty(nameof(ModifiedTask), "modifiedTask")]
    public partial class AddTaskViewModel : BaseViewModel
    {
        readonly TaskService taskService;

        [ObservableProperty]
        MyTask modifiedTask;

        [ObservableProperty]
        string formDescription;

        [ObservableProperty]
        DateTime formTaskDate;

        [ObservableProperty]
        TimeSpan formTaskTime;

        [ObservableProperty]
        string selectedReminder;

        [ObservableProperty]
        string selectedRepeatInterval;

        [ObservableProperty]
        bool formChecked;

        public List<string> ReminderInterval { get; set; } = new List<string>
        {
            "Нет",
            "За 5 минут",
            "За 10 минут",
            "За 15 минут",
            "За 30 минут",
            "За 1 час",
            "За 2 часа",
            "За 1 день"
        };
        public List<string> RepeatInterval { get; set; } = new List<string>
        {
            "Никогда",
            "Каждый час",
            "Каждый день",
            "Каждую неделю",
            "Каждый месяц",
            "Каждый год"
        };

        public AddTaskViewModel()
        {
            this.taskService = new TaskService();
        }

        public async Task AddTaskAsync()
        {
            await taskService.Add(new MyTask
            {
                Id = 1,
                Description = FormDescription,
                TaskDate = FormTaskDate + FormTaskTime,
                IsCompleted = false,
                TaskReminderInterval = SelectedReminder,
                TaskRepeatInterval = SelectedRepeatInterval
            });
            await Shell.Current.GoToAsync("..");
        }

        public async Task UpdatetaskAsync(MyTask task)
        {
            task.Description = FormDescription;
            task.TaskDate = FormTaskDate + FormTaskTime;
            task.TaskReminderInterval = SelectedReminder;
            task.TaskRepeatInterval = SelectedRepeatInterval;
            task.IsCompleted = FormChecked;
            await taskService.Update(task);
            await Shell.Current.GoToAsync("../..");
        }

        [RelayCommand]
        public async Task SaveChangesAsync()
        {
            if (FormTaskDate + FormTaskTime < DateTime.Now && !FormChecked)
            {
                await Shell.Current.DisplayAlert("Некорректная дата", "Нельзя выбирать прошлый период!", "OK");
            }
            else
            {
                if (ModifiedTask == null)
                    await AddTaskAsync();
                else
                    await UpdatetaskAsync(ModifiedTask);
            }
        }
    }
}
