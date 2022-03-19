use std::io::{self, stdout, Write};

struct DATE {
    month: u32,
    day: u32,
    year:u32,
    day_of_week:u32,
}
impl DATE {
    /**
     * create new date with given paramets
     */
    fn new(month:u32,day:u32,year:u32,day_of_week:u32) -> DATE {
        return DATE { month: month, day: day, year: year, day_of_week: day_of_week };
    }
    /**
     * create date from user input
     */
    fn new_from_input(prompt:&str) -> DATE {
        //DATA
        let mut raw_date: Vec<u32>;
        //get date
        //input loop
        loop {
            //get user input,
            raw_date = get_str_from_user(prompt)
            .split(',')//split it up by ',''s
            .filter_map(|s| s.parse::<u32>().ok()).collect();//convert it to numbers, ignore things that fail
            
            //if they didn't give enough data
            if raw_date.len() == 3 { //is it long enough? (3 elements)
                //are each ones valid things?
                if (1..=12).contains(&raw_date[0]) { //valid month
                    if (1..=31).contains(&raw_date[1]) { //valid day
                        break;
                    }
                }
            }
            //otherwise, print error message and go again
            println!("Invalid date, try again!");
        }

        //create date
        let mut date =DATE::new(raw_date[0], raw_date[1], raw_date[2], 0);
        date.update_day_of_week();
        //return date
        return date
    }
    /**
     * create a new date from a number of days
     */
    fn new_from_days(days:u32) -> DATE{
        let mut days_remaining = days;
        let d;
        let m;
        let y;

        //get the years
        y=(days_remaining as f64 / 365.25) as u32;
        //deduct
        days_remaining = (days_remaining as f64 % 365.25) as u32;

        //get months
        m=(days_remaining as f64 / 30.437) as u32;
        //deduct
        days_remaining = (days_remaining as f64 % 30.437) as u32;

        //get days
        d = days_remaining;

        //return new date
        return DATE::new(m, d, y, 0);
    }

    /**
     * caluclates the day of the week (1-7)
     * uses the methodology found here: https://cs.uwaterloo.ca/~alopez-o/math-faq/node73.html
     */
    fn update_day_of_week(&mut self) {
        //DATA
        let day = self.day as isize;
        let month = self.month as isize;
        let year = self.year as isize;
        let century = year/100;
        let year_of_century = year - century*100;
        let weekday; //as 0-6

        //calculate weekday
        if self.month <= 2 { //if jan or feb
            weekday = (day + (2.6 * ((month+10) as f64)-0.2)as isize - 2*century + (year_of_century-1) + (year_of_century-1)/4 + century/4) % 7;
        } else {
            weekday = (day + (2.6 * ((month-2)  as f64)-0.2)as isize - 2*century + year_of_century     + year_of_century/4     + century/4) % 7;
        }

        //update weekday
        self.day_of_week=(weekday+1) as u32; //weekday as 1-7
    }

    /**
     * return the string for the weekday
     */
    fn day_of_week_as_string(&self) -> String {
        match self.day_of_week {
            1 => {return String::from("SUNDAY")},
            2 => {return String::from("MONDAY")},
            3 => {return String::from("TUESDAY")},
            4 => {return String::from("WEDNESDAY")},
            5 => {return String::from("THURSDAY")},
            6 => {
                if self.day == 13 {return String::from("FRIDAY THE THIRTEENTH---BEWARE!")}
                else {return String::from("FRIDAY")}
            },
            7 => {return String::from("SATURDAY")}, 
            _ => {return String::from("")},
        }
    }

    /**
     * is the year a leap_year
     */
    fn _is_leap_year(&self) -> bool{
        if self.year % 4 != 0 {
            return false;
        } else if self.year %100 != 0 {
            return true;
        } else if self.year % 400 != 0 {
            return false;
        } else {
            return true;
        }
    }

    /**
     * calculates the day value, number of days the date represents
     */
    fn calc_days(&self) -> u32 {
        return (self.year as f64 * 365.25)as u32 + self.month*30 + self.day + self.month/2;
    }

    /**
     * calculates the time difference between self and the passed date,
     */
    fn time_since(&self, other: &DATE) -> Option<DATE> {
        //DATA
        // /*
        let diff = self.calc_days()as i32 - other.calc_days()as i32;
        if diff < 0 { 
            return None;
        } else {
            return Some(DATE::new_from_days(diff as u32));
        }
    }

    /**
     * formats the date in a different format, used for time table
     */
    fn format_ymd(&self, spacer:&str) -> String {
        return format!(
            "{}{}{}{}{}",
            self.year, spacer,
            self.month, spacer,
            self.day,
        );
    }
}
impl ToString for DATE {
    fn to_string(&self) -> String {
        return format!("{}/{}/{}",self.month,self.day,self.year);
    }
}

fn main() {
    //DATA
    let today_date: DATE;
    let other_date:DATE;
    let delta_date:DATE; //represents the difference between the two dates
    let today_value;
    let other_day_value;
    let mut other_is_today = false;

    //print welcome
    welcome();
    
    //get todays date
    today_date = DATE::new_from_input("ENTER TODAY'S DATE IN THE FORM: 3,24,1979  ");
    //check todays date
    if today_date.year < 1582 {
        println!("NOT PREPARED TO GIVE DAY OF WEEK PRIOR TO MDLXXXII.");
        return;
    }

    println!();

    //get other date
    other_date = DATE::new_from_input("ENTER DAY OF BIRTH (OR OTHER DAY OF INTEREST) (like MM,DD,YYYY)");
    //check other date
    if other_date.year < 1582 {
        println!("NOT PREPARED TO GIVE DAY OF WEEK PRIOR TO MDLXXXII.");
        return;
    }

    //do some calculations
    today_value = today_date.calc_days();
    other_day_value = other_date.calc_days();

    //print the other date in a nice format
    println!(
        "{} {} A {}",
        other_date.to_string(),
        { //use proper tense of To Be
            if today_value < other_day_value {"WILL BE"}
            else if today_value == other_day_value {other_is_today=true;"IS"}
            else {"WAS"}
        },
        other_date.day_of_week_as_string(),
    );

    //end if both days are the same
    if other_is_today {
        return;
    } 

    //create date representing the difference between the two dates
    delta_date = today_date.time_since(&other_date).unwrap();

    //print happy birthday message
    if delta_date.month == 0 && delta_date.day == 0 {
        println!("***HAPPY BIRTHDAY***");
    }

    //print report
    println!("
                      \tYEARS\tMONTHS\tDAYS
                      \t-----\t------\t----"
    );
    println!("YOUR AGE (IF BIRTHDATE)\t{}", delta_date.format_ymd("\t"));
    
    //how much have they slept
    println!(
        "YOU HAVE SLEPT\t\t{}",
        DATE::new_from_days( (0.35 * delta_date.calc_days() as f64) as u32).format_ymd("\t"), //35% of their life
    );

    //how much they have eaten
    println!(
        "YOU HAVE EATEN\t\t{}",
        DATE::new_from_days( (0.17 * delta_date.calc_days() as f64) as u32).format_ymd("\t"), //17% of their life
    );

    //how much they have worked
    println!(
        "YOU HAVE {}\t{}",
        {
            if delta_date.year <= 3 {"PLAYED"}
            else if delta_date.year <= 9 {"PLAYED/STUDIED"}
            else {"WORKED/PLAYED"}
        },
        DATE::new_from_days( (0.23 * delta_date.calc_days() as f64) as u32).format_ymd("\t"), //23% of their life
    );
    //how much they have relaxed
    println!(
        "YOU HAVE RELAXED\t{}",
        DATE::new_from_days( ( (1.0-0.35-0.17-0.23) * delta_date.calc_days() as f64) as u32).format_ymd("\t"), //remaining% of their life
    );
    //when they can retire
    println!(
        "YOU MAY RETIRE IN\t{}",
        DATE::new(other_date.month, other_date.day, other_date.year + 65, 0).time_since(&today_date).unwrap().format_ymd("\t")
    );
}


/**
 * print welome message
 */
fn welcome() {
    println!("
                               WEEKDAY
              CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY
    
    
    
    WEEKDAY IS A COMPUTER DEMONSTRATION THAT
    GIVES FACTS ABOUT A DATE OF INTEREST TO YOU.
    ");
}

/**
 * gets a string from user input
 */
fn get_str_from_user(prompt:&str) -> String {
    //DATA
    let mut raw_input = String::new();

    //print prompt
    print!("{}",prompt);
    //flust std out //allows prompt to be on same line as input
    stdout().flush().expect("failed to flush");

    //get input and trim whitespaces
    io::stdin().read_line(&mut raw_input).expect("Failed to read input");

    //return raw input
    return raw_input.trim().to_string();
}
