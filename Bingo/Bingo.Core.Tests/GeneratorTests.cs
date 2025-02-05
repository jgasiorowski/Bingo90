namespace Bingo.Core.Tests
{
    [TestClass]
    public class GeneratorTests
    {

        [TestMethod]
        public void Strip_Contains_SixTickets()
        {
            var expectedTicketsCount = 6;

            var strip = new Generator().Generate();

            Assert.AreEqual(expectedTicketsCount, strip.Tickets.Length);
            foreach (var ticket in strip.Tickets)
            {
                Assert.IsInstanceOfType<Ticket>(ticket);
            }

        }

        [TestMethod]
        public void TicketInStrip_Contains_3RowsAnd9Columns()
        {
            var expectedRowsCount = 3;
            var expectedColumnsCount = 9;
            var strip = new Generator().Generate();

            foreach (var ticket in strip.Tickets)
            {
                Assert.AreEqual(expectedRowsCount, ticket.Cells.Length);
                foreach (var row in ticket.Cells)
                {
                    Assert.AreEqual(expectedColumnsCount, row.Length);
                }
            }

        }

        [TestMethod]
        public void Generate_EachRowInTickets_Contains5Numbers()
        {
            var strip = new Generator().Generate();

            foreach (var tickets in strip.Tickets)
            {
                foreach (var row in tickets.Cells)
                {
                    var blanksCount = 0;
                    var numbersCount = 0;

                    foreach (var cell in row)
                    {
                        if (cell is null)
                        {
                            blanksCount++;
                        }
                        else
                        {
                            numbersCount++;
                        }
                    }

                    Assert.AreEqual(4, blanksCount);
                    Assert.AreEqual(5, numbersCount);
                }
            }
        }

        [TestMethod]
        public void Generate_EachColumnInTickets_ContainsBeween1And3Numbers()
        {
            var strip = new Generator().Generate();

            foreach (var tickets in strip.Tickets)
            {
                for (var i = 0; i < 9; i++)
                {
                    var numbersCount = 0;
                    for (var j = 0; j < 3; j++)
                    {
                        if (tickets.Cells[j][i] is not null)
                        {
                            numbersCount++;
                        };
                    }
                    Assert.IsTrue(numbersCount > 0 && numbersCount < 4);
                }
            }

        }

        [TestMethod]
        public void Generate_EachFirstColumnInTicket_ContainsNumbersBetween1And9()
        {
            var strip = new Generator().Generate();

            foreach (var tickets in strip.Tickets)
            {
                for (var i = 0; i < 3; i++)
                {
                    var value = tickets.Cells[i][0];
                    if (value is null)
                    {
                        continue;
                    }
                    Assert.IsTrue(value > 0 && value < 10, value.ToString());
                }
            }
        }

        [TestMethod]
        public void Generate_EachColumnInTheMiddleOfTicket_ContainsNumbersFromCorrect10NumbersRange()
        {
            var strip = new Generator().Generate();

            foreach (var tickets in strip.Tickets)
            {
                for (var i = 1; i < 8; i++)
                {
                    var range = Enumerable.Range(i * 10, 10).Cast<int?>();
                    for (var j = 0; j < 3; j++)
                    {
                        var value = tickets.Cells[j][i];

                        Assert.IsTrue(value is null || range.Contains(value), value.ToString());
                    }
                }
            }
        }

        [TestMethod]
        public void Generate_EachLastColumnInTicket_ContainsNumbersBetween80And90()
        {
            var strip = new Generator().Generate();

            foreach (var tickets in strip.Tickets)
            {
                for (var i = 0; i < 3; i++)
                {
                    var value = tickets.Cells[i][8];
                    if (value is null)
                    {
                        continue;
                    }
                    Assert.IsTrue(value > 79 && value < 91, value.ToString());
                }
            }
        }

        [TestMethod]
        public void Generate_EachColumnNumbers_AreOrderedAscending()
        {
            var strip = new Generator().Generate();

            var allNumbers = Enumerable.Range(1, 90);
            foreach (var tickets in strip.Tickets)
            {
                for (var i = 1; i < 8; i++)
                {
                    int? previous = null;
                    for (var j = 0; j < 3; j++)
                    {

                        var value = tickets.Cells[j][i];
                        if (previous is null)
                        {
                            previous = value;
                            continue;
                        }

                        if (value is null)
                        {
                            continue;
                        }

                        Assert.IsTrue(value > previous);

                    }
                }
            }
        }

        [TestMethod]
        public void Generate_Strip_ContainsNoDuplicates()
        {
            var strip = new Generator().Generate();

            var allNumbers = Enumerable.Range(1, 90).ToList();
            foreach (var tickets in strip.Tickets)
            {
                foreach (var row in tickets.Cells)
                {
                    foreach (var cell in row)
                    {
                        if (cell is null)
                        {
                            continue;
                        }

                        Assert.IsTrue(allNumbers.Remove(cell.Value));
                    }
                }
            }
        }

        [TestMethod]
        public void Generate_Strip_ContainsAllNumbers_Between1And90()
        {
            var strip = new Generator().Generate();

            var allNumbers = Enumerable.Range(1, 90).ToList();
            foreach (var tickets in strip.Tickets)
            {
                foreach (var row in tickets.Cells)
                {
                    foreach (var cell in row)
                    {
                        if (cell is null)
                        {
                            continue;
                        }

                        allNumbers.Remove(cell.Value);
                    }
                }
            }

            Assert.AreEqual(0, allNumbers.Count);
        }
    }
}
