using System.Collections.Generic;

namespace SOL_2.Models
{
    public class CashRegister
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public Dictionary<string, int> Bills { get; set; }
        public Dictionary<string, int> Coins { get; set; }
    }
}

