﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using AnacondaMVC.Models;
using Microsoft.AspNet.Identity;

namespace AnacondaMVC.Controllers
{
    [Authorize]
    public class WalletController : Controller
    {
        // GET: Credit
        public ActionResult Index()
        {
            using (var am = new AnacondaModel())
            {
                var user = HttpContext.User.Identity as ClaimsIdentity;
                var userId = user.GetUserId();

                bool hasWallet = am.Wallets.Any(x => x.UserId.Equals(userId));
                Wallet wallet;

                if (!hasWallet)
                {

                    var aspUser = am.AspNetUsers.First(u => u.Id.Equals(userId));


                    wallet = new Wallet()
                    {
                        AspNetUser = aspUser,
                        CasinoCredits = 100,
                        Credits = 0,
                        UserId = userId
                    };
                    am.Wallets.Add(wallet);
                    am.SaveChanges();
                }

                wallet = am.Wallets.First(x => x.UserId.Equals(userId));
                
                return View(wallet);
            }


            
        }

        [HttpPost]
        public ActionResult AddCredits(FormCollection form)
        {
            var creditsToAdd = form["creditsToAdd"].AsInt();
            var user = HttpContext.User.Identity as ClaimsIdentity;
            var userId = user.GetUserId();

            Wallet wal = null;
            using (var am = new AnacondaModel())
            {
                wal = am.Wallets.First(w => w.UserId == userId);
                wal.Credits += creditsToAdd;
                am.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [ChildActionOnly]
        public ActionResult PartialWallet()
        {
            var user = HttpContext.User.Identity as ClaimsIdentity;
            var userId = user.GetUserId();

            Wallet wal = null;
            int totalCredits = 0;
            using (var am = new AnacondaModel())
            {
                wal = am.Wallets.First(w => w.UserId == userId);
                totalCredits = wal.Credits + wal.CasinoCredits;
            }

            ViewBag.TotalCredits = totalCredits.ToString();

            return PartialView();
        }
    }
}