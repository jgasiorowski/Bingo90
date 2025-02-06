using System.Collections.Generic;

namespace Bingo.Core
{
    public class Generator
    {
        public Strip Generate()
        {
            var numbers1 = Enumerable.Range(1, 9).ToList();
            var numbers2 = Enumerable.Range(10, 10).ToList();
            var numbers3 = Enumerable.Range(20, 10).ToList();
            var numbers4 = Enumerable.Range(30, 10).ToList();
            var numbers5 = Enumerable.Range(40, 10).ToList();
            var numbers6 = Enumerable.Range(50, 10).ToList();
            var numbers7 = Enumerable.Range(60, 10).ToList();
            var numbers8 = Enumerable.Range(70, 10).ToList();
            var numbers9 = Enumerable.Range(80, 11).ToList();
            var all = new List<List<int>>(9)
            {
                numbers1,
                numbers2,
                numbers3,
                numbers4,
                numbers5,
                numbers6,
                numbers7,
                numbers8,
                numbers9
            };

            var strip = new StripBuilder();

            var random = new Random();

            foreach (var ticket in strip.Tickets)
            {
                for (int i = 0; i < all.Count; i++)
                {
                    GetForColumn(ticket, all[i], random, i);
                }
            }
            /// ===========
            /// 

            for (var i = 0; i < all.Count; i++)
            {
                var list = all[i];
                do
                {
                    var index = random.Next(list.Count);
                    strip.Distribute(list[index], i);
                    list.RemoveAt(index);
                } while (list.Count > 0);
            }

            ///--------------
            ///

            foreach (var ticket in strip.Tickets)
            {
                ticket.BalanceEmptyCells();
            }

            return strip.Build();
        }

        private static void GetForColumn(TicketBuilder ticket, List<int> numbers1, Random random, int rangeIndex)
        {
            var index = random.Next(numbers1.Count);
            ticket.Place(numbers1[index], rangeIndex);
            numbers1.RemoveAt(index);
        }
    }

}

