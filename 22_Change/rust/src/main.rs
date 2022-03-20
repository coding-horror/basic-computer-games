use std::io::{self, stdout, Write};

fn main() {
    //DATA
    let mut cost_in_cents:i16;
    let mut payment_in_cents:i16;
    let mut amount_owed_in_cents:i16;

    let mut hundred:i16;
    let mut fifty:i16;
    let mut twenty:i16;
    let mut ten:i16;
    let mut five:i16;
    let mut one:i16;
    let mut quarters:i16;
    let mut dimes:i16;
    let mut nickles:i16;
    let mut pennies:i16;

    //print welcome message
    welcome();

    //print prompt
    println!("I, YOUR FRIENDLY MICROCOMPUTER, WILL DETERMINE");
    println!("THE CORRECT CHANGE FOR ITEMS COSTING UP TO ${}.00.",i16::MAX/100);

    //game loop 
    loop {
        //get cost of items
        cost_in_cents = get_dollar_value_in_cents_from_user("COST OF ITEM:\t\t$");
        //get amount they already paid
        payment_in_cents = get_dollar_value_in_cents_from_user("AMOUNT OF PAYMENT:\t$");

        //calculate amount they owe
        amount_owed_in_cents = payment_in_cents - cost_in_cents;

        //check whether the payment is equal to, less than, or greater than, the cost
        if cost_in_cents == payment_in_cents {
            println!("CORRECT AMOUNT, THANK YOU.");
            continue;
        }
        else if payment_in_cents < cost_in_cents{ //amount_owed_in_cents is less than 0
            println!(
                "SORRY, YOU HAVE SHORT-CHANGED ME ${}.{}",
                -amount_owed_in_cents/100,//leading digits
                -amount_owed_in_cents%100,//trailing digits
            );
            continue;
        }
        else {
            println!("YOUR CHANGE, ${}.{}", amount_owed_in_cents/100, amount_owed_in_cents%100);
        }

        //calculate change due
        //hundred dollar bills owed
        hundred = amount_owed_in_cents / (100*100);
        if hundred > 0 {println!("HUNDRED DOLLAR BILL(S): {}", hundred);}
        amount_owed_in_cents = amount_owed_in_cents % (100*100);

        //fifty dollar bills owed
        fifty = amount_owed_in_cents / (50*100);
        if fifty > 0 {println!("FIFTY DOLLAR BILL(S): {}", fifty);}
        amount_owed_in_cents = amount_owed_in_cents % (50*100);

        //twenty dollar bills owed
        twenty = amount_owed_in_cents / (20*100);
        if twenty > 0 {println!("TWENTY DOLLAR BILL(S): {}", twenty);}
        amount_owed_in_cents = amount_owed_in_cents % (20*100);

        //ten dollar bills owed
        ten = amount_owed_in_cents / (10*100);
        if ten > 0 {println!("TEN DOLLAR BILL(S): {}", ten);}
        amount_owed_in_cents = amount_owed_in_cents % (10*100);

        //five dollar bills owed
        five = amount_owed_in_cents / (5*100);
        if five > 0 {println!("FIVE DOLLAR BILL(S): {}", five);}
        amount_owed_in_cents = amount_owed_in_cents % (5*100);

        //one dollar bills owed
        one = amount_owed_in_cents / (1*100);
        if one > 0 {println!("ONE DOLLAR BILL(S): {}", one);}
        amount_owed_in_cents = amount_owed_in_cents % (1*100);

        //quarters owed
        quarters = amount_owed_in_cents / 25;
        if quarters > 0 {println!("QUARTER(S): {}", quarters);}
        amount_owed_in_cents = amount_owed_in_cents % 25;

        //dimes owed
        dimes = amount_owed_in_cents / 10;
        if dimes > 0 {println!("DIME(S): {}", dimes);}
        amount_owed_in_cents = amount_owed_in_cents % 10;

        //nickles owed
        nickles = amount_owed_in_cents / 5;
        if nickles > 0 {println!("NICKEL(S): {}", nickles);}
        amount_owed_in_cents = amount_owed_in_cents % 5;

        //pennies owed
        pennies = amount_owed_in_cents / 1;
        if pennies > 0 {println!("PENNY(S): {}", pennies);}

        //print ending message
        println!("THANK YOU, COME AGAIN.\n\n");
    }
}

/**
 * print welcome message
 */
fn welcome() {
    println!("\t\t\t\tCHANGE\n\t      CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n");
}

/**
 * get number of money from user input
 */
fn get_dollar_value_in_cents_from_user(prompt:&str) -> i16 {
    let mut value:i16;
    //input loop
    loop {
        //data
        let mut raw_input = String::new();

        //print prompt
        print!("{}",prompt);
        //flush std out // allows prompt to be on same line as input
        stdout().flush().expect("failed to flush");

        //get input
        io::stdin().read_line(&mut raw_input).expect("failed to read input");
        //filter out characters that aren't numbers or '.'
        let mut no_prior_periods = true;
        raw_input = raw_input.chars().filter(|c| {
            if c.eq_ignore_ascii_case(&'.') && no_prior_periods {
                no_prior_periods = false;
                true
            } else {
                c.is_ascii_digit()
            }
        }).collect();

        //should only be (at most) 1 .
        if !raw_input.contains(".") { raw_input += ".00";} //if there are none, add one

        //ensure there are at least 2 trailing digits
        if raw_input[raw_input.find('.').unwrap_or(raw_input.len())..].len() <= 2 { //if a slice of the string from the . onwards is less than or equal to 2, add two 0's to the end
            raw_input += "00"
        }
        //truncate the trailing digits to 2 digits
        raw_input = raw_input[..=raw_input.find('.').unwrap_or(raw_input.len()-2)+2].to_string(); //raw_input = a slice of raw_input from the start to 2 past the . 
        
        //remove the '.' and convert the string to an integer
        raw_input = raw_input.chars().filter(|c| c.is_ascii_digit()).collect();
        match raw_input.parse::<i16>().ok() {
            Some(v) => {value = v;}
            None => {
                println!("INPUT OUTSIDE OF ACCEPTABLE RANGE, TRY AGAIN");
                continue;
            },
        }

        //println!("{}",value);

        if value <= 0 {continue;}
        else {return value;}
    }
}
