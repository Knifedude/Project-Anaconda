using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnacondaMVC.Games;
using System.Web.WebPages;
using AnacondaGames.Games.WheelOfFortune;
using AnacondaMVC.Games.WheelOfFortune;
using AnacondaMVC.Models;
using System.Security.Claims;
using Microsoft.AspNet.Identity;

namespace AnacondaMVC.Controllers
{
    [Authorize]
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
            return View(new GameResult() { Bet = 100 });
        }

        // POST: Wheel of Fortune
        [HttpPost]
        public ActionResult WheelOfFortune(FormCollection collection)
        {
            int bet = collection["Bet"].AsInt();
            GameResult result = new GameResult();

            var items = new List<RandomItem<ISpinAction>>
            {
                new RandomItem<ISpinAction>(0.6, new BetMultiplier(0.5m)),
                new RandomItem<ISpinAction>(0.2, new BetMultiplier(1.1m)),
                new RandomItem<ISpinAction>(0.1, new BetMultiplier(0m)),
                new RandomItem<ISpinAction>(0.1 / 3, new BetMultiplier(1.5m)),
                new RandomItem<ISpinAction>(0.1 / 3, new BetMultiplier(2m)),
                new RandomItem<ISpinAction>(0.1 / 3, new BetMultiplier(3m))
            };
            Random rand = new Random();
            var rp = new RandomPicker<ISpinAction>(items, rand.Next());

            //TODO: rand.next gives the same order for each player, would be better to have a different order for each player

            //TODO: communicate bet amount and fill in the bet field with it so the player can bet the same amount over and over easily
            
            //TODO: check if bet is not greater than total credits
                result = rp.Pick().Item.Execute(new GameContext(bet));

            var user = HttpContext.User.Identity as ClaimsIdentity;
            var userId = user.GetUserId();

            using (var anacondaModel = new AnacondaModel())
            {
                var wallet = anacondaModel.Wallets.First(u => u.UserId == userId);
                wallet.Credits += result.CreditsGained;
                anacondaModel.SaveChanges();
            }

            ViewBag.Bet = bet;

            return View(result);
        }
    }
}