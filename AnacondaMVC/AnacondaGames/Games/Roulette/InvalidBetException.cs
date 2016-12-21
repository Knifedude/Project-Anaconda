using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnacondaGames.Games.Roulette
{
    public class InvalidBetException : Exception
    {
        public InvalidBetException()
        {
        }

        public InvalidBetException(string message) : base(message)
        {
        }

        public InvalidBetException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
