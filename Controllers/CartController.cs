using System.Linq;
using System.Net;
//using System.Web.Mvc;
using shpop.Data;           
using shpop.Models;
using System.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

public class CartController : Controller
{
   private readonly ApplicationDbContext _db;

    public CartController(ApplicationDbContext db)
    {
        _db = db;
    }

    private List<OrderItem> GetCart()
    {
        // if (Session["cart"] == null)
        //     Session["cart"] = new List<OrderItem>();
        // return (List<OrderItem>)Session["cart"];
        var sessionCart = HttpContext.Session.GetObject<List<OrderItem>>("cart") ?? new List<OrderItem>();
    HttpContext.Session.SetObject("cart", sessionCart);
    return sessionCart;

    }

    public ActionResult Index()
    {
        return View(GetCart());
    }

    [HttpPost]
    public ActionResult AddToCart(int id)
    {
        var product = _db.Products.Find(id);
        if (product == null)
           // return HttpNotFound();
            return NotFound();
        var cart = GetCart();
        var item = cart.FirstOrDefault(x => x.Product.Id == id);

        if (item != null)
        {
            item.Quantity++;
        }
        else
        {
            cart.Add(new OrderItem { Product = product, Quantity = 1 });
        }

        return RedirectToAction("Index");
    }

    public ActionResult RemoveFromCart(int id)
    {
        var cart = GetCart();
        var item = cart.FirstOrDefault(x => x.Product.Id == id);
        if (item != null)
            cart.Remove(item);

        return RedirectToAction("Index");
    }

    [HttpPost]
    public ActionResult Checkout()
    {
        var cart = GetCart();
        if (!cart.Any())
            return RedirectToAction("Index");

        // В реална система вземаш текущия потребител
        var order = new Order
        {
            CustomerId = 1, // dummy user
            OrderDate = System.DateTime.Now,
            OrderItems = cart
        };

        _db.Orders.Add(order);
        _db.SaveChanges();

       // Session["cart"] = null;
        HttpContext.Session.Remove("cart");
        // Clear the cart session
        return RedirectToAction("Confirmation");
    }

    public ActionResult Confirmation()
    {
        return View();
    }
}
