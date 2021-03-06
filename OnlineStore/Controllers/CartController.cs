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

            //If http request is not AJAX request
            if (HttpContext.Request.Headers["X-Requested-With"] != "XMLHttpRequest")
            {
                return RedirectToAction("Index");
            }

            return ViewComponent("SmallCart");
        }

        //GET: cart/decrease/5
        public IActionResult Decrease(int id)
        {
            List<CartItemModel>? cartItemModelList =
                HttpContext.Session.GetJson<List<CartItemModel>>("Cart");

            if (cartItemModelList == null)
                throw new NullReferenceException("Object cartItemModelList can not be null!");

            CartItemModel? cartItemModel = cartItemModelList.Where(cart => cart.ProductId == id).FirstOrDefault();

            if (cartItemModel == null)
                throw new NullReferenceException("Object cartItemModel can not be null!");

            if(cartItemModel.Quantity > 1)
            {
                --cartItemModel.Quantity;
            }
            else
            {
                cartItemModelList.Remove(cartItemModel);
            }

            if (cartItemModelList.Count == 0)
                HttpContext.Session.Remove("Cart");
            else
                HttpContext.Session.SetJson("Cart", cartItemModelList);

            return RedirectToAction("Index");
        }

        //GET: /cart/remove/5
        public IActionResult Remove(int id)
        {
            List<CartItemModel>? cartItemModelList =
                HttpContext.Session.GetJson<List<CartItemModel>>("Cart");

            if (cartItemModelList == null)
                throw new NullReferenceException("Object cartItemModelList can not be null!");

            cartItemModelList.RemoveAll(cart => cart.ProductId == id);

            if (cartItemModelList.Count == 0)
                HttpContext.Session.Remove("Cart");
            else
                HttpContext.Session.SetJson("Cart", cartItemModelList);

            return RedirectToAction("Index");
        }

        //GET: /cart/clear
        public IActionResult Clear()
        {
            HttpContext.Session.Remove("Cart");

            return Redirect(Request.Headers["Referer"].ToString());
        }
    }
}
