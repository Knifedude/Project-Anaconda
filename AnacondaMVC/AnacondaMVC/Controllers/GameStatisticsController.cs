using AnacondaMVC.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnacondaMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class GameStatisticsController : Controller
    {
        // GET: GameStatistics
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.SortOrder = sortOrder;
            ViewBag.GameSortParm = String.IsNullOrEmpty(sortOrder) ? "game_desc" : "";
            ViewBag.ResultSortParm = sortOrder == "result" ? "result_desc" : "result";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            List<GameStatistic> gameStats;

            using (var am = new AnacondaModel())
            {
                var stats = am.GameStatistics.Select(s => s);

                if (!String.IsNullOrEmpty(searchString))
                {
                    stats = stats.Where(s => s.Game.Name.Contains(searchString));
                }

                switch (sortOrder)
                {
                    case "game_desc":
                        stats = stats.OrderByDescending(s => s.Game.Name);
                        break;
                    case "result":
                        stats = stats.OrderBy(s => s.CreditResult);
                        break;
                    case "result_desc":
                        stats = stats.OrderByDescending(s => s.CreditResult);
                        break;
                    default:
                        stats = stats.OrderBy(s => s.Game.Name);
                        break;
                }

                int pageSize = 20;
                int pageNumber = (page ?? 1);

                ViewBag.PageNumber = pageNumber;
                ViewBag.PageCount = (int)Math.Ceiling((double)stats.Count() / pageSize);

                var gameList = new List<string>();
                var avgResult = new List<string>();
                var timesPlayed = new List<string>();

                foreach (string gameName in stats.Select(g => g.Game.Name).Distinct())
                {
                    gameList.Add(gameName);
                    avgResult.Add(stats.Where(g => g.Game.Name == gameName).Select(g => g.CreditResult).Average().ToString("#.##"));
                    timesPlayed.Add(stats.Where(g => g.Game.Name == gameName).Count().ToString());
                }

                ViewBag.GameNames = gameList.ToArray();
                ViewBag.AvgResult = avgResult.ToArray();
                ViewBag.TimesPlayed = timesPlayed.ToArray();
                ViewBag.GameCount = gameList.Count();

                gameStats = stats.Include(s => s.Game).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }

            return View(gameStats);
        }
    }
}