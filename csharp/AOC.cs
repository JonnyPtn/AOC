﻿namespace year2021;

static class AOC
{
    // The client we'll use for requesting challenge input and submitting challenge output
    static HttpClient client = new HttpClient();

    // Root folder we'll cache things under
    const string cache_root = ".cache";

    // The base Uri for the AOC website
    static Uri base_uri = new Uri("https://adventofcode.com/");

    // Get and cache the input
    public static string input(int year, int day)
    {
        // First check if we have cached input
        var cacheFile = new FileInfo($"{cache_root}/{year}/{day}/input");
        if (File.Exists(cacheFile.FullName))
        {
            return File.ReadAllText(cacheFile.FullName);
        }
        // If there's no cache then fetch from the website and store in the cache
        else
        {
            if (!client.DefaultRequestHeaders.Contains("Cookie"))
            {
                client.DefaultRequestHeaders.Add("Cookie", $"session={Environment.GetEnvironmentVariable("AOC_COOKIE")}");
            }
            Console.WriteLine($"Cached input for {year} day {day} not found, fetching from website");
            var challenge_uri = new Uri(base_uri, $"{year}/day/{day}/input");
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

    public static void answer(int year, int day, int part, string answer)
    {
        // Check if we've already succesfully answered
        var cacheFile = new FileInfo($"{cache_root}/{year}/{day}/answer{part}");
        if (cacheFile.Exists)
        {
            var cachedAnswer = File.ReadAllText(cacheFile.FullName);
            if (cachedAnswer == answer)
            {
                Console.WriteLine($"Correct answer already cached: {answer}");
                return;
            }
            else if (cachedAnswer == "Unknown")
            {
                Console.WriteLine($"Challenge already completed, you gave {answer} as the answer but we have no cache to verify with");
                return;
            }
        }

        // Also check if we already gave this as an incorrect answer
        var incorrectFile = new FileInfo($"{cache_root}/{year}/{day}/incorrect{part}");
        if (incorrectFile.Exists)
        {
            var incorrectAnswer = File.ReadAllText(incorrectFile.FullName);
            if (incorrectAnswer == answer)
            {
                Console.WriteLine($"Incorrect answer already tried: {answer}");
                return;
            }
        }

        if (!client.DefaultRequestHeaders.Contains("Cookie"))
        {
            client.DefaultRequestHeaders.Add("Cookie", $"session={Environment.GetEnvironmentVariable("AOC_COOKIE")}");
        }
        var challenge_uri = new Uri(base_uri, $"{year}/day/{day}/answer");
        var formContent = new FormUrlEncodedContent(new[]
        {
            new KeyValuePair<string, string>("level", $"{part}"),
            new KeyValuePair<string, string>("answer", $"{answer}")
        });
        var response = client.PostAsync(challenge_uri, formContent).Result;
        var responseText = response.Content.ReadAsStringAsync().Result;

        // We do some crude parsing of the output with hardcoded text indices to look for
        if (responseText.Contains("You gave an answer too recently"))
        {
            var firstIndex = responseText.IndexOf("You");
            var lastIndex = responseText.IndexOf("wait.") + 5;
            Console.WriteLine($"{responseText.Substring(firstIndex, lastIndex - firstIndex)}, attempted answer: {answer}");
        }
        else if (responseText.Contains("That's not the right answer"))
        {
            Console.WriteLine($"{year} day {day} challenge failed, incorrect answer: {answer}");
            File.WriteAllText(incorrectFile.FullName, answer);
        }
        else if (responseText.Contains("That's the right answer!"))
        {
            Console.WriteLine($"{year} day {day} challenge part {part} completed!");
            File.WriteAllText(cacheFile.FullName, answer);
        }
        else if (responseText.Contains("Did you already complete it?"))
        {
            Console.WriteLine($"{year} day {day} part {part} already completed, but no cached answer to verify against");
            File.WriteAllText(cacheFile.FullName, "Unknown");
        }
        else
        {
            Console.WriteLine("Confusing response from website...");
        }
    }
}