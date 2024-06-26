﻿using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using SOL_2.Services;
using SOL_2.Models;

namespace SOL_2.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private string _username;
        private string _password;
        private string _message;

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

        public Action CloseAction { get; set; }

        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            _apiService = new ApiService();
            LoginCommand = new RelayCommand(async _ => await LoginAsync());
        }

        private async Task LoginAsync()
        {
            var user = new User { Username = Username, Password = Password };
            var tokenResponse = await _apiService.LoginAsync(user);

            if (tokenResponse != null)
            {
                Message = "Login erfolgreich!";
                ApplicationState.UserId = tokenResponse.UserId; // Store the UserId
                var mainWindow = new Views.MainWindow();
                mainWindow.Show();
                CloseAction?.Invoke(); // Close the LoginWindow
            }
            else
            {
                Message = "Login fehlgeschlagen.";
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}