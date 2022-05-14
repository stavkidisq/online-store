namespace OnlineStore.Models
{
    public class CartItemModel
    {
        public CartItemModel(ProductModel productModel)
        {
            ProductId = productModel.Id;
            ProductName = productModel.Name;
            Quantity = 1;
            Price = productModel.Price;
            Image = productModel.Image;
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalPrice => Quantity * Price;
        public string? Image { get; set; }
    }
}
