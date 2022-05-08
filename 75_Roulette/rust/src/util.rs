use std::ops::RangeInclusive;

pub const INSTRUCTIONS: [&str; 38] = [
    "\nTHIS IS THE BETTING LAYOUT",
    "\n(*=RED)\n",
    "1*\t2\t3*",
    "4\t5*\t6",
    "7*\t8\t9*",
    "10\t11\t12*",
    "---------------------",
    "13\t14*\t15",
    "16*\t17\t18*",
    "19*\t20\t21*",
    "22\t23*\t24",
    "---------------------",
    "25*\t26\t27*",
    "28\t29\t30*",
    "31\t32*\t33",
    "34*\t35\t36*",
    "---------------------",
    "\t\t00\t0\n",
    "TYPES OF BETS\n",
    "THE NUMBERS 1 TO 36 SIGNIFY A STRAIGHT BET",
    "ON THAT NUMBER",
    "THESE PAY OFF 35:1\n",
    "THE 2:1 BETS ARE:",
    "37) 1-12\t40) FIRST COLUMN",
    "38) 13-24\t41) SECOND COLUMN",
    "39) 25-36\t42) THIRD COLUMN\n",
    "THE EVEN MONEY BETS ARE:",
    "43) 1-18\t46) ODD",
    "44) 19-36\t47) RED",
    "45) EVEN\t48) BLACK\n",
    "\n49)0 AND 50)00 PAY OFF 35:1",
    "NOTE: 0 AND 00 DO NOT COUNT UNDER ANY",
    "\tBETS EXCEPT THEIR OWN\n",
    "WHEN I ASK FOR EACH BET,TYPE THE NUMBER",
    "AND THE AMOUNT,SEPARATED BY A COMMA",
    "FOR EXAMPLE:TO BET $500 ON BLACK,TYPE 48,500",
    "WHEN I ASK FOR A BET\n",
    "MINIMUM BET IS $5,MAXIMUM IS $500\n",
];

pub const REDS: [u8; 18] = [
    1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36,
];

pub fn print_check(money: usize) {
    let name = morristown::prompt_string("TO WHOM SHALL I MAKE THE CHECK?");
    //let _check_no; //  random
    /*PRINT THE CHECK */
    todo!()
}

pub fn process_bet(bet_num: u8, spin: u8) -> (bool, u8) {
    match bet_num {
        1..=36 => (bet_num == spin, 35),
        37 => (is_within_range(1..=12, spin), 2),
        38 => (is_within_range(13..=24, spin), 2),
        39 => (is_within_range(25..=36, spin), 2),
        40 => (spin % 3 == 1, 2),
        41 => (spin % 3 == 2, 2),
        42 => (spin % 3 == 0, 2),
        43 => (is_within_range(1..=18, spin), 1),
        44 => (is_within_range(19..=36, spin), 1),
        45 => (spin % 2 == 0, 1),
        46 => (spin % 2 == 1, 1),
        47 => (REDS.contains(&spin), 1),
        48 => (!REDS.contains(&spin), 1),
        _ => {
            println!("##INVALID BET##");
            return (false, 0);
        }
    }
}

fn is_within_range(r: RangeInclusive<u8>, n: u8) -> bool {
    r.contains(&n)
}
