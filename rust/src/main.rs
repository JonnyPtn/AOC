use chrono::{Duration, FixedOffset, Utc, NaiveDate, Datelike};
use reqwest::{blocking::ClientBuilder, Error, header};
use std::{collections::HashMap, env, io::Write};

fn main() -> Result<(), Error> {

    // Set up the client with the cookie first
    let cookie = env::var("AOC_COOKIE").unwrap_or("none".to_string());
    let mut headers = header::HeaderMap::new();
    let cookie_string = format!("session={}",cookie);
    headers.insert(header::COOKIE, header::HeaderValue::from_str(&cookie_string).unwrap());
    let client = ClientBuilder::new().default_headers(headers).cookie_store(true).build().unwrap();

    // Function to get the input for a particular challenge
    let get_input = |day| {
        let path_str = format!(".input/2022/{day}");
        let cache_path = std::path::Path::new(&path_str);
        if cache_path.exists() {
            return std::fs::read_to_string(cache_path).expect("Couldn't read cache file even though it exists");
        }
        else {
            let url = format!("https://adventofcode.com/2022/day/{day}/input");
            let response = client
                .get(&url)
                .send();
        
            let input = response.unwrap().text().unwrap();
            std::fs::create_dir_all(cache_path.parent().unwrap()).expect("Unable to create required cache directory(s)");
            let mut file = std::fs::File::create(cache_path).expect("Unable to create cache file");
            file.write_all(input.as_bytes()).expect("Unable to write to cache file");
            return input;
        }
    };

    // Function to submit the output, returning true if it's correct
    let check_answer = |day, level: i8, answer: &String| {
        let url = format!("https://adventofcode.com/2022/day/{day}/answer");
        let mut params = HashMap::new();
        params.insert("level", level.to_string());
        params.insert("answer",answer.to_string());
        let response = client.post(&url).form(&params).send().unwrap().text().unwrap();
        file.write_all(response.as_bytes()).unwrap();
        if response.contains("That's not the right answer") {
            return true;
        }
        else if response.contains("You gave an answer too recently") {
            eprintln!("Answering too quickly, wait a bit and try again");
            return false;
        }
        else if response.contains("That's the right answer!") || response.contains("Did you already complete it?") {
            return true;
        }
        else {
            eprintln!("Unknown response from submitting answer");
            return false;
        }
    };

    // Map of functions to solve the challenges
    let mut solvers = HashMap::new();
    solvers.insert(1, solve1);

    // Iterate every unlocked day
    let now = Utc::now();
    let est_tz = FixedOffset::west_opt(5*3600).unwrap();
    let est_now = now.with_timezone(&est_tz).date_naive();

    let mut day = NaiveDate::from_ymd_opt(2022,12,1).unwrap();
    while day <= est_now && solvers.contains_key(&day.day()){
        let input = get_input(day.day());

        let mut level = 1;
        let output1 = solvers[&day.day()](&input, level);
        let correct1 = check_answer(day.day(), level, &output1);
        println!("Day {}, level {level} {}: {output1}", day.day(), if correct1 {"right"} else {"wrong"});

        level = 2;
        let output2 = solvers[&day.day()](&input, level);
        let correct2 = check_answer(day.day(),level, &output2);
        println!("Day {}, level {level} {}: {output2}", day.day(), if correct2 {"right"} else {"wrong"});

        day = day + Duration::days(1);
    }
    Ok(())
}

fn solve1(input: &String, level: i8) -> String {
    // Each line is the calorie count for an item
    // Each group of space separated lines are items that belong to an elf
    let mut calories = Vec::new();
    let mut count = 0;
    for line in input.lines() {
        if line.is_empty() || line == input.lines().last().unwrap() {
            calories.push(count);
            count = 0;
        }
        else {
            count += line.parse::<i64>().unwrap();
        }
    }

    if level == 1 {
        // Highest number of calories
        return calories.iter().max().unwrap().to_string();
    }
    else {
        calories.sort();
        let (_, largest) = calories.split_at(calories.len() - 3);
        return largest.iter().sum::<i64>().to_string();
    }
}