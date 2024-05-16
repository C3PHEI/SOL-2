namespace SOL_2_API.Models
{
    public class RegisterModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class ChangeModel
    {
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
    }

    public class ApplyChangeModel
    {
        public Dictionary<string, int> Change { get; set; }
    }

    public class AddCashModel
    {
        public string Username { get; set; }
        public Dictionary<string, int> Bills { get; set; }
        public Dictionary<string, int> Coins { get; set; }
    }

    public class SendMessageModel
    {
        public string Message { get; set; }
    }
}

