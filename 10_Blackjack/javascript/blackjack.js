// BLACKJACK
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

var da = [];

var pa = [];
var qa = [];
var ca = [];
var ta = [];
var sa = [];
var ba = [];
var za = [];
var ra = [];

var ds = "N A  2  3  4  5  6  7N 8  9 10  J  Q  K";
var is = "H,S,D,/,"

var q;
var aa;
var ab;
var ac;
var h;
var h1;

function af(q) {
    return q >= 22 ? q - 11 : q;
}

function reshuffle()
{
    print("RESHUFFLING\n");
    for (; d >= 1; d--)
        ca[--c] = da[d];
    for (c1 = 52; c1 >= c; c1--) {
        c2 = Math.floor(Math.random() * (c1 - c + 1)) + c;
        c3 = ca[c2];
        ca[c2] = ca[c1];
        ca[c1] = c3;
    }
}

// Subroutine to get a card.
function get_card()
{
    if (c >= 51)
        reshuffle();
    return ca[c++];
}

// Card printing subroutine
function card_print(x)
{
    print(ds.substr(3 * x - 3, 3) + "  ");
}

// Alternate card printing subroutine
function alt_card_print(x)
{
    print(" " + ds.substr(3 * x - 2, 2) + "   ");
}

// Subroutine to add card 'which' to total 'q'
function add_card(which)
{
    x1 = which;
    if (x1 > 10)
        x1 = 10;
    q1 = q + x1;
    if (q < 11) {
        if (which <= 1) {
            q += 11;
            return;
        }
        if (q1 >= 11)
            q = q1 + 11;
        else
            q = q1;
        return;
    }
    if (q <= 21 && q1 > 21)
        q = q1 + 1;
    else
        q = q1;
    if (q >= 33)
        q = -1;
}

// Subroutine to evaluate hand 'which'. Total is put into
// qa[which]. Totals have the following meaning:
//  2-10...hard 2-10
// 11-21...soft 11-21
// 22-32...hard 11-21
//  33+....busted
function evaluate_hand(which)
{
    q = 0;
    for (q2 = 1; q2 <= ra[which]; q2++) {
        add_card(pa[i][q2]);
    }
    qa[which] = q;
}

// Subroutine to add a card to row i
function add_card_to_row(i, x) {
    ra[i]++;
    pa[i][ra[i]] = x;
    q = qa[i];
    add_card(x);
    qa[i] = q;
    if (q < 0) {
        print("...BUSTED\n");
        discard_row(i);
    }
}

// Subroutine to discard row i
function discard_row(i) {
    while (ra[i]) {
        d++;
        da[d] = pa[i][ra[i]];
        ra[i]--;
    }
}

// Prints total of hand i
function print_total(i) {
    print("\n");
    aa = qa[i];
    total_aa();
    print("TOTAL IS " + aa + "\n");
}

function total_aa()
{
    if (aa >= 22)
        aa -= 11;
}

function total_ab()
{
    if (ab >= 22)
        ab -= 11;
}

function total_ac()
{
    if (ac >= 22)
        ac -= 11;
}

function process_input(str)
{
    str = str.substr(0, 1);
    for (h = 1; h <= h1; h += 2) {
        if (str == is.substr(h - 1, 1))
            break;
    }
    if (h <= h1) {
        h = (h + 1) / 2;
        return 0;
    }
    print("TYPE " + is.substr(0, h1 - 1) + " OR " + is.substr(h1 - 1, 2) + " PLEASE");
    return 1;
}

// Main program
async function main()
{
    print(tab(31) + "BLACK JACK\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    // --pa[i][j] IS THE JTH CARD IN HAND I, qa[i] IS TOTAL OF HAND I
    // --C IS THE DECK BEING DEALT FROM, D IS THE DISCARD PILE,
    // --ta[i] IS THE TOTAL FOR PLAYER I, sa[i] IS THE TOTAL THIS HAND FOR
    // --PLAYER I, ba[i] IS TH BET FOR HAND I
    // --ra[i] IS THE LENGTH OF pa[I,*]
    
    // --Program starts here
    // --Initialize
    for (i = 1; i <= 15; i++)
        pa[i] = [];
    for (i = 1; i <= 13; i++)
        for (j = 4 * i - 3; j <= 4 * i; j++)
            da[j] = i;
    d = 52;
    c = 53;
    print("DO YOU WANT INSTRUCTIONS");
    str = await input();
    if (str.toUpperCase().substr(0, 1) != "N") {
        print("THIS IS THE GAME OF 21. AS MANY AS 7 PLAYERS MAY PLAY THE\n");
        print("GAME. ON EACH DEAL, BETS WILL BE ASKED FOR, AND THE\n");
        print("PLAYERS' BETS SHOULD BE TYPED IN. THE CARDS WILL THEN BE\n");
        print("DEALT, AND EACH PLAYER IN TURN PLAYS HIS HAND. THE\n");
        print("FIRST RESPONSE SHOULD BE EITHER 'D', INDICATING THAT THE\n");
        print("PLAYER IS DOUBLING DOWN, 'S', INDICATING THAT HE IS\n");
        print("STANDING, 'H', INDICATING HE WANTS ANOTHER CARD, OR '/',\n");
        print("INDICATING THAT HE WANTS TO SPLIT HIS CARDS. AFTER THE\n");
        print("INITIAL RESPONSE, ALL FURTHER RESPONSES SHOULD BE 'S' OR\n");
        print("'H', UNLESS THE CARDS WERE SPLIT, IN WHICH CASE DOUBLING\n");
        print("DOWN IS AGAIN PERMITTED. IN ORDER TO COLLECT FOR\n");
        print("BLACKJACK, THE INITIAL RESPONSE SHOULD BE 'S'.\n");
    }
    while (1) {
        print("NUMBER OF PLAYERS");
        n = parseInt(await input());
        print("\n");
        if (n < 1 || n > 7)
            continue;
        else
            break;
    }
    for (i = 1; i <= 8; i++)
        ta[i] = 0;
    d1 = n + 1;
    while (1) {
        if (2 * d1 + c >= 52) {
            reshuffle();
        }
        if (c == 2)
            c--;
        for (i = 1; i <= n; i++)
            za[i] = 0;
        for (i = 1; i <= 15; i++)
            ba[i] = 0;
        for (i = 1; i <= 15; i++)
            qa[i] = 0;
        for (i = 1; i <= 7; i++)
            sa[i] = 0;
        for (i = 1; i <= 15; i++)
            ra[i] = 0;
        print("BETS:\n");
        for (i = 1; i <= n; i++) {
            do {
                print("#" + i + " ");
                za[i] = parseFloat(await input());
            } while (za[i] <= 0 || za[i] > 500) ;
        }
        for (i = 1; i <= n; i++)
            ba[i] = za[i];
        print("PLAYER");
        for (i = 1; i <= n; i++) {
            print(" " + i + "    ");
        }
        print("DEALER\n");
        for (j = 1; j <= 2; j++) {
            print(tab(5));
            for (i = 1; i <= d1; i++) {
                pa[i][j] = get_card();
                if (j == 1 || i <= n)
                    alt_card_print(pa[i][j]);
            }
            print("\n");
        }
        for (i = 1; i <= d1; i++)
            ra[i] = 2;
        // --Test for insurance
        if (pa[d1][1] <= 1) {
            print("ANY INSURANCE");
            str = await input();
            if (str.substr(0, 1) == "Y") {
                print("INSURANCE BETS\n");
                for (i = 1; i <= n; i++) {
                    do {
                        print("#" + i + " ");
                        za[i] = parseFloat(await input());
                    } while (za[i] < 0 || za[i] > ba[i] / 2) ;
                }
                for (i = 1; i <= n; i++)
                    sa[i] = za[i] * ((pa[d1][2] >= 10 ? 3 : 0) - 1);
            }
        }
        // --Test for dealer blackjack
        l1 = 1;
        l2 = 1;
        if (pa[d1][1] == 1 && pa[d1][2] > 9) {
            l1 = 0;
            l2 = 0;
        }
        if (pa[d1][2] == 1 && pa[d1][1] > 9) {
            l1 = 0;
            l2 = 0;
        }
        if (l1 == 0 && l2 == 0) {
            print("\n");
            print("DEALER HAS A" + ds.substr(3 * pa[d1][2] - 3, 3) + " IN THE HOLE FOR BLACKJACK\n");
            for (i = 1; i <= d1; i++)
                evaluate_hand(i);
        } else {
            // --No dealer blackjack
            if (pa[d1][1] <= 1 || pa[d1][1] >= 10) {
                print("\n");
                print("NO DEALER BLACKJACK.\n");
            }
            // --Now play the hands
            for (i = 1; i <= n; i++) {
                print("PLAYER " + i + " ");
                h1 = 7;
                do {
                    str = await input();
                } while (process_input(str)) ;
                if (h == 1) {   // Player wants to be hit
                    evaluate_hand(i);
                    h1 = 3;
                    x = get_card();
                    print("RECEIVED A");
                    card_print(x);
                    add_card_to_row(i, x);
                    if (q > 0)
                        print_total(i);
                } else if (h == 2) {    // Player wants to stand
                    evaluate_hand(i);
                    if (qa[i] == 21) {
                        print("BLACKJACK\n");
                        sa[i] = sa[i] + 1.5 * ba[i];
                        ba[i] = 0;
                        discard_row(i);
                    } else {
                        print_total(i);
                    }
                } else if (h == 3) {    // Player wants to double down
                    evaluate_hand(i);
                    h1 = 3;
                    h = 1;
                    while (1) {
                        if (h == 1) {   // Hit
                            x = get_card();
                            print("RECEIVED A");
                            card_print(x);
                            add_card_to_row(i, x);
                            if (q < 0)
                                break;
                            print("HIT");
                        } else if (h == 2) {    // Stand
                            print_total(i);
                            break;
                        }
                        do {
                            str = await input();
                        } while (process_input(str)) ;
                        h1 = 3;
                    }
                } else if (h == 4) {    // Player wants to split
                    l1 = pa[i][1];
                    if (l1 > 10)
                        l1 = 10;
                    l2 = pa[i][2];
                    if (l2 > 10)
                        l2 = 10;
                    if (l1 != l2) {
                        print("SPLITTING NOT ALLOWED.\n");
                        i--;
                        continue;
                    }
                    // --Play out split
                    i1 = i + d1;
                    ra[i1] = 2;
                    pa[i1][1] = pa[i1][2];
                    ba[i + d1] = ba[i];
                    x = get_card();
                    print("FIRST HAND RECEIVES A");
                    card_print(x);
                    pa[i][2] = x;
                    evaluate_hand(i);
                    print("\n");
                    x = get_card();
                    print("SECOND HAND RECEIVES A");
                    i = i1;
                    card_print(x);
                    pa[i][2] = x;
                    evaluate_hand(i);
                    print("\n");
                    i = i1 - d1;
                    if (pa[i][1] != 1) {
                        // --Now play the two hands
                        do {
                            
                            print("HAND " + (i > d1 ? 2 : 1) + " ");
                            h1 = 5;
                            while (1) {
                                do {
                                    str = await input();
                                } while (process_input(str)) ;
                                h1 = 3;
                                if (h == 1) {   // Hit
                                    x = get_card();
                                    print("RECEIVED A");
                                    card_print(x);
                                    add_card_to_row(i, x);
                                    if (q < 0)
                                        break;
                                    print("HIT");
                                } else if (h == 2) {    // Stand
                                    print_total(i);
                                    break;
                                } else {    // Double
                                    x = get_card();
                                    ba[i] *= 2;
                                    print("RECEIVED A");
                                    card_print(x);
                                    add_card_to_row(i, x);
                                    if (q > 0)
                                        print_total(i);
                                    break;
                                }
                            }
                            i += d1;
                        } while (i == i1) ;
                        i = i1 - d1;
                    }
                }
            }
            // --Test for playing dealer's hand
            evaluate_hand(i);
            for (i = 1; i <= n; i++) {
                if (ra[i] > 0 || ra[i + d1] > 0)
                    break;
            }
            if (i > n) {
                print("DEALER HAD A");
                x = pa[d1][2];
                card_print(x);
                print(" CONCEALED.\n");
            } else {
                print("DEALER HAS A" + ds.substr(3 * pa[d1][2] - 3, 3) + " CONCEALED ");
                i = d1;
                aa = qa[i];
                total_aa();
                print("FOR A TOTAL OF " + aa + "\n");
                if (aa <= 16) {
                    print("DRAWS");
                    do {
                        
                        x = get_card();
                        alt_card_print(x);
                        add_card_to_row(i, x);
                        aa = q;
                        total_aa();
                    } while (q > 0 && aa < 17) ;
                    if (q < 0) {
                        qa[i] = q + 0.5;
                    } else {
                        qa[i] = q;
                    }
                    if (q >= 0) {
                        aa = q;
                        total_aa();
                        print("---TOTAL IS " + aa + "\n");
                    }
                }
                print("\n");
            }
        }
        // --TALLY THE RESULT
        str = "LOSES PUSHES WINS "
        print("\n");
        for (i = 1; i <= n; i++) {
            aa = qa[i]
            total_aa();
            ab = qa[i + d1];
            total_ab();
            ac = qa[d1];
            total_ac();
            signaaac = aa - ac;
            if (signaaac) {
                if (signaaac < 0)
                    signaaac = -1;
                else
                    signaaac = 1;
            }
            signabac = ab - ac;
            if (signabac) {
                if (signabac < 0)
                    signabac = -1;
                else
                    signabac = 1;
            }
            sa[i] = sa[i] + ba[i] * signaaac + ba[i + d1] * signabac;
            ba[i + d1] = 0;
            print("PLAYER " + i + " ");
            signsai = sa[i];
            if (signsai) {
                if (signsai < 0)
                    signsai = -1;
                else
                    signsai = 1;
            }
            print(str.substr(signsai * 6 + 6, 6) + " ");
            if (sa[i] == 0)
                print("      ");
            else
                print(" " + Math.abs(sa[i]) + " ");
            ta[i] = ta[i] + sa[i];
            print("TOTAL= " + ta[i] + "\n");
            discard_row(i);
            ta[d1] = ta[d1] - sa[i];
            i += d1;
            discard_row(i);
            i -= d1;
        }
        print("DEALER'S TOTAL= " + ta[d1] + "\n");
        print("\n");
        discard_row(i);
    }
}

main();
