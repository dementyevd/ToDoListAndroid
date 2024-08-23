using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using ToDoList.Models;
using ToDoList.Services;
using ToDoList.Views;

namespace ToDoList.ViewModels
{
    [QueryProperty(nameof(Tasks), "tasks")]
    public partial class DetailPageViewModel : BaseViewModel
    {
        readonly TaskService taskService;
        public DetailPageViewModel()
        {
            this.taskService = new TaskService();
            Title = "Список напоминаний";
        }

        [ObservableProperty]
        ObservableCollection<MyTask> tasks;


        [RelayCommand]
        public async Task DeleteTaskAsync(MyTask deletedTask)
        {
            if (deletedTask == null) return;
            else
            {
                await taskService.Delete(deletedTask);
                Tasks.Remove(deletedTask);
            }
        }

        [RelayCommand]
        public async Task EditTaskAsync(MyTask modifiedTask)
        {
            var navParam = new Dictionary<string, object>
            {
                { "modifiedTask", modifiedTask }
            };
            await Shell.Current.GoToAsync($"{nameof(AddTaskPage)}", true, navParam);
        }
    }
}
