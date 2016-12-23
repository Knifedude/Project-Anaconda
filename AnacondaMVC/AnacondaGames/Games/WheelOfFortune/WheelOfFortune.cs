using System.Collections.Generic;
using AnacondaGames.Games.Roulette;
using AnacondaGames.Games.SlotMachine;
using System;
using AnacondaMVC.Games;
using AnacondaMVC.Games.WheelOfFortune;

namespace AnacondaGames.Games.WheelOfFortune
{
    public class WheelOfFortune : ISingleBetGame<GameResult>
    {

        private Random _random;
        private List<RandomItem<ISpinAction>> _actions;

        public WheelOfFortune(string name, Random random)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name may not be null");
            }

            if (random == null)
            {
                throw new ArgumentNullException("random may not be null");
            }

            _random = random;
            _actions = new List<RandomItem<ISpinAction>>();
            Name = name;
        }

        public void AddAction(double chance, ISpinAction action)
        {
            this._actions.Add(new RandomItem<ISpinAction>(chance, action));
        }

        public List<RandomItem<ISpinAction>> Actions { get; set; }


        public GameResult Play(Bet bet = null)
        {
            var rp = new RandomPicker<ISpinAction>(_actions, _random.Next());
            var result = rp.Pick().Item.Execute(new GameContext(bet?.Credits ?? 0));
            return result;
        }


        public string Name { get; }

        public static WheelOfFortune CreateDefault(string name, Random random)
        {
            WheelOfFortune wheel = new WheelOfFortune(name, random);
            wheel._actions = new List<RandomItem<ISpinAction>>
            {
                new RandomItem<ISpinAction>(0.6, new BetMultiplier(0.5m)),
                new RandomItem<ISpinAction>(0.2, new BetMultiplier(1.1m)),
                new RandomItem<ISpinAction>(0.1, new BetMultiplier(0m)),
                new RandomItem<ISpinAction>(0.1 / 3, new BetMultiplier(1.5m)),
                new RandomItem<ISpinAction>(0.1 / 3, new BetMultiplier(2m)),   
                new RandomItem<ISpinAction>(0.1 / 3, new BetMultiplier(3m))
            };
            return wheel;
        }

        public static WheelOfFortune CreateDaily(string name, Random random)
        {
            WheelOfFortune wheel = new WheelOfFortune(name, random);

            double chance = 1.0;

            wheel._actions = new List<RandomItem<ISpinAction>>
            {
                new RandomItem<ISpinAction>(0.6, new CreditIncrease(100)),
                new RandomItem<ISpinAction>(0.2, new CreditIncrease(300)),
                new RandomItem<ISpinAction>(0.1, new CreditIncrease(600)),
                new RandomItem<ISpinAction>(0.1 / 3, new CreditIncrease(1000)),
                new RandomItem<ISpinAction>(0.1 / 3, new CreditIncrease(2000)),
                new RandomItem<ISpinAction>(0.1 / 3, new CreditIncrease(3000))
            };
            return wheel;
        }


        public static WheelOfFortune CreateHourly(string name, Random random)
        {
            WheelOfFortune wheel = new WheelOfFortune(name, random);
            wheel._actions = new List<RandomItem<ISpinAction>>
            {
                new RandomItem<ISpinAction>(0.6, new CreditIncrease(10)),
                new RandomItem<ISpinAction>(0.2, new CreditIncrease(30)),
                new RandomItem<ISpinAction>(0.1, new CreditIncrease(60)),
                new RandomItem<ISpinAction>(0.1 / 3, new CreditIncrease(100)),
                new RandomItem<ISpinAction>(0.1 / 3, new CreditIncrease(200)),
                new RandomItem<ISpinAction>(0.1 / 3, new CreditIncrease(300))
            };
            return wheel;
        }

    }

    


}