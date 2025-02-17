/** HELLO GAME BY DAVID AHL
 * https://github.com/coding-horror/basic-computer-games/blob/main/45_Hello/hello.bas
 * Direct conversion from BASIC to Rust by Pablo Marques (marquesrs).
 * No additional features or improvements were added. As a faithful translation, 
 * many of the code here are done in an unrecommended way by today's standards.
 * 17/02/25
*/

use std::io::Write;

fn main() {
    // 2 PRINT TAB(33);"HELLO"
    // 4 PRINT TAB(15);"CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
    // 6 PRINT: PRINT: PRINT
    print!(
        "{}{}\n{}{}\n\n\n\n", 
        " ".repeat(33),
        "HELLO",
        " ".repeat(15),
        "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
    );

    let mut input = String::new();
    
    //10 PRINT "HELLO.  MY NAME IS CREATIVE COMPUTER."
    //20 PRINT: PRINT: INPUT "WHAT'S YOUR NAME";N$: PRINT
    print!("HELLO. MY NAME IS CREATIVE COMPUTER.\n\nWHAT'S YOUR NAME? ");
    let _ = std::io::stdout().flush().unwrap();
    input.clear();
    std::io::stdin().read_line(&mut input).unwrap();
    let n = input.trim().to_uppercase();
    
    //30 PRINT "HI THERE, ";N$;", ARE YOU ENJOYING YOURSELF HERE";
    //40 INPUT B$: PRINT
    print!("\nHI THERE, {n}, ARE YOU ENJOYING YOURSELF HERE? ");
    loop {
        let _ = std::io::stdout().flush().unwrap();
        input.clear();
        std::io::stdin().read_line(&mut input).unwrap();
        let b = input.trim().to_uppercase();

        //50 IF B$="YES" THEN 70    
        if b == "YES" {
            //70 PRINT "I'M GLAD TO HEAR THAT, ";N$;".": PRINT
            //75 GOTO 100
            println!("\nI'M GLAD TO HEAR THAT, {n}.");
            break;
        }
        //55 IF B$="NO" THEN 80
        else if b == "NO" { 
            //80 PRINT "OH, I'M SORRY TO HEAR THAT, ";N$;". MAYBE WE CAN"
            //85 PRINT "BRIGHTEN UP YOUR VISIT A BIT."
            println!("\nOH, I'M SORRY TO HEAR THAT, {n}. MAYBE WE CAN\n{}",
                "BRIGHTEN UP YOUR VISIT A BIT."
            );
            break;
        }
        else {
            //60 PRINT N$;", I DON'T UNDERSTAND YOUR ANSWER OF '";B$;"'."
            //65 PRINT "PLEASE ANSWER 'YES' OR 'NO'.  DO YOU LIKE IT HERE";: GOTO 40
            print!("\n{n}, I DON'T UNDERSTAND YOUR ANSWER OF '{b}'.\n{}",
                "PLEASE ANSWER 'YES' OR 'NO'.  DO YOU LIKE IT HERE? "
            );
        }
    }
    
    //100 PRINT
    //105 PRINT "SAY, ";N$;", I CAN SOLVE ALL KINDS OF PROBLEMS EXCEPT"
    //110 PRINT "THOSE DEALING WITH GREECE.  WHAT KIND OF PROBLEMS DO"
    //120 PRINT "YOU HAVE (ANSWER SEX, HEALTH, MONEY, OR JOB)";
    //125 INPUT C$
    //126 PRINT
    print!("\nSAY, {n}, I CAN SOLVE ALL KINDS OF PROBLEMS EXCEPT\n{}\n{}",
        "THOSE DEALING WITH GREECE.  WHAT KIND OF PROBLEMS DO",
        "YOU HAVE (ANSWER SEX, HEALTH, MONEY, OR JOB)? "
    );
    'outer: loop {
        let _ = std::io::stdout().flush().unwrap();
        input.clear();
        std::io::stdin().read_line(&mut input).unwrap();
        let c = input.trim().to_uppercase();

        //130 IF C$="SEX" THEN 200
        if c == "SEX" {
            loop {
                //200 INPUT "IS YOUR PROBLEM TOO MUCH OR TOO LITTLE";D$: PRINT
                print!("\nIS YOUR PROBLEM TOO MUCH OR TOO LITTLE? ");
                let _ = std::io::stdout().flush().unwrap();
                input.clear();
                std::io::stdin().read_line(&mut input).unwrap();
                let d = input.trim().to_uppercase();
                
                //210 IF D$="TOO MUCH" THEN 220
                if d == "TOO MUCH" {
                    //220 PRINT "YOU CALL THAT A PROBLEM?!!  I SHOULD HAVE SUCH PROBLEMS!"
                    //225 PRINT "IF IT BOTHERS YOU, ";N$;", TAKE A COLD SHOWER."
                    //228 GOTO 250
                    println!("\nYOU CALL THAT A PROBLEM?!! I SHOULD HAVE SUCH PROBLEMS!\n{}",
                        format!("IF IT BOTHERS YOU, {n}, TAKE A COLD SHOWER.")
                    );
                    break;
                }
                //212 IF D$="TOO LITTLE" THEN 230
                else if d == "TOO LITTLE" {
                    //230 PRINT "WHY ARE YOU HERE IN SUFFERN, ";N$;"?  YOU SHOULD BE"
                    //235 PRINT "IN TOKYO OR NEW YORK OR AMSTERDAM OR SOMEPLACE WITH SOME"
                    //240 PRINT "REAL ACTION."
                    //250 PRINT
                    println!("\nWHY ARE YOU HERE IN SUFFERN, {n}? YOU SHOULD BE\n{}\n{}",
                        "IN TOKYO OR NEW YORK OR AMSTERDAM OR SOMEPLACE WITH",
                        "SOME REAL ACTION."
                    );
                    break;
                }
                else {
                    //215 PRINT "DON'T GET ALL SHOOK, ";N$;", JUST ANSWER THE QUESTION"
                    //217 INPUT "WITH 'TOO MUCH' OR 'TOO LITTLE'.  WHICH IS IT";D$:GOTO 210
                    println!("\nDON'T GET ALL SHOOK, {n}, JUST ANSWER THE QUESTION\n{}",
                        "WITH 'TOO MUCH' OR 'TOO LITTLE'. WHICH IS IT? "
                    );
                }
            }
        }
        //132 IF C$="HEALTH" THEN 180
        else if c == "HEALTH" {
            //180 PRINT "MY ADVICE TO YOU ";N$;" IS:"
            //185 PRINT "     1.  TAKE TWO ASPRIN"
            //188 PRINT "     2.  DRINK PLENTY OF FLUIDS (ORANGE JUICE, NOT BEER!)"
            //190 PRINT "     3.  GO TO BED (ALONE)"
            //195 GOTO 250
            println!("\nMY ADVICE TO YOU {n} IS:\n{}\n{}\n{}",
                "     1.  TAKE TWO ASPRIN",
                "     2.  DRINK PLENTY OF FLUIDS (ORANGE JUICE, NOT BEER!)",
                "     3.  GO TO BED (ALONE)"
            );
        }
        //134 IF C$="MONEY" THEN 160
        else if c == "MONEY" {
            //160 PRINT "SORRY, ";N$;", I'M BROKE TOO.  WHY DON'T YOU SELL"
            //162 PRINT "ENCYCLOPEADIAS OR MARRY SOMEONE RICH OR STOP EATING"
            //164 PRINT "SO YOU WON'T NEED SO MUCH MONEY?"
            //170 GOTO 250
            println!("\nSORRY, {n}, I'M BROKE TOO.  WHY DON'T YOU SELL\n{}\n{}",
                "ENCYCLOPEADIAS OR MARRY SOMEONE RICH OR STOP EATING",
                "SO YOU WON'T NEED SO MUCH MONEY? "
            );
        }
        //136 IF C$="JOB" THEN 145
        else if c == "JOB" {
            //145 PRINT "I CAN SYMPATHIZE WITH YOU ";N$;".  I HAVE TO WORK"
            //148 PRINT "VERY LONG HOURS FOR NO PAY -- AND SOME OF MY BOSSES"
            //150 PRINT "REALLY BEAT ON MY KEYBOARD.  MY ADVICE TO YOU, ";N$;","
            //153 PRINT "IS TO OPEN A RETAIL COMPUTER STORE.  IT'S GREAT FUN."
            //155 GOTO 250
            println!("\nI CAN SYMPATHIZE WITH YOU {n}.  I HAVE TO WORK\n{}\n{}\n{}",
                "VERY LONG HOURS FOR NO PAY -- AND SOME OF MY BOSSES",
                format!("REALLY BEAT ON MY KEYBOARD.  MY ADVICE TO YOU, {n}"),
                "IS TO OPEN A RETAIL COMPUTER STORE.  IT'S GREAT FUN."
            );
        }
        else {
            //138 PRINT "OH, ";N$;", YOUR ANSWER OF ";C$;" IS GREEK TO ME."
            //140 GOTO 250
            println!("\nOH, {n}, YOUR ANSWER OF {c} IS GREEK TO ME.");
        }
        
        loop {
            //255 PRINT "ANY MORE PROBLEMS YOU WANT SOLVED, ";N$;
            //260 INPUT E$: PRINT
            print!("\nANY MORE PROBLEMS YOU WANT SOLVED, {n}? ");
            let _ = std::io::stdout().flush().unwrap();
            input.clear();
            std::io::stdin().read_line(&mut input).unwrap();
            let e = input.trim().to_uppercase();
            
            //270 IF E$="YES" THEN 280
            if e == "YES" {
                //280 PRINT "WHAT KIND (SEX, MONEY, HEALTH, JOB)";
                //282 GOTO 125
                print!("\nWHAT KIND (SEX, MONEY, HEALTH, JOB)? ");
                continue 'outer;
            }
            //273 IF E$="NO" THEN 300
            else if e == "NO" {
                break 'outer;
            }
            else {
                //275 PRINT "JUST A SIMPLE 'YES' OR 'NO' PLEASE, ";N$;"."
                //277 GOTO 255
                println!("\nJUST A SIMPLE 'YES' OR 'NO' PLEASE, {n}.");
            }
        }
    }
    //300 PRINT
    //302 PRINT "THAT WILL BE $5.00 FOR THE ADVICE, ";N$;"."
    //305 PRINT "PLEASE LEAVE THE MONEY ON THE TERMINAL."
    println!("\nTHAT WILL BE $5.00 FOR THE ADVICE, {n}.\n{}",
        "PLEASE LEAVE THE MONEY ON THE TERMINAL."
    );
    //307 FOR I=1 TO 2000: NEXT I
    //310 PRINT: PRINT: PRINT
    loop {
        //315 PRINT "DID YOU LEAVE THE MONEY";
        //320 INPUT G$: PRINT
        print!("\nDID YOU LEAVE THE MONEY? ");
        let _ = std::io::stdout().flush().unwrap();
        input.clear();
        std::io::stdin().read_line(&mut input).unwrap();
        let g = input.trim().to_uppercase();
        
        //325 IF G$="YES" THEN 350
        if g == "YES" {
            //350 PRINT "HEY, ";N$;"??? YOU LEFT NO MONEY AT ALL!"
            //355 PRINT "YOU ARE CHEATING ME OUT OF MY HARD-EARNED LIVING."
            //360 PRINT:PRINT "WHAT A RIP OFF, ";N$;"!!!":PRINT
            //365 GOTO 385
            println!("\nHEY, {n}??? YOU LEFT NO MONEY AT ALL!\n{}\n{}",
                "YOU ARE CHEATING ME OUT OF MY HARD-EARNED LIVING.",
                format!("WHAT A RIP OFF, {n}!!!\n\nTAKE A WALK, {n}")
            );
            break;
        }
        //330 IF G$="NO" THEN 370
        else if g == "NO" {
            //370 PRINT "THAT'S HONEST, ";N$;", BUT HOW DO YOU EXPECT"
            //375 PRINT "ME TO GO ON WITH MY PSYCHOLOGY STUDIES IF MY PATIENTS"
            //380 PRINT "DON'T PAY THEIR BILLS?"
            //385 PRINT:PRINT "TAKE A WALK, ";N$;".":PRINT:PRINT:GOTO 999
            println!("\nTHAT'S HONEST, {n}, BUT HOW DO YOU EXPECT\n{}\n{}",
                "ME TO GO ON WITH MY PSYCHOLOGY STUDIES IF MY PATIENTS",
                format!("DON'T PAY THEIR BILLS?\n\nTAKE A WALK, {n}")
            );
            break;
        }
        else {
            //335 PRINT "YOUR ANSWER OF '";G$;"' CONFUSES ME, ";N$;"."
            //340 PRINT "PLEASE RESPOND WITH 'YES' OR 'NO'.": GOTO 315
            println!("YOUR ANSWER OF '{g}' CONFUSES ME, {n}.\n{}",
                "PLEASE RESPOND WITH 'YES' OR 'NO'."
            );
        }    
    }
    
    //390 PRINT "NICE MEETING YOU, ";N$;", HAVE A NICE DAY." -> unreachable
    //400 REM
    //999 END
}
