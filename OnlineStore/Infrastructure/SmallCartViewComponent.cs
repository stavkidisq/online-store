using Microsoft.AspNetCore.Mvc;
using OnlineStore.Models;

namespace OnlineStore.Infrastructure
{
    public class SmallCartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            List<CartItemModel>? cartModel = 
                HttpContext.Session.GetJson<List<CartItemModel>>("Cart");

            SmallCartViewModel? smallCartViewModel;

            if (cartModel == null || cartModel.Count == 0)
            {
                smallCartViewModel = null;
            }
            else
            {
                smallCartViewModel = new SmallCartViewModel()
                {
                    NumberOfItems = cartModel.Sum(cart => cart.Quantity),
                    TotalAmount = cartModel.Sum(cart => cart.Quantity * cart.Price)
                };
            }

            return View(smallCartViewModel);
        }
    }
}
