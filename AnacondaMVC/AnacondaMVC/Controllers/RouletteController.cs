using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using AnacondaGames.Games.Roulette;
using AnacondaMVC.Games;
using AnacondaMVC.Logic;
using AnacondaMVC.Models;
using Microsoft.AspNet.Identity;

namespace AnacondaMVC.Controllers
{
    [Authorize]
    public class RouletteController : Controller
    {
        // GET: Roulette
        public ActionResult Index()
        {
            var roulette = GetRoulette();

            ViewBag.BetTypes = roulette.GetBetTypes();

            var bets = (Dictionary<string, Bet>) Session["bets"] ?? (new Dictionary<string, Bet>());

            ViewBag.ActiveBets = bets.Values.AsEnumerable().ToList();

            if (Session["roulette-result"] != null)
            {
                var rrm = (Roulette.RouletteResultModel)Session["roulette-result"];
                return View(rrm);
            }

            return View();
        }


        public ActionResult Remove(Bet betType)
        {
            if (Session["bets"] != null)
            {
                var bets = (Dictionary<string, Bet>) Session["bets"];
                bets.Remove(betType.Type);
                Session["bets"] = bets;
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public ActionResult PlaceBet(Bet bet)
        {

            if (bet.Credits > 0)
            {
                var bets = Session["bets"] != null ? (Dictionary<string, Bet>)Session["bets"] : new Dictionary<string, Bet>();
                var type = bet.Type == "Number" ? bet.Type + bet.Number : bet.Type;

                if (bets.ContainsKey(type))
                {
                    bets[type].Credits += bet.Credits;
                }
                else
                {
                    bets[type] = bet;
                }

                Session["bets"] = bets;
                
            }
            return RedirectToAction("Index");
        }




        [HttpPost]
        public ActionResult SubmitBets()
        {
            if (Session["bets"] != null)
            {
                var bets = (Dictionary<string,Bet>)Session["bets"];
                var totalBet = bets.Values.Sum(x => x.Credits);
                var user = HttpContext.User.Identity as ClaimsIdentity;
                Roulette.RouletteResultModel rrm;
                Session["bets"] = null;

                using (var anacondaModel = new AnacondaModel())
                {
                    var walletDao = new WalletDAO(anacondaModel);
                    if (walletDao.Pay(user.GetUserId(), totalBet))
                    {
                        Roulette roulette = GetRoulette();

                        try
                        {
                            rrm = roulette.Spin(bets.Values);
                            rrm.Status = ResultStatus.Succes;
                        }
                        catch (InvalidBetException ex)
                        {
                            rrm = new Roulette.RouletteResultModel();
                            rrm.Status = ResultStatus.Failed;
                        }

                        var userId = user.GetUserId();
                        var wallet = walletDao.GetWallet(userId);
                        wallet.Credits += rrm.WinningBets.Sum(u => u.Payout);
                        var userStats = anacondaModel.UserStatistics.First(s => s.Id == userId);
                        userStats.Experience += 100;
                        var gameStats = anacondaModel.GameStatistics;
                        gameStats.Add(new GameStatistic()
                        {
                            GameId = 2,
                            UserId = userId,
                            CreditResult = rrm.WinningBets.Sum(u => u.Payout) - bets.Values.Select(b => b.Credits).Sum()
                        });
                        anacondaModel.SaveChanges();
                    }
                    else
                    {
                        rrm = new Roulette.RouletteResultModel { Status = ResultStatus.InsufficientCredits };
                    }

                }

                if (rrm.WinningBets == null)
                {
                    rrm.WinningBets = new List<WinningBet>();
                }


                Session["roulette-result"] = rrm;
            }


            return RedirectToAction("Index");
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