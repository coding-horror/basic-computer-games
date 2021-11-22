"""
PIZZA

A pizza delivery simulation

Ported by Dave LeCompte
"""

import random

PAGE_WIDTH = 64

customer_names = [chr(65 + x) for x in range(16)]
street_names = [str(n) for n in range(1, 5)]


def print_centered(msg):
    spaces = " " * ((PAGE_WIDTH - len(msg)) // 2)
    print(spaces + msg)


def print_header(title):
    print_centered(title)
    print_centered("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print()
    print()
    print()


def print_ticks():
    for t in range(4):
        print("-")


def print_ruler():
    print(" -----1-----2-----3-----4-----")


def print_street(i):
    street_number = 3 - i

    street_name = street_names[street_number]
    line = street_name

    space = " " * 5
    for customer_index in range(4):
        line += space
        customer_name = customer_names[4 * street_number + customer_index]
        line += customer_name
    line += space
    line += street_name
    print(line)


def print_map():
    print("MAP OF THE CITY OF HYATTSVILLE")
    print()
    print_ruler()
    for i in range(4):
        print_ticks()
        print_street(i)
    print_ticks()
    print_ruler()
    print()


def print_instructions():
    print("PIZZA DELIVERY GAME")
    print()
    print("WHAT IS YOUR FIRST NAME?")
    player_name = input()
    print()
    print(f"HI, {player_name}.  IN THIS GAME YOU ARE TO TAKE ORDERS")
    print("FOR PIZZAS.  THEN YOU ARE TO TELL A DELIVERY BOY")
    print("WHERE TO DELIVER THE ORDERED PIZZAS.")
    print()
    print()

    print_map()

    print("THE OUTPUT IS A MAP OF THE HOMES WHERE")
    print("YOU ARE TO SEND PIZZAS.")
    print()
    print("YOUR JOB IS TO GIVE A TRUCK DRIVER")
    print("THE LOCATION OR COORDINATES OF THE")
    print("HOME ORDERING THE PIZZA.")
    print()

    return player_name


def yes_no_prompt(msg):
    while True:
        print(msg)
        response = input().upper()

        if response == "YES":
            return True
        elif response == "NO":
            return False
        print("'YES' OR 'NO' PLEASE, NOW THEN,")


def print_more_directions(player_name):
    print()
    print("SOMEBODY WILL ASK FOR A PIZZA TO BE")
    print("DELIVERED.  THEN A DELIVERY BOY WILL")
    print("ASK YOU FOR THE LOCATION.")
    print("     EXAMPLE:")
    print("THIS IS J.  PLEASE SEND A PIZZA.")
    print(f"DRIVER TO {player_name}.  WHERE DOES J LIVE?")
    print("YOUR ANSWER WOULD BE 2,3")
    print()


def calculate_customer_index(x, y):
    return 4 * (y - 1) + x - 1


def deliver_to(customer_index, customer_name, player_name):
    print(f"  DRIVER TO {player_name}:  WHERE DOES {customer_name} LIVE?")

    coords = input()
    xc, yc = [int(c) for c in coords.split(",")]
    delivery_index = calculate_customer_index(xc, yc)
    if delivery_index == customer_index:
        print(f"HELLO {player_name}.  THIS IS {customer_name}, THANKS FOR THE PIZZA.")
        return True
    else:
        delivery_name = customer_names[delivery_index]
        print(f"THIS IS {delivery_name}.  I DID NOT ORDER A PIZZA.")
        print(f"I LIVE AT {xc},{yc}")
        return False


def play_game(num_turns, player_name):
    for turn in range(num_turns):
        x = random.randint(1, 4)
        y = random.randint(1, 4)
        customer_index = calculate_customer_index(x, y)
        customer_name = customer_names[customer_index]

        print()
        print(
            f"HELLO {player_name}'S PIZZA.  THIS IS {customer_name}.  PLEASE SEND A PIZZA."
        )
        while True:
            success = deliver_to(customer_index, customer_name, player_name)
            if success:
                break


def main():
    print_header("PIZZA")

    player_name = print_instructions()

    more_directions = yes_no_prompt("DO YOU NEED MORE DIRECTIONS?")

    if more_directions:
        print_more_directions(player_name)

        understand = yes_no_prompt("UNDERSTAND?")

        if not understand:
            print("THIS JOB IS DEFINITELY TOO DIFFICULT FOR YOU. THANKS ANYWAY")
            return

    print("GOOD.  YOU ARE NOW READY TO START TAKING ORDERS.")
    print()
    print("GOOD LUCK!!")
    print()

    while True:
        num_turns = 5
        play_game(num_turns, player_name)

        print()
        more = yes_no_prompt("DO YOU WANT TO DELIVER MORE PIZZAS?")
        if not more:
            print(f"O.K. {player_name}, SEE YOU LATER!")
            print()
            return


if __name__ == "__main__":
    main()
