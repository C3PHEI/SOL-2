using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        private Dictionary<string, int> _bills = new Dictionary<string, int>
        {
            { "10", 0 },
            { "20", 0 },
            { "50", 0 },
            { "100", 0 },
            { "200", 0 }
        };

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

        public Dictionary<string, int> Bills
        {
            get => _bills;
            set
            {
                _bills = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SumBills));
            }
        }

        public int SumBills => _bills.Sum(b => int.Parse(b.Key) * b.Value);

        public ICommand PriceTextInputCommand { get; }
        public ICommand PricePasteCommand { get; }

        public MainViewModel()
        {
            PriceTextInputCommand = new RelayCommand(OnPreviewTextInput);
            PricePasteCommand = new RelayCommand(OnPaste);
        }

        public void SetAccountInfo(string accountInfo)
        {
            AccountInfo = accountInfo;
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

        public void UpdateBill(string denomination, int value)
        {
            if (_bills.ContainsKey(denomination))
            {
                _bills[denomination] = value;
                OnPropertyChanged(nameof(Bills));
                OnPropertyChanged(nameof(SumBills));
            }
        }
    }
}