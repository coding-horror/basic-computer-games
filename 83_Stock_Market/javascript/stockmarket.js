// STOCKMARKET
//
// Converted from BASIC to Javascript by Oscar Toledo G. (nanochess)
//

function print(str)
{
    document.getElementById("output").appendChild(document.createTextNode(str));
}

function input()
{
    var input_element;
    var input_str;
    
    return new Promise(function (resolve) {
                       input_element = document.createElement("INPUT");
                       
                       print("? ");
                       input_element.setAttribute("type", "text");
                       input_element.setAttribute("length", "50");
                       document.getElementById("output").appendChild(input_element);
                       input_element.focus();
                       input_str = undefined;
                       input_element.addEventListener("keydown", function (event) {
                                                      if (event.keyCode == 13) {
                                                      input_str = input_element.value;
                                                      document.getElementById("output").removeChild(input_element);
                                                      print(input_str);
                                                      print("\n");
                                                      resolve(input_str);
                                                      }
                                                      });
                       });
}

function tab(space)
{
    var str = "";
    while (space-- > 0)
        str += " ";
    return str;
}

var sa = [];
var pa = [];
var za = [];
var ca = [];
var i1;
var n1;
var e1;
var i2;
var n2;
var e2;
var x1;
var w3;
var t8;
var a;
var s4;

// New stock values - subroutine
function randomize_initial()
{
    // RANDOMLY PRODUCE NEW STOCK VALUES BASED ON PREVIOUS
    // DAY'S VALUES
    // N1,N2 ARE RANDOM NUMBERS OF DAYS WHICH RESPECTIVELY
    // DETERMINE WHEN STOCK I1 WILL INCREASE 10 PTS. AND STOCK
    // I2 WILL DECREASE 10 PTS.
    // IF N1 DAYS HAVE PASSED, PICK AN I1, SET E1, DETERMINE NEW N1
    if (n1 <= 0) {
        i1 = Math.floor(4.99 * Math.random() + 1);
        n1 = Math.floor(4.99 * Math.random() + 1);
        e1 = 1;
    }
    // IF N2 DAYS HAVE PASSED, PICK AN I2, SET E2, DETERMINE NEW N2
    if (n2 <= 0) {
        i2 = Math.floor(4.99 * Math.random() + 1);
        n2 = Math.floor(4.99 * Math.random() + 1);
        e2 = 1;
    }
    // DEDUCT ONE DAY FROM N1 AND N2
    n1--;
    n2--;
    // LOOP THROUGH ALL STOCKS
    for (i = 1; i <= 5; i++) {
        x1 = Math.random();
        if (x1 < 0.25) {
            x1 = 0.25;
        } else if (x1 < 0.5) {
            x1 = 0.5;
        } else if (x1 < 0.75) {
            x1 = 0.75;
        } else {
            x1 = 0.0;
        }
        // BIG CHANGE CONSTANT:W3  (SET TO ZERO INITIALLY)
        w3 = 0;
        if (e1 >= 1 && Math.floor(i1 + 0.5) == Math.floor(i + 0.5)) {
            // ADD 10 PTS. TO THIS STOCK;  RESET E1
            w3 = 10;
            e1 = 0;
        }
        if (e2 >= 1 && Math.floor(i2 + 0.5) == Math.floor(i + 0.5)) {
            // SUBTRACT 10 PTS. FROM THIS STOCK;  RESET E2
            w3 -= 10;
            e2 = 0;
        }
        // C(I) IS CHANGE IN STOCK VALUE
        ca[i] = Math.floor(a * sa[i]) + x1 + Math.floor(3 - 6 * Math.random() + 0.5) + w3;
        ca[i] = Math.floor(100 * ca[i] + 0.5) / 100;
        sa[i] += ca[i];
        if (sa[i] <= 0) {
            ca[i] = 0;
            sa[i] = 0;
        } else {
            sa[i] = Math.floor(100 * sa[i] + 0.5) / 100;
        }
    }
    // AFTER T8 DAYS RANDOMLY CHANGE TREND SIGN AND SLOPE
    if (--t8 < 1) {
        // RANDOMLY CHANGE TREND SIGN AND SLOPE (A), AND DURATION
        // OF TREND (T8)
        t8 = Math.floor(4.99 * Math.random() + 1);
        a = Math.floor((Math.random() / 10) * 100 + 0.5) / 100;
        s4 = Math.random();
        if (s4 > 0.5)
            a = -a;
    }
}

// Main program
async function main()
{
    print(tab(30) + "STOCK MARKET\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    // STOCK MARKET SIMULATION     -STOCK-
    // REVISED 8/18/70 (D. PESSEL, L. BRAUN, C. LOSIK)
    // IMP VRBLS: A-MRKT TRND SLP; B5-BRKRGE FEE; C-TTL CSH ASSTS;
    // C5-TTL CSH ASSTS (TEMP); C(I)-CHNG IN STK VAL; D-TTL ASSTS;
    // E1,E2-LRG CHNG MISC; I-STCK #; I1,I2-STCKS W LRG CHNG;
    // N1,N2-LRG CHNG DAY CNTS; P5-TTL DAYS PRCHSS; P(I)-PRTFL CNTNTS;
    // Q9-NEW CYCL?; S4-SGN OF A; S5-TTL DYS SLS; S(I)-VALUE/SHR;
    // T-TTL STCK ASSTS; T5-TTL VAL OF TRNSCTNS;
    // W3-LRG CHNG; X1-SMLL CHNG(<$1); Z4,Z5,Z6-NYSE AVE.; Z(I)-TRNSCT
    // SLOPE OF MARKET TREND:A  (SAME FOR ALL STOCKS)
    x = 1;
    a = Math.floor(Math.random() / 10 * 100 + 0.5) / 100;
    t5 = 0;
    x9 = 0;
    n1 = 0;
    n2 = 0;
    e1 = 0;
    e2 = 0;
    // INTRODUCTION
    print("DO YOU WANT THE INSTRUCTIONS (YES-TYPE 1, NO-TYPE 0)");
    z9 = parseInt(await input());
    print("\n");
    print("\n");
    if (z9 >= 1) {
        print("THIS PROGRAM PLAYS THE STOCK MARKET.  YOU WILL BE GIVEN\n");
        print("$10,000 AND MAY BUY OR SELL STOCKS.  THE STOCK PRICES WILL\n");
        print("BE GENERATED RANDOMLY AND THEREFORE THIS MODEL DOES NOT\n");
        print("REPRESENT EXACTLY WHAT HAPPENS ON THE EXCHANGE.  A TABLE\n");
        print("OF AVAILABLE STOCKS, THEIR PRICES, AND THE NUMBER OF SHARES\n");
        print("IN YOUR PORTFOLIO WILL BE PRINTED.  FOLLOWING THIS, THE\n");
        print("INITIALS OF EACH STOCK WILL BE PRINTED WITH A QUESTION\n");
        print("MARK.  HERE YOU INDICATE A TRANSACTION.  TO BUY A STOCK\n");
        print("TYPE +NNN, TO SELL A STOCK TYPE -NNN, WHERE NNN IS THE\n");
        print("NUMBER OF SHARES.  A BROKERAGE FEE OF 1% WILL BE CHARGED\n");
        print("ON ALL TRANSACTIONS.  NOTE THAT IF A STOCK'S VALUE DROPS\n");
        print("TO ZERO IT MAY REBOUND TO A POSITIVE VALUE AGAIN.  YOU\n");
        print("HAVE $10,000 TO INVEST.  USE INTEGERS FOR ALL YOUR INPUTS.\n");
        print("(NOTE:  TO GET A 'FEEL' FOR THE MARKET RUN FOR AT LEAST\n");
        print("10 DAYS)\n");
        print("-----GOOD LUCK!-----\n");
    }
    // GENERATION OF STOCK TABLE: INPUT REQUESTS
    // INITIAL STOCK VALUES
    sa[1] = 100;
    sa[2] = 85;
    sa[3] = 150;
    sa[4] = 140;
    sa[5] = 110;
    // INITIAL T8 - # DAYS FOR FIRST TREND SLOPE (A)
    t8 = Math.floor(4.99 * Math.random() + 1);
    // RANDOMIZE SIGN OF FIRST TREND SLOPE (A)
    if (Math.random() <= 0.5)
        a -= a;
    // RANDOMIZE INITIAL VALUES
    randomize_initial();
    // INITIAL PORTFOLIO CONTENTS
    for (i = 1; i <= 5; i++) {
        pa[i] = 0;
        za[i] = 0;
    }
    print("\n");
    print("\n");
    // INITIALIZE CASH ASSETS:C
    c = 10000;
    z5 = 0;
    // PRINT INITIAL PORTFOLIO
    print("STOCK\t \t\t\tINITIALS\tPRICE/SHARE\n");
    print("INT. BALLISTIC MISSILES\t\t  IBM\t\t" + sa[1] + "\n");
    print("RED CROSS OF AMERICA\t\t  RCA\t\t" + sa[2] + "\n");
    print("LICHTENSTEIN, BUMRAP & JOKE\t  LBJ\t\t" + sa[3] + "\n");
    print("AMERICAN BANKRUPT CO.\t\t  ABC\t\t" + sa[4] + "\n");
    print("CENSURED BOOKS STORE\t\t  CBS\t\t" + sa[5] + "\n");
    while (1) {
        print("\n");
        // NYSE AVERAGE:Z5; TEMP. VALUE:Z4; NET CHANGE:Z6
        z4 = z5;
        z5 = 0;
        t = 0;
        for (i = 1; i <= 5; i++) {
            z5 += sa[i];
            t += sa[i] * pa[i];
        }
        z5 = Math.floor(100 * (z5 / 5) + 0.5) / 100;
        z6 = Math.floor((z5 - z4) * 100 + 0.5) / 100;
        // TOTAL ASSETS:D
        d = t + c;
        if (x9 <= 0) {
            print("NEW YORK STOCK EXCHANGE AVERAGE: " + z5 + "\n");
        } else {
            print("NEW YORK STOCK EXCHANGE AVERAGE: " + z5 + " NET CHANGE " + z6 + "\n");
        }
        print("\n");
        t = Math.floor(100 * t + 0.5) / 100;
        print("TOTAL STOCK ASSETS ARE   $" + t + "\n");
        c = Math.floor(100 * c + 0.5) / 100;
        print("TOTAL CASH ASSETS ARE    $" + c + "\n");
        d = Math.floor(100 * d + 0.5) / 100;
        print("TOTAL ASSETS ARE         $" + d + "\n");
        print("\n");
        if (x9 != 0) {
            print("DO YOU WISH TO CONTINUE (YES-TYPE 1, NO-TYPE 0)");
            q9 = parseInt(await input());
            if (q9 < 1) {
                print("HOPE YOU HAD FUN!!\n");
                return;
            }
        }
        // INPUT TRANSACTIONS
        while (1) {
            print("WHAT IS YOUR TRANSACTION IN\n");
            print("IBM");
            za[1] = parseInt(await input());
            print("RCA");
            za[2] = parseInt(await input());
            print("LBJ");
            za[3] = parseInt(await input());
            print("ABC");
            za[4] = parseInt(await input());
            print("CBS");
            za[5] = parseInt(await input());
            print("\n");
            // TOTAL DAY'S PURCHASES IN $:P5
            p5 = 0;
            // TOTAL DAY'S SALES IN $:S5
            s5 = 0;
            for (i = 1; i <= 5; i++) {
                za[i] = Math.floor(za[i] + 0.5);
                if (za[i] > 0) {
                    p5 += za[i] * sa[i];
                } else {
                    s5 -= za[i] * sa[i];
                    if (-za[i] > pa[i]) {
                        print("YOU HAVE OVERSOLD A STOCK; TRY AGAIN.\n");
                        break;
                    }
                }
            }
            if (i <= 5)
                contine;
            // TOTAL VALUE OF TRANSACTIONS:T5
            t5 = p5 + s5;
            // BROKERAGE FEE:B5
            b5 = Math.floor(0.01 * t5 * 100 + 0.5) / 100;
            // CASH ASSETS=OLD CASH ASSETS-TOTAL PURCHASES
            // -BROKERAGE FEES+TOTAL SALES:C5
            c5 = c - p5 - b5 + s5;
            if (c5 < 0) {
                print("YOU HAVE USED $" + (-c5) + " MORE THAN YOU HAVE.\n");
                continue;
            }
            break;
        }
        c = c5;
        // CALCULATE NEW PORTFOLIO
        for (i = 1; i <= 5; i++) {
            pa[i] += za[i];
        }
        // CALCULATE NEW STOCK VALUES
        randomize_initial();
        // PRINT PORTFOLIO
        // BELL RINGING-DIFFERENT ON MANY COMPUTERS
        print("\n");
        print("**********     END OF DAY'S TRADING     **********\n");
        print("\n");
        print("\n");
        if (x9 >= 1) ;
        print("STOCK\tPRICE/SHARE\tHOLDINGS\tVALUE\tNET PRICE CHANGE\n");
        print("IBM\t" + sa[1] + "\t\t" + pa[1] + "\t\t" + sa[1] * pa[1] + "\t" + ca[1] + "\n");
        print("RCA\t" + sa[2] + "\t\t" + pa[2] + "\t\t" + sa[2] * pa[2] + "\t" + ca[2] + "\n");
        print("LBJ\t" + sa[3] + "\t\t" + pa[3] + "\t\t" + sa[3] * pa[3] + "\t" + ca[3] + "\n");
        print("ABC\t" + sa[4] + "\t\t" + pa[4] + "\t\t" + sa[4] * pa[4] + "\t" + ca[4] + "\n");
        print("CBS\t" + sa[5] + "\t\t" + pa[5] + "\t\t" + sa[5] * pa[5] + "\t" + ca[5] + "\n");
        x9 = 1;
        print("\n");
        print("\n");
    }
}

main();
