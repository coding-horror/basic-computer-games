/*
 * Nim
 * Originally from the wonderful book: _Basic Computer Games_
 * Port to Rust By David Lotts
*/
use nanorand::{tls::TlsWyRand, Rng};
use std::io::{self, Write};
use text_io::{read, try_read};

/// Play Nim
// line numbers from the orginal Basic program are in the end of line comments. 
// If you see two number comments: the first one is a 
// GOTO or THEN destination, second is the current line. Example: // 800 //210
fn main() {
    let mut rng = nanorand::tls_rng();
    println!("{:>37}", "NIM"); //100
    println!("{:>15}{}", "", "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"); //110
    println!();
    println!();
    println!(); //120
    let mut piles = [0.0; 100];
    let mut b_piles = [[0.0; 100]; 11];
    let mut ix_do = [0usize; 3]; //210
    println!("THIS IS THE GAME OF NIM."); //220
                                          //230
    loop {
        let instuct_me = input("DO YOU WANT INSTRUCTIONS"); //240
        if instuct_me == "NO" || instuct_me == "no" {
            break;
        } //440   //260
        if instuct_me == "YES" || instuct_me == "yes" {
            instructions();
            break;
        } //310   //280
        println!("PLEASE ANSWER YES OR NO"); //290
    } //300
    'play_again: loop {
        println!(); //440
        let winner_take_last: bool = loop {
            let choice = input_int("ENTER WIN OPTION - 1 TO TAKE LAST, 2 TO AVOID LAST"); //460
            if (1..=2).contains(&choice) {
                break choice == 1;
            } //490  //470
        };
        let np = loop {
            //490
            let choice = input_int("ENTER NUMBER OF PILES"); //500
            if choice <= 100 && choice >= 1 {
                break choice;
            } //490   //510
              //490   //520
              //490   //530
        };
        println!("ENTER PILE SIZES"); //540
        for ix in 0..np as usize {
            //550
            piles[ix] = loop {
                let choice = input_int(&(ix + 1).to_string()); //570
                if choice <= 2000 && choice >= 1 && choice >= 1 {
                    break choice as f64;
                } //560   //580
                  //560   //600
            }
        } //610
        let human_first = loop {
            //620
            let choice = input("DO YOU WANT TO MOVE FIRST"); //630
            let choice = choice.to_lowercase();
            if choice == "yes" {
                break true;
            } //1450   //650
            if choice == "no" {
                break false;
            } //700   //670
            println!("PLEASE ANSWER YES OR NO."); //680
        }; //690

        let mut winner = WinState::GameOn;
        if human_first {
            winner = human_turn(winner_take_last, np, &mut piles);
        };
        //### main game loop
        if winner.is_game_on() {
            winner = loop {
                // break on winner returning from here:
                let win = machines_turn(
                    &mut rng,
                    winner_take_last,
                    np,
                    &mut piles,
                    &mut ix_do,
                    &mut b_piles,
                );
                if !win.is_game_on() {
                    break win;
                }; //starts at 700

                println!("PILE  SIZE"); //1380
                for ix in 0..np as usize {
                    //1390
                    println!("{} {}", ix + 1, piles[ix]); //1400
                } //  NEXT ix   //1410

                // break on winner returning from here:
                let win = human_turn(winner_take_last, np, &mut piles);
                if !win.is_game_on() {
                    break win;
                };
                //  GOTO 700   //1560
            };
        }
        println!(
            "MACHINE {}",
            if winner == WinState::ComputerWins {
                "WINS"
            } else {
                "LOSES"
            }
        );
        loop {
            // Game over //1640
            let choice = input("do you want to play another game"); //1650
            match choice.to_ascii_lowercase().as_str() {
                "yes" => break,                       //1720   //1660
                "no" => break 'play_again,            //1730   //1680
                _ => println!("PLEASE.  YES OR NO."), //1700
            } //  GOTO 1650    //1710
        } //  GOTO 440   //1720
    } //  END   //1730
}

#[derive(PartialEq)]
enum WinState {
    GameOn,
    ComputerWins,
    HumanWins,
}

impl WinState {
    /// Returns `true` if the win state is [`GameOn`].
    ///
    /// [`GameOn`]: WinState::GameOn
    fn is_game_on(&self) -> bool {
        matches!(self, Self::GameOn)
    }
}

/// Computer's turn
fn machines_turn(
    rng: &mut TlsWyRand,
    winner_take_last: bool,
    np: i32,
    piles: &mut [f64; 100],
    ix_do: &mut [usize; 3],
    b_piles: &mut [[f64; 100]; 11],
) -> WinState {
    if !winner_take_last {
        //940   //700
        //### Loser takes last, check for winner
        let mut count = 0; //710
        'wayout: loop {
            'outer: loop {
                for ix in 0..np as usize {
                    //720
                    if piles[ix] == 0.0 {
                        continue;
                    } //730
                    count += 1; //740
                    if count == 3 {
                        break 'outer;
                    } //840   //750
                    ix_do[count] = ix; //760
                } //770
                  // exactly two piles remain
                if count == 2 {
                    // println!("Only two piles remain : unused0={} pile1={} pile2={}",ix_do[0]+1,ix_do[1]+1,ix_do[2]+1); //diagnostic
                    if piles[ix_do[1]] == 1.0 || piles[ix_do[2]] == 1.0 {
                        //920
                        return WinState::ComputerWins;
                    } //820   //930
                    break 'wayout;
                } //920   //780
                  // exactly one pile remains, loser takes last, and before machine's turn
                  // println!("Only one pile remains : pile={}",ix_do[1]); //diagnostic
                assert!(piles[ix_do[1]] > 0.0);
                if piles[ix_do[1]] == 1.0 {
                    //820    //790
                    return WinState::HumanWins; //800
                                                // GOTO 1640   //810
                } else {
                    return WinState::ComputerWins; //820
                                                   // GOTO 1640   //830
                }
            }
            count = 0; //840
            let mut is_all_ones = true;
            for ix in 0..np as usize {
                // FOR ix=1 TO N   //850
                if piles[ix] > 1.0 {
                    is_all_ones = false;
                    break;
                } //940   //860
                if piles[ix] != 0.0 {
                    //890   //870
                    count = count + 1; //880
                }
            } // NEXT ix   //890
            if is_all_ones && count % 2 != 0 {
                //800   //900
                return WinState::HumanWins;
            }
            break; // GOTO 940   //910
        }
    }
    //### winner take last (or first?) -- check for winner
    for ix in 0..np as usize {
        //940
        let mut sticks = piles[ix]; //950
        for jx in 0..=10 {
            //960
            let half = sticks / 2.0; //970
            b_piles[ix][jx] = 2.0 * (half - half.trunc()); //980
            sticks = half.trunc(); //990
        } //  NEXT J   //1000
    } //  NEXT I   //1010

    let mut is_odd = false;
    let mut ix_max_pile = usize::MAX; // make sure this fails if ever used.
    for jx in (0..=10).rev() {
        //1020
        let mut count = 0; //1030
        let mut highest = 0.0; //1040
        for ix in 0..np as usize {
            //1050
            if b_piles[ix][jx] == 0.0 {
                continue;
            }; //1110   //1060
            count = count + 1; //1070
            if piles[ix] <= highest as f64 {
                continue;
            }; //1110   //1080
            highest = piles[ix]; //1090
            ix_max_pile = ix; //1100
        } //NEXT I   //1110
          // println!("if none are odd, use random: count={} odd={}", count,count%2!=0); //diagnostic
        if count % 2 != 0 {
            is_odd = true;
            break;
        } // C/2<>INT(C/2) //1190   //1120
    } //  NEXT J   //1130
    if !is_odd {
        let mut ix_random;
        loop {
            ix_random = rng.generate_range(0..np as usize); //(N*RND(1)+1).trunc();   //1140
            if piles[ix_random] != 0.0 {
                break;
            } //1140   //1150
        }
        let remove_random = rng.generate_range(1..=piles[ix_random] as i32); // INT(A[E]*RND(1)+1)   //1160
        piles[ix_random] = piles[ix_random] - remove_random as f64; //1170
        // println!("I choose random: pile={} removed={}",ix_random+1,remove_random) //diagnostic
        // GOTO 1380   //1180
    } else {
        // println!("max pile: pile={}, was={} setting to 0. Expect add back.",ix_max_pile+1,piles[ix_max_pile]); //diagnostic
        piles[ix_max_pile] = 0.0; //1190
        for jx in 0..=10 {
            //1200
            b_piles[ix_max_pile][jx] = 0.0; //1210
            let mut countum = 0; //1220
            for ix in 0..np as usize {
                //1230
                if b_piles[ix][jx] == 0.0 {
                    continue;
                } //1260   //1240
                countum += 1; // count non-empty  //1250
            } //  NEXT ix   //1260
            piles[ix_max_pile] =
                piles[ix_max_pile] + ((countum % 2) * 2usize.pow(jx as u32)) as f64;
            //1270
            // println!("I choose max pile : pile={}, add back=odd?2^{}:0={}",ix_max_pile+1,jx,((countum%2)*2usize.pow(jx as u32))); //diagnostic
        } //  NEXT J   //1280

        'done: loop {
            if !winner_take_last {
                //1380   //1290
                let mut counter = 0; //1300
                for ix in 0..np as usize {
                    //1310
                    if piles[ix] > 1.0 {
                        break 'done;
                    } //1380   //1320
                    if piles[ix] != 0.0 {
                        //1350  //1330
                        counter += 1; //1340
                    }
                } //  NEXT ix   //1350
                  // done if C is odd
                if counter % 2 != 0 {
                    break;
                } //1380   //1360
                  // println!("max pile if even: 1 - pile : pile={}, before 1-p = {}",ix_max_pile+1,piles[ix_max_pile]); //diagnostic
                piles[ix_max_pile] = 1.0 - piles[ix_max_pile]; //1370
            }
            break;
        }
    }
    return WinState::GameOn; //1380 is after this
}

/// Human decide what you want to do and see if there is a winner
fn human_turn(winner_take_last: bool, np: i32, piles: &mut [f64; 100]) -> WinState {
    if winner_take_last {
        //1450   //1420
        let is_all_empty = one_if_all_zero(np, &piles); //  GOSUB 1570   //1430
        if is_all_empty == 1 {
            return WinState::ComputerWins;
        } //820   //1440  //### machine wins
    }
    //### many things go here
    loop {
        //1450
        let (pile_choice, remove_choice) = input_2int("YOUR MOVE - PILE, NUMBER TO BE REMOVED"); //1460
        if pile_choice > np || pile_choice < 1 {
            continue;
        } //1450   //1480
        let ix_choice = (pile_choice - 1) as usize;
        if remove_choice < 1 || remove_choice as f64 > piles[ix_choice] {
            continue;
        }; //1450   //1500
           // remove the humans choice:
        piles[ix_choice] = piles[ix_choice] - remove_choice as f64; //1530
        break;
    }
    if one_if_all_zero(np, &piles) == 1
    //  GOSUB 1570   //1540
    {
        return WinState::HumanWins;
    } //800   //1550  //### machine loses!
    return WinState::GameOn;
}

/// returns 0 if all A are 0, otherwise 1
fn one_if_all_zero(np: i32, piles: &[f64; 100]) -> i32 {
    // sets Z to the return value
    //1570
    for ix in 0..np as usize {
        //1580
        if piles[ix] != 0.0 {
            return 0;
        } //1610   //1590
          //1600
    } //1610
    return 1; //1620
} //1630

fn instructions() {
    println!("THE GAME IS PLAYED WITH A NUMBER OF PILES OF OBJECTS.");
    println!("ANY NUMBER OF OBJECTS ARE REMOVED FROM ONE PILE BY YOU AND");
    println!("THE MACHINE ALTERNATELY.  ON YOUR TURN, YOU MAY TAKE");
    println!("ALL THE OBJECTS THAT REMAIN IN ANY PILE, BUT YOU MUST");
    println!("TAKE AT LEAST ONE OBJECT, AND YOU MAY TAKE OBJECTS FROM");
    println!("ONLY ONE PILE ON A SINGLE TURN.  YOU MUST SPECIFY WHETHER");
    println!("WINNING IS DEFINED AS TAKING OR NOT TAKING THE LAST OBJECT,");
    println!("THE NUMBER OF PILES IN THE GAME, AND HOW MANY OBJECTS ARE");
    println!("ORIGINALLY IN EACH PILE.  EACH PILE MAY CONTAIN A");
    println!("DIFFERENT NUMBER OF OBJECTS.");
    println!("THE MACHINE WILL SHOW ITS MOVE BY LISTING EACH PILE AND THE");
    println!("NUMBER OF OBJECTS REMAINING IN THE PILES AFTER  EACH OF ITS");
    println!("MOVES.");
}

/// print the prompt, wait for a number and newline.  Loop if invalid.
fn input(prompt: &str) -> String {
    loop {
        print!("{} ? ", prompt);
        io::stdout().flush().unwrap();
        // TODO:  asks twice on win10, not linux.  \r vs \n?
        let innn: String = read!("{}\n");
        let out: String = innn.trim().to_string();
        if out != "" {
            return out;
        }
    }
}
fn input_int(prompt: &str) -> i32 {
    loop {
        print!("{} ? ", prompt);
        io::stdout().flush().unwrap();
        match try_read!() {
            Ok(n) => return n,
            Err(_) => {}
        }
    }
}

fn input_2int(prompt: &str) -> (i32, i32) {
    loop {
        let inp = input(prompt);
        let nums: Vec<i32> = inp
            .split(",")
            .filter_map(|c| c.parse::<i32>().ok())
            .collect();
        if nums.len() != 2 {
            println!("Enter two numbers like: 9,9",);
            continue;
        }
        return (nums[0], nums[1]);
    }
}
