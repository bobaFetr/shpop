using System.Linq;
using System.Net;
//using System.Web.Mvc;
using shpop.Data;           
using shpop.Models;
using System.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Http;

public class OrdersController : Controller
{
    private readonly ApplicationDbContext _db;

    public OrdersController(ApplicationDbContext db)
    {
        _db = db;
    }

    public ActionResult Index()
    {
        int customerId = 1; // В реален проект ще вземеш логнатия потребител
        var orders = _db.Orders
            .Where(o => o.CustomerId == customerId)
            .Include(o => o.OrderItems.Select(oi => oi.Product))
            .OrderByDescending(o => o.OrderDate)
            .ToList();

        return View(orders);
    }

    public ActionResult Details(int id)
    {
        var order = _db.Orders
            .Include(o => o.OrderItems.Select(oi => oi.Product))
            .FirstOrDefault(o => o.Id == id);

        if (order == null)
            // return HttpNotFound();
            return NotFound();

        return View(order);
    }
}
