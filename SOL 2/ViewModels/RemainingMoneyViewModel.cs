using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using SOL_2.Models;
using SOL_2.Services;

namespace SOL_2.ViewModels
{
    public class RemainingMoneyViewModel : INotifyPropertyChanged
    {
        private readonly ApiService _apiService;
        private readonly string _userId;

        public ObservableCollection<CashRegisterItem> Bills { get; set; }
        public ObservableCollection<CashRegisterItem> Coins { get; set; }

        public ICommand LoadRemainingMoneyCommand { get; }

        public RemainingMoneyViewModel(string userId)
        {
            _userId = userId;
            _apiService = new ApiService();
            Bills = new ObservableCollection<CashRegisterItem>();
            Coins = new ObservableCollection<CashRegisterItem>();

            LoadRemainingMoneyCommand = new RelayCommand(async param => await LoadRemainingMoney());
        }

        public async Task LoadRemainingMoney()
        {
            var cashRegisters = await _apiService.GetCashRegisterByUserIdAsync(_userId);
            if (cashRegisters != null && cashRegisters.Any())
            {
                var cashRegister = cashRegisters.First();

                Bills.Clear();
                foreach (var bill in cashRegister.Bills)
                {
                    Bills.Add(new CashRegisterItem { Denomination = bill.Key, Quantity = bill.Value });
                }

                Coins.Clear();
                foreach (var coin in cashRegister.Coins)
                {
                    Coins.Add(new CashRegisterItem { Denomination = coin.Key, Quantity = coin.Value });
                }

                OnPropertyChanged(nameof(Bills));
                OnPropertyChanged(nameof(Coins));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}