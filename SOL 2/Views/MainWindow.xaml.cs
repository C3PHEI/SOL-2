using SOL_2.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;

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

        private void OnBillValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (DataContext is MainViewModel viewModel && sender is IntegerUpDown integerUpDown)
            {
                if (integerUpDown.Tag != null)
                {
                    var denomination = integerUpDown.Tag.ToString();
                    viewModel.UpdateBill(denomination, integerUpDown.Value ?? 0);
                }
            }
        }
    }
}