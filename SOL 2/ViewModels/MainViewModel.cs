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
                    _price = value?.Replace(',', '.') ?? string.Empty; // Replace comma with dot
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
                OnPropertyChanged(nameof(SumBillsWithCurrency));
            }
        }

        public int SumBills => _bills.Sum(b => int.Parse(b.Key) * b.Value);

        public string SumBillsWithCurrency => $"{SumBills} CHF";

        public ICommand PriceTextInputCommand { get; }
        public ICommand PricePasteCommand { get; }
        public ICommand ValidateCommand { get; }
        public ICommand ResetCommand { get; }

        public MainViewModel()
        {
            PriceTextInputCommand = new RelayCommand(OnPreviewTextInput);
            PricePasteCommand = new RelayCommand(OnPaste);
            ValidateCommand = new RelayCommand(Validate);
            ResetCommand = new RelayCommand(Reset);
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
    }
}