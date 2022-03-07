//Goal of this port is to keep Basic lang idioms where possible. Gotta have those single letter capital variables!
use num_integer::{sqrt};
use std::io::{self, Write};
use text_io::{read, try_read};
#[allow(non_snake_case)]
fn main() {
    println!("{:>30}", "ROCKET");
    println!("{:>15}", "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
    println!();println!();println!();
    println!("LUNAR LANDING SIMULATION");
    println!("----- ------- ----------");println!();
    let A= input("DO YOU WANT INSTRUCTIONS (YES OR NO) ");
    if !(A=="NO") {
        println!();
        println!("YOU ARE LANDING ON THE MOON AND AND HAVE TAKEN OVER MANUAL");
        println!("CONTROL 1000 FEET ABOVE A GOOD LANDING SPOT. YOU HAVE A DOWN-");
        println!("WARD VELOCITY OF 50 FEET/SEC. 150 UNITS OF FUEL REMAIN.");
        println!();
        println!("HERE ARE THE RULES THAT GOVERN YOUR APOLLO SPACE-CRAFT:"); println!();
        println!("(1) AFTER EACH SECOND THE HEIGHT, VELOCITY, AND REMAINING FUEL");
        println!("    WILL BE REPORTED VIA DIGBY YOUR ON-BOARD COMPUTER.");
        println!("(2) AFTER THE REPORT A '?' WILL APPEAR. ENTER THE NUMBER");
        println!("    OF UNITS OF FUEL YOU WISH TO BURN DURING THE NEXT");
        println!("    SECOND. EACH UNIT OF FUEL WILL SLOW YOUR DESCENT BY");
        println!("    1 FOOT/SEC.");
        println!("(3) THE MAXIMUM THRUST OF YOUR ENGINE IS 30 FEET/SEC/SEC");
        println!("    OR 30 UNITS OF FUEL PER SECOND.");
        println!("(4) WHEN YOU CONTACT THE LUNAR SURFACE. YOUR DESCENT ENGINE");
        println!("    WILL AUTOMATICALLY SHUT DOWN AND YOU WILL BE GIVEN A");
        println!("    REPORT OF YOUR LANDING SPEED AND REMAINING FUEL.");
        println!("(5) IF YOU RUN OUT OF FUEL THE '?' WILL NO LONGER APPEAR");
        println!("    BUT YOUR SECOND BY SECOND REPORT WILL CONTINUE UNTIL");
        println!("    YOU CONTACT THE LUNAR SURFACE.");println!();
    }
    loop {
        println!("BEGINNING LANDING PROCEDURE..........");println!();
        println!("G O O D  L U C K ! ! !");
        println!();println!();
        println!("SEC  FEET      SPEED     FUEL     PLOT OF DISTANCE");
        println!();
        let mut T=0;let mut H:i32=1000;let mut V=50;let mut F=150;
        let D:i32; let mut V1:i32; let mut B:i32;
        'falling: loop {
            println!(" {:<4}{:<11}{:<10}{:<8}I{capsule:>high$}", T,H,V,F,high=(H/15) as usize,capsule="*");
            B = input_int("");
            if B<0 { B=0 }
            else { if B>30 { B=30 } }
            if B>F { B=F }
            'nofuel: loop {
                V1=V-B+5;
                F=F-B;
                H=H- (V+V1)/2;
                if  H<=0 { break 'falling}
                T=T+1;
                V=V1;
                if F>0 { break 'nofuel }
                if B!=0 {
                    println!("**** OUT OF FUEL ****");
                }
                println!(" {:<4}{:<11}{:<10}{:<8}I{capsule:>high$}", T,H,V,F,high=(H/12+29) as usize,capsule="*");
                B=0;
            }
        }
        H=H+ (V1+V)/2;
        if B==5 {
            D=H/V;
        } else {
            D=(-V+sqrt(V*V+H*(10-2*B)))/(5-B);
            V1=V+(5-B)*D;
        }
        println!("***** CONTACT *****");
        println!("TOUCHDOWN AT {} SECONDS.",T+D);
        println!("LANDING VELOCITY={} FEET/SEC.",V1);
        println!("{} UNITS OF FUEL REMAINING.", F);
        if V1==0 {
            println!("CONGRATULATIONS! A PERFECT LANDING!!");
            println!("YOUR LICENSE WILL BE RENEWED.......LATER.");
        }
        if V1.abs()>=2 {
            println!("***** SORRY, BUT YOU BLEW IT!!!!");
            println!("APPROPRIATE CONDOLENCES WILL BE SENT TO YOUR NEXT OF KIN.");
        }
        println!();println!();println!();
        let A = input("ANOTHER MISSION");
        if !(A=="YES") { break };
    }
    println!();println!( "CONTROL OUT.");println!();
}


fn input(prompt:&str) -> String {
    loop {
        print!("{} ? ",prompt);io::stdout().flush().unwrap();
        let innn:String=read!("{}\n");
        let out:String = innn.trim().to_string();
        if out!="" {return out}
    }
}
fn input_int(prompt:&str) -> i32 {
    loop {
        print!("{} ? ",prompt);io::stdout().flush().unwrap();
        match try_read!() {
            Ok(n) => return n,
            Err(_) => println!("Enter a number 0-30"),
        }
    }
}
