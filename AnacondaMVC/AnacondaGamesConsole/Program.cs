using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AnacondaGames.Games.WheelOfFortune;

namespace AnacondaGamesConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            TestRandomPicker();

            Console.ReadKey();
        }

        private static void TestRandomPicker()
        {
            var items = new List<RandomItem<string>>
            {
                new RandomItem<string>(0.6, "a"),
                new RandomItem<string>(0.1 / 3, "b"),
                new RandomItem<string>(0.1 / 3, "c"),
                new RandomItem<string>(0.1 / 3, "d"),
                new RandomItem<string>(0.3, "e")
            };
            var rp = new RandomPicker<string>(items, 1337);
            var dict = new Dictionary<string, int>();
            for (var i = 0; i < 10000; i++)
            {
                var pick = rp.Pick();

                if (!dict.ContainsKey(pick.Item))
                {
                    dict[pick.Item] = 1;
                }
                else
                {
                    dict[pick.Item]++;
                }

            }

            foreach (var i in dict)
            {
                Console.WriteLine(i.Key + ": " + i.Value);
            }

        }

    }
}
