using System.Drawing;

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

    public class Day2
    {
        public string solve1(string input)
        {
            // find area of cuboid - 2*l*w + 2*w*h + 2*h*l
            var dimensions = input.Split("\n").Where(s => !string.IsNullOrEmpty(s));
            int total = 0;
            foreach (var dim in dimensions)
            {
                var lwh = dim.Split('x');
                var l = int.Parse(lwh[0]);
                var w = int.Parse(lwh[1]);
                var h = int.Parse(lwh[2]);
                var area = 2 * l * w + 2 * w * h + 2 * h * l;
                total += area + new[] { l * w, w * h, h * l }.Min();
            }

            return total.ToString();
        }

        public string solve2(string input)
        {
            //The ribbon required to wrap a present is the shortest distance around its sides
            var dimensions = input.Split("\n").Where(s => !string.IsNullOrEmpty(s));
            var total = 0;
            foreach (var dim in dimensions)
            {
                var lwh = dim.Split('x');
                var l = int.Parse(lwh[0]);
                var w = int.Parse(lwh[1]);
                var h = int.Parse(lwh[2]);
                List<int> values = new() { l, w, h };
                // ribbon required for the perfect bow is equal to the cubic feet of volume of the present
                var bowLength = values.Aggregate((a, b) => a * b);
                values.Remove(values.Max());
                total += values[0] * 2 + values[1] * 2 + bowLength;
            }

            return total.ToString();
        }
    }

    public class Day3
    {
        public HashSet<Point> getVisitedHouses(string input)
        {
            // ^ v < > directions, count locations we hit
            Point position = new();
            HashSet<Point> visitedHouses = new() { position };
            foreach (var direction in input)
            {
                switch (direction)
                {
                    case '^':
                        position.Y++;
                        break;
                    case 'v':
                        position.Y--;
                        break;
                    case '<':
                        position.X--;
                        break;
                    case '>':
                        position.X++;
                        break;
                }
                visitedHouses.Add(position);
            }

            return visitedHouses;
        }
        public string solve1(string input)
        {
            return getVisitedHouses(input).Count.ToString();
        }

        public string solve2(string input)
        {
            // Now there's two santas, taking alternate turns,
            // so split the input accordingly then count and combine
            var a = new string(input.Where((c,i) => i % 2 == 0).ToArray());
            var b = new string(input.Where((c,i) => i % 2 != 0).ToArray());
            var av = getVisitedHouses(a);
            var bv = getVisitedHouses(b);
            av.UnionWith(bv);
            return av.Count().ToString();
        }
    }
}