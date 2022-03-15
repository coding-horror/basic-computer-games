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
        let mut date =DATE{
            month:raw_date[0],
            day:raw_date[1],
            year:raw_date[2],
            day_of_week: 0 
        }; 
        date.update_day_of_week();
        //return date
        return date
    }

    /**
     * caluclates the day of the week (1-7)
     * uses the methodology found here: https://cs.uwaterloo.ca/~alopez-o/math-faq/node73.html
     */
    fn update_day_of_week(&mut self) {
        let day = self.day as isize;
        let month = self.month as isize;
        let year = self.year as isize;
        let century = year/100;
        let year_of_century = year - century*100;
        let weekday; //as 0-6
        if self.month <= 2 { //if jan or feb
            weekday = (day + (2.6 * ((month+10) as f64)-0.2)as isize - 2*century + (year_of_century-1) + (year_of_century-1)/4 + century/4) % 7;
        } else {
            weekday = (day + (2.6 * ((month-2)  as f64)-0.2)as isize - 2*century + year_of_century     + year_of_century/4     + century/4) % 7;
        }
        self.day_of_week=(weekday+1) as u32; //weekday as 1-7
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
    fn calc_day_value(&self) -> u32 {
        return (self.year*12 + self.month)*31 + self.day;
    }

    /**
     * returns true if passed date is before this date, false otherwise
     */
    fn is_after(&self,other:&DATE) -> bool {
        if self.calc_day_value() > other.calc_day_value() {
            return true;
        }
        else {
            return false;
        }
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
    today_value = today_date.calc_day_value();
    other_day_value = other_date.calc_day_value();

    //print the other date in a nice format
    println!(
        "{} {} A {}",
        other_date.to_string(),
        { //use proper tense of To Be
            if today_value < other_day_value {"WILL BE"}
            else if today_value == other_day_value {other_is_today=true;"IS A"}
            else {"WAS A"}
        },
        other_date.day_of_week,
    );

    //do nothing if both days are the same, or if the other date is after the first
    if other_is_today || other_date.is_after(&today_date) {
        return;
    } 

    //create date representing the difference between the two dates
    delta_date = DATE::new( //OVERFLOW ERROR, FIX LATER
        today_date.month - other_date.month,
        today_date.day - other_date.day,
        today_date.year - other_date.year,
        0
    );

    //print happy birthday message
    if delta_date.month == 0 && delta_date.day == 0 {
        println!("***HAPPY BIRTHDAY***");
    }

    //print report
    print!("
                      \tYEARS\tMONTHS\tDAYS
                      \t-----\t------\t----
    ");
    println!("YOUR AGE (IF BIRTHDATE)\t{}\t{}\t{}", delta_date.year,delta_date.month,delta_date.day);



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
