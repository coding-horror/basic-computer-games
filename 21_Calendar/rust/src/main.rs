use std::io::stdin;

const WIDTH: usize = 64;
const DAYS_WIDTH: usize = WIDTH / 8;
const MONTH_WIDTH: usize = WIDTH - (DAYS_WIDTH * 2);
const DAY_NUMS_WIDTH: usize = WIDTH / 7;

const DAYS: [&str; 7] = [
    "SUNDAY",
    "MONDAY",
    "TUESDAY",
    "WEDNESDAY",
    "THURSDAY",
    "FRIDAY",
    "SATURDAY",
];

fn main() {
    println!("\n\t\t CALENDAR");
    println!("CREATIVE COMPUTING MORRISTOWN, NEW JERSEY\n");

    let (starting_day, leap_year) = prompt();
    let (months, total_days) = get_months_and_days(leap_year);

    let mut days_passed = 0;
    let mut current_day_index = DAYS.iter().position(|d| *d == starting_day).unwrap();

    for (month, days) in months {
        print_header(month, days_passed, total_days - days_passed);
        print_days(&mut current_day_index, days);
        days_passed += days as u16;
        println!("\n");
    }
}

fn prompt() -> (String, bool) {
    let mut day = String::new();

    loop {
        println!("\nFirst day of the year?");
        if let Ok(_) = stdin().read_line(&mut day) {
            day = day.trim().to_uppercase();
            if DAYS.contains(&day.as_str()) {
                break;
            } else {
                day.clear();
            }
        }
    }

    let mut leap = false;

    loop {
        println!("Is this a leap year?");
        let mut input = String::new();
        if let Ok(_) = stdin().read_line(&mut input) {
            match input.to_uppercase().trim() {
                "Y" | "YES" => {
                    leap = true;
                    break;
                }
                "N" | "NO" => break,
                _ => (),
            }
        }
    }

    println!();
    (day, leap)
}

fn get_months_and_days(leap_year: bool) -> (Vec<(String, u8)>, u16) {
    let months = [
        "JANUARY",
        "FEBUARY",
        "MARCH",
        "APRIL",
        "MAY",
        "JUNE",
        "JULY",
        "AUGUST",
        "SEPTEMBER",
        "OCTOBER",
        "NOVEMBER",
        "DECEMBER",
    ];

    let mut months_with_days = Vec::new();
    let mut total_days: u16 = 0;

    for (i, month) in months.iter().enumerate() {
        let days = if i == 1 {
            if leap_year {
                29u8
            } else {
                28
            }
        } else if if i < 7 { (i % 2) == 0 } else { (i % 2) != 0 } {
            31
        } else {
            30
        };

        total_days += days as u16;
        months_with_days.push((month.to_string(), days));
    }

    (months_with_days, total_days)
}

fn print_between(s: String, w: usize, star: bool) {
    let s = format!(" {s} ");
    if star {
        print!("{:*^w$}", s);
        return;
    }
    print!("{:^w$}", s);
}

fn print_header(month: String, days_passed: u16, days_left: u16) {
    print_between(days_passed.to_string(), DAYS_WIDTH, true);
    print_between(month.to_string(), MONTH_WIDTH, true);
    print_between(days_left.to_string(), DAYS_WIDTH, true);
    println!();

    for d in DAYS {
        let d = d.chars().nth(0).unwrap();
        print_between(d.to_string(), DAY_NUMS_WIDTH, false);
    }
    println!();

    println!("{:*>WIDTH$}", "");
}

fn print_days(current_day_index: &mut usize, days: u8) {
    let mut current_date = 1u8;

    print!("{:>w$}", " ", w = DAY_NUMS_WIDTH * *current_day_index);

    for _ in 1..=days {
        print_between(current_date.to_string(), DAY_NUMS_WIDTH, false);

        if ((*current_day_index + 1) % 7) == 0 {
            *current_day_index = 0;
            println!();
        } else {
            *current_day_index += 1;
        }

        current_date += 1;
    }
}
