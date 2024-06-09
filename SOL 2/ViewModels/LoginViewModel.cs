using SOL_2.Models;
using SOL_2.Services;
using SOL_2.Views;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SOL_2.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private string _username;
        private string _password;
        private string _message;

        // Neue Property für das Schließen des Fensters
        public Action CloseAction { get; set; }

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged();
            }
        }

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged();
            }
        }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            _apiService = new ApiService();
            LoginCommand = new RelayCommand(async _ => await LoginAsync());
        }

        private async Task LoginAsync()
        {
            if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
            {
                Message = "Geben Sie etwas ein!";
                return;
            }

            var user = new User { Username = Username, Password = Password };
            var response = await _apiService.LoginAsync(user);

            if (response != null && response.Message == "Login erfolgreich!")
            {
                Message = response.Message;
                var mainWindow = new MainWindow();
                var mainViewModel = (MainViewModel)mainWindow.DataContext;
                mainViewModel.SetAccountInfo(Username);
                mainWindow.Show();
                CloseAction?.Invoke();
            }
            else
            {
                Message = response?.Message ?? "Login fehlgeschlagen.";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
