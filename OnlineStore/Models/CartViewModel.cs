namespace OnlineStore.Models
{
    public class CartViewModel
    {
        public List<CartItemModel> CartItems { get; set; } = null!;
        public decimal GrandTotal { get; set; }
    }
}
