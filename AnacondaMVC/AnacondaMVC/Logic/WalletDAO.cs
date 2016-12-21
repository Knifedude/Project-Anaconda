using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AnacondaMVC.Models;

namespace AnacondaMVC.Logic
{
    public class WalletDAO
    {

        private AnacondaModel _context;

        public WalletDAO(AnacondaModel context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("Context may not be null!");
            }
            _context = context;
        }

        public Wallet CreateWallet(string userId, int casinoCredits = 100)
        {
            var user = _context.AspNetUsers.First(u => u.Id == userId);
            if (user == null)
            {
                throw new UserNotFoundException("No user for userId '" + userId + "'");
            }
            Wallet wallet = new Wallet()
            {
                AspNetUser = user,
                CasinoCredits = casinoCredits,
                UserId = user.Id,
                Credits = 0
            };

            _context.Wallets.Add(wallet);
            return wallet;
        }

        public Wallet CreateWalletIfNotExist(string userId, int casinoCredits = 100)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("userId may not be null!");
            }

            if (!HasWallet(userId))
            {
                return CreateWallet(userId, casinoCredits);
                
            }
            return GetWallet(userId);
        }

        public Wallet GetWallet(string userId)
        {
            return _context.Wallets.First(w => w.UserId == userId);
        }

        public bool HasWallet(string userId)
        {
            return _context.Wallets.Any(w => w.UserId == userId);
        }

//        public bool HasSufficientCredits(string userId, int pay)
//        {
//            var wallet = GetWallet(userId);
//            return wallet.CasinoCredits + wallet.Credits >= pay;
//        }

        public bool Pay(string userId, int pay)
        {

            var wallet = GetWallet(userId);

            if ((wallet.CasinoCredits + wallet.Credits) >= pay)
            {
                var toPay = pay;
                if (wallet.CasinoCredits >= pay)
                {
                    wallet.CasinoCredits -= pay;
                    toPay = 0;
                }
                else
                {
                    var diff = Math.Abs(wallet.CasinoCredits - pay);
                    wallet.CasinoCredits -= diff;
                    toPay -= diff;
                }

                wallet.Credits -= toPay;
                return true;
            }
            return false;

        }

    }
}