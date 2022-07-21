use std::time::Duration;

use rand::Rng;

fn main() {
    println!("\n\t\tRUSSIAN ROULETTE");
    println!("CREATIVE COMPUTING MORRISTOWN, NEW JERSEY");
    println!("\nTHIS IS A GAME OF >>>>>>>>>>RUSSIAN ROULETTE.\n");
    println!("HERE IS A REVOLVER.");

    loop {
        println!("TYPE '1' TO SPIN CHAMBER AND PULL TRIGGER");
        println!("TYPE '2' TO GIVE UP.");
        println!("GO");

        let mut tries = 0;

        loop {
            let mut pull_trigger = true;

            loop {
                println!("?");
                let mut input = String::new();

                std::io::stdin()
                    .read_line(&mut input)
                    .expect("Error reading line!");

                match input.trim() {
                    "1" => break,
                    "2" => {
                        pull_trigger = false;
                        break;
                    }
                    _ => println!("Invalid input."),
                }
            }

            if pull_trigger {
                std::thread::sleep(Duration::from_secs(1));

                match rand::thread_rng().gen_range(0..6) {
                    0 => {
                        println!("\tBANG!!!!!   YOU'RE DEAD!");
                        println!("CONDOLENCES WILL BE SENT TO YOUR RELATIVES.");
                        println!("\n\n...NEXT VICTIM...");
                        break;
                    }
                    _ => {
                        println!("- CLICK -");
                        tries += 1;
                    }
                }

                if tries >= 10 {
                    println!("YOU WIN!!!!!");
                    println!("LET SOMEONE ELSE BLOW HIS BRAINS OUT.\n");
                    break;
                }
            } else {
                println!("\tCHICKEN!!!!!");
                println!("\n\n...NEXT VICTIM...");
                break;
            }
        }
    }
}
