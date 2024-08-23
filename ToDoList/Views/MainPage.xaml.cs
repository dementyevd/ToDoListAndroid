using ToDoList.Services;
using ToDoList.ViewModels;

namespace ToDoList
{
    public partial class MainPage : ContentPage
    {
        TaskViewModel viewModel;
        public MainPage(TaskViewModel taskViewModel)
        {
            InitializeComponent();
            viewModel = taskViewModel;
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await viewModel.GetTasksAsync();
        }
    }
}
