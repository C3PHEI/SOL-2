using SOL_2.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SOL_2.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (DataContext is MainViewModel viewModel)
            {
                viewModel.PriceTextInputCommand.Execute(e);
            }
        }

        private void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (DataContext is MainViewModel viewModel)
            {
                viewModel.PricePasteCommand.Execute(e);
            }
        }
    }
}