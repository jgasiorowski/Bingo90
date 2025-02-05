namespace Bingo.Core
{
    class TicketBuilder
    {
        public Ticket ToTicket()
        {
            var rows = new int?[3][];
            for (int i = 0; i < rows.Length; i++)
            {
                rows[i] = Cells[i].Select(c => (int?)(c < 1 ? null : c)).ToArray();
            }

            return new Ticket(rows);
        }

        public int NumbersCount = 0;
        public List<List<int>> Ranges = new List<List<int>>(9)
        {
            new List<int>(3),
            new List<int>(3),
            new List<int>(3),
            new List<int>(3),
            new List<int>(3),
            new List<int>(3),
            new List<int>(3),
            new List<int>(3),
            new List<int>(3),
        };
        public int[][] Cells { get; set; } = new int[3][];
        public int Index { get; set; }
        public TicketBuilder(int index)
        {
            Index = index;
            for (int i = 0; i < 3; i++)
            {
                Cells[i] = new int[9];
            }
        }
        public void Print()
        {
            Console.WriteLine("--------------------------");
            for (int i = 0; i < 3; i++)
            {
                var row = string.Join('|', Cells[i].Select(n => n == 99 ? "  " : n.ToString("00")));

                Console.WriteLine(row);
            }
            Console.WriteLine("--------------------------");
            Console.WriteLine();
        }

        internal void Place(int v)
        {
            var range = v / 10;
            range = range == 9 ? range - 1 : range;

            //Numbers.Add(v);
            NumbersCount++;
            Ranges[range].Add(v);
        }

        internal bool CanAccept(int v)
        {
            var range = v / 10;
            range = range == 9 ? range - 1 : range;
            var howManyInRange = Ranges[range].Count;

            return howManyInRange < 3 && NumbersCount < 15;
        }

        internal void BalanceEmptyCells()
        {
            var random = new Random();
            var noEmpty = new List<List<int>>();
            var oneEmpty = new List<List<int>>();
            var twoEmpty = new List<List<int>>();

            foreach (var range in Ranges)
            {
                if (range.Count == 1)
                {
                    twoEmpty.Add(range);
                    continue;
                }

                if (range.Count == 2)
                {
                    oneEmpty.Add(range);
                    continue;
                }

                if (range.Count == 3)
                {
                    noEmpty.Add(range);
                    continue;
                }
            }

            //var distribution = $"{noEmpty.Count}|{oneEmpty.Count}|{twoEmpty.Count}";

            //var posibilities = new List<string>
            //{
            //    "0|6|3",
            //    "1|4|4",
            //    "2|2|5",
            //    "3|0|6",
            //};

            //if (noEmpty.Count < 4)
            //{
            var emptyCellsLeft = new Pair[] { new Pair(1, 4), new Pair(0, 4), new Pair(2, 4) };
            //var row = new int[] { 0, 1, 2 };
            foreach (var range in twoEmpty)
            {
                var rowindicesLeft = new List<int>() { 0, 1, 2 };
                var row = emptyCellsLeft.OrderByDescending(x => x.Count).First();
                var columnIndex = Ranges.IndexOf(range);
                Cells[row.Index][columnIndex] = 0;
                rowindicesLeft.Remove(row.Index);
                row.Count--;

                row = emptyCellsLeft.OrderByDescending(x => x.Count).First();

                Cells[row.Index][columnIndex] = 0;
                rowindicesLeft.Remove(row.Index);
                row.Count--;

                Cells[rowindicesLeft.First()][columnIndex] = range.First();
            }

            foreach (var range in oneEmpty)
            {
                var rowindicesLeft = new List<int>() { 0, 1, 2 };
                var row = emptyCellsLeft.OrderByDescending(x => x.Count).First();
                var columnIndex = Ranges.IndexOf(range);
                Cells[row.Index][columnIndex] = 0;
                rowindicesLeft.Remove(row.Index);
                row.Count--;

                var ordered = range.Order().ToArray();
                Cells[rowindicesLeft.First()][columnIndex] = ordered[0];
                Cells[rowindicesLeft.Last()][columnIndex] = ordered[1];

            }

            foreach (var range in noEmpty)
            {
                var columnIndex = Ranges.IndexOf(range);
                var ordered = range.Order().ToArray();

                Cells[0][columnIndex] = ordered[0];
                Cells[1][columnIndex] = ordered[1];
                Cells[2][columnIndex] = ordered[2];
            }

        }

        class Pair
        {
            public Pair(int index, int count)
            {
                Index = index;
                Count = count;
            }
            public int Index;
            public int Count;
        }
    }

}

