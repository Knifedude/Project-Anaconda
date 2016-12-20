using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnacondaMVC.Games.WheelOfFortune;

namespace AnacondaGames.Games.WheelOfFortune
{
    public class RandomPicker<T>
    {

        private Random _random;
        private List<RandomItem<T>> _items;

        public RandomPicker(List<RandomItem<T>> randomItems, int seed = 1)
        {
            var sum = Math.Abs(randomItems.Sum(x => x.Probability));
            if (Math.Abs(sum - 1.0) > 0.001)
            {
                throw new ArgumentException("Sum of item probability must be 1.0");
            }

            _items = randomItems;
            _random = new Random(seed);
        }

        public RandomItem<T> Pick()
        {
            var diceRoll = _random.NextDouble();
            var cumulative = 0.0;

            RandomItem<T> item = null;

            for (int i = 0; i < _items.Count; i++)
            {
                cumulative += _items[i].Probability;
                if (diceRoll < cumulative)
                {
                    item =  _items[i];
                    break;
                }
            }

            return item;
        }

    }

    public class RandomItem<T>
    {
       
        public RandomItem(double probability, T item)
        {
            this.Probability = probability;
            this.Item = item;
        }

        public double Probability { get; }
        public T Item { get; }

    }
    
}
