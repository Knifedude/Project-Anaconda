using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnacondaGames.Games.Roulette;

namespace AnacondaMVC.Controllers
{
    [Authorize]
    public class RouletteController : Controller
    {
        // GET: Roulette
        public ActionResult Index()
        {
            Roulette roulette = GetRoulette();
            return View(roulette);
        }

        [HttpPost]
        public void PlaceBets(List<Bet> collection)
        {

            Roulette roulette = GetRoulette();
            

        }

        private Roulette GetRoulette()
        {
            Roulette roulette;

            if (HttpContext.Session["roulette"] == null)
            {
                Random random = new Random();
                roulette = new Roulette(random);
                HttpContext.Session["roulette"] = roulette;
            }
            else
            {
                roulette = (Roulette)HttpContext.Session["roulette"];
            }
            return roulette;

        }




    }
}