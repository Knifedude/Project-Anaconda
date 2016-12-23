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


        public GameResult Play(Bet bet)
        {
            var rp = new RandomPicker<ISpinAction>(_actions, _random.Next());
            var result = rp.Pick().Item.Execute(new GameContext(bet.Credits));
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

    }

    


}