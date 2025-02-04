﻿using System.Diagnostics;

namespace Bingo.Cli
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Strip strip = null;
            for (var i = 0; i < 10000; i++)
            {
                //Console.WriteLine($"iteracja {i}");
                strip = Generate();
            }

            sw.Stop();
            Console.WriteLine($"czas {sw.ElapsedMilliseconds}");

            for (int i = 0; i < 6; i++)
            {
                strip.Tickets[i].Out();
            }

            Console.WriteLine($"All 15? = {strip.Tickets.All(t => t.Numbers.Count == 15)}");
        }

        private static Strip Generate()
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

            var strip = new Strip();

            var random = new Random();

            foreach (var ticket in strip.Tickets)
            {
                GetForColumn(ticket, numbers1, random);
                GetForColumn(ticket, numbers2, random);
                GetForColumn(ticket, numbers3, random);
                GetForColumn(ticket, numbers4, random);
                GetForColumn(ticket, numbers5, random);
                GetForColumn(ticket, numbers6, random);
                GetForColumn(ticket, numbers7, random);
                GetForColumn(ticket, numbers8, random);
                GetForColumn(ticket, numbers9, random);
            }
            /// ===========
            /// 

            foreach (var list in all)
            {
                do
                {
                    var index = random.Next(list.Count);
                    strip.Distribute(list[index]);
                    list.RemoveAt(index);
                } while (list.Count > 0);

            }

            return strip;
        }

        private static void GetForColumn(Ticket ticket, List<int> numbers1, Random random)
        {
            var index = random.Next(numbers1.Count);
            ticket.Place(numbers1[index]);
            numbers1.RemoveAt(index);
        }
    }


    class Strip
    {
        public Ticket[] Tickets { get; set; } = new Ticket[6];

        public Strip()
        {
            for (int i = 0;i < 6; i++)
            {
                Tickets[i] = new Ticket(i);
            }
        }

        internal void Distribute(int v)
        {
            var random = new Random();
            var min = Tickets.Select(x => x.Numbers.Count).Min();
            var a = new List<int>();
            var b = new List<int>();

            foreach (var tick in Tickets)
            {
                if (tick.Numbers.Count == min)
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


        //looks more random
        internal void Distribute1(int v)
        {
            var random = new Random();
            var list = new List<int> { 0, 1, 2, 3, 4, 5 };

            for (int i = 0; i < 6; i++)
            {
                var index = random.Next(list.Count);
                var ticket = Tickets[list[index]];

                if (ticket.CanAccept(v))
                {
                    ticket.Place(v);
                    return;
                }

                list.RemoveAt(index);
            }

            var range = v / 10;
            range = range == 9 ? range - 1 : range;

            //Console.WriteLine($"!!!!!!!!!!!!!zostało: {v}");
        }
    }

    class Ticket
    {
        public List<int> Numbers { get; set; } = new List<int>(15);
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
        public Ticket(int index)
        {
            Index = index;
            for (int i = 0;i < 3; i++)
            {
                Cells[i] = new int[9];
            }
        }
        public void Out()
        {
            Console.WriteLine("--------------------------");
            for (int i = 0; i < 3; i++)
            {
                var row = string.Join('|', Cells[i].Select(n => n.ToString("00")));
                
                Console.WriteLine(row);
            }
            Console.WriteLine("--------------------------");
            for(var i = 0; i < 3; i++)
            {
                foreach (var range in Ranges)
                {
                    if (range.Count -1 < i)
                    {
                        Console.Write("  \t");
                    }
                    else
                    {
                        Console.Write($"{range.OrderBy(r => r).ToArray()[i].ToString("00")}\t");

                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine(string.Join(',', Numbers.Order()));

            Console.WriteLine();
        }

        internal void Place(int v)
        {
            var range = v / 10;
            range = range == 9 ? range - 1 : range;
            
            Numbers.Add(v);
            Ranges[range].Add(v);
        }

        internal bool CanAccept(int v)
        {
            var range = v / 10;
            range = range == 9 ? range - 1 : range;
            var howManyInRange = Ranges[range].Count();

            return howManyInRange < 3 && Numbers.Count < 15;
        }
    }
}
