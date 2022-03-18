import datetime

import pytest
from weekday import calculate_day_of_week


@pytest.mark.parametrize(
    ("year", "month", "day"),
    [
        (yr, m, d)
        for yr in range(1600, 2021)
        for m in range(1, 12)
        for d in range(1, 28)
    ],
)
@pytest.mark.slow  # Those are 125,037 tests!
def test_weekday_calc(year, month, day):
    dt = datetime.date(year, month, day)
    python_weekday = dt.weekday()  # Monday = 0, Sunday = 6

    basic_weekday = calculate_day_of_week(year, month, day)  # Sunday = 1, Saturday = 7

    if ((python_weekday + 2) % 7) != (basic_weekday % 7):
        print(f"testing yr {year} month {month} day {day}")
        print(f"python says {python_weekday}")
        print(f"BASIC says {basic_weekday}")
        assert False


@pytest.mark.parametrize(
    ("year", "month", "day"),
    [
        (yr, m, d)
        for yr in range(2016, 2021)
        for m in range(1, 12)
        for d in range(1, 28)
    ],
)
def test_weekday_calc_4_years(year, month, day):
    dt = datetime.date(year, month, day)
    python_weekday = dt.weekday()  # Monday = 0, Sunday = 6

    basic_weekday = calculate_day_of_week(year, month, day)  # Sunday = 1, Saturday = 7

    if ((python_weekday + 2) % 7) != (basic_weekday % 7):
        print(f"testing yr {year} month {month} day {day}")
        print(f"python says {python_weekday}")
        print(f"BASIC says {basic_weekday}")
        assert False
