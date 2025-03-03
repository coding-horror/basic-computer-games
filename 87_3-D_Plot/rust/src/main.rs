/** 3D Plot GAME 
 * https://github.com/coding-horror/basic-computer-games/blob/main/87_3-D_Plot/3dplot.bas
 * Direct conversion from BASIC to Rust by Pablo Marques (marquesrs).
 * No additional features or improvements were added. As a faithful translation, 
 * many of the code here are done in an unrecommended way by today's standards.
 * 03/03/25
*/

fn main() {
    //1 PRINT TAB(32);"3D PLOT"
    //2 PRINT TAB(15);"CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
    //3 PRINT:PRINT:PRINT
    print!("{}",
        format!("{}{}\n{}{}\n\n\n\n",
            " ".repeat(31),
            "3D PLOT",
            " ".repeat(14),
            "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
        )
    );

    //5 DEF FNA(Z)=30*EXP(-Z*Z/100)
    let fna = |z: f32| {30.0 * f32::exp(-z*z/100.0)};

    //100 PRINT
    println!();
    
    let mut line_content = String::new();
    //110 FOR X=-30 TO 30 STEP 1.5
    let mut x = -30.0;
    while x <= 30.0 {
        //120 L=0
        let mut l = 0;
        
        //130 Y1=5*INT(SQR(900-X*X)/5)
        let y1 = 5.0 * (f32::sqrt(900.0-x*x)/5.0).floor();

        //140 FOR Y=Y1 TO -Y1 STEP -5
        let mut y = y1;
        while y >= -y1 {
            //150 Z=INT(25+FNA(SQR(X*X+Y*Y))-.7*Y)
            let z = (25.0 + fna(f32::sqrt(x*x+y*y))-0.7*y) as i32;

            //160 IF Z<=L THEN 190
            if z <= l {
                y = y - 5.0;
                continue;
            }
            //170 L=Z
            l = z;
            //180 PRINT TAB(Z);"*";
            while (line_content.len() as i32) < (z-1) {
                line_content += " ";
            }
            line_content += "*";
            //190 NEXT Y
            y = y - 5.0;
        }
        print!("{}", line_content);
        line_content.clear();

        //200 PRINT
        println!();
        //210 NEXT X
        x = x + 1.5;
    }
    //300 END
}