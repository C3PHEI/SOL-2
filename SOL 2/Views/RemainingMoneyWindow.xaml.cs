using SOL_2.ViewModels;
using System.Windows;

namespace SOL_2.Views
{
    public partial class RemainingMoneyWindow : Window
    {
        public RemainingMoneyWindow(string userId)
        {
            InitializeComponent();
            var viewModel = new RemainingMoneyViewModel(userId);
            DataContext = viewModel;
            viewModel.LoadRemainingMoneyCommand.Execute(null);
        }
    }
}