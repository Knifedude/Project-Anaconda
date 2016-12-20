using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnacondaMVC.Games;
using System.Web.WebPages;
using AnacondaGames.Games.WheelOfFortune;
using AnacondaMVC.Games.WheelOfFortune;

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
        public ActionResult WheelOfFortune(FormCollection collection)
        {
            int bet = collection["Bet"].AsInt();
            GameResult result = new GameResult();

            var items = new List<RandomItem<ISpinAction>>
            {
                new RandomItem<ISpinAction>(0.6, new CreditIncrease(500)),
                new RandomItem<ISpinAction>(0.1 / 3, 999),
                new RandomItem<ISpinAction>(0.1 / 3, 999),
                new RandomItem<ISpinAction>(0.1 / 3, 999),
                new RandomItem<ISpinAction>(0.3, 999)
            };
            var rp = new RandomPicker<ISpinAction>(items, 1337);

            result = rp.Pick().Item.Execute(new GameContext() { Bet = bet });

            return View(result);
        }
    }
}