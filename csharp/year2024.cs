using System.Collections;
using System.Diagnostics;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Year2024
{
    public class Day1
    {
        public string solve1(string input)
        {
            // Sum the distances between sorted list of numbers
            var lines = input.Split('\n').Where(l => l.Length > 0);
            List<int> left = new();
            List<int> right = new();
            foreach (var line in lines)
            {
                var both = line.Split("   ");
                Debug.Assert(both.Length == 2);
                left.Add(int.Parse(both[0]));
                right.Add(int.Parse(both[1]));
            }

            left.Sort();
            right.Sort();

            var sum = 0;
            for (int i = 0; i < left.Count; i++)
            {
                sum += Math.Max(left[i], right[i]) - Math.Min(left[i], right[i]);
            }
            return sum.ToString();
        }

        public string solve2(string input)
        {
            // Sum the numbers on the left multiplied by the number of times they appear
            // in the right list
            var lines = input.Split('\n').Where(l => l.Length > 0);
            List<int> left = new();
            List<int> right = new();
            foreach (var line in lines)
            {
                var both = line.Split("   ");
                Debug.Assert(both.Length == 2);
                left.Add(int.Parse(both[0]));
                right.Add(int.Parse(both[1]));
            }

            var sum = 0;
            for (int i = 0; i < left.Count; i++)
            {
                sum += left[i] * right.Where(x => x == left[i]).Count();
            }
            return sum.ToString();
        }
    }

    public class Day2
    {
        static bool is_safe(List<int> levels)
        {
            bool increasing = levels[0] < levels[1];
            for (int i = 1; i < levels.Count; i++)
            {
                var current = levels[i];
                var last = levels[i - 1];
                var diff = current - last;
                var abs_diff = Math.Abs(diff);
                if (increasing && diff <= 0 || !increasing && diff >= 0 || abs_diff < 1 || abs_diff > 3)
                {
                    return false;
                }
            }
            return true;
        }
        public string solve1(string input)
        {
            // Input are lists of numbers, we need to count how many are "safe"
            // meaning they only increase or decrease, and only ever by 1 to 3 inclusive
            var lines = input.Split('\n').Where(s => s.Length > 0);
            return lines.Where(x => is_safe(x.Split(' ').Select(int.Parse).ToList())).Count().ToString();
        }

        public static string solve2(string input)
        {
            // as above but check each variation with 1 missing number
            var lines = input.Split('\n').Where(s => s.Length > 0).ToList();
            var count = 0;
            foreach (var line in lines)
            {
                var list = line.Split(' ').Select(int.Parse).ToList();
                if (is_safe(list))
                {
                    count++;
                }
                else
                {
                    for (int i = 0; i < list.Count(); i++)
                    {
                        var temp_list = new List<int>(list);
                        temp_list.RemoveAt(i);
                        if (is_safe(temp_list))
                        {
                            count++;
                            break;
                        }
                    }
                }
            }
            return count.ToString();
        }
    }

    public class Day3
    {
        public string solve1(string input)
        {
            // find "instructions" matching the pattern `mul(X,Y)`
            // and sum the results of multiplying X*Y for those
            // Surely must be regex...
            string pattern = @"mul\((\d+),(\d+)\)";
            var reg = new Regex(pattern);
            var matches = reg.Matches(input);
            var sum = 0;
            foreach (Match match in matches)
            {
                var x = int.Parse(match.Groups[1].Value);
                var y = int.Parse(match.Groups[2].Value);
                sum += x * y;
            }
            return sum.ToString();
        }

        public string solve2(string input)
        {
            // Now there's interspersed do/don't instructions to
            // enable/disable instructions. Seems simplest to just
            // strip any disabled instructions then use day 1's solution
            var enable_index = 0;
            var disable_index = input.IndexOf("don't()");
            while(disable_index != -1)
            {
                enable_index = input.IndexOf("do()", disable_index);
                if (enable_index == -1)
                {
                    enable_index = input.Length;
                }
                input = input.Remove(disable_index, enable_index - disable_index);
                disable_index = input.IndexOf("don't()");
            }
            return solve1(input);
        }
    }
}
