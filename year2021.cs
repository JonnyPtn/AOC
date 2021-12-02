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
}
