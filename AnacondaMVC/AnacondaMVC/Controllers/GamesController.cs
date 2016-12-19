using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnacondaMVC.Games;

namespace AnacondaMVC.Controllers
{
    public class GamesController : Controller
    {
        // GET: Games
        public ActionResult Index()
        {
            return View();
        }

        // GET: Wheel of Fortune
        public ActionResult WheelOfFortune()
        {
            return View();
        }

        // POST: Wheel of Fortune
        [HttpPost]
        public ActionResult WheelOfFortune(GameResult result)
        {
            return View();
        }
    }
}