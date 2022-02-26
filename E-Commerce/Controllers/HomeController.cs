using E_Commerce.Models;
using E_Commerce.repositry;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace E_Commerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductRepositry _prd;
        private readonly UserManager<ApplicationUser> _usermanager;
        private readonly Eshopcontext _db;
        public HomeController(IProductRepositry prd, UserManager<ApplicationUser> usermanager,Eshopcontext db)
        {
            _usermanager = usermanager;
            _prd = prd;
            _db = db;
        }

        
        public IActionResult Index(int pagenumber=1,string search="")
        {
      /*      pagenumber = 1;*/
            if (User.IsInRole("Admin"))
            {
                return Redirect("~/Admin/Admin/Index/");
            }
           /* else if (User.IsInRole("Customer")) { return Redirect("~/Home/Index/"); }*/
            else
            {
                /*     ViewBag.getid = _usermanager.GetUserId(HttpContext.User);*/
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
                        var pager = new PagenatedList(reccount, pagenumber,pagesize);
                        var recskip = (pagenumber -1) * pagesize;
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
        public async Task<IActionResult> viewproduct(int id)
        {
            var res = await _prd.getdetail(id);
            
            
            return View(res);
        }
        

    }
}
