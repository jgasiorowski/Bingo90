namespace Bingo.Core
{
    class StripBuilder
    {
        public TicketBuilder[] Tickets { get; set; } = new TicketBuilder[6];

        public StripBuilder()
        {
            for (int i = 0; i < 6; i++)
            {
                Tickets[i] = new TicketBuilder(i);
            }
        }

        internal void Distribute(int v, int rangeIndex)
        {
            var random = new Random();
            var min = Tickets.Select(x => x.NumbersCount).Min();
            var a = new List<int>();
            var b = new List<int>();

            foreach (var tick in Tickets)
            {
                if (tick.NumbersCount == min)
                {
                    a.Add(tick.Index);
                }
                else
                {
                    b.Add(tick.Index);
                }
            }

            random.Shuffle1(a);
            var ordered = a.Concat(b).ToList();


            for (int i = 0; i < 6; i++)
            {
                var ticket = Tickets[ordered[i]];

                if (ticket.CanAccept(v, rangeIndex))
                {
                    ticket.Place(v, rangeIndex);
                    return;
                }
            }

            throw new Exception("This shouldn't happen");
        }

        internal Strip Build()
        {
            var tickets = Tickets.Select(t => t.Build()).ToArray();


            return new Strip(tickets);
        }
    }

    static class RandomExtensions
    {
        public static void Shuffle1(this Random rng, List<int> array)
        {
            int n = array.Count;
            while (n > 1)
            {
                int k = rng.Next(n--);
                int temp = array[n];
                array[n] = array[k];
                array[k] = temp;
            }
        }
    }

}

