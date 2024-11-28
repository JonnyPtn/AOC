using System.Collections;
using System.Numerics;
using System.Text.RegularExpressions;

namespace Year2015
{
    public class Day1
    {
        public string solve1(string input)
        {
            // "(" is up, ")" is down, so just sum them
            var up = input.Count(f => f == '(');
            var down = input.Count(f => f == ')');
            return (up - down).ToString();
        }
        public string solve2(string input)
        {
            // Find the first time the floor goes negative
            var floor = 0;
            for (var i = 0; i < input.Length; i++)
            {
                var c = input[i];
                if (c == '(')
                {
                    floor++;
                }
                else
                {
                    floor--;
                }
                if (floor < 0)
                {
                    return (i + 1).ToString();
                }
            }
            return input.Length.ToString();
        }
    }
}