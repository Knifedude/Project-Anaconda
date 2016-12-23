using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
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
                }
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
                }

            }

            return PartialView(bonus);
        }

        public class BonusViewModel
        {
            
            public bool Daily { get; set; }

            public bool Hourly { get; set; }


        }




    }
}