########################################################
# Calendar
#
# From: BASIC Computer Games (1978)
#       Edited by David Ahl#
#
#    This program prints out a calendar
# for any year. You must specify the
# starting day of the week of the year in
# statement 130. (Sunday(0), Monday
# (-1), Tuesday(-2), etc.) You can determine
# this by using the program WEEKDAY.
# You must also make two changes
# for leap years in statement 360 and 620.
# The program listing describes the necessary
# changes. Running the program produces a
# nice 12-month calendar.
#    The program was written by Geofrey
# Chase of the Abbey, Portsmouth, Rhode Island.
#
########################################################

from typing import Tuple


def parse_input() -> Tuple[int, bool]:
    """
    function to parse input for weekday and leap year boolean
    """

    days_mapping = {
        "sunday": 0,
        "monday": -1,
        "tuesday": -2,
        "wednesday": -3,
        "thursday": -4,
        "friday": -5,
        "saturday": -6,
    }

    day = 0
    leap_day = False

    correct_day_input = False
    while not correct_day_input:
        weekday = input("INSERT THE STARTING DAY OF THE WEEK OF THE YEAR:")

        for day_k in days_mapping.keys():
            if weekday.lower() in day_k:
                day = days_mapping[day_k]
                correct_day_input = True
                break

    while True:
        leap = input("IS IT A LEAP YEAR?:")

        if "y" in leap.lower():
            leap_day = True
            break

        if "n" in leap.lower():
            leap_day = False
            break

    return day, leap_day


def calendar(weekday, leap_year):
    """
    function to print a year's calendar.

    input:
        _weekday_: int - the initial day of the week (0=SUN, -1=MON, -2=TUES...)
        _leap_year_: bool - indicates if the year is a leap year
    """
    months_days = [0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31]
    days = "S        M        T        W        T        F        S\n"
    sep = "*" * 59
    years_day = 365
    d = weekday

    if leap_year:
        months_days[2] = 29
        years_day = 366

    months_names = [
        " JANUARY ",
        " FEBRUARY",
        "  MARCH  ",
        "  APRIL  ",
        "   MAY   ",
        "   JUNE  ",
        "   JULY  ",
        "  AUGUST ",
        "SEPTEMBER",
        " OCTOBER ",
        " NOVEMBER",
        " DECEMBER",
    ]

    days_count = 0  # S in the original program

    # main loop
    for n in range(1, 13):
        days_count += months_days[n - 1]
        print(
            "** {} ****************** {} ****************** {} **\n".format(
                days_count, months_names[n - 1], years_day - days_count
            )
        )
        print(days)
        print(sep)

        for _ in range(1, 7):
            print("\n")
            for g in range(1, 8):  # noqa
                d += 1
                d2 = d - days_count

                if d2 > months_days[n]:
                    break

                if d2 <= 0:
                    print("{}".format("  "), end="       ")
                elif d2 < 10:
                    print(f" {d2}", end="       ")
                else:
                    print(f"{d2}", end="       ")
            print()

            if d2 >= months_days[n]:
                break

        if d2 > months_days[n]:
            d -= g

        print("\n")

    print("\n")


def main() -> None:
    print(" " * 32 + "CALENDAR")
    print(" " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print("\n" * 11)

    day, leap_year = parse_input()
    calendar(day, leap_year)


if __name__ == "__main__":
    main()

########################################################
#
########################################################
#
# Porting notes:
#
# It has been added an input at the beginning of the
# program so the user can specify the first day of the
# week of the year and if the year is leap or not.
#
########################################################
