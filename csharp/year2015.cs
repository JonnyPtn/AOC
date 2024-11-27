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
    }
}