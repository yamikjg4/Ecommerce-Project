using E_Commerce.Models;
using E_Commerce.repositry;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepositry _prd;
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly Eshopcontext _db;
        private readonly IAccountrepostry _accountrepostry;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IHttpContextAccessor _https;
        public HomeController(IProductRepositry prd, UserManager<ApplicationUser> usermanager, Eshopcontext db, IAccountrepostry accountrepostry, IHttpContextAccessor http, SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;
            _usermanager = usermanager;
            _prd = prd;
            _db = db;
            _https = http;
            _accountrepostry = accountrepostry;
        }
        List<Cart> li = new List<Cart>();

        public IActionResult Index(int pagenumber = 1, string search = "")
        {
            TempData["search"] = search;
            TempData.Keep(search);
            /*      pagenumber = 1;*/
            TempData["getid"] = _usermanager.GetUserId(HttpContext.User);
            if (User.IsInRole("Admin"))
            {
                return Redirect("~/Admin/Admin/Index/");
            }
            /* else if (User.IsInRole("Customer")) { return Redirect("~/Home/Index/"); }*/
            else
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
                /*  var prds = product.ToList();*/
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
            }
        }
        public async Task<IActionResult> viewproduct(int id, int catid)
        {
            var res = await _prd.getdetail(id);

            ViewBag.prd = await _db.tblproduct.Where(x => x.product_id != id && x.cat_id == catid && x.prd_status == 1).ToListAsync();
            var cat = await _db.tblcategory.Where(x => x.cat_id == catid).FirstOrDefaultAsync();
            ViewBag.catid = cat.category_name;

            return View(res);
        }
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> EditProfile(string id)
        {
            TempData["getid"] = _usermanager.GetUserId(HttpContext.User);
            if (string.IsNullOrEmpty(id))
            {

                var ids = TempData["getid"];
                id = Convert.ToString(ids);

            }
            return View(await _usermanager.FindByIdAsync(id.Trim()));

        }
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
                        TempData["AlertMessage"] = "Update Succesfully";
                        TempData.Keep();
                        /*    await _accountrepostry.logoutsync();*/
                        /*ModelState.Clear();*/
                        return View();
                    }
                    else
                    {
                        ViewBag.status = false;
                        TempData["AlertMessages"] = "Profile Update Fail";
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
        public IActionResult changepassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> changepassword(ChangePasswordModel change, string id)
        {
            if (ModelState.IsValid)
            {

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
        public IActionResult ManageAddress()
        {
            var user = _usermanager.GetUserId(HttpContext.User);
            return View(_db.tblAddresses.Where(x => x.Id == user && x.Status == 1).ToList());
        }
        public IActionResult Create(string returnUrl)
        {
            if (!string.IsNullOrEmpty(returnUrl))
            {
                TempData["returnUrl"] = returnUrl;
            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(tblAddress model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = _usermanager.GetUserId(HttpContext.User);
                tblAddress ads = new tblAddress()
                {
                    Address = model.Address,
                    name = model.name,
                    Phone = model.Phone,
                    pincode = model.pincode,
                    City = model.City,
                    State = model.State,
                    Id = user
                };
                await _db.tblAddresses.AddAsync(ads);
                await _db.SaveChangesAsync();
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return LocalRedirect(returnUrl);
                }
                return RedirectToAction("ManageAddress", "Home");
            }
            return View(model);
        }
        public IActionResult EditAddress(int id)
        {
            return View(_db.tblAddresses.Where(x => x.ad_id == id).FirstOrDefault());
        }
        [HttpPost]
        public async Task<IActionResult> EditAddress(tblAddress model)
        {
            if (ModelState.IsValid)
            {
                var user = _usermanager.GetUserId(HttpContext.User);
                /*  var user = _usermanager.GetUserId(HttpContext.User);*/
                tblAddress ads = new tblAddress()
                {
                    ad_id = model.ad_id,
                    Address = model.Address,
                    name = model.name,
                    Phone = model.Phone,
                    pincode = model.pincode,
                    City = model.City,
                    State = model.State,
                    Id = user
                };
                _db.tblAddresses.Update(ads);
                await _db.SaveChangesAsync();
                ViewBag.status = true;
                TempData["alert"] = "Update sucessfully";
                TempData.Keep();

                return RedirectToAction("ManageAddress", "Home");
            }
            ViewBag.status = false;
            TempData["alertread"] = "Something Wrong!!";
            TempData.Keep();
            return View(model);
        }
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var check = await _db.tblAddresses.Where(x => x.ad_id == id).FirstOrDefaultAsync();
            /*  _db.tblAddresses.Remove(await _db.tblAddresses.FindAsync(id));*/
            check.ad_id = check.ad_id;
            check.Status = 0;
            _db.tblAddresses.Update(check);
            var res = await _db.SaveChangesAsync();
            if (Convert.ToBoolean(res))
            {
                TempData["alert"] = "Delete Address Successfully";
                TempData.Keep();
                return RedirectToAction("ManageAddress");
            }
            else
            {
                TempData["read"] = "Delete Address UnSuccessfully";
                return View();
            }
        }

        public IActionResult cart(string cart)
        {

            Cart cartmodel = new Cart();


            var cartproduct = Request.Cookies["ProductId"];

            if (!(string.IsNullOrEmpty(cartproduct)))
            {
                var prdids = cartproduct;
                var id = prdids.Split(',');

                cartmodel.cartprouctid = id.Select(x => int.Parse(x)).ToList();

                cartmodel.cartproduct = GetProducts(cartmodel.cartprouctid);
            }
            else
            {
                return View();
            }

            return View(cartmodel);
        }

        public JsonResult getoutput(string Prefix)
        {

            /*    search = Convert.ToString(TempData["search"]);*/
            List<Category> categories = _db.tblcategory.ToList();
            List<Product> products = _db.tblproduct.ToList();

            var product = (from i in _db.tblproduct
                           where i.Product_name.Contains(Prefix)
                           select new
                           {
                               label = i.Product_name,
                               val = i.product_id,
                           }
                           );
            var json = JsonConvert.SerializeObject(product);

            return Json(product);
        }
        public List<Product> GetProducts(List<int> id)
        {
            return _db.tblproduct.Where(x => id.Contains(x.product_id)).ToList();
        }
        [HttpGet]
        [Authorize(Roles = "Customer")]
        public IActionResult checkout()
        {
            Cart cartmodel = new Cart();


            var cartproduct = Request.Cookies["ProductId"];

            if (!(string.IsNullOrEmpty(cartproduct)))
            {
                var prdids = cartproduct;
                var id = prdids.Split(',');

                cartmodel.cartprouctid = id.Select(x => int.Parse(x)).ToList();

                cartmodel.cartproduct = GetProducts(cartmodel.cartprouctid);
                ViewBag.Sum = cartmodel.cartproduct.Sum(x => x.product_price * cartmodel.cartprouctid.Where(productid => productid == x.product_id).Count());
                var userid = _usermanager.GetUserId(HttpContext.User);
                /*  TempData["getid"] = _usermanager.GetUserId(HttpContext.User);*/

                ViewBag.Address = _db.tblAddresses.Where(x => x.Id == Convert.ToString(userid) && x.Status == 1).ToList();


                ViewBag.count = _db.tblAddresses.Where(x => x.Id == Convert.ToString(userid) && x.Status == 1).Count();


            }


            if (ViewBag.Sum > 0)
            {
                return View();
            }
            return RedirectToAction("Index", "Home");


            /* return View();
 */
        }
        [HttpPost]
        public async Task<IActionResult> checkout(TBLorder ords, int ad_id)
        {



            Cart cartmodel = new Cart();


            var cartproduct = Request.Cookies["ProductId"];

            if (!(string.IsNullOrEmpty(cartproduct)))
            {
                var prdids = cartproduct;
                var id = prdids.Split(',');

                cartmodel.cartprouctid = id.Select(x => int.Parse(x)).ToList();

                cartmodel.cartproduct = GetProducts(cartmodel.cartprouctid);
                ViewBag.Sum = cartmodel.cartproduct.Sum(x => x.product_price * cartmodel.cartprouctid.Where(productid => productid == x.product_id).Count());
                var userid = _usermanager.GetUserId(HttpContext.User);
                ViewBag.Address = _db.tblAddresses.Where(x => x.Id == Convert.ToString(userid) && x.Status == 1).ToList();



                ViewBag.count = _db.tblAddresses.Where(x => x.Id == Convert.ToString(userid) && x.Status == 1).Count();
                /*  TempData["getid"] = _usermanager.GetUserId(HttpContext.User);*/
                if (ModelState.IsValid)
                {
                    foreach (var item in cartmodel.cartproduct)
                    {
                        TBLorder ord = new TBLorder();
                        var prd = _db.tblproduct.Where(x => x.product_id == item.product_id).FirstOrDefault();
                        var productqty = cartmodel.cartprouctid.Where(productid => productid == item.product_id).Count();
                        ord.qtys = productqty;
                        if (prd.product_qty > productqty)
                        {
                            ord.ad_id = ad_id;
                            /*    ord.Address = ad_id;*/
                            ord.Id = userid;
                            ord.product_id = item.product_id;
                            ord.status = "Pending";
                            ord.date = DateTime.Now;

                            ord.totalpay = Convert.ToInt32(productqty * item.product_price);
                            ord.payment = "COD";

                            /* prd.product_id = item.product_id;*/
                            prd.ImageFile = item.ImageFile;
                            prd.Product_name = item.Product_name;
                            prd.product_price = item.product_price;
                            prd.product_desc = item.product_desc;
                            var qty = item.product_qty - productqty;
                            prd.product_qty = qty;

                            await _db.tblorder.AddAsync(ord);
                            _db.tblproduct.Update(prd);
                        }

                        TempData["toastmessage"] = "Out Of Stock";
                        TempData.Keep();
                        /*  return View();*/


                    }

                    var save = await _db.SaveChangesAsync();
                    if (Convert.ToBoolean(save))
                    {
                        ViewBag.Message = "Success";
                        /*  return RedirectToAction("sucessorder");*/
                        TempData["message"] = "Order Placed Successfully";
                        TempData.Keep();
                        return View();
                    }


                }
            }

            return View();
        }
        public IActionResult aboutus()
        {
            return View();
        }
        public IActionResult contactus()
        {
            return View();
        }
        [HttpPost]
        public IActionResult contactus(string uname)
        {
            if (ModelState.IsValid)
            {
                var user = uname;
                ViewBag.Message = "Hello " + user + " ,we will get back to you soon. thankyou";
            }
            return View();
        }
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> buynow(int id)
        {
            TempData["id"] = id;
            TempData.Keep();
            var userid = _usermanager.GetUserId(HttpContext.User);

            ViewBag.Address = _db.tblAddresses.Where(x => x.Id == Convert.ToString(userid) && x.Status == 1).ToList();
            ViewBag.count = _db.tblAddresses.Where(x => x.Id == Convert.ToString(userid) && x.Status == 1).Count();

            var product = await _db.tblproduct.FindAsync(id);
            ViewBag.price = product.product_price;

            if (ViewBag.count > 0)
            {
                return View();
            }
            else if (ViewBag.Address != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        [HttpPost]
        [ActionName("buynow")]
        public async Task<IActionResult> buy(TBLorder ords, int ad_id)
        {
            var id = TempData["id"];
            TempData.Keep();
            var userid = _usermanager.GetUserId(HttpContext.User);
            if (ModelState.IsValid)
            {
                var product = await _db.tblproduct.FindAsync(id);

                TBLorder ord = new TBLorder();
                ord.ad_id = ad_id;
                /*    ord.Address = ad_id;*/
                ord.Id = userid;
                ord.product_id = product.product_id;
                ord.status = "Pending";
                ord.date = DateTime.Now;
                ord.qtys = 1;
                ord.totalpay = Convert.ToInt32(product.product_price);
                ord.payment = "COD";

                product.ImageFile = product.ImageFile;
                product.Product_name = product.Product_name;
                product.product_price = product.product_price;
                product.product_desc = product.product_desc;
                var qty = product.product_qty - ord.qtys;
                product.product_qty = qty;

                await _db.tblorder.AddAsync(ord);
                _db.tblproduct.Update(product);
                var check = await _db.SaveChangesAsync();
                if (Convert.ToBoolean(check))
                {
                    TempData["message"] = "Order Placed Successfully";
                    TempData.Keep();
                    return RedirectToAction("orders");

                }
            }
            /*  var userid = _usermanager.GetUserId(HttpContext.User);*/
            var products = await _db.tblproduct.FindAsync(id);
            ViewBag.Address = _db.tblAddresses.Where(x => x.Id == Convert.ToString(userid) && x.Status == 1).ToList();
            ViewBag.count = _db.tblAddresses.Where(x => x.Id == Convert.ToString(userid) && x.Status == 1).Count();

            /* var product = await _db.tblproduct.FindAsync(TempData["id"]);*/
            ViewBag.price = products.product_price;
            return View();
            /*return RedirectToAction("cart");*/
        }
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> orders(string id, int pagenumber = 1, string search = "")
        {
            if (id == null)
            {
                id = _usermanager.GetUserId(HttpContext.User);
            }
            var product = await _db.tblorder.Include(x => x.prd).Where(x => x.Id == id.Trim()).OrderByDescending(x => x.orderid).ToListAsync();
            /*  var prds = product.ToList();*/
            if (!(string.IsNullOrEmpty(search)))
            {
                /* var categorys = _db.tblcategory.ToList();*/

                var prd = await _db.tblorder.Include(x => x.prd).Where(x => x.prd.Product_name.Contains(search)).ToListAsync();


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
                    ViewBag.alert = "Order Not Found Here";
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

        }
        public JsonResult getoutputs(string Prefix)
        {

            /*    search = Convert.ToString(TempData["search"]);*/


            var product = _db.tblorder.Include(x => x.prd).Where(x => x.prd.Product_name.Contains(Prefix)).Select(x => x.prd.Product_name);
            var json = JsonConvert.SerializeObject(product);

            return Json(product);
        }
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> getdetails(int id)
        {

            return View(await _db.tblorder.Include(x => x.Address).Include(x => x.prd).Where(x => x.orderid == id).FirstOrDefaultAsync());
        }
        public async Task<IActionResult> cancelorder(int id)
        {
            var db = await _db.tblorder.Where(x => x.orderid == id).FirstOrDefaultAsync();
            var dbs = await _db.tblproduct.Where(x => x.product_id == db.product_id).FirstOrDefaultAsync();
            db.ad_id = db.ad_id;
            db.product_id = db.product_id;
            db.qtys = db.qtys;
            db.Id = db.Id;
            db.totalpay = db.totalpay;
            db.status = "Cancel";
            db.payment = db.payment;
            db.date = db.date;
            /*Update Product*/
            dbs.cat_id = dbs.cat_id;
            dbs.Product_name = dbs.Product_name;
            dbs.product_price = dbs.product_price;
            dbs.product_desc = dbs.product_desc;
            var qty = dbs.product_qty + db.qtys;
            dbs.product_qty = qty;
            dbs.ImageFile = dbs.ImageFile;

            _db.tblorder.Update(db);
            _db.tblproduct.Update(dbs);
            await _db.SaveChangesAsync();
            TempData["message"] = "Cancel Ordeer Successfully";
            TempData.Keep();
            return RedirectToAction("orders");
        }
    }

}
