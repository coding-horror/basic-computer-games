
def calendar(weekday, leap_year):
    """
    function to print a year's calendar.

    input: 
        _weekday_: int - the initial day of the week (0=SUN, -1=MON, -2=TUES...)
        _leap_year_: bool - indicates if the year is a leap year
    """
    months_days = [0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31]
    days = 'S        M        T        W        T        F        S\n'
    sep = "*" * 59
    years_day = 365
    d = weekday

    if leap_year:
        months_days[2] = 29
        years_day = 366

    months_names = [" JANUARY ",
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
                    " DECEMBER"]

    print(" "*32 + "CALENDAR")
    print(" "*15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print("\n"*11)
    days_count = 0  # S in the original program

    # main loop
    for n in range(1, 13):
        days_count += months_days[n-1]
        print("** {} ****************** {} ****************** {} **\n".format(days_count,
                                                                              months_names[n-1], years_day-days_count))
        print(days)
        print(sep)

        for w in range(1, 7):
            print("\n")
            for g in range(1, 8):
                d += 1
                d2 = d - days_count

                if d2 > months_days[n]:
                    break

                if d2 <= 0:
                    print("{}".format('  '), end='       ')
                elif d2 < 10:
                    print(" {}".format(d2), end='       ')
                else:
                    print("{}".format(d2), end='       ')

            if d2 >= months_days[n]:
                break

        if d2 > months_days[n]:
            d -= g

        print("\n")


if __name__ == "__main__":
    calendar(-6, False)
