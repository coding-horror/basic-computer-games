use std::io::{self, stdout, Write};

struct DATE {
    month: u32,
    day: u32,
    year:u32,
    day_of_week:u32,
}
impl DATE {
    //create one from user input
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
     */
    fn update_day_of_week(&mut self) {
        let month_table:[u32;12] = [0, 3, 3, 6, 1, 4, 6, 2, 5, 0, 3, 5];
        let i1 = (self.year - 1500) / 100;
        let a = i1 * 5 + (i1 + 3)/4;
        let i2 = a - (a/7) * 7;
        let y2 = self.year/100;
        let y3 = self.year - y2*100;
        let a = y3 / 4 + y3 + self.day + month_table[(self.month - 1) as usize] + i2;
        let  b = a - (a/7) * 7;

        //adjust weekday and set it
        if self.month <= 2 && self.is_leap_year() {
            self.day_of_week = b-1;
        } else {
            self.day_of_week = b;
        }
        if self.day_of_week == 0 {self.day_of_week = 7;}

    }

    /**
     * is the year a leap_year
     */
    fn is_leap_year(&self) -> bool{
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
}
fn main() {
    //DATA
    let todays_date: DATE;
    let other_date:DATE;

    //print welcome
    welcome();

    //get todays date
    todays_date = DATE::new_from_input("ENTER TODAY'S DATE IN THE FORM: 3,24,1979  ");
    //check todays date
    if todays_date.year < 1582 {
        println!("NOT PREPARED TO GIVE DAY OF WEEK PRIOR TO MDLXXXII.");
        return;
    }

    //get other date
    other_date = DATE::new_from_input("ENTER DAY OF BIRTH (OR OTHER DAY OF INTEREST) (like MM,DD,YYYY)");
    //check other date
    if other_date.year < 1582 {
        println!("NOT PREPARED TO GIVE DAY OF WEEK PRIOR TO MDLXXXII.");
        return;
    }

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
