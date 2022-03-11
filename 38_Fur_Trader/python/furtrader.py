#! /usr/bin/env python3

import random  # for generating random numbers
import sys  # for system function, like exit()

# global variables for storing player's status
player_funds = 0  # no money
player_furs = [0, 0, 0, 0]  # no furs


# Constants
FUR_MINK = 0
FUR_BEAVER = 1
FUR_ERMINE = 2
FUR_FOX = 3
MAX_FURS = 190
FUR_NAMES = ["MINK", "BEAVER", "ERMINE", "FOX"]

FORT_MONTREAL = 1
FORT_QUEBEC = 2
FORT_NEWYORK = 3
FORT_NAMES = ["HOCHELAGA (MONTREAL)", "STADACONA (QUEBEC)", "NEW YORK"]


def print_at_column(column: int, words: str):
    """Print the words at the specified column"""
    spaces = " " * column  # make a fat string of spaces
    print(spaces + words)


def show_introduction():
    """Show the player the introductory message"""
    print("YOU ARE THE LEADER OF A FRENCH FUR TRADING EXPEDITION IN ")
    print("1776 LEAVING THE LAKE ONTARIO AREA TO SELL FURS AND GET")
    print("SUPPLIES FOR THE NEXT YEAR.  YOU HAVE A CHOICE OF THREE")
    print("FORTS AT WHICH YOU MAY TRADE.  THE COST OF SUPPLIES")
    print("AND THE AMOUNT YOU RECEIVE FOR YOUR FURS WILL DEPEND")
    print("ON THE FORT THAT YOU CHOOSE.")
    print("")


def get_fort_choice():
    """Show the player the choices of Fort, get their input, if the
    input is a valid choice (1,2,3) return it, otherwise keep
    prompting the user."""
    result = 0
    while result == 0:
        print("")
        print("YOU MAY TRADE YOUR FURS AT FORT 1, FORT 2,")
        print("OR FORT 3.  FORT 1 IS FORT HOCHELAGA (MONTREAL)")
        print("AND IS UNDER THE PROTECTION OF THE FRENCH ARMY.")
        print("FORT 2 IS FORT STADACONA (QUEBEC) AND IS UNDER THE")
        print("PROTECTION OF THE FRENCH ARMY.  HOWEVER, YOU MUST")
        print("MAKE A PORTAGE AND CROSS THE LACHINE RAPIDS.")
        print("FORT 3 IS FORT NEW YORK AND IS UNDER DUTCH CONTROL.")
        print("YOU MUST CROSS THROUGH IROQUOIS LAND.")
        print("ANSWER 1, 2, OR 3.")

        player_choice = input(">> ")  # get input from the player

        # try to convert the player's string input into an integer
        try:
            result = int(player_choice)  # string to integer
        except:
            # Whatever the player typed, it could not be interpreted as a number
            pass

    return result


def show_fort_comment(which_fort):
    """Print the description for the fort"""
    print("")
    if which_fort == FORT_MONTREAL:
        print("YOU HAVE CHOSEN THE EASIEST ROUTE.  HOWEVER, THE FORT")
        print("IS FAR FROM ANY SEAPORT.  THE VALUE")
        print("YOU RECEIVE FOR YOUR FURS WILL BE LOW AND THE COST")
        print("OF SUPPLIES HIGHER THAN AT FORTS STADACONA OR NEW YORK.")
    elif which_fort == FORT_QUEBEC:
        print("YOU HAVE CHOSEN A HARD ROUTE.  IT IS, IN COMPARSION,")
        print("HARDER THAN THE ROUTE TO HOCHELAGA BUT EASIER THAN")
        print("THE ROUTE TO NEW YORK.  YOU WILL RECEIVE AN AVERAGE VALUE")
        print("FOR YOUR FURS AND THE COST OF YOUR SUPPLIES WILL BE AVERAGE.")
    elif which_fort == FORT_NEWYORK:
        print("YOU HAVE CHOSEN THE MOST DIFFICULT ROUTE.  AT")
        print("FORT NEW YORK YOU WILL RECEIVE THE HIGHEST VALUE")
        print("FOR YOUR FURS.  THE COST OF YOUR SUPPLIES")
        print("WILL BE LOWER THAN AT ALL THE OTHER FORTS.")
    else:
        print("Internal error #1, fort " + str(which_fort) + " does not exist")
        sys.exit(1)  # you have a bug
    print("")


def get_yes_or_no():
    """Prompt the player to enter 'YES' or 'NO'. Keep prompting until
    valid input is entered.  Accept various spellings by only
    checking the first letter of input.
    Return a single letter 'Y' or 'N'"""
    result = 0
    while result not in ("Y", "N"):
        print("ANSWER YES OR NO")
        player_choice = input(">> ")
        player_choice = player_choice.strip().upper()  # trim spaces, make upper-case
        if player_choice.startswith("Y"):
            result = "Y"
        elif player_choice.startswith("N"):
            result = "N"
    return result


def get_furs_purchase():
    """Prompt the player for how many of each fur type they want.
    Accept numeric inputs, re-prompting on incorrect input values"""
    results = []

    print("YOUR " + str(MAX_FURS) + " FURS ARE DISTRIBUTED AMONG THE FOLLOWING")
    print("KINDS OF PELTS: MINK, BEAVER, ERMINE AND FOX.")
    print("")

    for i in range(len(FUR_NAMES)):
        print("HOW MANY " + FUR_NAMES[i] + " DO YOU HAVE")
        count_str = input(">> ")
        try:
            count = int(count_str)
            results.append(count)
        except:
            # invalid input, prompt again by re-looping
            i -= 1
    return results


if __name__ == "__main__":

    print_at_column(31, "FUR TRADER")
    print_at_column(15, "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print_at_column(15, "(Ported to Python Oct 2012 krt@krt.com.au)")
    print("\n\n\n")

    game_state = "starting"
    fox_price = None  # sometimes this takes the "last" price (probably this was a bug)

    while True:

        if game_state == "starting":
            show_introduction()

            player_funds = 600  # Initial player start money
            player_furs = [0, 0, 0, 0]  # Player fur inventory

            print("DO YOU WISH TO TRADE FURS?")
            should_trade = get_yes_or_no()
            if should_trade == "N":
                sys.exit(0)  # STOP
            game_state = "trading"

        elif game_state == "trading":
            print("")
            print("YOU HAVE $ %1.2f IN SAVINGS" % (player_funds))
            print("AND " + str(MAX_FURS) + " FURS TO BEGIN THE EXPEDITION")
            player_furs = get_furs_purchase()

            if sum(player_furs) > MAX_FURS:
                print("")
                print("YOU MAY NOT HAVE THAT MANY FURS.")
                print("DO NOT TRY TO CHEAT.  I CAN ADD.")
                print("YOU MUST START AGAIN.")
                game_state = "starting"  # T/N: Wow, harsh.
            else:
                game_state = "choosing fort"

        elif game_state == "choosing fort":
            which_fort = get_fort_choice()
            show_fort_comment(which_fort)
            print("DO YOU WANT TO TRADE AT ANOTHER FORT?")
            change_fort = get_yes_or_no()
            if change_fort == "N":
                game_state = "travelling"

        elif game_state == "travelling":
            print("")
            if which_fort == FORT_MONTREAL:
                mink_price = (
                    int((0.2 * random.random() + 0.70) * 100 + 0.5) / 100
                )  # INT((.2*RND(1)+.7)*10^2+.5)/10^2
                ermine_price = (
                    int((0.2 * random.random() + 0.65) * 100 + 0.5) / 100
                )  # INT((.2*RND(1)+.65)*10^2+.5)/10^2
                beaver_price = (
                    int((0.2 * random.random() + 0.75) * 100 + 0.5) / 100
                )  # INT((.2*RND(1)+.75)*10^2+.5)/10^2
                fox_price = (
                    int((0.2 * random.random() + 0.80) * 100 + 0.5) / 100
                )  # INT((.2*RND(1)+.8)*10^2+.5)/10^2

                print("SUPPLIES AT FORT HOCHELAGA COST $150.00.")
                print("YOUR TRAVEL EXPENSES TO HOCHELAGA WERE $10.00.")
                player_funds -= 160

            elif which_fort == FORT_QUEBEC:
                mink_price = (
                    int((0.30 * random.random() + 0.85) * 100 + 0.5) / 100
                )  # INT((.3*RND(1)+.85)*10^2+.5)/10^2
                ermine_price = (
                    int((0.15 * random.random() + 0.80) * 100 + 0.5) / 100
                )  # INT((.15*RND(1)+.8)*10^2+.5)/10^2
                beaver_price = (
                    int((0.20 * random.random() + 0.90) * 100 + 0.5) / 100
                )  # INT((.2*RND(1)+.9)*10^2+.5)/10^2
                fox_price = (
                    int((0.25 * random.random() + 1.10) * 100 + 0.5) / 100
                )  # INT((.25*RND(1)+1.1)*10^2+.5)/10^2
                event_picker = int(10 * random.random()) + 1

                if event_picker <= 2:
                    print("YOUR BEAVER WERE TOO HEAVY TO CARRY ACROSS")
                    print("THE PORTAGE.  YOU HAD TO LEAVE THE PELTS, BUT FOUND")
                    print("THEM STOLEN WHEN YOU RETURNED.")
                    player_furs[FUR_BEAVER] = 0
                elif event_picker <= 6:
                    print("YOU ARRIVED SAFELY AT FORT STADACONA.")
                elif event_picker <= 8:
                    print("YOUR CANOE UPSET IN THE LACHINE RAPIDS.  YOU")
                    print("LOST ALL YOUR FURS.")
                    player_furs = [0, 0, 0, 0]
                elif event_picker <= 10:
                    print("YOUR FOX PELTS WERE NOT CURED PROPERLY.")
                    print("NO ONE WILL BUY THEM.")
                    player_furs[FUR_FOX] = 0
                else:
                    print(
                        "Internal Error #3, Out-of-bounds event_picker"
                        + str(event_picker)
                    )
                    sys.exit(1)  # you have a bug

                print("")
                print("SUPPLIES AT FORT STADACONA COST $125.00.")
                print("YOUR TRAVEL EXPENSES TO STADACONA WERE $15.00.")
                player_funds -= 140

            elif which_fort == FORT_NEWYORK:
                mink_price = (
                    int((0.15 * random.random() + 1.05) * 100 + 0.5) / 100
                )  # INT((.15*RND(1)+1.05)*10^2+.5)/10^2
                ermine_price = (
                    int((0.15 * random.random() + 0.95) * 100 + 0.5) / 100
                )  # INT((.15*RND(1)+.95)*10^2+.5)/10^2
                beaver_price = (
                    int((0.25 * random.random() + 1.00) * 100 + 0.5) / 100
                )  # INT((.25*RND(1)+1.00)*10^2+.5)/10^2
                if fox_price == None:
                    # Original Bug?  There is no Fox price generated for New York, it will use any previous "D1" price
                    # So if there was no previous value, make one up
                    fox_price = (
                        int((0.25 * random.random() + 1.05) * 100 + 0.5) / 100
                    )  # not in orginal code
                event_picker = int(10 * random.random()) + 1

                if event_picker <= 2:
                    print("YOU WERE ATTACKED BY A PARTY OF IROQUOIS.")
                    print("ALL PEOPLE IN YOUR TRADING GROUP WERE")
                    print("KILLED.  THIS ENDS THE GAME.")
                    sys.exit(0)
                elif event_picker <= 6:
                    print("YOU WERE LUCKY.  YOU ARRIVED SAFELY")
                    print("AT FORT NEW YORK.")
                elif event_picker <= 8:
                    print("YOU NARROWLY ESCAPED AN IROQUOIS RAIDING PARTY.")
                    print("HOWEVER, YOU HAD TO LEAVE ALL YOUR FURS BEHIND.")
                    player_furs = [0, 0, 0, 0]
                elif event_picker <= 10:
                    mink_price /= 2
                    fox_price /= 2
                    print("YOUR MINK AND BEAVER WERE DAMAGED ON YOUR TRIP.")
                    print("YOU RECEIVE ONLY HALF THE CURRENT PRICE FOR THESE FURS.")
                else:
                    print(
                        "Internal Error #4, Out-of-bounds event_picker"
                        + str(event_picker)
                    )
                    sys.exit(1)  # you have a bug

                print("")
                print("SUPPLIES AT NEW YORK COST $85.00.")
                print("YOUR TRAVEL EXPENSES TO NEW YORK WERE $25.00.")
                player_funds -= 105

            else:
                print("Internal error #2, fort " + str(which_fort) + " does not exist")
                sys.exit(1)  # you have a bug

            # Calculate sales
            beaver_value = beaver_price * player_furs[FUR_BEAVER]
            fox_value = fox_price * player_furs[FUR_FOX]
            ermine_value = ermine_price * player_furs[FUR_ERMINE]
            mink_value = mink_price * player_furs[FUR_MINK]

            print("")
            print("YOUR BEAVER SOLD FOR $%6.2f" % (beaver_value))
            print("YOUR FOX SOLD FOR    $%6.2f" % (fox_value))
            print("YOUR ERMINE SOLD FOR $%6.2f" % (ermine_value))
            print("YOUR MINK SOLD FOR   $%6.2f" % (mink_value))

            player_funds += beaver_value + fox_value + ermine_value + mink_value

            print("")
            print(
                "YOU NOW HAVE $ %1.2f INCLUDING YOUR PREVIOUS SAVINGS" % (player_funds)
            )

            print("")
            print("DO YOU WANT TO TRADE FURS NEXT YEAR?")
            should_trade = get_yes_or_no()
            if should_trade == "N":
                sys.exit(0)  # STOP
            else:
                game_state = "trading"
