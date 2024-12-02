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
}
