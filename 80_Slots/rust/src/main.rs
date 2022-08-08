use rand::Rng;

fn main() {
    println!("\n\n\t\tSLOTS");
    println!("CREATIVE COMPUTING MORRISTOWN, NEW JERSEY\n");

    println!("YOU ARE IN THE H&M CASINO, IN FRONT OF ONE OF OUR");
    println!("ONE-ARMED BANDITS. BET FROM $1 TO $100.");
    println!("TO PULL THE ARM, PUNCH THE RETURN KEY AFTER MAKING YOUR BET.\n");

    let possible_fruits = ["Bar", "Bell", "Orange", "Lemon", "Plum", "Cherry"];

    let mut profit = 0i32;

    let get_input = || {
        let mut input = String::new();
        std::io::stdin()
            .read_line(&mut input)
            .expect("Error reading line!");
        let input = input.trim();
        input.to_string()
    };

    loop {
        let mut bet = None;

        println!("\nYOUR BET?\n");

        let input = get_input();

        if let Ok(n) = input.parse::<i32>() {
            if n < 1 {
                println!("MINIMUM BET IS $1");
            } else if n > 100 {
                println!("HOUSE LIMITS ARE $100");
            } else {
                bet = Some(n as u8);
            }
        }

        if bet == None {
            continue;
        }

        if let Some(bet) = bet {
            let random_fruit = || {
                let random_index = rand::thread_rng().gen_range(0..possible_fruits.len());
                possible_fruits[random_index]
            };

            let wheel = [random_fruit(), random_fruit(), random_fruit()];

            println!("\n{} {} {}\n", wheel[0], wheel[1], wheel[2]);

            let mut multi: Option<(&str, u8)> = None;
            let mut current_fruit = "";

            for fruit in wheel {
                if fruit == current_fruit {
                    if let Some(_) = multi {
                        multi = Some((fruit, 3));
                    } else {
                        multi = Some((fruit, 2));
                    }
                } else {
                    current_fruit = fruit;
                }
            }

            let win = |m: u8| m as i32 * bet as i32 + bet as i32;

            if let Some((fruit, number)) = multi {
                if number == 2 {
                    if fruit == "Bar" {
                        println!("\n*DOUBLE BAR*");
                        profit += win(5);
                    } else {
                        println!("\nDOUBLE!!");
                        profit += win(2);
                    }
                } else if number == 3 {
                    if fruit == "Bar" {
                        println!("\n***JACKPOT***");
                        profit += win(100);
                    } else {
                        println!("\n**TOP DOLLAR**");
                        profit += win(10);
                    }
                }

                println!("YOU WON!")
            } else {
                println!("\nYOU LOST.");
                profit -= bet as i32;
            }
        }

        println!("YOUR STANDINGS ARE {profit}");

        println!("AGAIN?");
        let input = get_input();
        if input.to_lowercase() != "y" {
            if profit > 0 {
                println!("COLLECT YOUR WINNINGS FROM THE H&M CASHIER.");
            } else if profit < 0 {
                println!("PAY UP! PLEASE LEAVE YOUR MONEY AT THE TERMINAL");
            } else {
                println!("HEY, YOU BROKE EVEN.")
            }

            println!();
            break;
        }
    }
}
