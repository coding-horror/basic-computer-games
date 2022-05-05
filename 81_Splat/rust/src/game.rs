use crate::utility;

pub struct Game {
    altitude: f32,
    terminal_velocity: f32,
    acceleration: f32,
    interval: f32,
}

impl Game {
    pub fn new() -> Game {
        let altitude = utility::get_altitude();

        let terminal_velocity = utility::get_terminal_velocity(
            "SELECT YOUR OWN TERMINAL VELOCITY",
            "WHAT TERMINAL VELOCITY (MI/HR)?",
        );

        let acceleration = utility::get_acceleration(
            "WANT TO SELECT ACCELERATION DUE TO GRAVITY",
            "WHAT ACCELERATION (FT/SEC/SEC)?",
        );

        println!("");
        println!("      ALTITUDE        = {} FT", altitude);
        println!("      TERM. VELOCITY  = {} FT/SEC +-5%", terminal_velocity);
        println!("      ACCELERATION    = {} FT/SEC/SEC +-5%", acceleration);

        let seconds =
            utility::prompt_numeric("\nSET THE TIMER FOR YOUR FREEFALL.\nHOW MANY SECONDS?");

        println!("\nHERE WE GO.\n");

        println!("TIME (SEC)\tDIST TO FALL (FT)");
        println!("==========\t=================");

        Game {
            altitude,
            terminal_velocity,
            acceleration,
            interval: seconds / 8.,
        }
    }

    pub fn tick(&mut self) -> f32 {
        let mut splat = false;
        let mut terminal_velocity_reached = false;

        let (v, a) = (self.terminal_velocity, self.acceleration);
        let terminal_velocity_time = v / a;

        let initial_altitude = self.altitude;

        for i in 0..=8 {
            let dt = i as f32 * self.interval;

            if dt >= terminal_velocity_time {
                if !terminal_velocity_reached {
                    println!(
                        "TERMINAL VELOCITY REACHED AT T PLUS {} SECONDS.",
                        terminal_velocity_time
                    );
                    terminal_velocity_reached = true;
                }

                let d1 = v.powi(2) / (2. * a);
                let d2 = v * (dt - (terminal_velocity_time));
                self.altitude = initial_altitude - (d1 + d2);

                if self.altitude <= 0. {
                    let t = (initial_altitude - d1) / v;
                    utility::print_splat(t + terminal_velocity_time);

                    splat = true;
                    break;
                }
            } else {
                let d1 = (a * 0.5) * (dt.powi(2));
                self.altitude = initial_altitude - d1;

                if self.altitude <= 0. {
                    let t = (2. * initial_altitude / a).sqrt();
                    utility::print_splat(t);

                    splat = true;
                    break;
                }
            }

            println!("{}\t\t{}", dt, self.altitude);

            std::thread::sleep(std::time::Duration::from_secs(1));
        }

        let mut a = -1.;

        if !splat {
            println!("\nCHUTE OPEN\n");

            a = self.altitude;
        }

        a
    }
}
