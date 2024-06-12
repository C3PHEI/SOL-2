namespace SOL_2_API.Dtos
{
    public class CashRegisterDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public Dictionary<string, int> Bills { get; set; }
        public Dictionary<string, int> Coins { get; set; }
    }
}