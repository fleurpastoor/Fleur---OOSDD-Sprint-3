
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Grocery.Core.Interfaces.Services;
using Grocery.App.Views;


namespace Grocery.App.ViewModels
{
    public partial class RegistrationViewModel : BaseViewModel
    {
        private readonly IAuthService _authService;
        [ObservableProperty] private string name;
        [ObservableProperty] private string email;
        [ObservableProperty] private string password;
        [ObservableProperty] private string registrationMessage;

        public RegistrationViewModel(IAuthService authService)
        {
            _authService = authService;
        }

        [RelayCommand]
        public void RegisterUser()
        {
            if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                RegistrationMessage = "Vul alle velden in.";
                return;
            }

            try
            {
                _authService.Register(Name, Email, Password);
                RegistrationMessage = "Account succesvol aangemaakt, je kunt nu inloggen.";
            }
            catch
            {
                RegistrationMessage = "Ongeldige gegevens ingevoerd.";
            }
        }
        
        [RelayCommand]
        private void GoToLogin()
        {
            Application.Current.MainPage = new LoginView(new LoginViewModel(_authService, new GlobalViewModel()));
        }
    }
}
