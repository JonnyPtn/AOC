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
        System.Collections.BitArray getMostCommonBits(string[] input, bool reverse)
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
            var mostCommonBits = new System.Collections.BitArray(bitCounts.Count(), false);
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
            var lines = new List<string> (input.Split('\n'));
            lines.RemoveAll(line => line.Length == 0);
            var o2lines = new List<string>(lines);
            var co2lines = new List<string>(lines);

            int index = 0;
            while(o2lines.Count() > 1)
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
            return (Convert.ToUInt32(o2lines[0],2) * Convert.ToUInt32(co2lines[0],2)).ToString(); ;
        }
    }
}
