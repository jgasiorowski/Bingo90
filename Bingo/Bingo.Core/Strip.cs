namespace Bingo.Core
{
    public class Strip
    {
        public Ticket[] Tickets { get; private set; } = new Ticket[6];

        public Strip(IEnumerable<Ticket> tickets)
        {
            Tickets = tickets.ToArray();
        }

        public void Print()
        {
            for (int i = 0; i < 6; i++)
            {
                Tickets[i].Print();
            }
        }
    }
}
