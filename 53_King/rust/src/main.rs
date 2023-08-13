#![forbid(unsafe_code)]

use fastrand::Rng;
use std::io;
use std::io::{stdin, stdout, BufRead, Write};

// global variable `N5` in the original game
const TERM_LENGTH: u32 = 8;

fn main() {
    let mut rng = Rng::new();
    let mut input = stdin().lock();
    let mut state = intro(&mut input, &mut rng).expect("input error");

    loop {
        let land_price = 95 + rng.u32(0..10);
        let plant_price = 10 + rng.u32(0..5);
        print_state(&state, land_price, plant_price);
        state = match next_round(&mut input, &mut rng, &state, land_price, plant_price)
            .expect("input error")
        {
            RoundEnd::Next(s) => s,
            RoundEnd::GameOver(msg) => {
                println!("{}", msg);
                return;
            }
        }
    }
}

// The game is round based (one round per in-game year).
// This struct represents the state before each round
#[derive(Clone, PartialEq, Eq, Debug)]
struct State {
    // global variable `A` in the original game (currency: "rallods")
    money: u32,
    // global variable `B` in the original game
    countrymen: u32,
    // global variable `C` in the original game
    foreign_workers: u32,
    // global variable `D` in the original game
    land: u32,
    // global variable `X5` in original game
    year_in_office: u32,
    // global variable `X` in original game
    show_land_hint: bool,
    // global variable `V3` in original game
    previous_tourist_trade: u32,
}

#[derive(Clone, PartialEq, Eq, Debug)]
enum RoundEnd {
    GameOver(String),
    Next(State),
}

fn init_state(rng: &mut Rng) -> State {
    State {
        // the original formula for random values used floating point numbers.
        // e.g. `INT(60000+(1000*RND(1))-(1000*RND(1)))`
        // I want to avoid floats unless necessary. These values generated here should be close
        // enough to the original distribution
        money: 60000 + rng.u32(0..1000) - rng.u32(0..1000),
        countrymen: 500 + rng.u32(0..10) - rng.u32(0..10),
        foreign_workers: 0,
        land: 2000,
        year_in_office: 0,
        show_land_hint: true,
        previous_tourist_trade: 0,
    }
}

fn print_state(state: &State, land_price: u32, plant_price: u32) {
    print!(
        r"
YOU NOW HAVE {} RALLODS IN THE TREASURY.
 {} COUNTRYMEN, ",
        state.money, state.countrymen
    );
    if state.foreign_workers > 0 {
        print!("{} FOREIGN WORKERS, ", state.foreign_workers)
    }
    println!(
        r"AND {} SQ. MILES OF LAND.
THIS YEAR INDUSTRY WILL BUY LAND FOR {} RALLODS PER SQUARE MILE.
LAND CURRENTLY COSTS {} RALLODS PER SQUARE MILE TO PLANT.
",
        state.land, land_price, plant_price
    );
}

// print the intro, optional instructions or a previous savegame
fn intro<R: BufRead>(mut input: R, rng: &mut Rng) -> io::Result<State> {
    println!("⚠️ This game includes references to suicide or self-harm.");
    println!("                                  KING");
    println!("               CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n");
    print!("DO YOU WANT INSTRUCTIONS?? ");
    // In the original game, all inputs were made in the same line as the previous output.
    // I try to replicate this behaviour here, but if I do not print a line break, the stdout buffer
    // will not be flushed and the user may not see the input prompt before the input.
    // this means in these cases I have to explicitly flush stdout.
    stdout().flush()?;
    let mut buf = String::with_capacity(16);
    input.read_line(&mut buf)?;
    buf.make_ascii_lowercase();
    match buf.trim() {
        "n" => Ok(init_state(rng)),
        "again" => {
            let year_in_office = read_and_verify_int(
                &mut input,
                &mut buf,
                "HOW MANY YEARS HAD YOU BEEN IN OFFICE WHEN INTERRUPTED?? ",
                |v| {
                    if v < 8 {
                        Ok(v)
                    } else {
                        Err(format!(
                            "   COME ON, YOUR TERM IN OFFICE IS ONLY {} YEARS.",
                            TERM_LENGTH
                        ))
                    }
                },
            )?;
            // The original game exits here when you enter a negative number for any of
            // the following values. This looks like intentional behaviour. However, to replicate that
            // I would have to change everything to signed number which I do not want right now.
            print!("HOW MUCH DID YOU HAVE IN THE TREASURY?? ");
            stdout().flush()?;
            let money = read_int(&mut input, &mut buf)?;
            print!("HOW MANY COUNTRYMEN?? ");
            stdout().flush()?;
            let countrymen = read_int(&mut input, &mut buf)?;
            print!("HOW MANY WORKERS?? ");
            stdout().flush()?;
            let foreign_workers = read_int(&mut input, &mut buf)?;
            let land = read_and_verify_int(
                &mut input,
                &mut buf,
                "HOW MANY SQUARE MILES OF LAND?? ",
                |v| {
                    if !(1000..=2000).contains(&v) {
                        // Note: the original says "10,000 SQ. MILES OF FOREST LAND", but this is
                        // inconsistent and listed as Bug 3 in the README.md
                        Err("   COME ON, YOU STARTED WITH 1000 SQ. MILES OF FARM LAND\n   AND 1000 SQ. MILES OF FOREST LAND.".to_owned())
                    } else {
                        Ok(v)
                    }
                },
            )?;
            Ok(State {
                money,
                countrymen,
                foreign_workers,
                land,
                year_in_office,
                show_land_hint: true,
                previous_tourist_trade: 0,
            })
        }
        _ => {
            println!(
                r"
CONGRATULATIONS! YOU'VE JUST BEEN ELECTED PREMIER OF SETATS
DETINU, A SMALL COMMUNIST ISLAND 30 BY 70 MILES LONG. YOUR
JOB IS TO DECIDE UPON THE CONTRY'S BUDGET AND DISTRIBUTE
MONEY TO YOUR COUNTRYMEN FROM THE COMMUNAL TREASURY.
THE MONEY SYSTEM IS RALLODS, AND EACH PERSON NEEDS 100
RALLODS PER YEAR TO SURVIVE. YOUR COUNTRY'S INCOME COMES
FROM FARM PRODUCE AND TOURISTS VISITING YOUR MAGNIFICENT
FORESTS, HUNTING, FISHING, ETC. HALF YOUR LAND IS FARM LAND
WHICH ALSO HAS AN EXCELLENT MINERAL CONTENT AND MAY BE SOLD
TO FOREIGN INDUSTRY (STRIP MINING) WHO IMPORT AND SUPPORT
THEIR OWN WORKERS. CROPS COST BETWEEN 10 AND 15 RALLODS PER
SQUARE MILE TO PLANT.
YOUR GOAL IS TO COMPLETE YOUR {} YEAR TERM OF OFFICE.
GOOD LUCK!
",
                TERM_LENGTH
            );
            Ok(init_state(rng))
        }
    }
}

static POLLUTION: &[&str] = &[
    "FISH POPULATION HAS DWINDLED DUE TO WATER POLLUTION.",
    "AIR POLLUTION IS KILLING GAME BIRD POPULATION.",
    "MINERAL BATHS ARE BEING RUINED BY WATER POLLUTION.",
    "UNPLEASANT SMOG IS DISCOURAGING SUN BATHERS.",
    "HOTELS ARE LOOKING SHABBY DUE TO SMOG GRIT.",
];

fn next_round<R: BufRead>(
    mut input: R,
    rng: &mut Rng,
    state: &State,
    land_price: u32,
    plant_price: u32,
) -> io::Result<RoundEnd> {
    let mut buf = String::with_capacity(16);
    let mut show_land_hint = state.show_land_hint;
    // global variable `H` in the original game
    let land_sold = read_and_verify_int(
        &mut input,
        &mut buf,
        "HOW MANY SQUARE MILES DO YOU WISH TO SELL TO INDUSTRY?? ",
        |v| {
            if v + 1000 <= state.land {
                Ok(v)
            } else if show_land_hint {
                show_land_hint = false;
                Err(format!(
                    r"***  THINK AGAIN. YOU ONLY HAVE {} SQUARE MILES OF FARM LAND.
(FOREIGN INDUSTRY WILL ONLY BUY FARM LAND BECAUSE
FOREST LAND IS UNECONOMICAL TO STRIP MINE DUE TO TREES,
THICKER TOP SOIL, ETC.)",
                    state.land - 1000
                ))
            } else {
                Err(format!(
                    r"***  THINK AGAIN. YOU ONLY HAVE {} SQUARE MILES OF FARM LAND.\n",
                    state.land - 1000
                ))
            }
        },
    )?;

    let land = state.land - land_sold;
    let money = state.money + land_sold * land_price;

    // global variable `I` in the original game
    let money_distributed = read_and_verify_int(
        &mut input,
        &mut buf,
        "HOW MANY RALLODS WILL YOU DISTRIBUTE AMONG YOUR COUNTRYMEN?? ",
        |v| {
            if v <= money {
                Ok(v)
            } else {
                Err(format!(
                    "   THINK AGAIN. YOU'VE ONLY {} RALLODS IN THE TREASURY",
                    money
                ))
            }
        },
    )?;
    let money = money - money_distributed;

    // global variable `J` in the original game
    let land_planted = if money > 0 {
        read_and_verify_int(
            &mut input,
            &mut buf,
            "HOW MANY SQUARE MILES DO YOU WISH TO PLANT?? ",
            |v| {
                if v > 2 * state.countrymen {
                    Err("   SORRY, BUT EACH COUNTRYMAN CAN ONLY PLANT 2 SQ. MILES.".to_owned())
                } else if v + 1000 > land {
                    Err(format!(
                        "   SORRY, BUT YOU'VE ONLY {} SQ. MILES OF FARM LAND.",
                        land - 1000
                    ))
                } else if v * plant_price > money {
                    Err(format!(
                        "   THINK AGAIN. YOU'VE ONLY {} RALLODS LEFT IN THE TREASURY.",
                        money
                    ))
                } else {
                    Ok(v)
                }
            },
        )?
    } else {
        0
    };
    let money = money - land_planted * plant_price;

    // global variable `K` in the original game
    let pollution_control = if money > 0 {
        read_and_verify_int(
            &mut input,
            &mut buf,
            "HOW MANY RALLODS DO YOU WISH TO SPEND ON POLLUTION CONTROL?? ",
            |v| {
                if v <= money {
                    Ok(v)
                } else {
                    Err(format!(
                        "   THINK AGAIN. YOU ONLY HAVE {} RALLODS REMAINING.",
                        money
                    ))
                }
            },
        )?
    } else {
        0
    };
    let money = money - pollution_control;

    if land_sold == 0 && money_distributed == 0 && land_planted == 0 && pollution_control == 0 {
        return Ok(RoundEnd::GameOver(
            r"
GOODBYE.
(IF YOU WISH TO CONTINUE THIS GAME AT A LATER DATE, ANSWER
'AGAIN' WHEN ASKED IF YOU WANT INSTRUCTIONS AT THE START
OF THE GAME)."
                .to_owned(),
        ));
    }

    println!("\n\n");

    let money_after_expenses = money;

    let starvation_deaths = state.countrymen.saturating_sub(money_distributed / 100);
    if starvation_deaths > 0 {
        println!("{starvation_deaths} COUNTRYMEN DIED OF STARVATION");
    }

    // the original was using `RND(1)` as factor, but I do not want to deal with floats.
    // this solution should do the same in the range of numbers we expect
    let pollution = ((2000 - land) * rng.u32(0..=2000)) / 2000;
    let pollution_deaths = if pollution_control >= 25 {
        pollution / (pollution_control / 25)
    } else {
        pollution
    };

    if pollution_deaths > 0 {
        println!(
            "{} COUNTRYMEN DIED OF CARBON-MONOXIDE AND DUST INHALATION",
            pollution_deaths
        );
    }

    let (money, land) = if pollution_deaths + starvation_deaths > 0 {
        let funeral_costs = (pollution_deaths + starvation_deaths) * 9;
        println!("   YOU WERE FORCED TO SPEND {funeral_costs} RALLODS ON FUNERAL EXPENSES");
        if funeral_costs > money {
            println!("   INSUFFICIENT RESERVES TO COVER COST - LAND WAS SOLD");
            (
                0,
                // I only handle integers here, but I think the basic code implicitly turns integers
                // to floats on division. So in order to round up to the next full land unit, I have
                // to do this weird modulo stuff here
                land - funeral_costs / land_price
                    + if funeral_costs % land_price == 0 {
                        0
                    } else {
                        1
                    },
            )
        } else {
            (money - funeral_costs, land)
        }
    } else {
        (money, land)
    };

    let mut countrymen = state
        .countrymen
        .saturating_sub(starvation_deaths)
        .saturating_sub(pollution_deaths);

    let tourist_trade_positive = countrymen * 22 + rng.u32(0..500);
    let tourist_trade_negative = (2000 - land) * 15;

    let new_foreign_workers: i32 = if land_sold > 0 {
        land_sold as i32 + rng.i32(0..10) - rng.i32(0..20)
            + if state.foreign_workers == 0 { 20 } else { 0 }
    } else {
        0
    };
    // In theory, we could come up with negative foreign workers here (and in the original game)
    // This does not seem to be right, so let's cap them at 0
    let foreign_workers = if new_foreign_workers < 0 {
        state
            .foreign_workers
            .saturating_sub(new_foreign_workers.unsigned_abs())
    } else {
        state.foreign_workers + new_foreign_workers as u32
    };
    print!(" {new_foreign_workers} WORKERS CAME TO THE COUNTRY AND ");

    let countryman_migration = (money_distributed as i32 / 100 - countrymen as i32) / 10
        + pollution_control as i32 / 25
        - (2000 - land as i32) / 50
        - pollution_deaths as i32 / 2;

    if countryman_migration < 0 {
        println!("{} COUNTRYMEN LEFT THE ISLAND", countryman_migration.abs());
        countrymen = countrymen.saturating_sub(countryman_migration.unsigned_abs())
    } else {
        println!("{countryman_migration} COUNTRYMEN CAME TO THE ISLAND");
        countrymen += countryman_migration as u32
    }

    let harvest_lost = ((2000 - land) * (rng.u32(0..2000) + 3000)) / 4000;
    // in the original game, this checked for foreign_workers == 0 instead of land_planted == 0
    // this is documented as Bug 4 in the README.md
    if land_planted == 0 {
        print!("OF {land_planted} SQ. MILES PLANTED,");
    }
    println!(
        " YOU HARVESTED {} SQ. MILES OF CROPS.",
        land_planted.saturating_sub(harvest_lost)
    );

    if harvest_lost > 0 {
        // There was a bug here in the original code (Bug 2 in README.md).
        // Based on the variable `V1`, the word `INCREASED` was inserted.
        // However, no value was ever assigned to `V1`. Since the pollution comes from land that has
        // been sold, I used the land difference to check whether the pollution increased.
        if state.land < land {
            println!("   (DUE TO INCREASED AIR AND WATER POLLUTION FROM FOREIGN INDUSTRY.)");
        } else {
            println!("   (DUE TO AIR AND WATER POLLUTION FROM FOREIGN INDUSTRY.)");
        }
    }

    let crop_winnings = land_planted.saturating_sub(harvest_lost) * land_price / 2;
    println!("MAKING {crop_winnings} RALLODS.");
    let money = money + crop_winnings;

    // In the original game, there are two bugs here (documented as Bug 1 and Bug 5 in the README.md)
    // The first one made the game ignore the income from tourists and duplicated the money that was
    // left at this point (the "DECREASE BECAUSE"-message would never have been shown).
    // The second bug made the tourist trade profitable again is the pollution was too high (i.e.
    // if `tourist_trade_positive` was less than `tourist_trade_negative`
    let tourist_trade = tourist_trade_positive.saturating_sub(tourist_trade_negative);
    println!(" YOU MADE {tourist_trade} RALLODS FROM TOURIST TRADE.");
    if tourist_trade < state.previous_tourist_trade {
        println!(
            "   DECREASE BECAUSE {}",
            POLLUTION[rng.usize(0..POLLUTION.len())]
        );
    }
    let money = money + tourist_trade;

    if starvation_deaths + pollution_deaths > 200 {
        let reason = rng.u8(0..10);
        return Ok(RoundEnd::GameOver(format!(
            r"

 {} COUNTRYMEN DIED IN ONE YEAR!!!!!
DUE TO THIS EXTREME MISMANAGEMENT, YOU HAVE NOT ONLY
BEEN IMPEACHED AND THROWN OUT OF OFFICE, BUT YOU
{}
",
            starvation_deaths + pollution_deaths,
            // The reasons are not equally probable in the original game.
            // I wonder if this was intentional.
            if reason <= 3 {
                "ALSO HAD YOUR LEFT EYE GOUGED OUT!"
            } else if reason <= 6 {
                "HAVE ALSO GAINED A VERY BAD REPUTATION."
            } else {
                "HAVE ALSO BEEN DECLARED NATIONAL FINK."
            }
        )));
    }
    if countrymen < 343 {
        // This is not entirely fair, it is possible that some of them just left, not died.
        // Also: the initial number of countrymen varies a bit, but this boundary is fix, so it is
        // not always a third
        return Ok(RoundEnd::GameOver(format!(
            r"

OVER ONE THIRD OF THE POPULTATION HAS DIED SINCE YOU
WERE ELECTED TO OFFICE. THE PEOPLE (REMAINING)
HATE YOUR GUTS.
{}
",
            departure_flavour(rng)
        )));
    }
    if money_after_expenses / 100 > 5 && starvation_deaths >= 2 {
        return Ok(RoundEnd::GameOver(
            r"
MONEY WAS LEFT OVER IN THE TREASURY WHICH YOU DID
NOT SPEND. AS A RESULT, SOME OF YOUR COUNTRYMEN DIED
OF STARVATION. THE PUBLIC IS ENRAGED AND YOU HAVE
BEEN FORCED TO EITHER RESIGN OR COMMIT SUICIDE.
THE CHOICE IS YOURS.
IF YOU CHOOSE THE LATTER, PLEASE TURN OFF YOUR COMPUTER
BEFORE PROCEEDING.
"
            .to_owned(),
        ));
    }
    if foreign_workers > countrymen {
        return Ok(RoundEnd::GameOver(format!(
            r"

THE NUMBER OF FOREIGN WORKERS HAS EXCEEDED THE NUMBER
OF COUNTRYMEN. AS A MINORITY, THEY HAVE REVOLTED AND
TAKEN OVER THE COUNTRY.
{}
",
            departure_flavour(rng)
        )));
    }
    if state.year_in_office + 1 >= TERM_LENGTH {
        return Ok(RoundEnd::GameOver(format!(
            r"

CONGRATULATIONS!!!!!!!!!!!!!!!!!!
YOU HAVE SUCCESFULLY COMPLETED YOUR {} YEAR TERM
OF OFFICE. YOU WERE, OF COURSE, EXTREMELY LUCKY, BUT
NEVERTHELESS, IT'S QUITE AN ACHIEVEMENT. GOODBYE AND GOOD
LUCK - YOU'LL PROBABLY NEED IT IF YOU'RE THE TYPE THAT
PLAYS THIS GAME.
",
            TERM_LENGTH
        )));
    }

    Ok(RoundEnd::Next(State {
        money,
        countrymen,
        foreign_workers,
        land,
        year_in_office: state.year_in_office + 1,
        show_land_hint,
        previous_tourist_trade: tourist_trade,
    }))
}

fn departure_flavour(rng: &mut Rng) -> &'static str {
    if rng.bool() {
        "YOU HAVE BEEN THROWN OUT OF OFFICE AND ARE NOW\nRESIDING IN PRISON.\n"
    } else {
        "YOU HAVE BEEN ASSASSINATED.\n"
    }
}

fn read_int<R: BufRead>(mut input: R, buf: &mut String) -> io::Result<u32> {
    loop {
        buf.clear();
        input.read_line(buf)?;
        let line = buf.trim();
        // This is implicit behaviour in the original code: empty input is equal to 0
        if line.is_empty() {
            return Ok(0);
        }
        if let Ok(n) = line.parse::<u32>() {
            return Ok(n);
        } else {
            print!("??REENTER\n?? ");
            stdout().flush()?;
        }
    }
}

fn read_and_verify_int<R: BufRead, Verify>(
    mut input: R,
    buf: &mut String,
    prompt: &str,
    mut verify: Verify,
) -> io::Result<u32>
where
    Verify: FnMut(u32) -> Result<u32, String>,
{
    loop {
        print!("{}", prompt);
        stdout().flush()?;
        let v = read_int(&mut input, buf)?;
        match verify(v) {
            Ok(v) => {
                return Ok(v);
            }
            Err(msg) => {
                println!("{}", msg);
            }
        }
    }
}
