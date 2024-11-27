
// For authentication we just re-use the cookie from your browser session.
// Once you've logged in to the website you should find this in your browser settings
// then set it to an environment variable called AOC_COOKIE

using System.Net;
using year2021;

var cookie = Environment.GetEnvironmentVariable("AOC_COOKIE");
if (cookie == null)
{
    Console.Error.WriteLine("AOC_COOKIE environment variable not set, authentication failed");
    return;
}

// Try to solve the challenge for a given date
void trySolveChallenge(DateTime challengeDate)
{
    // Next check if we've actually got a method to solve this day's challenge
    // expected function syntax is Solve{year}.Solve{day}.solve{part}()
    var className = $"Year{challengeDate.Year}.Day{challengeDate.Day}";
    var classType = Type.GetType(className);
    if (classType != null)
    {
        for (int part = 1; part <= 2; ++part)
        {
            var solverName = $"solve{part}";
            var solver = classType.GetMethod(solverName);
            if (solver != null)
            {
                // We have a method, so fetch the input, run the solver and check the outpu
                Console.WriteLine($"Solving {challengeDate.Year} day {challengeDate.Day} part {part} challenge... ");
                var input = AOC.input(challengeDate.Year, challengeDate.Day);
                object? instance = Activator.CreateInstance(classType);
                if (instance != null)
                {
                    var output = (string?)solver.Invoke(instance, new object[] { input });
                    if (output != null)
                    {
                        AOC.answer(challengeDate.Year, challengeDate.Day, part, output);
                    }
                }
            }
            else
            {
                Console.WriteLine($"{solverName} not attempted");
            }
        }
    }
    else
    {
        Console.WriteLine($"{className} not attempted");
    }
}

// If we've been given a specific challenge to solve then just do that
// args expecting year followed by day
if (args.Length == 2)
{
    trySolveChallenge(new DateTime(int.Parse(args[0]), 12, int.Parse(args[1])));
}
// Otherwise just try and solve every challenge
else
{
    var firstYear = 2015;
    var now = DateTime.Now;
    for (var year = firstYear; year <= now.Year; ++year)
    {
        // There's a challenge for each of the first 25 days in december
        for (var day = 1; day <= 25; ++day)
        {
            // Challenges are unlocked in EST so convert to that
            var est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            var estNow = TimeZoneInfo.ConvertTime(DateTime.Now, est);
            var challengeUnlockTime = new DateTime(year, 12, day);
            if (estNow < challengeUnlockTime)
            {
                Console.WriteLine($"Challenge for {year} day {day} unlocking in {challengeUnlockTime - estNow}");
                break;
            }

            trySolveChallenge(challengeUnlockTime);
        }
    }
}
