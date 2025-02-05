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

        internal void Distribute(int v)
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

            var aa = a.ToArray();
            random.Shuffle(aa);
            var ordered = aa.Concat(b).ToList();

            //var minlist = Tickets.Where(x => x.Numbers.Count == min).Select(x => x.Index).ToArray();
            //var otherlist = Tickets.Where(x => x.Numbers.Count != min).Select(a => a.Index).ToList();
            //random.Shuffle(minlist);
            //var ordered = minlist.Concat(otherlist).ToList();

            for (int i = 0; i < 6; i++)
            {
                var ticket = Tickets[ordered[i]];

                if (ticket.CanAccept(v))
                {
                    ticket.Place(v);
                    return;
                }
            }

            throw new Exception("This shouldn't happen");
        }

        internal Strip Build()
        {
            var tickets = Tickets.Select(t => t.ToTicket()).ToArray();


            return new Strip(tickets);
        }
    }

}

