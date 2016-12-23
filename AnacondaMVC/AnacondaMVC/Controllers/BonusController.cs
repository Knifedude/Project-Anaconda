using System;
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
    public class BonusController : Controller
    {
        //        // GET: Bonus
        public ActionResult Index()
        {
            var user = HttpContext.User.Identity as ClaimsIdentity;
            var userId = user.GetUserId();
            var bonus = new BonusViewModel();
            using (var am = new AnacondaModel())
            {

                var daily = am.UserDailies.Any() ? am.UserDailies.First(u => u.Id == userId) : new UserDaily();
                if (daily == null)
                {
                    bonus.Daily = true;
                    bonus.Hourly = true;
                }
                else
                {
                    bonus.Daily = daily.LastDaily == null || ((DateTime.Now - daily.LastDaily).Value.Hours >= 24);
                    bonus.Hourly = daily.LastHourly == null || ((DateTime.Now - daily.LastDaily).Value.Minutes >= 60);

                    if (daily.LastDaily != null) bonus.LastDaily = daily.LastDaily.Value;
                    if (daily.LastHourly != null) bonus.LastHourly = daily.LastHourly.Value;
                }
            }

            if (Session["last-user-daily-credits"] != null)
            {
                bonus.DailyCredits = (int) Session["last-user-daily-credits"];
//                Session.Remove("last-user-daily-credits");
            }
            else
            {
                bonus.DailyCredits = 0;
            }

            if (Session["last-user-hourly-credits"] != null)
            {
                bonus.HourlyCredits = (int) Session["last-user-hourly-credits"];
//                Session.Remove("last-user-hourly-credits");
            }
            else
            {
                bonus.HourlyCredits = 0;
            }

            return View(bonus);
        }

        [ChildActionOnly]
        public ActionResult PartialBonus()
        {
            var user = HttpContext.User.Identity as ClaimsIdentity;
            var userId = user.GetUserId();


            var bonus = new BonusViewModel();

            using (var am = new AnacondaModel())
            {
                UserDaily daily = null;
                if (am.UserDailies.Any())
                {
                    daily = am.UserDailies.First(u => u.Id == userId);
                }

                if (daily == null)
                {
                    var aspUser = am.AspNetUsers.First(u => u.Id == userId);
                    daily = new UserDaily() {AspNetUser = aspUser, Id = aspUser.Id};
                    bonus.Daily = true;
                    bonus.Hourly = true;

                    am.SaveChanges();
                }
                else
                {
                    bonus.Daily = daily.LastDaily == null || ((DateTime.Now - daily.LastDaily).Value.Hours >= 24);
                    bonus.Hourly = daily.LastHourly == null || ((DateTime.Now - daily.LastDaily).Value.Minutes >= 60);
                }

            }

            return PartialView(bonus);
        }

        public class BonusViewModel
        {
            
            public bool Daily { get; set; }

            public bool Hourly { get; set; }

            public int HourlyCredits { get; set; }

            public int DailyCredits { get; set; }

            public DateTime LastDaily { get; set; }

            public DateTime LastHourly { get; set; }

        }

        public ActionResult DailyFortune()
        {
            var wheelOfFortune = AnacondaGames.Games.WheelOfFortune.WheelOfFortune.CreateDaily("Wheel of Fortune", new Random());
            var user = HttpContext.User.Identity as ClaimsIdentity;
            var userId = user.GetUserId();
            GameResult result;
            using (var anacondaModel = new AnacondaModel())
            {
                var walletDAO = new WalletDAO(anacondaModel);
                var wallet = walletDAO.GetWallet(userId);

                result = wheelOfFortune.Play();

                var daily = anacondaModel.UserDailies.Any() ? anacondaModel.UserDailies.First(u => u.Id == userId) : null;
                if (daily == null)
                {
                    var aspUser = anacondaModel.AspNetUsers.First(u => u.Id == userId);
                    daily = new UserDaily()
                    {
                        Id = aspUser.Id,
                        AspNetUser = aspUser,
                        LastDaily = DateTime.Now
                    };
                    anacondaModel.UserDailies.Add(daily);
                }
                else
                {

                    if (daily.LastDaily != null)
                    {

                        var timespan = DateTime.Now - daily.LastDaily.Value;
                        if (timespan.Hours < 24)
                        {
                            return RedirectToAction("Index");
                        }

                    }

                    daily.LastDaily = DateTime.Now;
                }
                
                wallet.Credits += result.CreditsGained;
                var userStats = anacondaModel.UserStatistics.First(s => s.Id == userId);
                userStats.Experience += 100;

                Session["last-user-daily-date"] = DateTime.Now;
                Session["last-user-daily-credits"] = result.CreditsGained;


                anacondaModel.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public ActionResult HourlyFortune()
        {
            var wheelOfFortune = AnacondaGames.Games.WheelOfFortune.WheelOfFortune.CreateHourly("Wheel of Fortune", new Random());
            var user = HttpContext.User.Identity as ClaimsIdentity;
            var userId = user.GetUserId();
            GameResult result;
            using (var anacondaModel = new AnacondaModel())
            {
                var walletDAO = new WalletDAO(anacondaModel);
                var wallet = walletDAO.GetWallet(userId);

                result = wheelOfFortune.Play();

                var daily = anacondaModel.UserDailies.Any() ? anacondaModel.UserDailies.First(u => u.Id == userId) : null;
                if (daily == null)
                {
                    var aspUser = anacondaModel.AspNetUsers.First(u => u.Id == userId);
                    daily = new UserDaily()
                    {
                        Id = aspUser.Id,
                        AspNetUser = aspUser,
                        LastHourly = DateTime.Now
                    };
                    anacondaModel.UserDailies.Add(daily);
                }
                else
                {
                    if (daily.LastHourly != null)
                    {

                        var timespan = DateTime.Now - daily.LastHourly.Value;
                        if (timespan.Minutes < 60)
                        {
                            return RedirectToAction("Index");
                        }

                    }

                    daily.LastHourly = DateTime.Now;
                }

                wallet.Credits += result.CreditsGained;
                var userStats = anacondaModel.UserStatistics.First(s => s.Id == userId);
                userStats.Experience += 100;

                Session["last-user-hourly-date"] = DateTime.Now;
                Session["last-user-hourly-credits"] = result.CreditsGained;

                anacondaModel.SaveChanges();
            }

            return RedirectToAction("Index");
        }


    }
}