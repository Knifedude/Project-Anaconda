using AnacondaMVC.DAO;
using AnacondaMVC.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace AnacondaMVC.Controllers
{
    [Authorize]
    public class PlayerStatisticsController : Controller
    {
        // GET: PlayerStatistics
        public ActionResult Index()
        {
            CheckIfPlayerStatisticsExist();

            var user = HttpContext.User.Identity as ClaimsIdentity;
            var userId = user.GetUserId();

            using (var am = new AnacondaModel())
            {
                var playerStatisticsDao = new PlayerStatisticsDAO(am);

                var playerStatistics = playerStatisticsDao.GetPlayerStatistics(userId);

                return View(playerStatistics);
            }
        }

        // GET: Leaderboard
        public ActionResult Leaderboard(string sortOrder, string currentFilter, string searchString, int? page)
        {
            CheckIfPlayerStatisticsExist();

            var user = HttpContext.User.Identity as ClaimsIdentity;
            var userId = user.GetUserId();

            ViewBag.SortOrder = sortOrder;
            ViewBag.UserNameSortParm = String.IsNullOrEmpty(sortOrder) ? "username_desc" : "";
            ViewBag.XPSortParm = sortOrder == "xp" ? "xp_desc" : "xp";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            using (var am = new AnacondaModel())
            {
                var players = am.UserStatistics.Select(p => p);
                
                if (!String.IsNullOrEmpty(searchString))
                {
                    players = players.Where(p => p.AspNetUser.UserName.Contains(searchString));
                }

                switch (sortOrder)
                {
                    case "username_desc":
                        players = players.OrderByDescending(p => p.AspNetUser.UserName);
                        break;
                    case "xp":
                        players = players.OrderBy(p => p.Experience);
                        break;
                    case "xp_desc":
                        players = players.OrderByDescending(p => p.Experience);
                        break;
                    default:
                        players = players.OrderBy(p => p.AspNetUser.UserName);
                        break;
                }

                int pageSize = 2;
                int pageNumber = (page ?? 1);

                ViewBag.PageNumber = pageNumber;
                ViewBag.PageCount = (int)Math.Ceiling((double)players.Count() / pageSize);

                return View(players.Include(p => p.AspNetUser).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList());
            }
        }

        [ChildActionOnly]
        public ActionResult PartialXP()
        {
            CheckIfPlayerStatisticsExist();

            var user = HttpContext.User.Identity as ClaimsIdentity;
            var userId = user.GetUserId();

            using (var am = new AnacondaModel())
            {
                var playerStatisticsDao = new PlayerStatisticsDAO(am);

                var playerStatistics = playerStatisticsDao.GetPlayerStatistics(userId);

                return PartialView(playerStatistics);
            }
        }

        private void CheckIfPlayerStatisticsExist()
        {
            var user = HttpContext.User.Identity as ClaimsIdentity;
            var userId = user.GetUserId();

            using (var am = new AnacondaModel())
            {
                var playerStatisticsDao = new PlayerStatisticsDAO(am);
                playerStatisticsDao.CreatePlayerStatisticsIfNotExist(userId);
                am.SaveChanges();

                playerStatisticsDao.GetPlayerStatistics(userId);
            }
        }
    }
}