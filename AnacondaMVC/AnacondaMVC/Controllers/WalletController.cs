﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
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

                Wallet wallet = am.Wallets.FirstOrDefault(x => x.UserId.Equals(userId));

                if (wallet == null)
                {

                    var aspUser = am.AspNetUsers.FirstOrDefault(u => u.Id.Equals(userId));


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


                return View(wallet);
            }


            
        }
    }
}