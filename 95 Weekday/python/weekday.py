"""
WEEKDAY

Calculates which weekday an entered date is.

Also estimates how long a person has done certain activities, if they
entered their birthday.

Also calculates the year of retirement, assuming retiring at age 65.

Ported by Dave LeCompte.
"""

import datetime

GET_TODAY_FROM_SYSTEM = True


def print_with_tab(space_count, s):
    if space_count > 0:
        spaces = " " * space_count
    else:
        spaces = ""
    print(spaces + s)


def get_date_from_user(prompt):
    while True:
        print(prompt)
        date_str = input()
        try:
            month_num, day_num, year_num = [int(x) for x in date_str.split(",")]
        except Exception as e:
            print("I COULDN'T UNDERSTAND THAT. TRY AGAIN.")
        return month_num, day_num, year_num


def get_date_from_system():
    dt = datetime.datetime.today()
    return dt.month, dt.day, dt.year


def get_day_of_week(weekday_index, day):
    day_names = {
        1: "SUNDAY",
        2: "MONDAY",
        3: "TUESDAY",
        4: "WEDNESDAY",
        5: "THURSDAY",
        6: "FRIDAY",
        7: "SATURDAY",
    }

    if weekday_index == 6 and day == 13:
        return "FRIDAY THE THIRTEENTH---BEWARE!"
    return day_names[weekday_index]


def previous_day(b):
    if b == 0:
        b = 6
    return b - 1


def is_leap_year(year):
    if (year % 4) != 0:
        return False
    if (year % 100) != 0:
        return True
    if (year % 400) != 0:
        return False
    return True


def adjust_day_for_leap_year(b, year):
    if is_leap_year(year):
        b = previous_day(b)
    return b


def adjust_weekday(b, month, year):
    if month <= 2:
        b = adjust_day_for_leap_year(b, year)
    if b == 0:
        b = 7
    return b


def calc_day_value(year, month, day):
    return (year * 12 + month) * 31 + day


def deduct_time(frac, days, years_remain, months_remain, days_remain):
    # CALCULATE TIME IN YEARS, MONTHS, AND DAYS
    days_available = int(frac * days)
    years_used = int(days_available / 365)
    days_available -= years_used * 365
    months_used = int(days_available / 30)
    days_used = days_available - (months_used * 30)
    years_remain = years_remain - years_used
    months_remain = months_remain - months_used
    days_remain = days_remain - days_used

    while days_remain < 0:
        days_remain += 30
        months_remain -= 1

    while months_remain < 0 and years_remain > 0:
        months_remain += 12
        years_remain -= 1
    return years_remain, months_remain, days_remain, years_used, months_used, days_used


def time_report(msg, years, months, days):
    leading_spaces = 23 - len(msg)
    print_with_tab(leading_spaces, msg + f"\t{years}\t{months}\t{days}")


def make_occupation_label(years):
    if years <= 3:
        return "PLAYED"
    elif years <= 9:
        return "PLAYED/STUDIED"
    else:
        return "WORKED/PLAYED"


def calculate_day_of_week(year, month, day):
    # Initial values for months
    month_table = [0, 3, 3, 6, 1, 4, 6, 2, 5, 0, 3, 5]

    i1 = int((year - 1500) / 100)
    a = i1 * 5 + (i1 + 3) / 4
    i2 = int(a - int(a / 7) * 7)
    y2 = int(year / 100)
    y3 = int(year - y2 * 100)
    a = y3 / 4 + y3 + day + month_table[month - 1] + i2
    b = int(a - int(a / 7) * 7) + 1
    b = adjust_weekday(b, month, year)

    return b


def end():
    for i in range(5):
        print()


def main():
    print_with_tab(32, "WEEKDAY")
    print_with_tab(15, "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print()
    print()
    print()
    print("WEEKDAY IS A COMPUTER DEMONSTRATION THAT")
    print("GIVES FACTS ABOUT A DATE OF INTEREST TO YOU.")
    print()

    if GET_TODAY_FROM_SYSTEM:
        month_today, day_today, year_today = get_date_from_system()
    else:
        month_today, day_today, year_today = get_date_from_user(
            "ENTER TODAY'S DATE IN THE FORM: 3,24,1979"
        )

    # This program determines the day of the week
    # for a date after 1582

    print()

    month, day, year = get_date_from_user(
        "ENTER DAY OF BIRTH (OR OTHER DAY OF INTEREST) (like MM,DD,YYYY)"
    )

    print()

    # Test for date before current calendar
    if year < 1582:
        print("NOT PREPARED TO GIVE DAY OF WEEK PRIOR TO MDLXXXII.")
        end()
        return

    b = calculate_day_of_week(year, month, day)

    today_day_value = calc_day_value(year_today, month_today, day_today)
    target_day_value = calc_day_value(year, month, day)

    is_today = False
    is_future = False

    if today_day_value < target_day_value:
        label = "WILL BE A"
        is_future = False
    elif today_day_value == target_day_value:
        label = "IS A"
        is_today = True
    else:
        label = "WAS A"

    day_name = get_day_of_week(b, day)

    # print the day of the week the date falls on.
    print(f"{month}/{day}/{year} {label} {day_name}.")

    if is_today:
        # nothing to report for today
        end()
        return

    print()

    el_years = year_today - year
    el_months = month_today - month
    el_days = day_today - day

    if el_days < 0:
        el_months = el_months - 1
        el_days = el_days + 30
    if el_months < 0:
        el_years = el_years - 1
        el_months = el_months + 12
    if el_years < 0:
        # target date is in the future
        end()
        return

    if (el_months == 0) and (el_days == 0):
        print("***HAPPY BIRTHDAY***")

    # print report
    print_with_tab(23, "\tYEARS\tMONTHS\tDAYS")
    print_with_tab(23, "\t-----\t------\t----")
    print(f"YOUR AGE (IF BIRTHDATE)\t{el_years}\t{el_months}\t{el_days}")

    life_days = (el_years * 365) + (el_months * 30) + el_days + int(el_months / 2)
    rem_years = el_years
    rem_months = el_months
    rem_days = el_days

    rem_years, rem_months, rem_days, used_years, used_months, used_days = deduct_time(
        0.35, life_days, rem_years, rem_months, rem_days
    )
    time_report("YOU HAVE SLEPT", used_years, used_months, used_days)
    rem_years, rem_months, rem_days, used_years, used_months, used_days = deduct_time(
        0.17, life_days, rem_years, rem_months, rem_days
    )
    time_report("YOU HAVE EATEN", used_years, used_months, used_days)

    label = make_occupation_label(rem_years)
    rem_years, rem_months, rem_days, used_years, used_months, used_days = deduct_time(
        0.23, life_days, rem_years, rem_months, rem_days
    )
    time_report("YOU HAVE " + label, used_years, used_months, used_days)
    time_report("YOU HAVE RELAXED", rem_years, rem_months, rem_days)

    print()

    # Calculate retirement date
    e = year + 65
    print_with_tab(16, f"***  YOU MAY RETIRE IN {e} ***")
    end()


def test_weekday_calc(year, month, day):
    dt = datetime.date(year, month, day)
    python_weekday = dt.weekday()  # Monday = 0, Sunday = 6

    basic_weekday = calculate_day_of_week(year, month, day)  # Sunday = 1, Saturday = 7

    test = ((python_weekday + 2) % 7) == (basic_weekday % 7)
    if test == False:
        print(f"testing yr {year} month {month} day {day}")
        print(f"python says {python_weekday}")
        print(f"BASIC says {basic_weekday}")
        assert False


def test_harness():
    for yr in range(1600, 2021):
        for m in range(1, 12):
            for d in range(1, 28):
                test_weekday_calc(yr, m, d)


if __name__ == "__main__":
    main()

    # test_harness()
