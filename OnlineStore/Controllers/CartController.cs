using Microsoft.AspNetCore.Mvc;
using OnlineStore.Infrastructure;
using OnlineStore.Models;

namespace OnlineStore.Controllers
{
    public class CartController : Controller
    {
        private readonly OnlineStoreDbContext _context;

        public CartController(OnlineStoreDbContext context)
        {
            _context = context;
        }

        //GET: /cart/index
        [Route("Index")]
        public IActionResult Index()
        {
            List<CartItemModel> cartItemModelList =
                HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new();

            CartViewModel cartViewModel = new CartViewModel
            {
                CartItems = cartItemModelList,
                GrandTotal = cartItemModelList.Sum(cart => cart.Price * cart.Quantity)
            };

            return View(cartViewModel);
        }

        //GET: /cart/add/5
        public async Task<IActionResult> Add(int id)
        {
            ProductModel? productModel = await _context.Products.FindAsync(id);

            if (productModel == null)
                throw new NullReferenceException("product can not be null!");

            List<CartItemModel> cartItemModelList =
                HttpContext.Session.GetJson<List<CartItemModel>>("Cart") ?? new();

            CartItemModel? cartItemModel = cartItemModelList.Where(cart => cart.ProductId == id).FirstOrDefault();

            if(cartItemModel == null)
            {
                cartItemModelList.Add(new CartItemModel(productModel));
            }
            else
            {
                cartItemModel.Quantity++;
            }

            HttpContext.Session.SetJson("Cart", cartItemModelList);

            return RedirectToAction("Index");
        }
    }
}
