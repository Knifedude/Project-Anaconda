using AnacondaMVC.Logic;
using AnacondaMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AnacondaMVC.DAO
{
    public class PlayerStatisticsDAO
    {
        private AnacondaModel _context;

        public PlayerStatisticsDAO(AnacondaModel context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("Context may not be null!");
            }
            _context = context;
        }

        public UserStatistic CreatePlayerStatistics(string userId, int xp = 100)
        {
            var user = _context.AspNetUsers.First(u => u.Id == userId);
            if (user == null)
            {
                throw new UserNotFoundException("No user for userId '" + userId + "'");
            }
            UserStatistic playerStatistics = new UserStatistic()
            {
                AspNetUser = user,
                Experience = xp,
                Id = user.Id
            };

            _context.UserStatistics.Add(playerStatistics);
            return playerStatistics;
        }

        public UserStatistic CreatePlayerStatisticsIfNotExist(string userId, int xp = 100)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("userId may not be null!");
            }

            if (!HasPlayerStatistics(userId))
            {
                return CreatePlayerStatistics(userId, xp);

            }
            return GetPlayerStatistics(userId);
        }

        public UserStatistic GetPlayerStatistics(string userId)
        {
            return _context.UserStatistics.First(s => s.Id == userId);
        }

        public bool HasPlayerStatistics(string userId)
        {
            return _context.UserStatistics.Any(s => s.Id == userId);
        }
    }
}