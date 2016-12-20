using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnacondaMVC.Controllers
{
    public class StaticContentController : Controller
    {
        // GET: Index
        public ActionResult Index()
        {
            return View();
        }

        // GET: PageNotFound
        public ActionResult PageNotFound()
        {
            return View();
        }
    }
}