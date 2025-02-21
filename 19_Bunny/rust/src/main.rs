/** BUNNY GAME 
 * https://github.com/coding-horror/basic-computer-games/tree/main/19_Bunny
 * Direct conversion from BASIC to Rust by Pablo Marques (marquesrs).
 * No additional features or improvements were added. As a faithful translation, 
 * many of the code here are done in an unrecommended way by today's standards.
 * 17/02/25
*/

//290 DATA 2,21,14,14,25
//300 DATA 1,2,-1,0,2,45,50,-1,0,5,43,52,-1,0,7,41,52,-1
//310 DATA 1,9,37,50,-1,2,11,36,50,-1,3,13,34,49,-1,4,14,32,48,-1
//320 DATA 5,15,31,47,-1,6,16,30,45,-1,7,17,29,44,-1,8,19,28,43,-1
//330 DATA 9,20,27,41,-1,10,21,26,40,-1,11,22,25,38,-1,12,22,24,36,-1
//340 DATA 13,34,-1,14,33,-1,15,31,-1,17,29,-1,18,27,-1
//350 DATA 19,26,-1,16,28,-1,13,30,-1,11,31,-1,10,32,-1
//360 DATA 8,33,-1,7,34,-1,6,13,16,34,-1,5,12,16,35,-1
//370 DATA 4,12,16,35,-1,3,12,15,35,-1,2,35,-1,1,35,-1
//380 DATA 2,34,-1,3,34,-1,4,33,-1,6,33,-1,10,32,34,34,-1
//390 DATA 14,17,19,25,28,31,35,35,-1,15,19,23,30,36,36,-1
//400 DATA 14,18,21,21,24,30,37,37,-1,13,18,23,29,33,38,-1
//410 DATA 12,29,31,33,-1,11,13,17,17,19,19,22,22,24,31,-1
//420 DATA 10,11,17,18,22,22,24,24,29,29,-1
//430 DATA 22,23,26,29,-1,27,29,-1,28,29,-1,4096

// 4096 is the end of file
// -1 is the end of line
const DATA: [i32;233] = [
    2,21,14,14,25,
    1,2,-1,0,2,45,50,-1,0,5,43,52,-1,0,7,41,52,-1,
    1,9,37,50,-1,2,11,36,50,-1,3,13,34,49,-1,4,14,32,48,-1,
    5,15,31,47,-1,6,16,30,45,-1,7,17,29,44,-1,8,19,28,43,-1,
    9,20,27,41,-1,10,21,26,40,-1,11,22,25,38,-1,12,22,24,36,-1,
    13,34,-1,14,33,-1,15,31,-1,17,29,-1,18,27,-1,
    19,26,-1,16,28,-1,13,30,-1,11,31,-1,10,32,-1,
    8,33,-1,7,34,-1,6,13,16,34,-1,5,12,16,35,-1,
    4,12,16,35,-1,3,12,15,35,-1,2,35,-1,1,35,-1,
    2,34,-1,3,34,-1,4,33,-1,6,33,-1,10,32,34,34,-1,
    14,17,19,25,28,31,35,35,-1,15,19,23,30,36,36,-1,
    14,18,21,21,24,30,37,37,-1,13,18,23,29,33,38,-1,
    12,29,31,33,-1,11,13,17,17,19,19,22,22,24,31,-1,
    10,11,17,18,22,22,24,24,29,29,-1,
    22,23,26,29,-1,27,29,-1,28,29,-1,4096,
];

fn main() {
    let mut col = 0;

    //10 PRINT TAB(33);"BUNNY"
    //20 PRINT TAB(15);"CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
    //30 PRINT: PRINT: PRINT
    //100 REM  "BUNNY" FROM DAVID AHL'S 'BASIC COMPUTER GAMES'
    print!("{}{}\n{}{}\n",
        " ".repeat(33),
        "BUNNY",
        " ".repeat(15),
        "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
    );

    //120 FOR I=0 TO 4: READ B(I): NEXT I
    let mut b = [0;5];
    for i in 0..=4 {
        b[i] = DATA[col];
        col = col + 1;
    }

    //130 GOSUB 260
    //260 FOR I=1 TO 6: PRINT CHR$(10);: NEXT I
    for _ in 1..=6 {
        println!();
    }
    //270 RETURN
    
    //140 L=64: REM  ASCII LETTER CODE...
    let l = 64;
    let mut prev = 0;
    loop {
        //170 READ X: IF X<0 THEN 160
        let x = DATA[col];
        col = col + 1;
        if x < 0 {
            //160 PRINT
            println!();
            prev = 0;
            continue;
        }
        //175 IF X>128 THEN 240
        else if x > 128 {
            //240 GOSUB 260: GOTO 450
            for _ in 1..=6 {
                println!();
            }
            break;
        }
        else {
            //180 PRINT TAB(X);: READ Y
            print!("{}", " ".repeat((x - prev) as usize)); // verificar
            prev = x;
            let y = DATA[col];
            col = col + 1;
            //190 FOR I=X TO Y: J=I-5*INT(I/5)
            for i in x..=y {
                let j = i - 5 * (i / 5);
                //200 PRINT CHR$(L+B(J));
                print!("{}", (l + b[j as usize]) as u8 as char);
                prev = prev + 1;
                //210 NEXT I

            }
            //220 GOTO 170
        }
    }
    //450 END
}

