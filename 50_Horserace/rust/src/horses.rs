use rand::Rng;

pub struct Horse {
    pub name: String,
    pub no: u8,
    pub odd: f32,
    pub position: u8,
}

impl Horse {
    fn new(name: &str, no: u8) -> Self {
        Horse {
            name: name.to_string(),
            no,
            odd: 0.,
            position: 0,
        }
    }
}

pub struct Horses {
    horses: [Horse; 8],
}

impl Horses {
    pub fn new() -> Self {
        Horses {
            horses: [
                Horse::new("JOE MAW", 1),
                Horse::new("L.B.J.", 2),
                Horse::new("MR.WASHBURN", 3),
                Horse::new("MISS KAREN", 4),
                Horse::new("JOLLY", 5),
                Horse::new("HORSE", 6),
                Horse::new("JELLY DO NOT", 7),
                Horse::new("MIDNIGHT", 8),
            ],
        }
    }

    pub fn randomize_odds(&mut self) {
        let mut odds = Vec::new();

        for _ in 1..=8 {
            odds.push(rand::thread_rng().gen_range(1.0..=10.));
        }

        let total: f32 = odds.iter().sum();

        for (i, o) in odds.iter().enumerate() {
            let o = total / o;
            self.horses[i].odd = o;
        }
    }

    pub fn advance(&mut self) {
        for h in self.horses.iter_mut() {
            let distance = rand::thread_rng().gen_range(1..=100);
            let scale = h.odd.ceil() as i32;

            let dt = if distance < 10 {
                1
            } else if distance < scale + 17 {
                2
            } else if distance < scale + 37 {
                3
            } else if distance < scale + 57 {
                4
            } else if distance < scale + 77 {
                5
            } else if distance < scale + 92 {
                6
            } else {
                7
            };

            h.position += dt as u8;
        }
    }

    pub fn get_at(&self, row: usize) -> Vec<&Horse> {
        self.horses
            .iter()
            .filter(|h| h.position == row as u8)
            .collect()
    }

    pub fn print_table(&self) {
        println!("HORSE\t\tNUMBER\t\tODDS\t\t\n");
        for horse in self.horses.iter() {
            let (h, n, o) = (horse.name.clone(), horse.no, horse.odd);

            if h.len() > 7 {
                println!("{}\t{}\t\t{:.2} :1", h, n, o);
            } else {
                println!("{}\t\t{}\t\t{:.2} :1", h, n, o);
            }
        }
        println!("-----------------------------------------\n")
    }

    pub fn print_placements(&mut self) -> u8 {
        self.horses.sort_by(|a, b| b.position.cmp(&a.position));

        println!("\nTHE RACE RESULTS ARE:\n");

        for (i, h) in self.horses.iter_mut().enumerate() {
            println!("{} PLACE HORSE NO. {}\t\tAT {:.2} :1", i + 1, h.no, h.odd);
            h.position = 0;
        }

        self.horses[0].no
    }
}
