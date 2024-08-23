using ToDoList.Models;
using ToDoList.ViewModels;

namespace ToDoList.Views;

public partial class DetailsPage : ContentPage
{
	public DetailsPage(DetailPageViewModel detailPageViewModel)
	{
		InitializeComponent();
		BindingContext = detailPageViewModel;
	}
}