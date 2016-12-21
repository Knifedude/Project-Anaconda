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
        private Dictionary<string, BetType> _betTypes;

        private Random _random;
        private RoulletteBoard _board;

        public Roulette(Random random) : this(RoulletteBoard.CreateDefault(), random)
        {
           
        }

        public Roulette(RoulletteBoard board, Random random)
        {
            _board = board;
            _random = random;
            _betTypes = new Dictionary<string, BetType>();

            for (var i = 1; i < 36; i++)
            {
                _betTypes[Convert.ToString(i)] = new BetType(36, sp => sp.Number == i);
            }

            _betTypes["black"] = new BetType(2, sp => sp.Color == Color.Black);
            _betTypes["red"] = new BetType(2, sp => sp.Color == Color.Red);
            _betTypes["odd"] = new BetType(2, sp => sp.Number != 0 && (sp.Number % 2 != 0));
            _betTypes["even"] = new BetType(2, sp => sp.Number != 0 && (sp.Number % 2 == 0));
            _betTypes["1st 12"] = new BetType(3, sp => sp.Number >= 1 && sp.Number <= 12);
            _betTypes["2nd 12"] = new BetType(3, sp => sp.Number >= 13 && sp.Number <= 24);
            _betTypes["3rd 12"] = new BetType(3, sp => sp.Number >= 25 && sp.Number <= 36);

            _betTypes["1st Col"] = new BetType(3, sp => sp.Number != 0 && (sp.Number + 2) % 3 == 0);
            _betTypes["2nd Col"] = new BetType(3, sp => sp.Number != 0 && (sp.Number + 1) % 3 == 0);
            _betTypes["3rd Col"] = new BetType(3, sp => sp.Number != 0 && sp.Number % 3 == 0);

            _betTypes["1 to 18"] = new BetType(2, sp => sp.Number >= 1 && sp.Number <= 18);
            _betTypes["19 to 36"] = new BetType(2, sp => sp.Number >= 19 && sp.Number <= 36);
        }


        public ICollection<WinningBet> Spin(ICollection<Bet> bets)
        {
            // Validate Bets
            foreach (var bet in bets)
            {
                if (!_betTypes.ContainsKey(bet.Type))
                {
                    throw new InvalidBetException("Invalid bet type '" + bet.Type + "'");
                }
            }

            var spin = _random.Next(0, 36);
            var spinResult = new RouletteSpinResult(_board.GetColor(spin), spin);


            var winningBets = new List<WinningBet>();

            foreach (var betsByType in bets.GroupBy(u => u.Type))
            {

                var betType = _betTypes[betsByType.Key];

                if (betType.IsWinningMethod.Invoke(spinResult))
                {

                    var payout = betsByType.Sum(bet => bet.Credits*betType.Multiplier);
                    var winningBet = new WinningBet()
                    {
                        AppliedMultiplier = betType.Multiplier,
                        Credits = betsByType.First().Credits,
                        Payout = payout
                    };
                    winningBets.Add(winningBet);
                }
            }
            return winningBets;
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

            public Color GetColor(int number)
            {
                return _tiles[number];
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
        public RouletteSpinResult(Color color, int number)
        {
            Color = color;
            Number = number;
        }

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
            IsWinningMethod = winning;
        }

        public IsWinning IsWinningMethod { get; }

        public int Multiplier { get; set; }

    }



}
