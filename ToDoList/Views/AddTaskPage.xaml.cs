using System.Runtime.Intrinsics.X86;
using ToDoList.ViewModels;

namespace ToDoList.Views;

public partial class AddTaskPage : ContentPage
{
    AddTaskViewModel vm;
    public AddTaskPage(AddTaskViewModel addTaskViewModel)
    {
        InitializeComponent();
        vm = addTaskViewModel;
        BindingContext = vm;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (vm.ModifiedTask != null)
        {
            Title = "Изменение напоминания";
            vm.FormDescription = vm.ModifiedTask.Description;
            vm.FormTaskDate = vm.ModifiedTask.TaskDate.Date;
            vm.FormTaskTime = vm.ModifiedTask.TaskDate.TimeOfDay;
            vm.SelectedReminder = vm.ModifiedTask.TaskReminderInterval;
            vm.SelectedRepeatInterval = vm.ModifiedTask.TaskRepeatInterval;
            vm.FormChecked = vm.ModifiedTask.IsCompleted;
        }
        else
        {
            Title = "Добавление напоминания";
            vm.FormTaskDate = DateTime.Now;
            vm.FormTaskTime = DateTime.Now.TimeOfDay;
            vm.SelectedReminder = vm.ReminderInterval[0];
            vm.SelectedRepeatInterval = vm.RepeatInterval[0];
        }
    }
}