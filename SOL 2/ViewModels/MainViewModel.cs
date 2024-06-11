using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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
        private string _message;

        private Dictionary<string, int> _bills = new Dictionary<string, int>
        {
            { "10", 0 },
            { "20", 0 },
            { "50", 0 },
            { "100", 0 },
            { "200", 0 }
        };

        private Dictionary<string, int> _change = new Dictionary<string, int>();

        public string Price
        {
            get => _price;
            set
            {
                if (IsTextAllowed(value))
                {
                    _price = value?.Replace(',', '.') ?? string.Empty; // Replace comma with dot
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(CanCalculate));
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

        public string Message
        {
            get => _message;
            set
            {
                _message = value;
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
                OnPropertyChanged(nameof(SumBillsWithCurrency));
                OnPropertyChanged(nameof(CanCalculate));
            }
        }

        public Dictionary<string, int> Change
        {
            get => _change;
            set
            {
                _change = value;
                OnPropertyChanged();
            }
        }

        public int SumBills => _bills.Sum(b => int.Parse(b.Key) * b.Value);

        public string SumBillsWithCurrency => $"{SumBills} CHF";

        public bool CanCalculate => decimal.TryParse(Price, NumberStyles.Any, CultureInfo.InvariantCulture, out var priceValue) && SumBills >= priceValue;

        public ICommand PriceTextInputCommand { get; }
        public ICommand PricePasteCommand { get; }
        public ICommand ValidateCommand { get; }
        public ICommand ResetCommand { get; }
        public ICommand CalculateCommand { get; }

        public MainViewModel()
        {
            PriceTextInputCommand = new RelayCommand(OnPreviewTextInput);
            PricePasteCommand = new RelayCommand(OnPaste);
            ValidateCommand = new RelayCommand(Validate);
            ResetCommand = new RelayCommand(Reset);
            CalculateCommand = new RelayCommand(Calculate, param => CanCalculate);
        }

        public void SetAccountInfo(string accountInfo)
        {
            AccountInfo = accountInfo;
        }

        private void OnPreviewTextInput(object parameter)
        {
            if (parameter is TextCompositionEventArgs e)
            {
                string currentText = Price ?? string.Empty;
                int caretIndex = GetCaretIndex();

                string newText = currentText.Insert(caretIndex, e.Text);
                if (!IsTextAllowed(newText))
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
                    string currentText = Price ?? string.Empty;
                    int caretIndex = GetCaretIndex();

                    string newText = currentText.Insert(caretIndex, text);
                    if (!IsTextAllowed(newText))
                    {
                        e.CancelCommand();
                    }
                    else
                    {
                        text = text.Replace(',', '.');
                        Price = newText;
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
            return Regex.IsMatch(text, @"^[0-9]*[.,]?[0-9]{0,2}$");
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
                OnPropertyChanged(nameof(SumBillsWithCurrency));
                OnPropertyChanged(nameof(CanCalculate));
            }
        }

        private void Validate(object parameter)
        {
            if (decimal.TryParse(Price?.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal priceValue))
            {
                if (SumBills < priceValue)
                {
                    // Zeige Fehlermeldung in neuem Fenster
                    MessageBox.Show("Der Betrag der hinzugefügten Scheine ist geringer als der zu zahlende Betrag.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    // Speichere die Werte
                    MessageBox.Show("Der Betrag wurde erfolgreich gespeichert.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                    // Hier können Sie den Code zum Speichern der Werte hinzufügen
                    SaveValues();
                }
            }
            else
            {
                MessageBox.Show("Ungültiger Preis eingegeben.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveValues()
        {
            // Implementieren Sie hier die Logik zum Speichern der Werte
            // Beispiel: Speichern Sie die Werte in einer Datenbank oder einer Datei
        }

        private int GetCaretIndex()
        {
            // Implementieren Sie eine Methode, um den aktuellen Caret-Index zu erhalten
            // In WPF können Sie dies durch ein Ereignis oder eine Bindung erreichen
            // Beispiel: TextBox.CaretIndex
            return 0; // Placeholder-Wert, ersetzen Sie dies durch die tatsächliche Implementierung
        }

        private void Reset(object parameter)
        {
            foreach (var key in Bills.Keys.ToList())
            {
                Bills[key] = 0;
            }

            OnPropertyChanged(nameof(Bills));
            OnPropertyChanged(nameof(SumBills));
            OnPropertyChanged(nameof(SumBillsWithCurrency));
            Price = string.Empty;
        }

        private void Calculate(object parameter)
        {
            if (CanCalculate)
            {
                // Berechnung des Rückgelds
                CalculateChange();

                // Öffnen des MoneyBack-Fensters
                var moneyBackWindow = new Views.MoneyBack();
                moneyBackWindow.DataContext = this; // Setze das DataContext des MoneyBack-Fensters
                moneyBackWindow.Show();
            }
        }

        private void CalculateChange()
        {
            if (decimal.TryParse(Price?.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal priceValue))
            {
                decimal changeAmount = SumBills - priceValue;
                Change = CalculateBillsAndCoins(changeAmount);
            }
        }

        private Dictionary<string, int> CalculateBillsAndCoins(decimal amount)
        {
            var result = new Dictionary<string, int>
            {
                { "200", 0 },
                { "100", 0 },
                { "50", 0 },
                { "20", 0 },
                { "10", 0 },
                { "5", 0 },
                { "2", 0 },
                { "1", 0 },
                { "0.5", 0 },
                { "0.2", 0 },
                { "0.1", 0 },
                { "0.05", 0 },
                { "0.01", 0 }
            };

            var denominations = new List<decimal> { 200m, 100m, 50m, 20m, 10m, 5m, 2m, 1m, 0.5m, 0.2m, 0.1m, 0.05m, 0.01m };

            foreach (var denom in denominations)
            {
                var key = denom.ToString(CultureInfo.InvariantCulture); // Ensure consistent key format

                while (amount >= denom)
                {
                    result[key]++;
                    amount -= denom;
                }
            }

            return result;
        }
    }
}