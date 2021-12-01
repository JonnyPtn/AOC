
// The client we'll use for requesting challenge input and submitting challenge output
HttpClient client = new HttpClient();

// The base Uri for the AOC website
Uri base_uri = new Uri("https://adventofcode.com/");

// Advent of code started in 2015, so go through each year since then until now
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

        // Next check if we've actually got a method to solve this day's challenge
        // expected function syntax is Solve{year}.Solve{day}.solve()
        var className = $"Year{year}.Day{day}";
        var classType = Type.GetType(className);
        if (classType is not null)
        {
            var solverName = "solve";
            var solver = classType.GetMethod(solverName);
            if (solver is not null)
            {
                Console.WriteLine($"Solving {year} day {day} challenge...");

                // First we get the input from the website
                var challenge_uri = new Uri(base_uri, $"{year}/day/{day}");
                var response = await client.GetAsync(challenge_uri);
            }
        }
        //var input_uri = new Uri(base_uri, $"{year}/day/{day}" );
        //var response = client.GetAsync(input_uri).Result;
        //Console.Write(response);
    }
}
