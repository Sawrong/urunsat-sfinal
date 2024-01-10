using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrunSatisPortali.Models;
using UrunSatisPortali.ViewModels;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace UrunSatisPortali.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly AppDbContext _context;
        public AdminController(UserManager<AppUser> userManager, AppDbContext context, RoleManager<AppRole> roleManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _context = context;

        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetUserList()
        {
            var userModels = await _userManager.Users.Select(x => new UserModel()
            {

                Id = x.Id,
                FullName = x.FullName,
                Email = x.Email,
                UserName = x.UserName,
                City = x.City
            }).ToListAsync();
            return View(userModels);
        }
        public async Task<IActionResult> GetRoleList()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }

        public IActionResult RoleAdd()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> RoleAdd(AppRole model)
        {
            var role = await _roleManager.FindByNameAsync(model.Name);
            if (role == null)
            {

                var newrole = new AppRole();
                newrole.Name = model.Name; ;
                await _roleManager.CreateAsync(newrole);
            }
            return RedirectToAction("GetRoleList");
        }

        public IActionResult GetUrunlerList()
        {
            return View();
        }

        //Kategoriler
        //public IActionResult Kategori()
        //{
        //    var degerler = _context.kategoris.ToList();
        //    return View(degerler);
        //}
        //[HttpGet]
        //public IActionResult KategoriYeni()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public IActionResult KategoriYeni(Kategori d)
        //{
        //    _context.Add(d);
        //    _context.SaveChanges();
        //    return RedirectToAction("Kategori");
        //}
        //public IActionResult KategoriSil(int id)
        //{
        //    var dep = _context.kategoris.Find(id);
        //    _context.kategoris.Remove(dep);
        //    _context.SaveChanges();
        //    return RedirectToAction("Kategori");
        //}
        //public IActionResult KategoriGetir(int id)
        //{
        //    var dep = _context.kategoris.Find(id);
        //    return View("KategoriGetir", dep);
        //}

        //public IActionResult KategoriDetay(int id)
        //{
        //    var degerler = _context.uruns.Where(x => x.kategori.kategoriID == id).ToList();
        //    return View(degerler);
        //}
        //public IActionResult KategoriGuncelle(Kategori d)
        //{
        //    var dep = _context.kategoris.Find(d.kategoriID);
        //    dep.kategoriAD = d.kategoriAD;
        //    _context.SaveChanges();
        //    return RedirectToAction("Kategori");
        //}
        //URUNLER 
        public IActionResult Product()
        {
            var degerler = _context.Products.Include(x => x.categoryies).ToList();
            return View(degerler);
        }
        [HttpGet]
        public IActionResult AddProduct()
        {
            List<SelectListItem> degerler = (from x in _context.Categories.ToList()
                                             select new SelectListItem
                                             {
                                                 Text = x.kategoriAD,
                                                 Value = x.kategoriID.ToString()
                                             }).ToList();
            ViewBag.dgr = degerler;
            return View();
        }
        [HttpPost]
        public IActionResult AddProduct(UrunEkle d)
        {
            Product f = new Product();
            var per = _context.Categories.Where(x => x.kategoriID == d.categoryID).FirstOrDefault();
            if (d.productImage != null)
            {
                string imageExtension = Path.GetExtension(d.productImage.FileName);

                string imageName = Guid.NewGuid() + imageExtension;

                string path = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot/img/{imageName}");

                var stream = new FileStream(path, FileMode.Create);

                d.productImage.CopyTo(stream);
                f.productImage = imageName;

            }
            f.productName = d.productName;
            f.productStock = d.productStock;
            f.productExplain = d.productExplain;
            f.productPrice = d.productPrice;
            f.categoryID = d.categoryID;
            _context.Products.Add(f);
            _context.SaveChanges();
            return RedirectToAction("Product");
        }
        public IActionResult DeleteProduct(int id)
        {
            var dep = _context.Products.Find(id);
            _context.Products.Remove(dep);
            _context.SaveChanges();
            return RedirectToAction("Product");
        }
        public IActionResult UrunGetir(int id)
        {
            var dep = _context.Products.Find(id);
            return View("UrunGetir", dep);
        }
    }
}
