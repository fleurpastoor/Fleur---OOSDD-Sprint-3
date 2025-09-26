using Grocery.App.ViewModels;

namespace Grocery.App.Views;

public partial class RegisterView : ContentPage
{
	public RegisterView(RegistrationViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}
}