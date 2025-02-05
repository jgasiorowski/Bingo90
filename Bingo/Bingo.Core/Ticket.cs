namespace Bingo.Core
{
    public class Ticket
    {
        public int?[][] Cells { get; private set; } = new int?[3][];

        internal Ticket(IEnumerable<IEnumerable<int?>> rows)
        {
            Cells = (int?[][])rows;
        }

        public void Print()
        {
            Console.WriteLine("--------------------------");
            for (int i = 0; i < 3; i++)
            {
                var row = string.Join('|', Cells[i].Select(n => n is null ? "  " : n?.ToString("00")));

                Console.WriteLine(row);
            }
            Console.WriteLine("--------------------------");
            Console.WriteLine();
        }
    }
}
