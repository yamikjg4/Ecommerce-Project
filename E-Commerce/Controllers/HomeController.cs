using E_Commerce.Models;
using E_Commerce.repositry;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        public HomeController(IProductRepositry prd, UserManager<ApplicationUser> usermanager, Eshopcontext db, IAccountrepostry accountrepostry)
        {
            _usermanager = usermanager;
            _prd = prd;
            _db = db;
            _accountrepostry = accountrepostry;
        }


        public IActionResult Index(int pagenumber = 1, string search = "")
        {
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
                              from e2 in tabel1.Where(x => e1.category_name == search || x.Product_name.Contains(search))
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

            ViewBag.prd = _db.tblproduct.Where(x => x.product_id != id && x.cat_id == catid).ToList();
            var cat = _db.tblcategory.Where(x => x.cat_id == catid).FirstOrDefault();
            ViewBag.catid = cat.category_name;

            return View(res);
        }
        public async Task<IActionResult> EditProfile(string id)
        {

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
                        ViewBag.status = true;
                        ViewBag.alertmesaage = "Update Succesfully";
                        await _accountrepostry.logoutsync();
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
        public IActionResult changepassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> changepassword(ChangePasswordModel change,string id)
        {
            if (ModelState.IsValid)
            {

                var user = await _usermanager.FindByIdAsync(id.Trim());
               var res= await _usermanager.ChangePasswordAsync(user,change.Currentpassword,change.Newpassword);
                if (res.Succeeded)
                {
                    ModelState.Clear();
                    ViewBag.status = true;
                    ViewBag.alertmesaage = "Password Update Succesfully";
                    return View();
                }
                ViewBag.status = false;
                ViewBag.alertmesaage = "Password Update Fail";
                foreach (var result in res.Errors)
                {
                    ModelState.AddModelError("", result.Description);
                }
            }
            ViewBag.status = false;
            ViewBag.alertmesaage = "Password Update Fail";
            return View(change);
        }
        public IActionResult ManageAddress()
        {
           var user= _usermanager.GetUserId(HttpContext.User);
            return View(_db.tblAddresses.Where(x=>x.Id==user).ToList());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(tblAddress model)
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
                return RedirectToAction("ManageAddress", "Home");
            }
            return View(model);
        }
        public IActionResult EditAddress(int id)
        {
            return View(_db.tblAddresses.Where(x=>x.ad_id==id).FirstOrDefault());
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
                    Id=user
                };

                _db.tblAddresses.Update(ads);
                await _db.SaveChangesAsync();
                ViewBag.status = true;
                ViewBag.alertmesaage = "Update sucessfully";
                return RedirectToAction("ManageAddress", "Home");
            }
            ViewBag.status = false;
            ViewBag.alertmesaage = "Something Wrong!!";
            return View(model);
        }
        public async Task<IActionResult> DeleteAddress(int id)
        {
            _db.tblAddresses.Remove(await _db.tblAddresses.FindAsync(id));
            await _db.SaveChangesAsync();
            return RedirectToAction("ManageAddress");
        }
    }
   
}
