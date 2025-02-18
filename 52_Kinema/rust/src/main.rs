use std::io::Write;
use rand::Rng;

/** KINEMA BY RICHARD PAV
 * https://github.com/coding-horror/basic-computer-games/blob/main/52_Kinema/kinema.bas
 * Direct conversion from BASIC to Rust by Pablo Marques (marquesrs).
 * As a faithful translation, many of the code here are done in an unrecommended way by
 *  today's standards.
 * 
 * ATTENTION: The original code has mathematical imprecision and uses simplifications
 * instead of the real formulation, which could lead to incorrect results. I have solved
 * this issue, but kept the old lines. To compile the original version, just uncomment the 
 * code with the OLD label and comment the lines with the NEW label.
 * example: gravity is now 9.81 instead of 10
 * 17/02/25
*/

fn subroutine(a: f64, q: &mut i32) {
    std::io::stdout().flush().unwrap();
    //500 INPUT G
    let mut input = String::new();
    let g;
    loop {
        std::io::stdin().read_line(&mut input).unwrap();
        match input.trim().parse::<f64>() {
            Ok(e) => { g = e; break; },
            Err(_) => { print!("\nINVALID. TRY AGAIN: "); continue; },
        };
    }
    //502 IF ABS((G-A)/A)<.15 THEN 510
    if f64::abs((g-a)/a) < 0.15 {
        //510 PRINT "CLOSE ENOUGH."        
        print!("CLOSE ENOUGH.");
        //511 Q=Q+1
        *q = *q + 1;
    }
    else {
        //504 PRINT "NOT EVEN CLOSE...."
        print!("NOT EVEN CLOSE...");
        //506 GOTO 512
    }
    //512 PRINT "CORRECT ANSWER IS ";A
    print!("\nCORRECT ANSWER IS {a:.2}\n");
    //520 PRINT
    //530 RETURN
}

fn main() {
    let mut rng = rand::rng();

    //10 PRINT TAB(33);"KINEMA"
    //20 PRINT TAB(15);"CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
    //30 PRINT: PRINT: PRINT
    //100 PRINT
    //105 PRINT
    print!("{}KINEMA\n{}CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n\n", 
        " ".repeat(33),
        " ".repeat(15)
    );
    loop {
        //106 Q=0
        let mut q = 0;
        //110 V=5+INT(35*RND(1))
        let v: f64 = 5.0 + 35.0 * rng.random_range(0.0..1.0);
        //111 PRINT "A BALL IS THROWN UPWARDS AT";V;"METERS PER SECOND."
        //112 PRINT
        print!("\nA BALL IS THROWN UPWARDS AT {v:.2} METERS PER SECOND.\n");
        //115 A=.05*V^2
        //let a = 0.05 * v.powf(2.0); // OLD
        let mut a = v.powf(2.0) / (2.0 * 9.81); // NEW
        //116 PRINT "HOW HIGH WILL IT GO (IN METERS)";
        print!("\nHOW HIGH WILL IT GO (IN METERS)? ");
        
        //117 GOSUB 500
        subroutine(a, &mut q);
        
        //120 A=V/5
        //a = v / 5.0; // OLD
        a = 2.0 * v / 9.81; // NEW
        //122 PRINT "HOW LONG UNTIL IT RETURNS (IN SECONDS)";
        print!("\nHOW LONG UNTIL IT RETURNS (IN SECONDS)? ");
        //124 GOSUB 500
        subroutine(a, &mut q);

        //130 T=1+INT(2*V*RND(1))/10
        let t = 1.0 + (2.0 * v * rng.random_range(0.0..1.0) / 10.0);
        //132 A=V-10*T
        a = v + (-9.81 * t);
        //134 PRINT "WHAT WILL ITS VELOCITY BE AFTER";T;"SECONDS";
        print!("\nWHAT WILL ITS VELOCITY BE AFTER {t:.2} SECONDS? ");

        //136 GOSUB 500
        subroutine(a, &mut q);

        //140 PRINT
        //150 PRINT Q;"RIGHT OUT OF 3.";
        print!("\n{q} RIGHT OUT OF 3.\n");
        //160 IF Q<2 THEN 100
        if q < 2 {
            continue;
        }
        //170 PRINT "  NOT BAD."
        //print!("  NOT BAD.");
        //180 GOTO 100
    }
    //999 END
}