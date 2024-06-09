using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace SOL_2.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string _price;
        private string _accountInfo;

        public string Price
        {
            get => _price;
            set
            {
                if (IsTextAllowed(value))
                {
                    _price = value;
                    OnPropertyChanged();
                }
            }
        }

        public string AccountInfo
        {
            get => _accountInfo;
            set
            {
                _accountInfo = value;
                OnPropertyChanged();
            }
        }

        public ICommand PriceTextInputCommand { get; }
        public ICommand PricePasteCommand { get; }

        public MainViewModel()
        {
            PriceTextInputCommand = new RelayCommand(OnPreviewTextInput);
            PricePasteCommand = new RelayCommand(OnPaste);
        }

        private void OnPreviewTextInput(object parameter)
        {
            if (parameter is TextCompositionEventArgs e)
            {
                if (!Regex.IsMatch(e.Text, "^[0-9,.]$"))
                {
                    e.Handled = true;
                    return;
                }

                if ((e.Text == "." || e.Text == ",") && (Price.Contains(".") || Price.Contains(",")))
                {
                    e.Handled = true;
                }
            }
        }

        private void OnPaste(object parameter)
        {
            if (parameter is DataObjectPastingEventArgs e)
            {
                if (e.DataObject.GetDataPresent(typeof(string)))
                {
                    string text = (string)e.DataObject.GetData(typeof(string));
                    if (!IsTextAllowed(text))
                    {
                        e.CancelCommand();
                    }
                }
                else
                {
                    e.CancelCommand();
                }
            }
        }

        private static bool IsTextAllowed(string text)
        {
            return Regex.IsMatch(text, @"^[0-9]*[.,]?[0-9]*$");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}