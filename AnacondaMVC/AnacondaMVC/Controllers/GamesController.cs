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
using AnacondaMVC.Logic;
using AnacondaGames.Games.SlotMachine;

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
            return ViewWithMaxBet();
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

            var user = HttpContext.User.Identity as ClaimsIdentity;
            var userId = user.GetUserId();

            using (var anacondaModel = new AnacondaModel())
            {
                WalletDAO walletDAO = new WalletDAO(anacondaModel);
                if (walletDAO.Pay(userId, bet))
                {
                    result = rp.Pick().Item.Execute(new GameContext(bet));
                }
                else
                {
                    result = new GameResult() { Bet = bet, CreditsGained = 0, Status = ResultStatus.InsufficientCredits };
                }
            }

            using (var anacondaModel = new AnacondaModel())
            {
                var wallet = anacondaModel.Wallets.First(u => u.UserId == userId);
                wallet.Credits += result.CreditsGained;
                anacondaModel.SaveChanges();
            }

            ViewBag.Bet = bet;

            return View(result);
        }

        // GET: Slotmachine - Simple
        public ActionResult SlotMachineSimple()
        {
            return ViewWithMaxBet();
        }

        [HttpPost]
        public ActionResult SlotMachineSimple(FormCollection collection)
        {
            int bet = collection["Bet"].AsInt();
            GameResult result = new GameResult();

            List<SlotItem>[] slotColumns = new List<SlotItem>[]
            {
                new List<SlotItem>(),
                new List<SlotItem>(),
                new List<SlotItem>()
            };

            RandomColumns rc = new RandomColumns();
            Random rand = new Random();

            for (int i = 0; i < 3; i++)
            {
                slotColumns[i] = rc.GetRandomColumnFruit(rand);
            }

            ViewBag.Column1Row1 = slotColumns[0][0].DisplayName;
            ViewBag.Column1Row2 = slotColumns[0][1].DisplayName;
            ViewBag.Column1Row3 = slotColumns[0][2].DisplayName;
            ViewBag.Column2Row1 = slotColumns[1][0].DisplayName;
            ViewBag.Column2Row2 = slotColumns[1][1].DisplayName;
            ViewBag.Column2Row3 = slotColumns[1][2].DisplayName;
            ViewBag.Column3Row1 = slotColumns[2][0].DisplayName;
            ViewBag.Column3Row2 = slotColumns[2][1].DisplayName;
            ViewBag.Column3Row3 = slotColumns[2][2].DisplayName;

            var user = HttpContext.User.Identity as ClaimsIdentity;
            var userId = user.GetUserId();

            CheckRows cr = new CheckRows();

            using (var anacondaModel = new AnacondaModel())
            {
                WalletDAO walletDAO = new WalletDAO(anacondaModel);
                if (walletDAO.Pay(userId, bet))
                {
                    result = cr.WinResult(slotColumns, new GameContext(bet));
                }
                else
                {
                    result = new GameResult() { Bet = bet, CreditsGained = 0, Status = ResultStatus.InsufficientCredits };
                }
            }

            using (var anacondaModel = new AnacondaModel())
            {
                var wallet = anacondaModel.Wallets.First(u => u.UserId == userId);
                wallet.Credits += result.CreditsGained;
                anacondaModel.SaveChanges();
            }

            ViewBag.Bet = bet;

            return View(result);
        }

        private ActionResult ViewWithMaxBet()
        {
            var user = HttpContext.User.Identity as ClaimsIdentity;
            var userId = user.GetUserId();

            using (var anacondaModel = new AnacondaModel())
            {
                WalletDAO walletDAO = new WalletDAO(anacondaModel);
                if (walletDAO.HasWallet(userId))
                {
                    Wallet wallet = walletDAO.GetWallet(userId);
                    ViewBag.MaxBet = wallet.Credits + wallet.CasinoCredits;
                }
                else
                {
                    ViewBag.MaxBet = int.MaxValue;
                }
            }

            return View(new GameResult() { Bet = 100 });
        } 
    }
}