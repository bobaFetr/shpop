using System.Linq;
using System.Net;
//using System.Web.Mvc;
using shpop.Data;           
using shpop.Models;
using System.Data.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


public class ProductsController : Controller
{
    //private ApplicationDbContext db = new ApplicationDbContext();
    private readonly ApplicationDbContext _db;

    public ProductsController(ApplicationDbContext db)
    {
        _db = db;
    }


    // GET: Products
    public ActionResult Index()
    {
        var products = _db.Products.Include(p => p.Category).ToList();
        return View(products);
    }

    // GET: Products/Details/5
    public ActionResult Details(int? id)
    {
        if (id == null)
            //return new StatusCodes(HttpStatusCode.BadRequest);
            return BadRequest();
        var product = _db.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
        if (product == null)
            return NotFound();

        return View(product);
    }

    // GET: Products/Create
    public ActionResult Create()
    {
        ViewBag.CategoryId = new  SelectList(_db.Categories, "Id", "Name");
        return View();
    }

    // POST: Products/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(Product product)
    {
        if (ModelState.IsValid)
        {
            _db.Products.Add(product);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        ViewBag.CategoryId = new SelectList(_db.Categories, "Id", "Name", product.CategoryId);
        return View(product);
    }
}
