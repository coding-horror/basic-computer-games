use crate::utility;

pub struct Game {
    altitude: f32,
    terminal_velocity: f32,
    acceleration: f32,
    seconds: f32,
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
            seconds,
            interval: seconds / 8.,
        }
    }

    pub fn tick(&mut self) -> bool {
        let mut splat = false;
        let mut terminal_velocity_reached = false;

        let (v, a) = (self.terminal_velocity, self.acceleration);
        let terminal_velocity_time = v / a;

        let mut final_altitude;

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
                final_altitude = self.altitude - (d1 + d2);

                if final_altitude <= 0. {
                    let t = (self.altitude - d1) / v;
                    utility::print_splat(t + terminal_velocity_time);

                    splat = true;
                    break;
                }
            } else {
                let d1 = (a * 0.5) * (dt.powi(2));
                final_altitude = self.altitude - d1;

                if final_altitude <= 0. {
                    let t = (2. * self.altitude * a).sqrt();
                    utility::print_splat(t);

                    splat = true;
                    break;
                }
            }

            println!("{}\t\t{}", dt, final_altitude);

            std::thread::sleep(std::time::Duration::from_secs(1));
        }

        if !splat {
            println!("\nCHUTE OPEN\n")

            // get saved statistics from previous games
            // compare and present a message.
        }

        use utility::prompt_bool;
        if !prompt_bool("DO YOU WANT TO PLAY AGAIN?", true) {
            if !prompt_bool("PLEASE?", false) {
                if !prompt_bool("YES OR NO PLEASE?", false) {
                    println!("SSSSSSSSSS.");
                    return false;
                }
            }
        }

        true
    }
}
