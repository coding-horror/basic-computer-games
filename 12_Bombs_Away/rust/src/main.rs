use std::{
    io::{self, Write},
    str::FromStr,
};

use rand::Rng;

struct Italy;
struct Allies;
struct Japan;
struct Germany;

#[derive(Debug, PartialEq, Eq)]
struct ParseChoiceTargetError;

#[derive(PartialEq)]
enum ThreeTarget {
    One,
    Two,
    Three,
}

impl FromStr for ThreeTarget {
    type Err = ParseChoiceTargetError;

    fn from_str(s: &str) -> Result<Self, Self::Err> {
        let n = s.parse::<u8>().map_err(|_| ParseChoiceTargetError)?;
        match n {
            1 => Ok(Self::One),
            2 => Ok(Self::Two),
            3 => Ok(Self::Three),
            _ => Err(ParseChoiceTargetError),
        }
    }
}

enum FourTarget {
    One,
    Two,
    Three,
    Four,
}

impl FromStr for FourTarget {
    type Err = ParseChoiceTargetError;

    fn from_str(s: &str) -> Result<Self, Self::Err> {
        let n = s.parse::<u8>().map_err(|_| ParseChoiceTargetError)?;
        match n {
            1 => Ok(Self::One),
            2 => Ok(Self::Two),
            3 => Ok(Self::Three),
            4 => Ok(Self::Four),
            _ => Err(ParseChoiceTargetError),
        }
    }
}

pub trait Brefing {
    type TargetOption;
    fn prompt<'a>(&self) -> &'a str;
    fn targets_to_messages<'a>(&self, target: Self::TargetOption) -> &'a str;
}

impl Brefing for Italy {
    type TargetOption = ThreeTarget;

    fn prompt<'a>(&self) -> &'a str {
        "YOUR TARGET -- ALBANIA(1), GREECE(2), NORTH AFRICA(3)"
    }

    fn targets_to_messages<'a>(&self, target: Self::TargetOption) -> &'a str {
        match target {
            ThreeTarget::One => "SHOULD BE EASY -- YOU'RE FLYING A NAZI-MADE PLANE.",
            ThreeTarget::Two => "BE CAREFUL!!!",
            ThreeTarget::Three => "YOU'RE GOING FOR THE OIL, EH?",
        }
    }
}

impl Brefing for Allies {
    type TargetOption = FourTarget;

    fn prompt<'a>(&self) -> &'a str {
        "AIRCRAFT -- LIBERATOR(1), B-29(2), B-17(3), LANCASTER(4): "
    }

    fn targets_to_messages<'a>(&self, target: Self::TargetOption) -> &'a str {
        match target {
            FourTarget::One => "YOU'VE GOT 2 TONS OF BOMBS FLYING FOR PLOESTI.",
            FourTarget::Two => "YOU'RE DUMPING THE A-BOMB ON HIROSHIMA.",
            FourTarget::Three => "YOU'RE CHASING THE BISMARK IN THE NORTH SEA.",
            FourTarget::Four => "YOU'RE BUSTING A GERMAN HEAVY WATER PLANT IN THE RUHR.",
        }
    }
}

impl Brefing for Germany {
    type TargetOption = ThreeTarget;

    fn prompt<'a>(&self) -> &'a str {
        "A NAZI, EH?  OH WELL.  ARE YOU GOING FOR RUSSIA(1),\nENGLAND(2), OR FRANCE(3)? "
    }

    fn targets_to_messages<'a>(&self, target: Self::TargetOption) -> &'a str {
        match target {
            ThreeTarget::One => "YOU'RE NEARING STALINGRAD.",
            ThreeTarget::Two => "NEARING LONDON.  BE CAREFUL, THEY'VE GOT RADAR.",
            ThreeTarget::Three => "NEARING VERSAILLES.  DUCK SOUP.  THEY'RE NEARLY DEFENSELESS.",
        }
    }
}

enum Side {
    Italy(Italy),
    Allies(Allies),
    Japan(Japan),
    Germany(Germany),
}

impl From<FourTarget> for Side {
    fn from(value: FourTarget) -> Self {
        match value {
            FourTarget::One => Self::Italy(Italy),
            FourTarget::Two => Self::Allies(Allies),
            FourTarget::Three => Self::Japan(Japan),
            FourTarget::Four => Self::Germany(Germany),
        }
    }
}

fn stdin_choice<C: FromStr>(prompt: &str) -> C {
    let mut buffer = String::new();

    print!("{prompt}");
    io::stdout().flush().unwrap();
    io::stdin().read_line(&mut buffer).unwrap();
    loop {
        if let Ok(choice) = buffer.trim().parse::<C>() {
            return choice;
        }
        print!("TRY AGAIN...");
        io::stdout().flush().unwrap();
        io::stdin().read_line(&mut buffer).unwrap();
    }
}

fn stdin_y_or_n(prompt: &str) -> bool {
    let mut buffer = String::new();

    print!("{prompt}");
    io::stdout().flush().unwrap();
    io::stdin().read_line(&mut buffer).unwrap();
    buffer.trim().to_uppercase() == "Y"
}

fn commence_non_kamikazi_attack() {
    let nmissions = loop {
        let nmissions = stdin_choice::<i32>("HOW MANY MISSIONS HAVE YOU FLOWN? ");
        if nmissions < 160 {
            break nmissions;
        }
        println!("MISSIONS, NOT MILES...");
        println!("150 MISSIONS IS HIGH EVEN FOR OLD-TIMERS");
        print!("NOW THEN, ");
    };

    if nmissions >= 100 {
        println!("THAT'S PUSHING THE ODDS!");
    }

    if nmissions < 25 {
        println!("FRESH OUT OF TRAINING, EH?");
    }

    println!();

    let mut rng = rand::thread_rng();
    let y: f32 = rng.gen();

    if nmissions as f32 >= 160_f32 * y {
        mission_success();
    } else {
        mission_failure();
    }
}

fn play_japan() {
    if !stdin_y_or_n("YOUR FIRST KAMIKAZE MISSION? (Y OR N): ") {
        player_death();
        return;
    }

    let mut rng = rand::thread_rng();
    let y: f32 = rng.gen();
    if y > 0.65 {
        mission_success();
    } else {
        player_death();
    }
}

fn player_death() {
    println!("* * * * BOOM * * * *");
    println!("YOU HAVE BEEN SHOT DOWN.....");
    println!("DEARLY BELOVED, WE ARE GATHERED HERE TODAY TO PAY OUR");
    println!("LAST TRIBUTE...");
}

fn mission_success() {
    let mut rng = rand::thread_rng();
    let y: f32 = rng.gen();

    let killed = (100f32 * y) as i32;
    println!("DIRECT HIT!!!! {killed} KILLED.");
    println!("MISSION SUCCESSFUL.");
}

fn mission_failure() {
    let mut rng = rand::thread_rng();
    let y: f32 = rng.gen();
    let miles = 2 + (30f32 * y) as i32;
    println!("MISSED TARGET BY {miles} MILES!");
    println!("NOW YOU'RE REALLY IN FOR IT !!");
    println!();
    let enemy_weapons =
        stdin_choice::<ThreeTarget>("DOES THE ENEMY HAVE GUNS(1), MISSILES(2), OR BOTH(3)? ");

    let enemy_gunner_accuracy = if enemy_weapons != ThreeTarget::Two {
        let m = loop {
            let m =
                stdin_choice::<i32>("WHAT'S THE PERCENT HIT RATE OF ENEMY GUNNERS (10 TO 50)? ");
            if m <= 50 {
                break m;
            }
            println!("TRY AGAIN...");
        };
        if m < 10 {
            println!("YOU LIE, BUT YOU'LL PAY...");
            player_death();
            return;
        }
        m
    } else {
        0
    };

    let missile_threat_weighting = if enemy_weapons == ThreeTarget::One {
        0
    } else {
        35
    };

    let death =
        death_with_chance((enemy_gunner_accuracy + missile_threat_weighting) as f32 / 100f32);

    if death {
        player_death();
    } else {
        player_survived();
    }
}

fn player_survived() {
    println!("YOU MADE IT THROUGH TREMENDOUS FLAK!!");
}

fn death_with_chance(p_death: f32) -> bool {
    let mut rng = rand::thread_rng();
    let y: f32 = rng.gen();
    p_death > y
}

fn main() {
    loop {
        println!("YOU ARE A PILOT IN A WORLD WAR II BOMBER.");
        let side: Side =
            stdin_choice::<FourTarget>("WHAT SIDE -- ITALY(1), ALLIES(2), JAPAN(3), GERMANY(4): ")
                .into();

        match side {
            Side::Japan(_) => {
                println!("YOU'RE FLYING A KAMIKAZE MISSION OVER THE USS LEXINGTON.");
            }
            Side::Italy(ref s) => {
                let target = stdin_choice(s.prompt());
                println!("{}", s.targets_to_messages(target));
            }
            Side::Allies(ref s) => {
                let target = stdin_choice(s.prompt());
                println!("{}", s.targets_to_messages(target));
            }
            Side::Germany(ref s) => {
                let target = stdin_choice(s.prompt());
                println!("{}", s.targets_to_messages(target));
            }
        }

        match side {
            Side::Japan(_) => play_japan(),
            _ => commence_non_kamikazi_attack(),
        }

        println!();
        if !stdin_y_or_n("ANOTHER MISSION? (Y OR N): ") {
            break;
        }
    }
}
