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