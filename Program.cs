
// The client we'll use for requesting challenge input and submitting challenge output
HttpClient client = new HttpClient();

// The base Uri for the AOC website
Uri base_uri = new Uri("https://adventofcode.com/");

// For authentication we just re-use the cookie from your browser session.
// Once you've logged in to the website you should find this in your browser settings
// then set it to an environment variable called AOC_COOKIE
var cookie = Environment.GetEnvironmentVariable("AOC_COOKIE");
if (cookie != null)
{
    client.DefaultRequestHeaders.Add("Cookie", $"session={cookie}");
}
else
{
    Console.Error.WriteLine("AOC_COOKIE environment variable not set, authentication failed");
    return;
}

// Fetch the input for a given date
string fetchInput(DateTime challengeDate)
{
    // First check if we have cached input
    var cacheFile = new FileInfo($".input/{challengeDate.Year}/{challengeDate.Day}");
    if (File.Exists(cacheFile.FullName))
    {
        return File.ReadAllText(cacheFile.FullName);
    }
    // If there's no cache then fetch from the website and store in the cache
    else
    {
        Console.WriteLine($"Cached input for {challengeDate.Year} day {challengeDate.Day} not found, fetching from website");
        var challenge_uri = new Uri(base_uri, $"{challengeDate.Year}/day/{challengeDate.Day}/input");
        var response = client.GetAsync(challenge_uri).Result;
        var input = response.Content.ReadAsStringAsync().Result;
        if (cacheFile.Directory != null)
        {
            cacheFile.Directory.Create();
            File.WriteAllText(cacheFile.FullName, input);
        }
        else
        {
            Console.Error.WriteLine("Failed to cache input, please fix to prevent API spam!");
        }
        return input;
    }
}

// Send the output to the form and check the result
void checkOutput(DateTime challengeDate, int part, string output)
{
    var challenge_uri = new Uri(base_uri, $"{challengeDate.Year}/day/{challengeDate.Day}/answer");
    var formContent = new FormUrlEncodedContent(new[]
    {
        new KeyValuePair<string, string>("level", $"{part}"),
        new KeyValuePair<string, string>("answer", $"{output}")
    });
    var response = client.PostAsync(challenge_uri, formContent).Result;
    var responseText = response.Content.ReadAsStringAsync().Result;

    // We do some crude parsing of the output with hardcoded text indices to look for
    if (responseText.Contains("You gave an answer too recently"))
    {
        var firstIndex = responseText.IndexOf("You");
        var lastIndex = responseText.IndexOf("wait.") + 5;
        Console.WriteLine(responseText.Substring(firstIndex, lastIndex - firstIndex));
    }
    else if (responseText.Contains("That's not the right answer"))
    {
        var firstIndex = responseText.IndexOf("<code>") + 6;
        var lastIndex = responseText.IndexOf("</code>");
        var guess = responseText.Substring(firstIndex, lastIndex - firstIndex);
        Console.WriteLine($"{challengeDate.Year} day {challengeDate.Day} challenge failed, you guessed: {guess}");
    }
    else if (responseText.Contains("That's the right answer!") || responseText.Contains("Did you already complete it?"))
    {
        Console.WriteLine($"{challengeDate.Year} day {challengeDate.Day} challenge part {part} completed!");
    }
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
                var input = fetchInput(challengeDate);
                object? instance = Activator.CreateInstance(classType);
                if (instance != null)
                {
                    var output = (string?)solver.Invoke(instance, new object[] { input });
                    if (output != null)
                    {
                        checkOutput(challengeDate, part, output);
                    }
                }
            }
        }
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
