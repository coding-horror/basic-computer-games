use anyhow::Result;
use std::io;

fn print_centered(text: &str) {
    println!("{:^72}", text);
}

fn read_line() -> Result<String> {
    let mut input = String::new();
    io::stdin().read_line(&mut input)?;
    input.truncate(input.trim_end().len());
    Ok(input)
}

fn read_number() -> Result<u32> {
    let line = read_line()?;
    let num = line.parse()?;
    Ok(num)
}

fn print_banner() {
    print_centered("PIZZA");
    print_centered("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
    println!();
    println!();
    println!();
}

fn print_instructions() -> Result<String> {
    println!("PIZZA DELIVERY GAME");
    println!();
    println!("WHAT IS YOUR FIRST NAME");

    let name = read_line()?;
    println!("HI, {name}.  IN THIS GAME YOU ARE TO TAKE ORDERS");
    println!("FOR PIZZAS.  THEN YOU ARE TO TELL A DELIVERY BOY");
    println!("WHERE TO DELIVER THE ORDERED PIZZAS.");
    println!();
    println!();

    print_map();

    println!("THE OUTPUT IS A MAP OF THE HOMES WHERE");
    println!("YOU ARE TO SEND PIZZAS.");
    println!();
    println!("YOUR JOB IS TO GIVE A TRUCK DRIVER");
    println!("THE LOCATION OR COORDINATES OF THE");
    println!("HOME ORDERING THE PIZZA.");
    println!();

    Ok(name)
}

fn print_ruler() {
    println!(" -----1-----2-----3-----4-----");
}

fn print_ticks() {
    println!("-");
    println!("-");
    println!("-");
    println!("-");
}

fn print_street(i: u32) {
    let street_number = 3 - i;
    let street_name = (street_number + 1).to_string();

    let mut line = street_name.clone();
    let space = "     ";
    for customer_idx in 0..4 {
        line.push_str(space);
        let customer = 4 * street_number + customer_idx;
        line.push(char::from_u32(65 + customer).unwrap());
    }
    line.push_str(space);
    line.push_str(&street_name);
    println!("{line}");
}

fn print_map() {
    println!("MAP OF THE CITY OF HYATTSVILLE");
    println!();
    print_ruler();
    for i in 0..4 {
        print_ticks();
        print_street(i);
    }
    print_ticks();
    print_ruler();
    println!();
}

fn yes_no_prompt(text: &str) -> Result<Option<bool>> {
    println!("{text}");
    let input = read_line()?;
    match input.as_str() {
        "YES" => Ok(Some(true)),
        "NO" => Ok(Some(false)),
        _ => Ok(None),
    }
}

fn play_game(turns: u32, player_name: &str) -> Result<()> {
    for _ in 0..turns {
        let customer = fastrand::char('A'..='P');
        println!("HELLO {player_name}'S PIZZA.  THIS IS {customer}.");
        println!("  PLEASE SEND A PIZZA.");
        loop {
            println!("  DRIVER TO {player_name}:  WHERE DOES {customer} LIVE");

            let x = read_number()?;
            let y = read_number()?;

            let input = x - 1 + (y - 1) * 4;
            match char::from_u32(65 + input) {
                Some(c) if c == customer => {
                    println!("HELLO {player_name}.  THIS IS {customer}, THANKS FOR THE PIZZA.");
                    break;
                }
                Some(c @ 'A'..='P') => {
                    println!("THIS IS {c}.  I DID NOT ORDER A PIZZA.");
                    println!("I LIVE AT {x},{y}");
                }
                // this is out of bounds in the original game
                _ => (),
            }
        }
    }

    Ok(())
}

fn main() -> Result<()> {
    print_banner();
    let player_name = print_instructions()?;
    let more_directions = loop {
        if let Some(x) = yes_no_prompt("DO YOU NEED MORE DIRECTIONS")? {
            break x;
        } else {
            println!("'YES' OR 'NO' PLEASE, NOW THEN,");
        }
    };

    if more_directions {
        println!();
        println!("SOMEBODY WILL ASK FOR A PIZZA TO BE");
        println!("DELIVERED.  THEN A DELIVERY BOY WILL");
        println!("ASK YOU FOR THE LOCATION.");
        println!("     EXAMPLE:");
        println!("THIS IS J.  PLEASE SEND A PIZZA.");
        println!("DRIVER TO {player_name}.  WHERE DOES J LIVE?");
        println!("YOUR ANSWER WOULD BE 2,3");
        println!();

        if let Some(true) = yes_no_prompt("UNDERSTAND")? {
            println!("GOOD.  YOU ARE NOW READY TO START TAKING ORDERS.");
            println!();
            println!("GOOD LUCK!!");
            println!();
        } else {
            println!("THIS JOB IS DEFINITELY TOO DIFFICULT FOR YOU. THANKS ANYWAY");
            return Ok(());
        }
    }

    loop {
        play_game(5, &player_name)?;
        println!();

        if let Some(false) | None = yes_no_prompt("DO YOU WANT TO DELIVER MORE PIZZAS")? {
            println!();
            println!("O.K. {player_name}, SEE YOU LATER!");
            println!();
            return Ok(());
        }
    }
}
