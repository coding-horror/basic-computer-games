"""
CHANGE

Change calculator

Port by Dave LeCompte
"""

PAGE_WIDTH = 64


def print_centered(msg):
    spaces = " " * ((PAGE_WIDTH - len(msg)) // 2)
    print(spaces + msg)


def print_header(title):
    print_centered(title)
    print_centered("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print()
    print()
    print()


def print_introduction():
    print("I, YOUR FRIENDLY MICROCOMPUTER, WILL DETERMINE")
    print("THE CORRECT CHANGE FOR ITEMS COSTING UP TO $100.")
    print()
    print()


def pennies_to_dollar_string(p):
    d = p / 100
    ds = f"${d:0.2f}"
    return ds


def compute_change():
    print("COST OF ITEM?")
    cost = float(input())
    print("AMOUNT OF PAYMENT?")
    payment = float(input())

    change_in_pennies = round((payment - cost) * 100)
    if change_in_pennies == 0:
        print("CORRECT AMOUNT, THANK YOU.")
        return

    if change_in_pennies < 0:
        short = -change_in_pennies / 100

        print(f"SORRY, YOU HAVE SHORT-CHANGED ME ${short:0.2f}")
        print()
        return

    print(f"YOUR CHANGE, {pennies_to_dollar_string(change_in_pennies)}")

    d = change_in_pennies // 1000
    if d > 0:
        print(f"{d} TEN DOLLAR BILL(S)")
    change_in_pennies -= d * 1000

    e = change_in_pennies // 500
    if e > 0:
        print(f"{e} FIVE DOLLAR BILL(S)")
    change_in_pennies -= e * 500

    f = change_in_pennies // 100
    if f > 0:
        print(f"{f} ONE DOLLAR BILL(S)")
    change_in_pennies -= f * 100

    g = change_in_pennies // 50
    if g > 0:
        print("ONE HALF DOLLAR")
    change_in_pennies -= g * 50

    h = change_in_pennies // 25
    if h > 0:
        print(f"{h} QUARTER(S)")
    change_in_pennies -= h * 25

    i = change_in_pennies // 10
    if i > 0:
        print(f"{i} DIME(S)")
    change_in_pennies -= i * 10

    j = change_in_pennies // 5
    if j > 0:
        print(f"{j} NICKEL(S)")
    change_in_pennies -= j * 5

    if change_in_pennies > 0:
        print(f"{change_in_pennies} PENNY(S)")


def print_thanks():
    print("THANK YOU, COME AGAIN.")
    print()
    print()


def main():
    print_header("CHANGE")
    print_introduction()

    while True:
        compute_change()
        print_thanks()


if __name__ == "__main__":
    main()
