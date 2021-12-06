using System.Collections;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Year2021
{
    public class Day1
    {
        public string solve1(string input)
        {
            // We go through each entry and count how many increased from the previous entry
            int increaseCount = 0;
            int? previousMeasurement = null;
            var reader = new StringReader(input);
            for (string? line; (line = reader.ReadLine()) != null;)
            {
                var value = int.Parse(line);
                if (previousMeasurement != null)
                {
                    if (value > previousMeasurement)
                    {
                        ++increaseCount;
                    }
                }
                previousMeasurement = value;
            }
            return increaseCount.ToString();
        }

        public string solve2(string input)
        {
            // Now we do the same but for a sliding group of three values
            int increaseCount = 0;
            Queue<int> group = new Queue<int>();
            int? previousMeasurement = null;
            var reader = new StringReader(input);
            for (string? line; (line = reader.ReadLine()) != null;)
            {
                var value = int.Parse(line);
                group.Enqueue(value);
                if (group.Count > 3)
                {
                    group.Dequeue();
                }

                if (group.Count == 3)
                {
                    int sum = 0;
                    foreach (int i in group)
                    {
                        sum += i;
                    }
                    if (previousMeasurement != null && sum > previousMeasurement)
                    {
                        ++increaseCount;
                    }
                    previousMeasurement = sum;
                }
            }
            return increaseCount.ToString();
        }
    }

    public class Day2
    {
        public string solve1(string input)
        {
            int horizontal = 0, depth = 0;
            var reader = new StringReader(input);
            for (string? line; (line = reader.ReadLine()) != null;)
            {
                const string forwardString = "forward ";
                const string downString = "down ";
                const string upString = "up ";
                if (line.StartsWith(forwardString))
                {
                    horizontal += int.Parse(line.Substring(forwardString.Length));
                }
                else if (line.StartsWith(downString))
                {
                    depth += int.Parse((line.Substring(downString.Length)));
                }
                else if (line.StartsWith(upString))
                {
                    depth -= int.Parse((line.Substring(upString.Length)));
                }
            }
            return (horizontal * depth).ToString();
        }

        public string solve2(string input)
        {
            int horizontal = 0, depth = 0, aim = 0;
            var reader = new StringReader(input);
            for (string? line; (line = reader.ReadLine()) != null;)
            {
                const string forwardString = "forward ";
                const string downString = "down ";
                const string upString = "up ";
                if (line.StartsWith(forwardString))
                {
                    var value = int.Parse(line.Substring(forwardString.Length));
                    horizontal += value;
                    depth += aim * value;
                }
                else if (line.StartsWith(downString))
                {
                    aim += int.Parse((line.Substring(downString.Length)));
                }
                else if (line.StartsWith(upString))
                {
                    aim -= int.Parse((line.Substring(upString.Length)));
                }
            }
            return (horizontal * depth).ToString();
        }
    }

    public class Day3
    {
        BitArray getMostCommonBits(string[] input, bool reverse)
        {
            var bitCounts = new List<int>();
            foreach (var line in input)
            {
                int index = 0;
                foreach (var bit in line)
                {
                    if (index >= bitCounts.Count())
                    {
                        bitCounts.Add(0);
                    }
                    if (bit == '1')
                    {
                        ++bitCounts[index];
                    }
                    ++index;
                }
            }
            var mostCommonBits = new BitArray(bitCounts.Count(), false);
            var bitToSet = 0;
            if (reverse)
            {
                bitCounts.Reverse();
            }
            foreach (var count in bitCounts)
            {
                if ((float)count >= (float)input.Count() / 2)
                {
                    mostCommonBits[bitToSet] = true;
                }
                ++bitToSet;
            }
            return mostCommonBits;
        }

        public string solve1(string input)
        {
            var mostCommonBits = getMostCommonBits(input.Split('\n'), true);
            var rates = new int[2];
            mostCommonBits.CopyTo(rates, 0); // gamma
            mostCommonBits.Not().CopyTo(rates, 1); // epsilon
            return (rates[0] * rates[1]).ToString();
        }

        public string solve2(string input)
        {
            var lines = new List<string>(input.Split('\n'));
            lines.RemoveAll(line => line.Length == 0);
            var o2lines = new List<string>(lines);
            var co2lines = new List<string>(lines);

            int index = 0;
            while (o2lines.Count() > 1)
            {
                var mostCommonBits = getMostCommonBits(o2lines.ToArray(), false);
                o2lines.RemoveAll(line => line[index] != (mostCommonBits[index] ? '1' : '0'));
                ++index;
            }

            index = 0;
            while (co2lines.Count() > 1)
            {
                var mostCommonBits = getMostCommonBits(co2lines.ToArray(), false);
                co2lines.RemoveAll(line => line[index] != (mostCommonBits[index] ? '0' : '1'));
                ++index;
            }
            return (Convert.ToUInt32(o2lines[0], 2) * Convert.ToUInt32(co2lines[0], 2)).ToString(); ;
        }
    }

    public class Day4
    {
        class BingoBoard
        {
            public List<int> numbers = new List<int>(5 * 5);

            // if wins, returns value, otherwise 0
            public int processNumber(int number)
            {
                for (int i = 0; i < numbers.Count(); i++)
                {
                    if (number == numbers[i])
                    {
                        numbers[i] = 0;
                    }
                }

                for (int i = 0; i < 5; i++)
                {
                    if (numbers.Skip(i * 5).Take(5).Sum() == 0)
                    {
                        return numbers.Sum();
                    }

                    if (numbers.Where(num => (numbers.IndexOf(num) + i) % 5 == 0).Sum() == 0)
                    {
                        return numbers.Sum();
                    }
                }

                return 0;
            }
        }

        List<BingoBoard> fillBoards(List<string> input)
        {
            var boards = new List<BingoBoard>();

            while (input.Count() > 0)
            {
                boards.Add(new BingoBoard());
                for (int i = 0; i < 5; i++)
                {
                    var boardRow = input[i].Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    for (int j = 0; j < 5; j++)
                    {
                        boards.Last().numbers.Add(int.Parse(boardRow[j]));
                    }
                }
                input.RemoveRange(0, 5);
            }
            return boards;
        }

        public string solve1(string input)
        {
            var lines = new List<string>(input.Split('\n'));
            lines.RemoveAll(line => line.Length == 0);

            var numbers = lines[0].Split(',');
            lines.RemoveAt(0);

            var boards = fillBoards(lines);

            foreach (var number in numbers)
            {
                foreach (var board in boards)
                {
                    var num = int.Parse(number);
                    var result = board.processNumber(num);
                    if (result != 0)
                    {
                        return (num * result).ToString();
                    }
                }
            }
            return "Whoops I shouldn't be here!!";
        }
        public string solve2(string input)
        {
            var lines = new List<string>(input.Split('\n'));
            lines.RemoveAll(line => line.Length == 0);

            var numbers = lines[0].Split(',');
            lines.RemoveAt(0);

            var boards = fillBoards(lines);


            foreach (var number in numbers)
            {
                var num = int.Parse(number);
                if (boards.Count() == 1)
                {
                    var result = boards[0].processNumber(num);
                    if (result != 0)
                    {
                        return (num * result).ToString();
                    }
                }
                boards.RemoveAll(b => b.processNumber(num) != 0);
            }
            return "Whoops I shouldn't be here!!";
        }
    }

    public class Day5
    {

        List<Tuple<Vector2, Vector2>> getVectors(string input, bool includeDiagonals)
        {
            Regex vecRegex = new Regex(@"(\d+),(\d+) -> (\d+),(\d+)", RegexOptions.Compiled);
            var matches = vecRegex.Matches(input);
            var result = new List<Tuple<Vector2, Vector2>>();
            foreach (Match match in matches)
            {
                var g = match.Groups;
                var vec1 = new float[2] { float.Parse(g[1].ToString()), float.Parse(g[2].ToString()) };
                var vec2 = new float[2] { float.Parse(g[3].ToString()), float.Parse(g[4].ToString()) };
                if (includeDiagonals || (vec1[0] == vec2[0] || vec1[1] == vec2[1]))
                {
                    result.Add(new Tuple<Vector2, Vector2>(new Vector2(vec1), new Vector2(vec2)));
                }
            }
            return result;
        }

        List<int> generateGrid(List<Tuple<Vector2, Vector2>> vectors)
        {
            var width = (int)vectors.Max(vec => Math.Max(vec.Item1.X, vec.Item2.X)) + 1;
            var height = (int)vectors.Max(vec => Math.Max(vec.Item1.Y, vec.Item2.Y)) + 1;
            var grid = new List<int>(new int[width * height]);
            foreach (var vec in vectors)
            {
                var minX = Math.Min(vec.Item1.X, vec.Item2.X);
                var minY = Math.Min(vec.Item1.Y, vec.Item2.Y);
                var maxX = Math.Max(vec.Item1.X, vec.Item2.X);
                var maxY = Math.Max(vec.Item1.Y, vec.Item2.Y);
                if (minX == maxX)
                {
                    for (int y = (int)minY; y <= maxY; y++)
                    {
                        grid[(int)minX + y * width]++;
                    }
                }
                else if (minY == maxY)
                {
                    for (int x = (int)minX; x <= maxX; x++)
                    {
                        grid[x + (int)minY * width]++;
                    }
                }
                else
                {
                    var ascending = vec.Item1.X == minX ? vec.Item1.Y < vec.Item2.Y : vec.Item2.Y < vec.Item1.Y;
                    var y = vec.Item1.X == minX ? vec.Item1.Y : vec.Item2.Y;
                    for (int x = (int)minX; x <= maxX; x++)
                    {
                        grid[x + (int)y * width]++;
                        if (ascending)
                        {
                            y++;
                        }
                        else
                        {
                            y--;
                        }
                    }
                }
            }
            return grid;
        }
        public string solve1(string input)
        {
            var vectors = getVectors(input, false);
            var grid = generateGrid(vectors);
            return grid.FindAll(count => count > 1).Count().ToString();
        }
        public string solve2(string input)
        {
            var vectors = getVectors(input, true);
            var grid = generateGrid(vectors);
            return grid.FindAll(count => count > 1).Count().ToString();
        }
    }

    public class Day6
    {
        UInt64 simulateReproduction(List<UInt64> FishBodyClocks, int days)
        {
            var bodyClockCounts = Enumerable.Repeat<UInt64>(0, 9).ToList();
            foreach (var bodyClock in FishBodyClocks)
            {
                bodyClockCounts[(int)bodyClock]++;
            }

            for (var day = 0; day < days; day++)
            {
                var spawned = bodyClockCounts[0];
                bodyClockCounts.RemoveAt(0);
                bodyClockCounts.Add(spawned);
                bodyClockCounts[6] += spawned;
            }
            return bodyClockCounts.Aggregate((a, b) => a + b);
        }
        public string solve1(string input)
        {
            var fishBodyClocks = input.Split(',').Select(UInt64.Parse).ToList();
            return simulateReproduction(fishBodyClocks, 80).ToString();
        }

        public string solve2(string input)
        {
            var fishBodyClocks = input.Split(',').Select(UInt64.Parse).ToList();
            return simulateReproduction(fishBodyClocks, 256).ToString();
        }
    }
}
