namespace Bingo.Core
{

    class Column : List<int>
    {
        public Column(int index) : base(3)
        {
            Index = index;
        }
        public int Index { get; set; }
    }
    class TicketBuilder
    {
        public Ticket Build()
        {
            return new Ticket(Cells);
        }

        public int NumbersCount = 0;
        public Column[] Ranges = new Column[9];
        public int?[][] Cells { get; set; } = new int?[3][];
        public int Index { get; set; }
        public TicketBuilder(int index)
        {
            Index = index;
            for (int i = 0; i < 3; i++)
            {
                Cells[i] = new int?[9];
            }

            for (int i = 0; i < 9; i++)
            {
                Ranges[i] = new Column(i);
            }
        }

        internal void Place(int v, int rangeIndex)
        {
            NumbersCount++;
            Ranges[rangeIndex].Add(v);
        }

        internal bool CanAccept(int v, int rangeIndex)
        {
            var howManyInRange = Ranges[rangeIndex].Count;

            return howManyInRange < 3 && NumbersCount < 15;
        }

        internal void BalanceEmptyCells()
        {
            var random = new Random();
            var emptyCellsLeft = new Pair[] { new Pair(0, 4), new Pair(1, 4), new Pair(2, 4) };

            foreach (var range in Ranges.OrderBy(r => r.Count))
            {
                if (range.Count == 1)
                {
                    var rowindicesLeft = new List<int>() { 0, 1, 2 };
                    var ordo = emptyCellsLeft.OrderByDescending(x => x.Count).ToArray();
                    var row = ordo[0];
                    var columnIndex = range.Index;
                    rowindicesLeft.Remove(row.Index);
                    row.Count--;

                    row = ordo[1];

                    rowindicesLeft.Remove(row.Index);
                    row.Count--;

                    Cells[rowindicesLeft.First()][columnIndex] = range.First();
                }

                if (range.Count == 2)
                {
                    var rowindicesLeft = new List<int>() { 0, 1, 2 };
                    var row = emptyCellsLeft.OrderByDescending(x => x.Count).First();
                    var columnIndex = range.Index;

                    rowindicesLeft.Remove(row.Index);
                    row.Count--;

                    var ordered = range.Order().ToArray();
                    Cells[rowindicesLeft.First()][columnIndex] = ordered[0];
                    Cells[rowindicesLeft.Last()][columnIndex] = ordered[1];
                }

                if (range.Count == 3)
                {
                    var columnIndex = range.Index;

                    var ordered = range.Order().ToArray();

                    Cells[0][columnIndex] = ordered[0];
                    Cells[1][columnIndex] = ordered[1];
                    Cells[2][columnIndex] = ordered[2];
                }
            }
            //    "0|6|3",
            //    "1|4|4",
            //    "2|2|5",
            //    "3|0|6",
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

