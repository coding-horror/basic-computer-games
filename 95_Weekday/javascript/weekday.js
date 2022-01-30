// WEEKDAY
//
// Converted from BASIC to Javascript by Oscar Toledo G. (nanochess)
//

function print(str)
{
    document.getElementById("output").appendChild(document.createTextNode(str));
}

function input()
{
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

function tab(space)
{
    let str = "";
    while (space-- > 0)
        str += " ";
    return str;
}

function fna(arg) {
    return Math.floor(arg / 4);
}

function fnb(arg) {
    return Math.floor(arg / 7);
}

// in a non-leap year the day of the week for the first of each month moves by the following amounts.
const MONTHLY_DAY_OF_WEEK_OFFSETS = [0, 3, 3, 6, 1, 4, 6, 2, 5, 0, 3, 5];

/**
 * Reads a date, and extracts the date information.
 * This expects date parts to be comma separated, using US date ordering,
 * i.e. Month,Day,Year.
 * @returns {Promise<[number,number,number]>} [year, month, dayOfMonth]
 */
async function readDateElements() {
    let dateString = await input();
    const month = parseInt(dateString);
    const dayOfMonth = parseInt(dateString.substr(dateString.indexOf(",") + 1));
    const year = parseInt(dateString.substr(dateString.lastIndexOf(",") + 1));
    return [year, month, dayOfMonth];
}

/**
 * Returns a US formatted date, i.e. Month/Day/Year.
 * @param year
 * @param month
 * @param dayOfMonth
 * @returns {string}
 */
function getFormattedDate(year, month, dayOfMonth) {
    return month + "/" + dayOfMonth + "/" + year;
}

let k5;
let k6;
let k7;

function time_spent(f, a8)
{
    let k1 = Math.floor(f * a8);
    const i5 = Math.floor(k1 / 365);
    k1 -= i5 * 365;
    const i6 = Math.floor(k1 / 30);
    const i7 = k1 - (i6 * 30);
    k5 -= i5;
    k6 -= i6;
    k7 -= i7;
    if (k7 < 0) {
        k7 += 30;
        k6--;
    }
    if (k6 <= 0) {
        k6 += 12;
        k5--;
    }
    print(i5 + "\t" + i6 + "\t" + i7 + "\n");
}

function getDayOfWeek(dobYear, dobMonth, dobDayOfMonth) {
    const i1 = Math.floor((dobYear - 1500) / 100);
    let a = i1 * 5 + (i1 + 3) / 4;
    const i2 = Math.floor(a - fnb(a) * 7);
    const y2 = Math.floor(dobYear / 100);
    const y3 = Math.floor(dobYear - y2 * 100);
    a = y3 / 4 + y3 + dobDayOfMonth + MONTHLY_DAY_OF_WEEK_OFFSETS[dobMonth-1] + i2;
    let dayOfWeek = Math.floor(a - fnb(a) * 7) + 1;
    if (dobMonth <= 2) {
        if (y3 !== 0) {
            t1 = Math.floor(dobYear - fna(dobYear) * 4);
        } else {
            a = i1 - 1;
            t1 = Math.floor(a - fna(a) * 4);
        }
        if (t1 === 0) {
            if (dayOfWeek === 0) {
                dayOfWeek = 6;
            }
            dayOfWeek--;
        }
    }
    if (dayOfWeek === 0) {
        dayOfWeek = 7;
    }
    return dayOfWeek;
}

/**
 * The following performs a special hash on the day parts which guarantees
 * that different days will return different numbers, and the numbers returned are in ordered.
 * @param todayYear
 * @param todayMonth
 * @param todayDayOfMonth
 * @returns {*}
 */
function getNormalisedDay(todayYear, todayMonth, todayDayOfMonth) {
    return (todayYear * 12 + todayMonth) * 31 + todayDayOfMonth;
}

// Main control section
async function main()
{
    print(tab(32) + "WEEKDAY\n");
    print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
    print("\n");
    print("\n");
    print("\n");
    print("WEEKDAY IS A COMPUTER DEMONSTRATION THAT\n");
    print("GIVES FACTS ABOUT A DATE OF INTEREST TO YOU.\n");
    print("\n");
    print("ENTER TODAY'S DATE IN THE FORM: 3,24,1979  ");
    const [todayYear, todayMonth, todayDayOfMonth] = await readDateElements();
    // This program determines the day of the week
    //  for a date after 1582
    print("ENTER DAY OF BIRTH (OR OTHER DAY OF INTEREST)");
    const [dobYear, dobMonth, dobDayOfMonth] = await readDateElements();
    print("\n");
    // Test for date before current calendar.
    // Note: this test is unreliable - the Gregorian calendar was introduced on Friday 15 October 1582
    // and the weekday algorithm fails for dates prior to that
    if (dobYear - 1582 < 0) {
        print("NOT PREPARED TO GIVE DAY OF WEEK PRIOR TO MDLXXXII.\n");
    } else {
        const dayOfWeek = getDayOfWeek(dobYear, dobMonth, dobDayOfMonth);

        const normalisedToday = getNormalisedDay(todayYear, todayMonth, todayDayOfMonth);
        const normalisedDob = getNormalisedDay(dobYear, dobMonth, dobDayOfMonth);

        if (normalisedToday < normalisedDob) {
            print(getFormattedDate(dobYear, dobMonth, dobDayOfMonth) + " WILL BE A ");
        } else if (normalisedToday === normalisedDob) {
            print(getFormattedDate(dobYear, dobMonth, dobDayOfMonth) + " IS A ");
        } else {
            print(getFormattedDate(dobYear, dobMonth, dobDayOfMonth) + " WAS A ");
        }
        switch (dayOfWeek) {
            case 1: print("SUNDAY.\n"); break;
            case 2: print("MONDAY.\n"); break;
            case 3: print("TUESDAY.\n"); break;
            case 4: print("WEDNESDAY.\n"); break;
            case 5: print("THURSDAY.\n"); break;
            case 6:
                if (dobDayOfMonth === 13) {
                    print("FRIDAY THE THIRTEENTH---BEWARE!\n");
                } else {
                    print("FRIDAY.\n");
                }
                break;
            case 7: print("SATURDAY.\n"); break;
        }
        if (normalisedToday !== normalisedDob) {
            let yearsBetweenDates = todayYear - dobYear;
            print("\n");
            let monthsBetweenDates = todayMonth - dobMonth;
            let daysBetweenDates = todayDayOfMonth - dobDayOfMonth;
            if (daysBetweenDates < 0) {
                monthsBetweenDates--;
                daysBetweenDates += 30;
            }
            if (monthsBetweenDates < 0) {
                yearsBetweenDates--;
                monthsBetweenDates += 12;
            }
            if (yearsBetweenDates >= 0) {
                if (daysBetweenDates === 0 && monthsBetweenDates === 0) {
                    print("***HAPPY BIRTHDAY***\n");
                }
                print("                        \tYEARS\tMONTHS\tDAYS\n");
                print("                        \t-----\t------\t----\n");
                print("YOUR AGE (IF BIRTHDATE) \t" + yearsBetweenDates + "\t" + monthsBetweenDates + "\t" + daysBetweenDates + "\n");
                const a8 = (yearsBetweenDates * 365) + (monthsBetweenDates * 30) + daysBetweenDates + Math.floor(monthsBetweenDates / 2);
                k5 = yearsBetweenDates;
                k6 = monthsBetweenDates;
                k7 = daysBetweenDates;
                // Calculate retirement date.
                const e = dobYear + 65;
                // Calculate time spent in the following functions.
                print("YOU HAVE SLEPT \t\t\t");
                time_spent(0.35, a8);
                print("YOU HAVE EATEN \t\t\t");
                time_spent(0.17, a8);
                if (k5 <= 3) {
                    print("YOU HAVE PLAYED \t\t\t");
                } else if (k5 <= 9) {
                    print("YOU HAVE PLAYED/STUDIED \t\t");
                } else {
                    print("YOU HAVE WORKED/PLAYED \t\t");
                }
                time_spent(0.23, a8);
                if (k6 === 12) {
                    k5++;
                    k6 = 0;
                }
                print("YOU HAVE RELAXED \t\t" + k5 + "\t" + k6 + "\t" + k7 + "\n");
                print("\n");
                print(tab(16) + "***  YOU MAY RETIRE IN " + e + " ***\n");
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
