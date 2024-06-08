using SOL_2.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace SOL_2.Views
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            var viewModel = new LoginViewModel();
            viewModel.CloseAction = new Action(this.Close);
            DataContext = viewModel;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is LoginViewModel viewModel)
            {
                viewModel.Password = ((PasswordBox)sender).Password;
            }
        }
    }
}