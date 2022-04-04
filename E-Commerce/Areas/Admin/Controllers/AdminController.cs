using E_Commerce.Models;
using E_Commerce.repositry;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]

    public class AdminController : Controller
    {
        private readonly Eshopcontext _db;
        private readonly IWebHostEnvironment _webhost;
        private readonly IAccountrepostry _accountrepostry;
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public AdminController(Eshopcontext db, IWebHostEnvironment webhost, IAccountrepostry accountrepostry,
            UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
            _usermanager = userManager;
            _db = db;
            _webhost = webhost;
            _accountrepostry = accountrepostry;
        }

        public IActionResult Index()
        {
            TempData["getid"] = _usermanager.GetUserId(HttpContext.User);
            var id = TempData["getid"];
            ViewBag.q2 = _db.tblproduct.Where(x => x.prd_status == 1).Count();
            ViewBag.q1 = _db.tblcategory.Count();
            ViewBag.q3 = _accountrepostry.getcount();
            ViewBag.q4 = _db.tblorder.Where(x => x.status == "Deliver").Sum(x => x.totalpay);
            return View();
        }
        [Route("AddProduct")]
        public IActionResult AddProduct()
        {

            ViewBag.list = new SelectList(GetCategories(), "cat_id", "category_name");
            return View();
        }
        private List<Category> GetCategories()
        {
            var dbs = _db.tblcategory.ToList();
            return dbs;
        }
        [Route("AddProduct")]
        [HttpPost]
        public async Task<IActionResult> AddProduct(ProductsModel prd)
        {
            if (ModelState.IsValid)
            {
                string filename = uploadfile(prd);

                var product = new Product
                {
                    Product_name = prd.Product_name,
                    ImageFile = filename,
                    product_desc = prd.product_desc,
                    product_price = prd.product_price.HasValue ? prd.product_price.Value : 0,
                    product_qty = prd.product_qty.HasValue ? prd.product_qty.Value : 0,
                    cat_id = prd.cat_id.HasValue ? prd.cat_id.Value : 0
                };
                await _db.tblproduct.AddAsync(product);
                await _db.SaveChangesAsync();
                ViewBag.status = true;
                ViewBag.alertmesaage = "Product Inserted SuccesFully";
                return RedirectToAction("ViewProduct", "Admin", new { area = "Admin" });
            }
            ViewBag.list = new SelectList(GetCategories(), "cat_id", "category_name");
            ViewBag.status = false;
            ViewBag.alertmesaage = "Product Inserted Fail";
            return View(prd);

        }
        public string uploadfile(ProductsModel prd)
        {
            string filename = null;
            if (prd.ImageFile != null)
            {
                string uploadDir = Path.Combine(_webhost.WebRootPath, "Products");
                filename = Guid.NewGuid().ToString() + "-" + prd.ImageFile.FileName;
                string filePath = Path.Combine(uploadDir, filename);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    prd.ImageFile.CopyTo(fileStream);

                }
            }
            return filename;

        }
        [Route("ViewProduct")]
        public IActionResult ViewProduct(int pagenumber = 1, string search = "")
        {
            List<Category> categories = _db.tblcategory.ToList();
            List<Product> products = _db.tblproduct.ToList();
            var product = from e1 in categories
                          join e2 in products on e1.cat_id equals e2.cat_id into tabel1
                          from e2 in tabel1.ToList()
                          where e2.prd_status == 1
                          orderby e2.product_id
                          select new Category_Product
                          {
                              categories = e1,
                              products = e2

                          };

            if (!(string.IsNullOrEmpty(search)))
            {
                /* var categorys = _db.tblcategory.ToList();*/

                var prd = from e1 in categories
                          join e2 in products on e1.cat_id equals e2.cat_id into tabel1
                          from e2 in tabel1.Where(x => e1.category_name == search || x.Product_name.Contains(search) && x.prd_status == 1)
                          .ToList()

                          select new Category_Product
                          {
                              categories = e1,
                              products = e2

                          };


                if (prd.Count() > 0)
                {
                    const int pagesize = 9;
                    if (pagenumber < 1)

                        pagenumber = 1;
                    int reccount = prd.Count();
                    var pager = new PagenatedList(reccount, pagenumber, pagesize);
                    var recskip = (pagenumber - 1) * pagesize;
                    var data = prd.Skip(recskip).Take(pager.PageSize).ToList();
                    this.ViewBag.Pager = pager;
                    return View(data);
                    /*return View(prd);*/
                }
                else
                {
                    ViewBag.alert = "Product Not Found Here";
                    int pagesize = 9;
                    if (pagenumber < 1)

                        pagenumber = 1;
                    int reccount = product.Count();
                    var pager = new PagenatedList(reccount, pagenumber, pagesize);
                    var recskip = (pagenumber - 1) * pagesize;
                    var data = product.Skip(recskip).Take(pager.PageSize).ToList();
                    this.ViewBag.Pager = pager;
                    return View(data);

                }
            }

            else
            {

                const int pagesize = 9;
                if (pagenumber < 1)
                {
                    pagenumber = 1;
                }
                else
                {
                    int reccount = product.Count();
                    var pager = new PagenatedList(reccount, pagenumber, pagesize);
                    var recskip = (pagenumber - 1) * pagesize;
                    var data = product.Skip(recskip).Take(pager.PageSize).ToList();
                    this.ViewBag.Pager = pager;
                    return View(data);
                }
            }
            return View(product);
        }            /*return View(product);*/

        //get
        /* [HttpGet]*/
        [Route("AddCategory")]
        public IActionResult AddCategory()
        {
            return View();
        }

        //post
        [Route("AddCategory")]
        [HttpPost]
        public async Task<IActionResult> AddCategory(Category obj)
        {
            if (ModelState.IsValid)
            {

                var check = _db.tblcategory.Where(x => x.category_name == obj.category_name);
                if (check.Count() == 0)
                {
                    await _db.tblcategory.AddAsync(obj);
                    await _db.SaveChangesAsync();
                    ViewBag.status = true;
                    ViewBag.alertmesaage = "Category Inserted SuccesFully";

                    return RedirectToAction("ViewCategory", "Admin", new { area = "Admin" });
                }
                else
                {
                    ViewBag.status = false;
                    ViewBag.alertmesaage = "Category Name Allready Exsists";
                    ModelState.Clear();
                    return View();
                }
                /* if (obj.cat_id > 0) {*/
            }

            /*        }*/
            ViewBag.status = false;
            ViewBag.alertmesaage = "Category Inserted Fail";
            return View();


        }




        [Route("viewcategory")]
        public IActionResult ViewCategory(int pagenumber = 1)
        {

            List<Category> categories = _db.tblcategory.ToList();
            const int pagesize = 5;
            if (pagenumber < 1)

                pagenumber = 1;
            int reccount = categories.Count();
            var pager = new PagenatedList(reccount, pagenumber, pagesize);
            var recskip = (pagenumber - 1) * pagesize;
            var data = categories.Skip(recskip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            return View(data);
        }
        [HttpGet]
        [Route("EditProduct")]
        public IActionResult EditProduct(int id)
        {


            var edit = _db.tblproduct.Where(x => x.product_id == id).FirstOrDefault();
            ViewBag.list = new SelectList(GetCategories(), "cat_id", "category_name");
            TempData["Image"] = edit.ImageFile;
            ViewBag.Image = TempData.Peek("Image");
            return View(edit);
        }

        [Route("EditProduct")]
        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductsModel prd, int id)
        {
            try
            {
                /* IFormFile file = Request.Form.Files[0];*/
                /*   if (ModelState.IsValid)
                   {
                */
                string file = null;

                if (prd.ImageFile != null)
                {
                    file = uploadfile(prd);
                }
                else if (prd.ImageFile == null)
                {
                    ViewBag.Image = TempData.Peek("Image");
                    file = ViewBag.Image;
                }

                if (prd.Product_name == null || prd.product_desc == null || prd.product_price == null || prd.product_qty == null)
                {
                    ViewBag.status = false;
                    ViewBag.alertmesaage = "Product Updated Failed";
                }
                else
                {
                    var product = new Product()
                    {
                        product_id = prd.product_id,
                        Product_name = prd.Product_name,
                        ImageFile = file,
                        product_desc = prd.product_desc,
                        product_price = prd.product_price.HasValue ? prd.product_price.Value : 0,
                        product_qty = prd.product_qty.HasValue ? prd.product_qty.Value : 0,
                        cat_id = prd.cat_id.HasValue ? prd.cat_id.Value : 0
                    };
                    _db.tblproduct.Update(product);
                    await _db.SaveChangesAsync();
                    ViewBag.status = true;
                    ViewBag.alertmesaage = "Product Updated Sucessfully";
                    return RedirectToAction("ViewProduct", "Admin", new { area = "Admin" });
                    /*return View(product);*/
                    /* }*/
                }
                ViewBag.list = new SelectList(GetCategories(), "cat_id", "category_name");
                ViewBag.Image = TempData.Peek("Image");
                return View();

            }
            catch (Exception)
            {
                return View("Error");
            }

        }



        [Route("DeleteProduct")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var del = await _db.tblproduct.Where(x => x.product_id == id).FirstOrDefaultAsync();
            /*   _db.tblproduct.Remove(del);
               await _db.SaveChangesAsync();*/
            del.product_id = del.product_id;
            del.prd_status = 0;
            _db.tblproduct.Update(del);
            _db.SaveChanges();
            return RedirectToAction("ViewProduct", "Admin");
        }
        [HttpGet]
        [Route("EditCategory")]
        public async Task<IActionResult> EditCategory(int? id)
        {
            if (id == null || id <= 0)
                return BadRequest();

            var CategoryInDB = await _db.tblcategory.FirstOrDefaultAsync(x => x.cat_id == id);

            if (CategoryInDB == null)
                return NotFound();
            /*ViewBag.value = CategoryInDB.category_name;*/
            return View(CategoryInDB);
        }

        [HttpPost]
        [Route("EditCategory")]

        public async Task<IActionResult> EditCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                _db.tblcategory.Update(category);
                await _db.SaveChangesAsync();
                ViewBag.status = true;
                ViewBag.alertmesaage = "Category Updated SuccesFully";
                return RedirectToAction(nameof(ViewCategory));
            }
            ViewBag.status = false;
            ViewBag.alertmesaage = "Category Updated UnSuccesFully";
            return View(category);

        }

        [Route("deletecategory")]
        public async Task<IActionResult> DeleteCategory(int? id)
        {
            if (id == null || id < 0)
                return BadRequest();

            var CategoryInDB = await _db.tblcategory.FirstOrDefaultAsync(x => x.cat_id == id);
            var del = _db.tblproduct.Where(x => x.cat_id == id).ToList();

            if (CategoryInDB == null)
                return NotFound();

            _db.tblcategory.Remove(CategoryInDB);
            foreach (var item in del)
            {
                _db.tblproduct.Remove(item);
            }
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(ViewCategory));

        }



        [Route("AddUser")]
        public IActionResult AddUser()
        {

            return View();
        }

        [Route("AddUser")]
        [HttpPost]
        public async Task<IActionResult> AddUser(singupmodel singup)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountrepostry.createuserAsync(singup);
                if (!result.Succeeded)
                {
                    foreach (var errorMessage in result.Errors)
                    {
                        ModelState.AddModelError("", errorMessage.Description);
                    }
                    ViewBag.status = false;
                    ViewBag.alertmesaage = "User Inserted Fail";
                    return View(singup);
                }
                ModelState.Clear();
                ViewBag.status = true;
                ViewBag.alertmesaage = "User Inserted Successfully";
                return RedirectToAction("ViewUser", "Admin", new { area = "Admin" });
            }
            ViewBag.status = false;
            ViewBag.alertmesaage = "Something Wrong";
            return View(singup);

        }
        [Route("ViewUser")]
        public IActionResult viewuser(int pagenumber = 1, string search = "")
        {
            var user = _accountrepostry.alluser();
            /* string ids = _usermanager.GetUserId(HttpContext.User);*/
            if (!(string.IsNullOrEmpty(search)))
            {
                var prd = _usermanager.Users.Where(x => x.fname.Contains(search)).ToList();
                if (prd.Count() > 0)
                {
                    const int pagesize = 9;
                    if (pagenumber < 1)

                        pagenumber = 1;
                    int reccount = prd.Count();
                    var pager = new PagenatedList(reccount, pagenumber, pagesize);
                    var recskip = (pagenumber - 1) * pagesize;
                    var data = prd.Skip(recskip).Take(pager.PageSize).ToList();
                    this.ViewBag.Pager = pager;
                    return View(data);
                    /*return View(prd);*/
                }
                else
                {
                    ViewBag.alert = "User Not Found Here";
                    int pagesize = 9;
                    if (pagenumber < 1)

                        pagenumber = 1;
                    int reccount = user.Count();
                    var pager = new PagenatedList(reccount, pagenumber, pagesize);
                    var recskip = (pagenumber - 1) * pagesize;
                    var data = user.Skip(recskip).Take(pager.PageSize).ToList();
                    this.ViewBag.Pager = pager;
                    return View(data);

                }
            }
            else
            {

                const int pagesize = 10;
                if (pagenumber < 1)

                    pagenumber = 1;
                int reccount = user.Count();
                var pager = new PagenatedList(reccount, pagenumber, pagesize);
                var recskip = (pagenumber - 1) * pagesize;
                var data = user.Skip(recskip).Take(pager.PageSize).ToList();
                this.ViewBag.Pager = pager;
                return View(data);
                /*  return View(_accountrepostry.alluser());*/
            }
            /* return View(user);*/
        }
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser(string id,string active,string delete)
        {
            var res = await _usermanager.FindByIdAsync(id);
            if (active != null)
            {

                res.EmailConfirmed = true;
                var ress=await _usermanager.UpdateAsync(res);
                if (ress.Succeeded)
                {
                    return RedirectToAction("ViewUser", "Admin", new { area = "Admin" });
                }
            }
            else if(delete!=null)
            {
                res.EmailConfirmed = false;
                var ress=await _usermanager.UpdateAsync(res);
                if (ress.Succeeded)
                {
                    return RedirectToAction("ViewUser", "Admin", new { area = "Admin" });
                }
            }
            return RedirectToAction("ViewUser", "Admin", new { area = "Admin" });
            
        }



        [Route("EditProfile")]
        public async Task<IActionResult> EditProfile(string id)
        {
            ApplicationUser user = await _usermanager.FindByIdAsync(id.Trim());
            if (user != null)
            {
                return View(user);
            }
            else
            {
                return RedirectToAction("Index");
            }

        }
        [Route("EditProfile")]
        [HttpPost]
        public async Task<IActionResult> EditProfile(string id, string UserName, string fname, string PhoneNumber)
        {
            ApplicationUser user = await _usermanager.FindByIdAsync(id.Trim());
            if (user != null)
            {
                if (!string.IsNullOrEmpty(UserName))
                {
                    user.Email = UserName;
                }
                else
                {
                    ModelState.AddModelError("", "Email cannot be empty");
                }
                if (!string.IsNullOrEmpty(PhoneNumber))
                {
                    user.PhoneNumber = PhoneNumber;
                }
                else
                {
                    ModelState.AddModelError("", "Phoneno cannot be empty");
                }
                if (!string.IsNullOrEmpty(fname))
                {
                    user.fname = fname;
                }
                else
                {
                    ModelState.AddModelError("", "Fullname cannot be empty");
                }
                if (!(string.IsNullOrEmpty(fname) && string.IsNullOrEmpty(PhoneNumber) && string.IsNullOrEmpty(UserName)))
                {
                    IdentityResult res = await _usermanager.UpdateAsync(user);
                    if (res.Succeeded)
                    {
                        await _signInManager.RefreshSignInAsync(user);
                        ViewBag.status = true;
                        ViewBag.alertmesaage = "Update Succesfully";
                        /*await _accountrepostry.logoutsync();*/
                        return RedirectToAction("login", "Account");
                    }
                    else
                    {
                        ViewBag.status = false;
                        ViewBag.alertmesaage = "Profile Update Fail";
                        return View(user);
                    }
                }
            }
            else
            {
                ViewBag.status = false;
                ViewBag.alertmesaage = "Profile Update Fail";
                ModelState.AddModelError("", "User Not Found");
            }
            ViewBag.status = false;
            ViewBag.alertmesaage = "Profile Update Fail";
            return View(user);
        }

        [Route("logout")]
        public IActionResult logout()
        {
            _accountrepostry.logoutsync();
            return RedirectToAction("Index", "Home");
        }
        [Route("viewpayment")]
        public IActionResult viewpayment(int pagenumber = 1)
        {
            var categories = _db.tblorder.Include(x => x.prd).Include(x => x.Address).Include(x => x.user).Where(x => x.status == "Deliver").ToList();
            const int pagesize = 5;
            if (pagenumber < 1)

                pagenumber = 1;
            int reccount = categories.Count();
            var pager = new PagenatedList(reccount, pagenumber, pagesize);
            var recskip = (pagenumber - 1) * pagesize;
            var data = categories.Skip(recskip).Take(pager.PageSize).ToList();
            this.ViewBag.Pager = pager;
            return View(data);
            /*   return View();*/
        }
        [Route("manageorder")]
        public IActionResult manageorder(int? ids, int pagenumber = 1, string search = "")
        {


            var categories = _db.tblorder.Include(x => x.prd).Include(x => x.Address).Include(x => x.user).OrderByDescending(x => x.orderid).ToList();
            if (!string.IsNullOrEmpty(search) || ids != null)
            {
                var prd = _db.tblorder.Include(x => x.prd).Include(x => x.Address).Include(x => x.user).Where(x => x.orderid == ids || x.status == search).ToList();
                if (prd.Count() > 0)
                {
                    const int pagesize = 9;
                    if (pagenumber < 1)

                        pagenumber = 1;
                    int reccount = prd.Count();
                    var pager = new PagenatedList(reccount, pagenumber, pagesize);
                    var recskip = (pagenumber - 1) * pagesize;
                    var data = prd.Skip(recskip).Take(pager.PageSize).ToList();
                    this.ViewBag.Pager = pager;
                    return View(data);
                    /*return View(prd);*/
                }
                else
                {
                    ViewBag.alert = "Order Detail Not Found Here";
                    int pagesize = 9;
                    if (pagenumber < 1)

                        pagenumber = 1;
                    int reccount = categories.Count();
                    var pager = new PagenatedList(reccount, pagenumber, pagesize);
                    var recskip = (pagenumber - 1) * pagesize;
                    var data = categories.Skip(recskip).Take(pager.PageSize).ToList();
                    this.ViewBag.Pager = pager;
                    return View(data);

                }

            }
            else
            {
                /*var categories = _db.tblorder.Include(x => x.prd).Include(x => x.Address).Include(x => x.user).OrderByDescending(x => x.orderid).ToList();
*/
                const int pagesize = 5;
                if (pagenumber < 1)

                    pagenumber = 1;
                int reccount = categories.Count();
                var pager = new PagenatedList(reccount, pagenumber, pagesize);
                var recskip = (pagenumber - 1) * pagesize;
                var data = categories.Skip(recskip).Take(pager.PageSize).ToList();
                this.ViewBag.Pager = pager;
                return View(data);
            }
  /*          return View(categories)*/;
        }
        [HttpPost]
        [Route("updatestatus")]
        public async Task<IActionResult> updatestatus(int id, TBLorder ords)
        {
            var det = await _db.tblorder.FindAsync(id);
            if (det != null)
            {
                det.status = ords.status;
                _db.tblorder.Update(det);
                await _db.SaveChangesAsync();
                return RedirectToAction("manageorder", "Admin", new { Areas = "Admin" });
            }
            else
            {
                return RedirectToAction("manageorder", "Admin", new { Areas = "Admin" });
            }

        }
        [HttpGet]
        [Route("changepassword")]
        public IActionResult changepassword()
        {
            return View();
        }
        [HttpPost]
        [Route("ChangePassword")]
        public async Task<IActionResult> changepassword(ChangePasswordModel change)
        {
            if (ModelState.IsValid)
            {
                string id=_usermanager.GetUserId(HttpContext.User);
                var user = await _usermanager.FindByIdAsync(id.Trim());
                var res = await _usermanager.ChangePasswordAsync(user, change.Currentpassword, change.Newpassword);
                if (res.Succeeded)
                {
                    ModelState.Clear();
                    ViewBag.status = true;
                    TempData["Password"] = "Password Update Succesfully";
                    TempData.Keep();
                    return View();
                }
                TempData["error"] = "Password Update Fail";
                TempData.Keep();
                foreach (var result in res.Errors)
                {
                    ModelState.AddModelError("", result.Description);
                }
            }

            return View(change);
        }
    }
}

