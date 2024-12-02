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
}
