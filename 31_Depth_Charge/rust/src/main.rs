use std::io::Write;

use rand::Rng;

/** DEPTH CHARGE GAME 
 * https://github.com/marquesrs/basic-computer-games/blob/main/31_Depth_Charge/depthcharge.bas
 * Direct conversion from BASIC to Rust by Pablo Marques (marquesrs).
 * No additional features or improvements were added. As a faithful translation, 
 * many of the code here are done in an unrecommended way by today's standards.
 * 03/03/25
*/

fn input(msg: &str) -> String {
    print!("{}", msg);
    let _ =std::io::stdout().flush().unwrap();
    let mut input = String::new();
    std::io::stdin().read_line(&mut input).unwrap();
    return input.trim().to_uppercase();
}

fn main() {
    // 2 PRINT TAB(30);"DEPTH CHARGE"
    // 4 PRINT TAB(15);"CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
    // 6 PRINT: PRINT: PRINT
    print!("{}", format!("{}{}\n{}{}\n\n\n\n",
        " ".repeat(29),
        "DEPTH CHARGE",
        " ".repeat(14),
        "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
    ));

    // 20 INPUT "DIMENSION OF SEARCH AREA";G: PRINT
    let g = input("DIMENSION OF SEARCH AREA: ").parse::<i32>().unwrap();

    // 30 N=INT(LOG(G)/LOG(2))+1
    let n = (f32::ln(g as f32) / f32::ln(2.0) + 1.0) as i32;

    // 40 PRINT "YOU ARE THE CAPTAIN OF THE DESTROYER USS COMPUTER"
    // 50 PRINT "AN ENEMY SUB HAS BEEN CAUSING YOU TROUBLE.  YOUR"
    // 60 PRINT "MISSION IS TO DESTROY IT.  YOU HAVE";N;"SHOTS."
    // 70 PRINT "SPECIFY DEPTH CHARGE EXPLOSION POINT WITH A"
    // 80 PRINT "TRIO OF NUMBERS -- THE FIRST TWO ARE THE"
    // 90 PRINT "SURFACE COORDINATES; THE THIRD IS THE DEPTH."
    // 100 PRINT : PRINT "GOOD LUCK !": PRINT
    let a = (g * rand::rng().random_range(0..=1)) as i32;
    let b = (g * rand::rng().random_range(0..=1)) as i32;
    let c = (g * rand::rng().random_range(0..=1)) as i32;
    print!("{}", format!("{}{}{}{}{}{}{}{}{}{}{}",
        "\nYOU ARE THE CAPTAIN OF THE DESTROYER USS COMPUTER\n",
        "AN ENEMY SUB HAS BEEN CAUSING YOU TROUBLE. YOUR\n",
        "MISSION IS TO DESTROY IT. YOU HAVE ",
        n,
        " SHOTS.\n",
        "SPECIFY DEPTH CHARGE EXPLOSION POINT WITH A\n",
        "TRIO OF NUMBERS -- THE FIRST TWO ARE THE\n",
        "SURFACE COORDINATES; THE THIRD IS THE DEPTH.\n",
        format!("EXAMPLE FOR DIMENSION {}: ", g),
        format!("{}, {}, {}", a, b, c),
        "\nGOOD LUCK !\n"
    ));

    'main: loop {
        // 110 A=INT(G*RND(1)) : B=INT(G*RND(1)) : C=INT(G*RND(1))
        let a = (g * rand::rng().random_range(0..=1)) as i32;
        let b = (g * rand::rng().random_range(0..=1)) as i32;
        let c = (g * rand::rng().random_range(0..=1)) as i32;
        // 120 FOR D=1 TO N : PRINT : PRINT "TRIAL #";D; : INPUT X,Y,Z
        let mut x;
        let mut y;
        let mut z;
        for d in 1..=n {
            print!("\nTRIAL #{}\n", d);
            x = input("X: ").parse::<i32>().unwrap();
            y = input("Y: ").parse::<i32>().unwrap();
            z = input("Z: ").parse::<i32>().unwrap();
            // 130 IF ABS(X-A)+ABS(Y-B)+ABS(Z-C)=0 THEN 300
            if i32::abs(x-a) + i32::abs(y-b) + i32::abs(z-c) == 0 {
                // 300 PRINT : PRINT "B O O M ! ! YOU FOUND IT IN";D;"TRIES!"
                print!("{}", format!("{}{}{}{}{}",
                    "\n",
                    "B O O M ! ! YOU FOUND IT IN ",
                    d,
                    " TRIES!\n",
                    "\n"
                ));
                // 400 PRINT : PRINT: INPUT "ANOTHER GAME (Y OR N)";A$
                if replay() { continue 'main; }
                else { break 'main; }
            }
        
            // 140 GOSUB 500 : PRINT : NEXT D
            subroutine(x, y, z, a, b, c);
            println!();
        }
        // 200 PRINT : PRINT "YOU HAVE BEEN TORPEDOED!  ABANDON SHIP!"
        // 210 PRINT "THE SUBMARINE WAS AT";A;",";B;",";C : GOTO 400
        print!("{}", format!("{}{}{}{}{}{}{}{}",
            "\nYOU HAVE BEEN TORPEDOED! ABANDON SHIP!\n",
            "THE SUBMARINE WAS AT ",
            a,
            ",",
            b,
            ",",
            c,
            "\n"
        ));
        
        replay();
    }
    // 600 END
}

// 500 PRINT "SONAR REPORTS SHOT WAS ";
// 510 IF Y>B THEN PRINT "NORTH";
// 520 IF Y<B THEN PRINT "SOUTH";
// 530 IF X>A THEN PRINT "EAST";
// 540 IF X<A THEN PRINT "WEST";
// 550 IF Y<>B OR X<>A THEN PRINT " AND";
// 560 IF Z>C THEN PRINT " TOO LOW."
// 570 IF Z<C THEN PRINT " TOO HIGH."
// 580 IF Z=C THEN PRINT " DEPTH OK."
// 590 RETURN
fn subroutine(x: i32, y: i32, z:i32, a:i32, b:i32, c:i32) {
    print!("SONAR REPORTS SHOT WAS ");
    if y>b { print!("NORTH"); };
    if y<b { print!("SOUTH"); };
    if x>a { print!("EAST"); };
    if x<a { print!("WEST"); };
    if y!=b || x!=a { print!(" AND"); };
    if z>c { print!(" TOO LOW."); };
    if z<c { print!(" TOO HIGH."); };
    if z==c { print!(" DEPTH OK."); };
}

fn replay() -> bool {
    let r = input("ANOTHER GAME (Y OR N): ");
    // 410 IF A$="Y" THEN 100
    if r == "Y" {
        return true;
    }
    else {
        // 420 PRINT "OK.  HOPE YOU ENJOYED YOURSELF." : GOTO 600
        println!("OK. HOPE YOU ENJOYED YOURSELF.");
        return false; 
    }
}