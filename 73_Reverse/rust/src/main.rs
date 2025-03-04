/** REVERSE GAME 
 * https://github.com/marquesrs/basic-computer-games/blob/main/73_Reverse/reverse.bas
 * Direct conversion from BASIC to Rust by Pablo Marques (marquesrs).
 * No additional features or improvements were added. As a faithful translation, 
 * many of the code here are done in an unrecommended way by today's standards.
 * 03/03/25
*/

use rand::Rng;
use std::io::Write;

fn input(msg: &str) -> String {
    print!("{}", msg);
    let _ =std::io::stdout().flush().unwrap();
    let mut input = String::new();
    std::io::stdin().read_line(&mut input).unwrap();
    return input.trim().to_uppercase();
}

fn main() {
    // 10 PRINT TAB(32);"REVERSE"
    // 20 PRINT TAB(15);"CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
    // 30 PRINT:PRINT:PRINT
    // 100 PRINT "REVERSE -- A GAME OF SKILL": PRINT
    print!("{}", format!("{}{}{}{}{}{}{}{}",
        " ".repeat(31),
        "REVERSE",
        "\n",
        " ".repeat(14),
        "CREATIVE COMPUTING MORRISTOWN, NEW JERSEY\n",
        "\n\n\n",
        "REVERSE -- A GAME OF SKILL\n",
        "\n"
    ));

    // 130 DIM A(20)
    let mut a = vec![0; 20];
    
    // 140 REM *** N=NUMBER OF NUMBERS
    // 150 N=9
    let n = 9;
    
    // 160 PRINT "DO YOU WANT THE RULES";
    // 170 INPUT A$
    let opt = input("DO YOU WANT THE RULES (YES OR NO)? ");

    
    if opt == "YES" || opt == "Y" {
        // 190 GOSUB 710        
        sub1(n);
    }
    // 180 IF A$="NO" THEN 210
    'c : loop {
        // 200 REM *** MAKE A RANDOM LIST A(1) TO A(N)
        // 210 A(1)=INT((N-1)*RND(1)+2)
        // element 0
        a[0] = ((n-1) as f32 * rand::rng().random_range(0.0..=1.0) + 2.0) as i32;
    
        // 220 FOR K=2 TO N
        for k in 2..=n {
            'a : loop {
                // element k
                // 230 A(K)=INT(N*RND(1)+1)
                a[k-1] = (n as f32 * rand::rng().random_range(0.0..=1.0) + 1.0) as i32;
                
                // element 0 to k-1
                // 240 FOR J=1 TO K-1
                for j in 1..k {
                    // 250 IF A(K)=A(J) THEN 230
                    if a[k-1] == a[j-1] {
                        continue 'a;
                    }
                    // 260 NEXT J:
                }
                break;
            }
            //NEXT K
        }    

        // 280 REM *** PRINT ORIGINAL LIST AND START GAME
        // 290 PRINT: PRINT "HERE WE GO ... THE LIST IS:"
        print!("\nHERE WE GO ... THE LIST IS:\n");

        // 310 T=0
        let mut t = 0;

        // 320 GOSUB 610
        sub2(&a, n);

        'b : loop {
            // 330 PRINT "HOW MANY SHALL I REVERSE";
            // 340 INPUT R
            let r = input("HOW MANY SHALL I REVERSE: ").parse::<usize>().unwrap();

            // 350 IF R=0 THEN 520
            if r == 0 {
                if replay() { continue 'c; }
                else { break 'c; }
            }
            // 360 IF R<=N THEN 390
            if r <= n {
                // 390 T=T+1
                t = t + 1;

                // 400 REM *** REVERSE R NUMBERS AND PRINT NEW LIST
                // 410 FOR K=1 TO INT(R/2)
                for k in 1..=((r/2) as usize) {
                    // 420 Z=A(K)
                    let z = a[k-1];
                    // 430 A(K)=A(R-K+1)
                    a[k-1] = a[r-k];
                    // 440 A(R-K+1)=Z
                    a[r-k] = z;

                    // 450 NEXT K
                }
                // 460 GOSUB 610
                sub2(&a, n);

                // 470 REM *** CHECK FOR A WIN
                // 480 FOR K=1 TO N
                for k in 1..=n {
                    // 490 IF A(K)<>K THEN 330
                    if a[k-1] != k as i32 {
                        continue 'b;
                    }
                    // 500 NEXT K
                }
                // 510 PRINT "YOU WON IT IN";T;"MOVES!!!":PRINT
                print!("{}{}{}", "YOU WON IT IN ", t, " MOVES!!!\n\n");

                if replay() { continue 'c; }
                else { break 'c; }
            }
            else {
                // 370 PRINT "OOPS! TOO MANY! I CAN REVERSE AT MOST";N:GOTO 330
                print!("OOPS! TOO MANY! I CAN REVERSE AT MOST {n}");
            }
        }
    }
    // 999 END
}

// 600 REM *** SUBROUTINE TO PRINT LIST
fn sub2(a: &Vec<i32>, n: usize) {
    // 610 PRINT:FOR K=1 TO N:PRINT A(K);:NEXT K
    // 650 PRINT:PRINT:RETURN
    for k in 1..=n {
        print!("{} ", a[k-1]);
    }
    print!("\n\n");
}

// 700 REM *** SUBROUTINE TO PRINT THE RULES
fn sub1(n: usize) {
    // 710 PRINT:PRINT "THIS IS THE GAME OF 'REVERSE'.  TO WIN, ALL YOU HAVE"
    // 720 PRINT "TO DO IS ARRANGE A LIST OF NUMBERS (1 THROUGH";N;")"
    // 730 PRINT "IN NUMERICAL ORDER FROM LEFT TO RIGHT.  TO MOVE, YOU"
    // 740 PRINT "TELL ME HOW MANY NUMBERS (COUNTING FROM THE LEFT) TO"
    // 750 PRINT "REVERSE.  FOR EXAMPLE, IF THE CURRENT LIST IS:"
    // 760 PRINT:PRINT "2 3 4 5 1 6 7 8 9"
    // 770 PRINT:PRINT "AND YOU REVERSE 4, THE RESULT WILL BE:"
    // 780 PRINT:PRINT "5 4 3 2 1 6 7 8 9"
    // 790 PRINT:PRINT "NOW IF YOU REVERSE 5, YOU WIN!"
    // 800 PRINT:PRINT "1 2 3 4 5 6 7 8 9":PRINT
    // 810 PRINT "NO DOUBT YOU WILL LIKE THIS GAME, BUT"
    // 820 PRINT "IF YOU WANT TO QUIT, REVERSE 0 (ZERO).":PRINT: RETURN

    print!("{}{}{}{}{}{}{}{}{}{}{}{}{}{}{}{}{}{}{}{}{}{}", 
        "\n",
        "THIS IS THE GAME OF 'REVERSE'. TO WIN, ALL YOU HAVE\n",
        "TO DO IS ARRANGE A LIST OF NUMBERS (1 THROUGH ",
        n,
        ")\n",
        "IN NUMERICAL ORDER FROM LEFT TO RIGHT. TO MOVE, YOU\n",
        "TELL ME HOW MANY NUMBERS (COUNTING FROM THE LEFT) TO\n",
        "REVERSE. FOR EXAMPLE, IF THE CURRENT LIST IS:\n",
        "\n",
        "2 3 4 5 1 6 7 8 9\n",
        "\n",
        "AND YOU REVERSE 4, THE RESULT WILL BE:\n",
        "\n",
        "5 4 3 2 1 6 7 8 9\n",
        "\n",
        "NOW IF YOU REVERSE 5, YOU WIN!\n",
        "\n",
        "1 2 3 4 5 6 7 8 9\n",
        "\n",
        "NO DOUBT YOU WILL LIKE THIS GAME, BUT\n",
        "IF YOU WANT TO QUIT, REVERSE 0 (ZERO).\n",
        "\n",
    )
}

fn replay() -> bool {
    // 520 PRINT
    // 530 PRINT "TRY AGAIN (YES OR NO)";
    // 540 INPUT A$
    let r = input("\nTRY AGAIN (YES OR NO): ");
    // 550 IF A$="YES" THEN 210
    if r == "YES" || r == "Y" {
        return true;
    }
    else {
        // 560 PRINT: PRINT "O.K. HOPE YOU HAD FUN!!":GOTO 999
        println!("\nO.K. HOPE YOU HAD FUN!!");
        return false; 
    }
}