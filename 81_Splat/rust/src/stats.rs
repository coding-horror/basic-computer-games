use std::{
    fs::{self, File},
    io::Write,
};

use crate::utility;

pub struct Stats {
    altitudes: Vec<f32>,
}

impl Stats {
    pub fn new() -> Option<Self> {
        if utility::prompt_bool("WOULD YOU LIKE TO LOAD PREVIOUS GAME DATA?", false) {
            let path = "src/stats.txt";
            let mut altitudes = Vec::new();

            if let Ok(stats) = fs::read_to_string(path) {
                if stats.is_empty() {
                    return Some(Stats {
                        altitudes: Vec::new(),
                    });
                }

                let stats: Vec<&str> = stats.trim().split(",").collect();

                for s in stats {
                    if s.is_empty() {
                        continue;
                    }

                    let s = s.parse::<f32>().expect("Corrupt stats file!");
                    altitudes.push(s);
                }

                return Some(Stats { altitudes });
            } else {
                println!("PREVIOUS GAME DATA NOT FOUND!");

                if !utility::prompt_bool("WOULD YOU LIKE TO CREATE ONE?", false) {
                    return None;
                } else {
                    let mut file = File::create(path).expect("Invalid file path!");
                    file.write_all("".as_bytes())
                        .expect("Could not create file!");

                    return Some(Stats {
                        altitudes: Vec::new(),
                    });
                }
            }
        }

        println!("\nRESULTS OF THIS SESSION WILL NOT BE SAVED.");
        None
    }

    pub fn add_altitude(&mut self, a: f32) {
        let all_jumps = self.altitudes.len() + 1;
        let mut placement = all_jumps;

        for (i, altitude) in self.altitudes.iter().enumerate() {
            if a <= *altitude {
                placement = i + 1;
                break;
            }
        }

        utility::print_win(all_jumps, placement);

        self.altitudes.push(a);
        self.altitudes.sort_by(|a, b| a.partial_cmp(b).unwrap());

        self.write();
    }

    fn write(&self) {
        let mut file = File::create("src/stats.txt").expect("Error loading stats data!");

        let mut altitudes = String::new();

        for a in &self.altitudes {
            altitudes.push_str(format!("{},", a).as_str());
        }

        write!(&mut file, "{}", altitudes.trim()).expect("ERROR WRITING Stats FILE!");
    }
}
