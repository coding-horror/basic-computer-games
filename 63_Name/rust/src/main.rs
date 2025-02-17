/** NAME GAME BY GEOFFREY CHASE
 * https://github.com/coding-horror/basic-computer-games/blob/main/63_Name/name.bas
 * Direct conversion from BASIC to Rust by Pablo Marques (marquesrs).
 * No additional features or improvements were added. As a faithful translation, 
 * many of the code here are done in an unrecommended way by today's standards.
 * 17/02/25
*/

use std::io::Write;

fn main() {
    let mut input = String::new();
    
    //1 PRINT TAB(34);"NAME"
    //2 PRINT TAB(15);"CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
    //3 PRINT: PRINT: PRINT
    print!("{}{}\n{}{}\n\n\n\n",
        " ".repeat(34),
        "NAME",
        " ".repeat(15),
        "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
    );
    
    //5 DIM B$(40)
    let mut b = [0; 40];
    
    //10 PRINT "HELLO.": PRINT "MY NAME IS CREATIVE COMPUTER."
    //20 PRINT "WHAT'S YOUR NAME (FIRST AND LAST";: INPUT A$: L=LEN(A$)
    print!("{}\n{}\n{}",
        "HELLO.",
        "MY NAME IS CREATIVE COMPUTER.",
        "WHAT'S YOUR NAME (FIRST AND LAST)? "
    );
    
    let _ = std::io::stdout().flush().unwrap();
    std::io::stdin().read_line(&mut input).unwrap();
    let a = input.trim().to_uppercase();
    let l = a.len();
    
    //30 PRINT: PRINT "THANK YOU, ";
    print!("\nTHANK YOU, ");
    
    //40 FOR I=1 TO L: B$(I)=MID$(A$,I,1): NEXT I
    for i in 1..=l {
        b[i-1] = a.chars().nth(i-1).unwrap() as u8;
    }
    
    //50 FOR I=L TO 1 STEP -1: PRINT B$(I);: NEXT I
    for i in (1..=l).rev() {
        print!("{}", b[i-1] as char);
    }
    
    //60 PRINT ".": PRINT "OOPS!  I GUESS I GOT IT BACKWARDS.  A SMART"
    //70 PRINT "COMPUTER LIKE ME SHOULDN'T MAKE A MISTAKE LIKE THAT!": PRINT
    //80 PRINT "BUT I JUST NOTICED YOUR LETTERS ARE OUT OF ORDER."
    //90 PRINT "LET'S PUT THEM IN ORDER LIKE THIS: ";
    print!("{}\n{}\n{}\n\n{}\n{}",
        ".",
        "OOPS! I GUESS I GOT IT BACKWARDS. A SMART",
        "COMPUTER LIKE ME SHOULDN'T MAKE A MISTAKE LIKE THAT!",
        "BUT I JUST NOTICED YOUR LETTERS ARE OUT OF ORDER.",
        "LET'S PUT THEM IN ORDER LIKE THIS: "
    );

    //100 FOR J=2 TO L: I=J-1: T$=B$(J)
    let mut i;
    let mut t;
    for j in 2..=l { 
        i = j - 1;
        t = b[j-1];
        loop {
            //110 IF T$>B$(I) THEN 130
            if i == 0 || t > b[i-1] {
                //130 B$(I+1)=T$: NEXT J
                b[i] = t;
                break;
            } else {
                //120 B$(I+1)=B$(I): I=I-1: IF I>0 THEN 110
                b[i] = b[i-1];
                i = i - 1;
            }
        } 
    }
    
    //140 FOR I=1 TO L: PRINT B$(I);: NEXT I: PRINT: PRINT
    for i in 1..=l {
        print!("{}", b[i-1] as char);
    }
    print!("\n\n");
    
    //150 PRINT "DON'T YOU LIKE THAT BETTER";: INPUT D$
    print!("DON'T YOU LIKE THAT BETTER? ");
    
    let _ = std::io::stdout().flush().unwrap();
    input.clear();
    std::io::stdin().read_line(&mut input).unwrap();
    let d = input.trim().to_uppercase();
    
    //160 IF D$="YES" THEN 180
    if d == "YES" || d == "Y" {
        //180 PRINT: PRINT "I KNEW YOU'D AGREE!!"
        print!("\nI KNEW YOU'D AGREE!!");
    }
    else {
        //170 PRINT: PRINT "I'M SORRY YOU DON'T LIKE IT THAT WAY.": GOTO 200
        print!("\nI'M SORRY YOU DON'T LIKE IT THAT WAY.");
    }
    
    print!("\n\n{}{}.\n{}\n",
        "I REALLY ENJOYED MEETING YOU ",
        a,
        "HAVE A NICE DAY!"
    );
    
    //999 END
}
