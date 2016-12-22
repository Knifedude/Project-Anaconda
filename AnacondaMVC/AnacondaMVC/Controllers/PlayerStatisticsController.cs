using AnacondaMVC.DAO;
using AnacondaMVC.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace AnacondaMVC.Controllers
{
    public class PlayerStatisticsController : Controller
    {
        // GET: PlayerStatistics
        public ActionResult Index()
        {
            ViewBag.TotalXP = TotalXP();

            return View();
        }

        [ChildActionOnly]
        public ActionResult PartialXP()
        {
            ViewBag.TotalXP = TotalXP();

            return PartialView();
        }

        private string TotalXP()
        {
            var user = HttpContext.User.Identity as ClaimsIdentity;
            var userId = user.GetUserId();

            int totalXP = 0;
            using (var am = new AnacondaModel())
            {
                var playerStatisticsDao = new PlayerStatisticsDAO(am);
                playerStatisticsDao.CreatePlayerStatisticsIfNotExist(userId);
                am.SaveChanges();

                totalXP = am.UserStatistics.First(s => s.Id == userId).Experience;
            }

            return totalXP.ToString();
        }
    }
}