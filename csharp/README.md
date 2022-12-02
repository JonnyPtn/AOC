# AOC
A program for solving [Advent of code](https://adventofcode.com/) challenges

It will automatically download the input for the challenge and verify the output.
Every year of the challenge is supported. To add a solver for a particular challenge create a function using the Year as the namespace name, day as the class name and call the method solve1 or solve2 for part 1 or 2 of the challenge, e.g:
```csharp
namespace Year2021
{
  public class Day1
  {
    public string solve1(string input)
    {
      // Your solution here...
    }
  }
}
```

This function must take a string as a parameter for the input, and return a string for the output of the solution

## Requirements
- [dotnet SDK 6+](https://dotnet.microsoft.com/download)

## Building and running
Open a terminal in the root folder and run `dotnet build` or `dotnet run` 

## Authentication
Because the website requires authentication to retrieve your input and submit your output, This uses your existing cookie for authentication.
It expects an environment variable called `AOC_COOKIE` to be set to the session ID, which you can get from your browser after logging in to the website
