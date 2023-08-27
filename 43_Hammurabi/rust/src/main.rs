use std::io;
use rand::Rng;

fn run() {
    let mut year = 0;
    let mut population = 95;
    let mut immigrants = 5;
    let mut starved = 0;
    let mut total_starved = 0;
    let mut plague = false;
    let mut grain = 2800;
    let mut bushels_fed;
    let mut harvest;
    let mut planted;
    let mut yield_acre = 3;
    let mut eaten_rats = 200;
    let mut acres = 1000;
    let mut land_price;
    let mut bought_land;
    let mut perc_starved = 0.0;
    let mut game_failed = false;

    'main: loop {
        year += 1;
        if year > 11 {
            break;
        }
        println!("\n\n\nHAMURABI: I BEG TO REPORT TO YOU,");
        println!("IN YEAR {year}, {starved} PEOPLE STARVED, {immigrants} CAME TO THE CITY,");
        population = population + immigrants;
        if plague{
            population = population / 2;
            plague = false;
            println!("A HORRIBLE PLAGUE STRUCK! HALF THE PEOPLE DIED.");
        }
        println!("POPULATION IS NOW {population}");
        println!("THE CITY NOW OWNS {acres} ACRES.");
        println!("YOU HARVESTED {yield_acre} BUSHELS PER ACRE.");
        println!("THE RATS ATE {eaten_rats} BUSHELS.");
        println!("YOU NOW HAVE {grain} BUSHELS IN STORE.\n");
        let r = rand::thread_rng().gen_range(1..10);
        land_price = r + 17;
        println!("LAND IS TRADING AT {land_price} BUSHELS PER ACRE.");

        loop {  
            println!("HOW MANY ACRES DO YOU WISH TO BUY? ");
            if let Some(qty) = get_input() {
                if qty < 0 {
                    impossible_task();
                    game_failed = true;
                    break 'main;
                }
                if qty == 0 {
                    bought_land = false;
                    break;
                }
                if land_price * qty as u32 > grain {
                    insufficient_grain(grain);
                    continue;
                }
                if land_price * qty as u32 <= grain {
                    acres += qty as u32;
                    grain -= land_price * qty as u32;
                    bought_land = true;
                    break;
                }
            }
        }

        if bought_land == false {
            loop {  
                println!("HOW MANY ACRES DO YOU WISH TO SELL? ");
                if let Some(qty) = get_input() {
                    if qty < 0 {
                        impossible_task();
                        game_failed = true;
                        break 'main;
                    }
                    if qty as u32 <= acres {
                        acres -= qty as u32;
                        grain += land_price * qty as u32;
                        break;
                    }
                    insufficient_land(acres);
                }
            }
        }

        loop {  
            println!("HOW MANY BUSHELS DO YOU WISH TO FEED YOUR PEOPLE? ");
            if let Some(qty) = get_input() {
                if qty < 0 {
                    impossible_task();
                    game_failed = true;
                    break 'main;
                }
                // Trying to use more grain than is in silos?
                if qty as u32 > grain {
                    insufficient_grain(grain);
                    continue;
                }
                bushels_fed = qty as u32;
                grain -= bushels_fed;
                break;
            }
        }

        loop {  
            println!("HOW MANY ACRES DO YOU WISH TO PLANT WITH SEED? ");
            if let Some(qty) = get_input() {
                if qty < 0 {
                    impossible_task();
                    game_failed = true;
                    break 'main;
                }
                // Trying to plant more acres than you own?
                if qty as u32 > acres {
                    insufficient_land(acres);
                    continue;
                }
                // Enough grain for seed?
                if qty as u32 / 2 > grain {
                    insufficient_grain(grain);
                    continue;
                }
                // Enough people to tend the crops?
                if qty as u32 > (10 * population) {
                    insufficient_people(population);
                    continue;
                }
                planted = qty as u32;
                grain = grain - (planted / 2);
                break;
            }
        }

        // A bountiful harvest!
        yield_acre = gen_random();
        harvest = planted * yield_acre;
        eaten_rats = 0;

        // Determine if any grain was eaten by rats
        let mut c = gen_random();
        if c % 2 == 0 { // If c is even...
            // Rats are running wild!
            eaten_rats = grain / c;
        }
        // Update the amount of grain held
        grain = grain - eaten_rats + harvest;

        // Let's have some babies
        c = gen_random();
        immigrants = c * (20 * acres + grain) / population / 100 + 1;

        // How many people had full tummies?
        c = bushels_fed / 20;
        // Horrors, a 15% chance of plague
        let rf: f32 = rand::thread_rng().gen();
        let plague_chance = (10. * ((2. * rf) - 0.3)) as i32;
        if plague_chance == 0 {
            plague = true;
        }
        if population >= c {
            // Starve enough for impeachment?
            starved = population - c;
            if starved > (0.45 * population as f32) as u32 {
                println!("YOU STARVED {starved} PEOPLE IN ONE YEAR!!!");
                national_fink();
                game_failed = true;
                break;
            }
            // Calculate percentages here
            perc_starved = ((year - 1) as f32 * perc_starved + starved as f32 * 100. / population as f32) / year as f32;
            population = c;
            total_starved = total_starved + starved;
        }
    }

    if game_failed == false {
        println!("IN YOUR 10-YEAR TERM OF OFFICE {perc_starved} PERCENT OF THE");
        println!("POPULATION STARVED PER YEAR ON THE AVERAGE, I.E. A TOTAL OF");
        println!("{total_starved} PEOPLE DIED!!");
        let acres_head = acres / population;
        println!("YOU STARTED WITH 10 ACRES PER PERSON AND ENDED WITH");
        println!("{acres_head} ACRES PER PERSON.\n");
        if perc_starved > 33. || acres_head < 7 {
            national_fink();
        }
        else if perc_starved > 10. || acres_head < 9 {
            println!("YOUR HEAVY-HANDED PERFORMANCE SMACKS OF NERO AND IVAN IV.");
            println!("THE PEOPLE (REMAINING) FIND YOU AND UNPLEASANT RULER, AND,");
            println!("FRANKLY, HATE YOUR GUTS!!");
        }
        else if perc_starved > 3. || acres_head < 10 {
            let haters = (population as f32 * 0.8 * gen_random() as f32) as u32;
            println!("YOUR PERFORMANCE COULD HAVE BEEN SOMEWHAT BETTER, BUT");
            println!("REALLY WASN'T TOO BAD AT ALL. {haters} PEOPLE");
            println!("WOULD DEARLY LIKE TO SEE YOU ASSASSINATED BUT WE ALL HAVE OUR");
            println!("TRIVIAL PROBLEMS.");
        } else {
            println!("A FANTASTIC PERFORMANCE!!! CHARLEMANGE, DISRAELI, AND");
            println!("JEFFERSON COMBINED COULD NOT HAVE DONE BETTER!\n");
        }
        for _ in 1..10 {
            print!("\n");
        }
    }
            
    println!("\nSO LONG FOR NOW.\n");
}

fn get_input() -> Option<i32> {
    let mut input = String::new();
    io::stdin().read_line(&mut input).expect("Failed read_line");
    match input.trim().parse() {
        Ok(num) => Some(num),
        Err(_) => None,
    }
}

fn gen_random() -> u32 {
    let r: f32 = rand::thread_rng().gen();
    (r * 5.0 + 1.0) as u32
}

fn impossible_task() {
    println!("HAMURABI:  I CANNOT DO WHAT YOU WISH.");
    println!("GET YOURSELF ANOTHER STEWARD!!!!!");
}

fn insufficient_grain(grain: u32) {
    println!("HAMURABI:  THINK AGAIN.  YOU HAVE ONLY");
    println!("{grain} BUSHELS OF GRAIN.  NOW THEN,");
}

fn insufficient_land(acres: u32) {
    println!("HAMURABI: THINK AGAIN. YOU OWN ONLY {acres} ACRES.  NOW THEN,");
}

fn insufficient_people(population: u32) {
    println!("BUT YOU HAVE ONLY {population} PEOPLE TO TEND THE FIELDS!  NOW THEN,");
}

fn national_fink() {
    println!("DUE TO THIS EXTREME MISMANAGEMENT YOU HAVE NOT ONLY");
    println!("BEEN IMPEACHED AND THROWN OUT OF OFFICE BUT YOU HAVE");
    println!("ALSO BEEN DECLARED NATIONAL FINK!!!!");
}

fn main() {
    println!("                 HAMURABI");
    println!("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
    print!("\n\n\n\n");
    println!("TRY YOUR HAND AT GOVERNING ANCIENT SUMERIA");
    println!("FOR A TEN-YEAR TERM OF OFFICE.\n");

    run();
}
