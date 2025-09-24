namespace ItemManagement.WebAPI.Models
{
    public class TransactionApiModel
    {
        public int TransactionId { get; set; }
        public DateTime TimeStamp { get; set; }
        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public double Price { get; set; }
        public int BeforeQty { get; set; }
        public int SoldQty { get; set; }
        public string? CashierName { get; set; }
    }
}
