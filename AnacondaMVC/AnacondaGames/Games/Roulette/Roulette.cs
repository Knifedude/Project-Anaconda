using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace AnacondaGames.Games.Roulette
{
    public class Roulette
    {
        private Dictionary<string, BetType> betTypes;

        private Random random;
        private RoulletteBoard _board;

        public Roulette() : this(RoulletteBoard.CreateDefault())
        {
           
        }

        public Roulette(RoulletteBoard board)
        {
            _board = board;
            betTypes = new Dictionary<string, BetType>();

            for (var i = 1; i < 36; i++)
            {
                betTypes[Convert.ToString(i)] = new BetType(36, sp => sp.Number == i);
            }

            betTypes["black"] = new BetType(2, sp => sp.Color == Color.Black);
            betTypes["red"] = new BetType(2, sp => sp.Color == Color.Red);
            betTypes["odd"] = new BetType(2, sp => sp.Number != 0 && (sp.Number % 2 != 0));
            betTypes["even"] = new BetType(2, sp => sp.Number != 0 && (sp.Number % 2 == 0));
            betTypes["1st 12"] = new BetType(3, sp => sp.Number >= 1 && sp.Number <= 12);
            betTypes["2nd 12"] = new BetType(3, sp => sp.Number >= 13 && sp.Number <= 24);
            betTypes["3rd 12"] = new BetType(3, sp => sp.Number >= 25 && sp.Number <= 36);

            betTypes["1st Col"] = new BetType(3, sp => sp.Number != 0 && (sp.Number + 2) % 3 == 0);
            betTypes["2nd Col"] = new BetType(3, sp => sp.Number != 0 && (sp.Number + 1) % 3 == 0);
            betTypes["3rd Col"] = new BetType(3, sp => sp.Number != 0 && sp.Number % 3 == 0);

            betTypes["1 to 18"] = new BetType(2, sp => sp.Number >= 1 && sp.Number <= 18);
            betTypes["19 to 36"] = new BetType(2, sp => sp.Number >= 19 && sp.Number <= 36);
        }


        public ICollection<WinningBet> Spin(ICollection<Bet> bets)
        {
            // Validate Bets
            foreach (var bet in bets)
            {
                if (!betTypes.ContainsKey(bet.Type))
                {
                    throw new InvalidBetException("Invalid bet type '" + bet.Type + "'");
                }
            }





            return null;
        }

        public class RoulletteBoard
        {
            private const int BOARD_SIZE = 36;
            private Color[] _tiles;

            public RoulletteBoard()
            {
                _tiles = new Color[BOARD_SIZE];
            }

            public void SetColor(Color color, int number)
            {
                _tiles[number] = color;
            }

            public void SetColor(Color color, params int[] number)
            {
                foreach (var n in number)
                {
                    _tiles[n] = color;
                }
            }

            public static RoulletteBoard CreateDefault()
            {
                var board = new RoulletteBoard();
                board.SetColor(Color.Black, 2, 4, 6, 8, 10, 11, 13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35);
                board.SetColor(Color.Red, 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 25, 27, 30, 32, 34, 36);
                board.SetColor(Color.Green, 0);
                return board;
            }


        }




    }

    public class WinningBet : Bet
    {

        public int AppliedMultiplier { get; set; }

        public int Payout { get; set; }
        
    }

    public class Bet
    {
        
        public string Type { get; set; }

        public int Credits { get; set; }

    }


    public class RouletteSpinResult
    {

        public Color Color { get; set; }

        public int Number { get; set; }
        
    }

    public enum Color
    {
        Black, 
        Red,
        Green
    }

    public class BetType
    {

        public delegate bool IsWinning(RouletteSpinResult sp);

        public BetType(int multiplier, IsWinning winning)
        {
            Multiplier = multiplier;
        }

        public int Multiplier { get; set; }

    }



}
