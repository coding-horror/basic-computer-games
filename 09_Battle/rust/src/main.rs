use rand::Rng;
use std::{
    cmp::Ordering,
    fmt,
    io::{self, Write},
};

#[derive(Clone, Copy)]
#[repr(C)]
enum ShipLength {
    Destroyer = 2,
    Cruiser = 3,
    AircraftCarrier = 4,
}

#[derive(Clone, Copy, PartialEq)]
struct Point(i8, i8);

impl core::ops::Add for Point {
    type Output = Self;

    fn add(self, rhs: Self) -> Self::Output {
        Self(self.0 + rhs.0, self.1 + rhs.1)
    }
}

impl Point {
    pub fn is_outside(&self, width: usize) -> bool {
        let w = width as i8;
        (!(0..w).contains(&self.0)) || (!(0..w).contains(&self.1))
    }

    pub fn userinput2coordinate(self) -> Self {
        Self(self.0 - 1, See::WIDTH as i8 - self.1)
    }
}

struct Ship(Vec<Point>);

impl Ship {
    pub fn new(length: ShipLength) -> Self {
        'try_again: loop {
            let start = Point(
                rand::thread_rng().gen_range(0..See::WIDTH) as i8,
                rand::thread_rng().gen_range(0..See::WIDTH) as i8,
            );
            let vector = Self::random_vector();

            let mut ship = vec![start];
            for _ in 1..length as usize {
                let last = ship.last().unwrap();
                let new_part = *last + vector;
                if new_part.is_outside(See::WIDTH) {
                    continue 'try_again;
                }
                ship.push(new_part);
            }

            return Self(ship);
        }
    }

    fn random_vector() -> Point {
        loop {
            let vector = Point(
                rand::thread_rng().gen_range(-1..2),
                rand::thread_rng().gen_range(-1..2),
            );
            if vector != Point(0, 0) {
                return vector;
            }
        }
    }

    pub fn collide(&self, see: &[Vec<i8>]) -> bool {
        self.0.iter().any(|p| see[p.0 as usize][p.1 as usize] != 0)
    }

    pub fn place(self, see: &mut [Vec<i8>], code: i8) {
        for p in self.0.iter() {
            see[p.0 as usize][p.1 as usize] = code;
        }
    }
}

enum Report {
    Already(i8),
    Splash,
    Hit(i8),
}

struct See {
    data: Vec<Vec<i8>>,
}

impl See {
    pub const WIDTH: usize = 6;

    fn place_ship(data: &mut [Vec<i8>], length: ShipLength, code: i8) {
        let ship = loop {
            let ship = Ship::new(length);
            if ship.collide(data) {
                continue;
            }
            break ship;
        };
        ship.place(data, code);
    }

    pub fn new() -> Self {
        let mut data = vec![vec![0; Self::WIDTH]; Self::WIDTH];

        Self::place_ship(&mut data, ShipLength::Destroyer, 1);
        Self::place_ship(&mut data, ShipLength::Destroyer, 2);
        Self::place_ship(&mut data, ShipLength::Cruiser, 3);
        Self::place_ship(&mut data, ShipLength::Cruiser, 4);
        Self::place_ship(&mut data, ShipLength::AircraftCarrier, 5);
        Self::place_ship(&mut data, ShipLength::AircraftCarrier, 6);

        Self { data }
    }

    pub fn report(&mut self, point: Point) -> Report {
        let (x, y) = (point.0 as usize, point.1 as usize);
        let value = self.data[x][y];
        match value.cmp(&0) {
            Ordering::Less => Report::Already(-value),
            Ordering::Equal => Report::Splash,
            Ordering::Greater => {
                self.data[x][y] = -value;
                Report::Hit(value)
            }
        }
    }

    pub fn has_ship(&self, code: i8) -> bool {
        self.data.iter().any(|v| v.contains(&code))
    }

    pub fn has_any_ship(&self) -> bool {
        (1..=6).any(|c| self.has_ship(c))
    }

    pub fn count_sunk(&self, ship: ShipLength) -> i32 {
        let codes = match ship {
            ShipLength::Destroyer => (1, 2),
            ShipLength::Cruiser => (3, 4),
            ShipLength::AircraftCarrier => (5, 6),
        };

        let ret = if self.has_ship(codes.0) { 0 } else { 1 };
        ret + if self.has_ship(codes.1) { 0 } else { 1 }
    }
}

impl fmt::Display for See {
    fn fmt(&self, f: &mut fmt::Formatter) -> fmt::Result {
        for row in &self.data {
            write!(f, "\r\n")?;
            for cell in row {
                write!(f, "{:2} ", cell)?;
            }
        }
        write!(f, "\r\n")
    }
}

fn input_point() -> Result<Point, ()> {
    let mut input = String::new();
    io::stdin()
        .read_line(&mut input)
        .expect("Failed to read line");
    let point_str: Vec<&str> = input.trim().split(',').collect();

    if point_str.len() != 2 {
        return Err(());
    }

    let x = point_str[0].parse::<i8>().map_err(|_| ())?;
    let y = point_str[1].parse::<i8>().map_err(|_| ())?;

    Ok(Point(x, y))
}

fn get_next_target() -> Point {
    loop {
        print!("? ");
        let _ = io::stdout().flush();

        if let Ok(p) = input_point() {
            let p = p.userinput2coordinate();
            if !p.is_outside(See::WIDTH) {
                return p;
            }
        }

        println!(
            "INVALID. SPECIFY TWO NUMBERS FROM 1 TO {}, SEPARATED BY A COMMA.",
            See::WIDTH
        );
    }
}

fn main() {
    let mut see = See::new();
    println!(
        "
                BATTLE
CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY

THE FOLLOWING CODE OF THE BAD GUYS' FLEET DISPOSITION
HAS BEEN CAPTURED BUT NOT DECODED:
        "
    );

    println!("{see}");

    println!(
        "

DE-CODE IT AND USE IT IF YOU CAN
BUT KEEP THE DE-CODING METHOD A SECRET.

START GAME
        "
    );

    let mut splashes = 0;
    let mut hits = 0;

    loop {
        let target = get_next_target();

        let r = see.report(target);
        if let Report::Hit(c) = r {
            println!("A DIRECT HIT ON SHIP NUMBER {c}");
            hits += 1;

            if !see.has_ship(c) {
                println!("AND YOU SUNK IT. HURRAH FOR THE GOOD GUYS.");
                println!("SO FAR, THE BAD GUYS HAVE LOST");
                println!("{} DESTROYER(S),", see.count_sunk(ShipLength::Destroyer));
                println!("{} CRUISER(S),", see.count_sunk(ShipLength::Cruiser));
                println!(
                    "AND {} AIRCRAFT CARRIER(S),",
                    see.count_sunk(ShipLength::AircraftCarrier)
                );
            }
        } else {
            if let Report::Already(c) = r {
                println!("YOU ALREADY PUT A HOLE IN SHIP NUMBER {c} AT THAT POINT.");
            }
            println!("SPLASH! TRY AGAIN.");
            splashes += 1;
            continue;
        }

        if see.has_any_ship() {
            println!("YOUR CURRENT SPLASH/HIT RATIO IS {splashes}/{hits}");
            continue;
        }

        println!("YOU HAVE TOTALLY WIPED OUT THE BAD GUYS' FLEET ");
        println!("WITH A FINAL SPLASH/HIT RATIO OF {splashes}/{hits}");

        if splashes == 0 {
            println!("CONGRATULATIONS -- A DIRECT HIT EVERY TIME.");
        }

        println!("\n****************************");
        break;
    }
}
