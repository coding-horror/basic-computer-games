#!/usr/bin/env python3
# TRAIN
#
# Converted from BASIC to Python by Trevor Hobson

import random


def play_game():
    """Play one round of the game"""
    car_speed = random.randint(40, 65)
    time_difference = random.randint(5, 20)
    train_speed = random.randint(20, 39)
    print("\nA car travelling", car_speed, "MPH can make a certain trip in")
    print(time_difference, "hours less than a train travelling at", train_speed, "MPH")
    time_answer = 0
    while time_answer == 0:
        try:
            time_answer = float(input("How long does the trip take by car "))
        except ValueError:
            print("Please enter a number.")
    car_time = time_difference*train_speed/(car_speed-train_speed)
    error_percent = int(abs((car_time-time_answer)*100/time_answer)+.5)
    if error_percent > 5:
        print("Sorry. You were off by", error_percent, "percent.")
        print("Correct answer is", round(car_time, 6), "hours")
    else:
        print("Good! Answer within", error_percent, "percent.")


def main():
    print(" " * 33 + "TRAIN")
    print(" " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n")
    print("Time - speed distance exercise")

    keep_playing = True
    while keep_playing:
        play_game()
        keep_playing = input(
            "\nAnother problem (yes or no) ").lower().startswith("y")


if __name__ == "__main__":
    main()
