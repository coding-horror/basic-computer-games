/** BOUNCE GAME 
 * https://github.com/coding-horror/basic-computer-games/blob/main/13_Bounce/bounce.bas
 * Direct conversion from BASIC to Rust by Pablo Marques (marquesrs).
 * No additional features or improvements were added. As a faithful translation, 
 * many of the code here are done in an unrecommended way by today's standards.
 * 03/03/25
*/

use std::io::Write;

fn input(msg: &str) -> String {
    print!("{}", msg);
    let _ =std::io::stdout().flush().unwrap();
    let mut input = String::new();
    std::io::stdin().read_line(&mut input).unwrap();
    return input.trim().to_uppercase();
}

fn main() {
    //10 PRINT TAB(33);"BOUNCE"
    //20 PRINT TAB(15);"CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
    //30 PRINT:PRINT:PRINT
    print!("{}{}\n{}{}\n\n\n",
        " ".repeat(33),
        "BOUNCE",
        " ".repeat(15),
        "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
    );

    //90 DIM T(20)
    let mut t: Vec<f32> = Vec::with_capacity(20);
    
    //100 PRINT "THIS SIMULATION LETS YOU SPECIFY THE INITIAL VELOCITY"
    //110 PRINT "OF A BALL THROWN STRAIGHT UP, AND THE COEFFICIENT OF"
    //120 PRINT "ELASTICITY OF THE BALL.  PLEASE USE A DECIMAL FRACTION"
    //130 PRINT "COEFFICIENCY (LESS THAN 1)."
    //131 PRINT
    //132 PRINT "YOU ALSO SPECIFY THE TIME INCREMENT TO BE USED IN"
    //133 PRINT "'STROBING' THE BALL'S FLIGHT (TRY .1 INITIALLY)."
    //134 PRINT
    print!("{}\n{}\n{}\n{}\n\n{}\n{}\n\n",
        "THIS SIMULATION LETS YOU SPECIFY THE INITIAL VELOCITY",
        "OF A BALL THROWN STRAIGHT UP, AND THE COEFFICIENT OF",
        "ELASTICITY OF THE BALL.  PLEASE USE A DECIMAL FRACTION",
        "COEFFICIENCY (LESS THAN 1).",
        "YOU ALSO SPECIFY THE TIME INCREMENT TO BE USED IN",
        "'STROBING' THE BALL'S FLIGHT (TRY .1 INITIALLY).",
    );

    loop {
       //135 INPUT "TIME INCREMENT (SEC)";S2
        let s2 = input("TIME INCREMENT (SEC): ").parse::<f32>().unwrap();
        //let s2 = 0.2f32;

        //140 PRINT
        println!();
        
        //150 INPUT "VELOCITY (FPS)";V
        let v = input("VELOCITY (FPS): ").parse::<f32>().unwrap();
        //let v = 20.0f32;

        //160 PRINT
        println!();

        //170 INPUT "COEFFICIENT";C
        let c = input("COEFFICIENT: ").parse::<f32>().unwrap();
        //let c = 0.6f32;

        //180 PRINT
        //182 PRINT "FEET"
        //184 PRINT
        print!("\nFEET\n\n");

        //186 S1=INT(70/(V/(16*S2))) // verified
        let s1 = (70.0 / (v/(16.0*s2))) as i32;

        //190 FOR I=1 TO S1
        for i in 1..=s1 {
            //200 T(I)=V*C^(I-1)/16
            t.push(v * c.powf(i as f32 - 1.0) / 16.0); // verified
            //210 NEXT I
        }

        let mut l = 0.0;

        //220 FOR H=INT(-16*(V/32)^2+V^2/32+.5) TO 0 STEP -.5
        let mut h = (-16.0 * (v / 32.0).powi(2) + (v.powi(2)) / 32.0 + 0.5).floor();
        while h >= 0.0 {
            let mut line_content = String::new();
            //221 IF INT(H)<>H THEN 225
            if h.floor() == h {
                //222 PRINT H;
                line_content.push_str(h.to_string().as_str());
                line_content.push(' ');
            }
            //225 L=0
            l = 0.0;
            //230 FOR I=1 TO S1
            for i in 1..=s1 {
                let mut t_val = 0.0;
                //240 FOR T=0 TO T(I) STEP S2
                while t_val <= t[(i - 1) as usize] {
                    //245 L=L+S2
                    l = l + s2;
                    
                    //250 IF ABS(H-(.5*(-32)*T^2+V*C^(I-1)*T))>.25 THEN 270
                    let condition = h - (0.5 * (-32.0) * t_val.powf(2.0) + v * c.powf((i-1) as f32) * t_val);
                    if condition.abs() >= 0.25{
                        t_val = t_val + s2;
                        continue;
                    }
                    // TABS ARE NOT SPACES, BUT A TERMINAL POSITION
                    //260 PRINT TAB(L/S2);"0";
                    let spaces = ((l / s2) - 1.0) as usize;
                    while line_content.len() < spaces  {
                        line_content.push(' ');
                    }
                    line_content.push('0');
                   
                    //270 NEXT T
                    t_val = t_val + s2;
                }
                
                //275 T=T(I+1)/2
                if i as usize == t.len() { break; }
                t_val = t[i as usize] / 2.0;

                //276 IF -16*T^2+V*C^(I-1)*T<H THEN 290
                if -16.0 * t_val.powf(2.0) + v * c.powf(i as f32 -1.0) * t_val <= h {
                    break;
                }

                //280 NEXT I
            }
            print!("{}", line_content);
            //290 PRINT
            println!();

            //300 NEXT H
            h = h - 0.5;
        }

        let mut line_content = String::from("");

        //310 PRINT TAB(1);
        print!(" ");

        //320 FOR I=1 TO INT(L+1)/S2+1
        for _ in 1..=((l+1.0) / s2 + 1.0) as i32 {
            //330 PRINT ".";
            line_content.push('.');
            //340 NEXT I
        }

        //350 PRINT
        //355 PRINT " 0";
        println!("{}", line_content);

        line_content = String::from(" 0");
        
        //360 FOR I=1 TO INT(L+.9995)
        for i in 1..=((l + 0.9995) as i32) {
            //380 PRINT TAB(INT(I/S2));I;
            while line_content.len() < (i as f32 / s2) as usize {
            line_content.push(' ');
            }
            line_content.push_str(i.to_string().as_str());
            //390 NEXT I
        }

        println!("{}", line_content);

        //400 PRINT
        //410 PRINT TAB(INT(L+1)/(2*S2)-2);"SECONDS"
        //420 PRINT
        let tabs = ((l+1.0) / (2.0 * s2) - 2.0) as usize;
        println!("{}SECONDS\n", " ".repeat(tabs));

        //430 GOTO 135
        //break;
    }
    //440 END
}