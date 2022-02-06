// WEEKDAY
//
// Converted from BASIC to Javascript by Oscar Toledo G. (nanochess)
//

/**
 * Print given string to the end of the "output" element.
 * @param str
 */
function print(str) {
    document.getElementById("output").appendChild(document.createTextNode(str));
}

/**
 * Obtain user input
 * @returns {Promise<String>}
 */
function input() {
    return new Promise(function (resolve) {
        const input_element = document.createElement("INPUT");

        print("? ");
        input_element.setAttribute("type", "text");
        input_element.setAttribute("length", "50");
        document.getElementById("output").appendChild(input_element);
        input_element.focus();
        input_element.addEventListener("keydown", function (event) {
            if (event.keyCode === 13) {
                const input_str = input_element.value;
                document.getElementById("output").removeChild(input_element);
                print(input_str);
                print("\n");
                resolve(input_str);
            }
        });
    });
}

/**
 * Create a string consisting of the given number of spaces
 * @param spaceCount
 * @returns {string}
 */
function tab(spaceCount) {
    let str = "";
    while (spaceCount-- > 0)
        str += " ";
    return str;
}

const MONTHS_PER_YEAR = 12;
const DAYS_PER_COMMON_YEAR = 365;
const DAYS_PER_IDEAL_MONTH = 30;
const MAXIMUM_DAYS_PER_MONTH = 31;

/**
 * Date representation.
 */
class DateStruct {
    #year;
    #month;
    #day;

    /**
     * Build a DateStruct
     * @param {number} year
     * @param {number} month
     * @param {number} day
     */
    constructor(year, month, day) {
        this.#year = year;
        this.#month = month;
        this.#day = day;
    }

    get year() {
        return this.#year;
    }

    get month() {
        return this.#month;
    }

    get day() {
        return this.#day;
    }

    /**
     * Determine if the date could be a Gregorian date.
     * Be aware the Gregorian calendar was not introduced in all places at once,
     * see https://en.wikipedia.org/wiki/Gregorian_calendar
     * @returns {boolean} true if date could be Gregorian; otherwise false.
     */
    isGregorianDate() {
        let result = false;
        if (this.#year > 1582) {
            result = true;
        } else if (this.#year === 1582) {
            if (this.#month > 10) {
                result = true;
            } else if (this.#month === 10 && this.#day >= 15) {
                result = true;
            }
        }
        return result;
    }

    /**
     * Returns a US formatted date, i.e. Month/Day/Year.
     * @returns {string}
     */
    toString() {
        return this.#month + "/" + this.#day + "/" + this.#year;
    }
}

/**
 * Duration representation.
 * Note: this class only handles positive durations well
 */
class Duration {
    #years;
    #months;
    #days;

    /**
     * Build a Duration
     * @param {number} years
     * @param {number} months
     * @param {number} days
     */
    constructor(years, months, days) {
        this.#years = years;
        this.#months = months;
        this.#days = days;
        this.#fixRanges();
    }

    get years() {
        return this.#years;
    }

    get months() {
        return this.#months;
    }

    get days() {
        return this.#days;
    }

    clone() {
        return new Duration(this.#years, this.#months, this.#days);
    }

    /**
     * Adjust Duration by removing years, months and days from supplied Duration.
     * This is a naive calculation which assumes all months are 30 days.
     * @param {Duration} timeToRemove
     */
    remove(timeToRemove) {
        this.#years -= timeToRemove.years;
        this.#months -= timeToRemove.months;
        this.#days -= timeToRemove.days;
        this.#fixRanges();
    }

    /**
     * Move days and months into expected range.
     */
    #fixRanges() {
        if (this.#days < 0) {
            this.#days += DAYS_PER_IDEAL_MONTH;
            this.#months--;
        }
        if (this.#months < 0) {
            this.#months += MONTHS_PER_YEAR;
            this.#years--;
        }
    }

    toString() {
        return this.#years + "/" + this.#months + "/" + this.#days;
    }

    /**
     * Determine approximate Duration between two dates.
     * This is a naive calculation which assumes all months are 30 days.
     * @param {DateStruct} date1
     * @param {DateStruct} date2
     * @returns {Duration}
     */
    static between(date1, date2) {
        let years = date1.year - date2.year;
        let months = date1.month - date2.month;
        let days = date1.day - date2.day;
        return new Duration(years, months, days);
    }
}

// In a common (non-leap) year the day of the week for the first of each month moves by the following amounts.
const COMMON_YEAR_MONTH_OFFSET = [0, 3, 3, 6, 1, 4, 6, 2, 5, 0, 3, 5];

/**
 * Reads a date, and extracts the date information.
 * This expects date parts to be comma separated, using US date ordering,
 * i.e. Month,Day,Year.
 * @returns {Promise<DateStruct>}
 */
async function readDateElements() {
    let dateString = await input();
    const month = parseInt(dateString);
    const day = parseInt(dateString.substr(dateString.indexOf(",") + 1));
    const year = parseInt(dateString.substr(dateString.lastIndexOf(",") + 1));
    return new DateStruct(year, month, day);
}

/**
 * Calculate years, months and days as factor of days.
 * This is a naive calculation which assumes all months are 30 days.
 * @param factor
 * @param dayCount
 * @returns {Duration}
 */
function time_spent(factor, dayCount) {
    let totalDays = Math.floor(factor * dayCount);
    const years = Math.floor(totalDays / DAYS_PER_COMMON_YEAR);
    totalDays -= years * DAYS_PER_COMMON_YEAR;
    const months = Math.floor(totalDays / DAYS_PER_IDEAL_MONTH);
    const days = totalDays - (months * DAYS_PER_IDEAL_MONTH);
    return new Duration(years, months, days);
}

/**
 * Print the supplied duration.
 * @param {Duration} duration
 */
function printTimeSpent(duration) {
    print(duration.years + "\t" + duration.months + "\t" + duration.days + "\n");
}

/**
 * Determine if the given year is a leap year.
 * @param year
 * @returns {boolean}
 */
function isLeapYear(year) {
    if ((year % 4) !== 0) {
        return false;
    } else if ((year % 100) !== 0) {
        return true;
    } else if ((year % 400) !== 0) {
        return false;
    }
    return true;
}

/**
 * Determine the day of the week.
 * This calculation returns a number between 1 and 7 where Sunday=1, Monday=2, ..., Saturday=7.
 * @param {DateStruct} date
 * @returns {number} Value between 1 and 7 representing Sunday to Saturday.
 */
function getDayOfWeek(date) {
    // Calculate an offset based on the century part of the year.
    const centuriesSince1500 = Math.floor((date.year - 1500) / 100);
    let centuryOffset = centuriesSince1500 * 5 + (centuriesSince1500 + 3) / 4;
    centuryOffset = Math.floor(centuryOffset % 7);

    // Calculate an offset based on the shortened two digit year.
    // January 1st moves forward by approximately 1.25 days per year
    const yearInCentury = date.year % 100;
    const yearInCenturyOffsets = yearInCentury / 4 + yearInCentury;

    // combine offsets with day and month
    let dayOfWeek = centuryOffset + yearInCenturyOffsets + date.day + COMMON_YEAR_MONTH_OFFSET[date.month - 1];

    dayOfWeek = Math.floor(dayOfWeek % 7) + 1;
    if (date.month <= 2 && isLeapYear(date.year)) {
        dayOfWeek--;
    }
    if (dayOfWeek === 0) {
        dayOfWeek = 7;
    }
    return dayOfWeek;
}

/**
 * Obtain text for the day of the week.
 * @param {DateStruct} date
 * @returns {string}
 */
function getDayOfWeekText(date) {
    const dayOfWeek = getDayOfWeek(date);
    let dayOfWeekText = "";
    switch (dayOfWeek) {
        case 1:
            dayOfWeekText = "SUNDAY.";
            break;
        case 2:
            dayOfWeekText = "MONDAY.";
            break;
        case 3:
            dayOfWeekText = "TUESDAY.";
            break;
        case 4:
            dayOfWeekText = "WEDNESDAY.";
            break;
        case 5:
            dayOfWeekText = "THURSDAY.";
            break;
        case 6:
            if (date.day === 13) {
                dayOfWeekText = "FRIDAY THE THIRTEENTH---BEWARE!";
            } else {
                dayOfWeekText = "FRIDAY.";
            }
            break;
        case 7:
            dayOfWeekText = "SATURDAY.";
            break;
    }
    return dayOfWeekText;
}

/**
 * The following performs a hash on the day parts which guarantees that
 * 1. different days will return different numbers
 * 2. the numbers returned are ordered.
 * @param {DateStruct} date
 * @returns {number}
 */
function getNormalisedDay(date) {
    return (date.year * MONTHS_PER_YEAR + date.month) * MAXIMUM_DAYS_PER_MONTH + date.day;
}

// Main control section
async function main() {
    print(tab(32) + "WEEKDAY\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("WEEKDAY IS A COMPUTER DEMONSTRATION THAT\n");
    print("GIVES FACTS ABOUT A DATE OF INTEREST TO YOU.\n");
    print("\n");
    print("ENTER TODAY'S DATE IN THE FORM: 3,24,1979  ");
    const today = await readDateElements();
    // This program determines the day of the week
    //  for a date after 1582
    print("ENTER DAY OF BIRTH (OR OTHER DAY OF INTEREST)");
    const dateOfBirth = await readDateElements();
    print("\n");
    // Test for date before current calendar.
    if (!dateOfBirth.isGregorianDate()) {
        print("NOT PREPARED TO GIVE DAY OF WEEK PRIOR TO X.XV.MDLXXXII.\n");
    } else {
        const normalisedToday = getNormalisedDay(today);
        const normalisedDob = getNormalisedDay(dateOfBirth);

        let dayOfWeekText = getDayOfWeekText(dateOfBirth);
        if (normalisedToday < normalisedDob) {
            print(dateOfBirth + " WILL BE A " + dayOfWeekText + "\n");
        } else if (normalisedToday === normalisedDob) {
            print(dateOfBirth + " IS A " + dayOfWeekText + "\n");
        } else {
            print(dateOfBirth + " WAS A " + dayOfWeekText + "\n");
        }

        if (normalisedToday !== normalisedDob) {
            print("\n");
            let differenceBetweenDates = Duration.between(today, dateOfBirth);
            if (differenceBetweenDates.years >= 0) {
                if (differenceBetweenDates.days === 0 && differenceBetweenDates.months === 0) {
                    print("***HAPPY BIRTHDAY***\n");
                }
                print("                        \tYEARS\tMONTHS\tDAYS\n");
                print("                        \t-----\t------\t----\n");
                print("YOUR AGE (IF BIRTHDATE) \t");
                printTimeSpent(differenceBetweenDates);
                const approximateDaysBetween = (differenceBetweenDates.years * DAYS_PER_COMMON_YEAR) + (differenceBetweenDates.months * DAYS_PER_IDEAL_MONTH) + differenceBetweenDates.days + Math.floor(differenceBetweenDates.months / 2);
                // Create an object containing time unaccounted for
                const unaccountedTime = differenceBetweenDates.clone();

                // Calculate time spent in the following functions.
                print("YOU HAVE SLEPT \t\t\t");
                const sleepTimeSpent = time_spent(0.35, approximateDaysBetween);
                printTimeSpent(sleepTimeSpent);

                unaccountedTime.remove(sleepTimeSpent);
                print("YOU HAVE EATEN \t\t\t");
                const eatenTimeSpent = time_spent(0.17, approximateDaysBetween);
                printTimeSpent(eatenTimeSpent);

                unaccountedTime.remove(eatenTimeSpent);
                if (unaccountedTime.years <= 3) {
                    print("YOU HAVE PLAYED \t\t");
                } else if (unaccountedTime.years <= 9) {
                    print("YOU HAVE PLAYED/STUDIED \t\t");
                } else {
                    print("YOU HAVE WORKED/PLAYED \t\t");
                }
                const workPlayTimeSpent = time_spent(0.23, approximateDaysBetween);
                printTimeSpent(workPlayTimeSpent);

                unaccountedTime.remove(workPlayTimeSpent);
                print("YOU HAVE RELAXED \t\t");
                printTimeSpent(unaccountedTime);

                const retirementYear = dateOfBirth.year + 65;
                print("\n");
                print(tab(16) + "***  YOU MAY RETIRE IN " + retirementYear + " ***\n");
                print("\n");
            }
        }
    }
    print("\n");
    print("\n");
    print("\n");
    print("\n");
    print("\n");
}

main();
