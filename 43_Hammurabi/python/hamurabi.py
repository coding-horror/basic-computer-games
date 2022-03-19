from random import random, seed


def gen_random():
    return int(random() * 5) + 1


def bad_input_850():
    print("\nHAMURABI:  I CANNOT DO WHAT YOU WISH.")
    print("GET YOURSELF ANOTHER STEWARD!!!!!")


def bad_input_710(S):
    print("HAMURABI:  THINK AGAIN.  YOU HAVE ONLY")
    print(S, "BUSHELS OF GRAIN.  NOW THEN,")


def bad_input_720(A):
    print("HAMURABI:  THINK AGAIN.  YOU OWN ONLY", A, "ACRES.  NOW THEN,")


def national_fink():
    print("DUE TO THIS EXTREME MISMANAGEMENT YOU HAVE NOT ONLY")
    print("BEEN IMPEACHED AND THROWN OUT OF OFFICE BUT YOU HAVE")
    print("ALSO BEEN DECLARED NATIONAL FINK!!!!")


def b_input(promptstring):  # emulate BASIC input. It rejects non-numeric values
    x = input(promptstring)
    while x.isalpha():
        x = input("?REDO FROM START\n? ")
    return int(x)


def main() -> None:
    seed()
    title = "HAMURABI"
    title = title.rjust(32, " ")
    print(title)
    attribution = "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
    attribution = attribution.rjust(15, " ")
    print(attribution)
    print("\n\n\n")
    print("TRY YOUR HAND AT GOVERNING ANCIENT SUMERIA")
    print("FOR A TEN-YEAR TERM OF OFFICE.\n")

    D1 = 0
    P1: float = 0
    year = 0
    population = 95
    grain_stores = 2800
    H = 3000
    eaten_rats = H - grain_stores
    bushels_per_acre = (
        3  # yield (amount of production from land). Reused as price per acre
    )
    acres = H / bushels_per_acre  # acres of land
    immigrants = 5
    plague = 1  # boolean for plague, also input for buy/sell land
    people = 0

    while year < 11:  # line 270. main loop. while the year is less than 11
        print("\n\n\nHAMURABI:  I BEG TO REPORT TO YOU")
        year = year + 1  # year
        print(
            "IN YEAR",
            year,
            ",",
            people,
            "PEOPLE STARVED,",
            immigrants,
            "CAME TO THE CITY,",
        )
        population = population + immigrants

        if plague == 0:
            population = int(population / 2)
            print("A HORRIBLE PLAGUE STRUCK!  HALF THE PEOPLE DIED.")

        print("POPULATION IS NOW", population)
        print("THE CITY NOW OWNS", acres, "ACRES.")
        print("YOU HARVESTED", bushels_per_acre, "BUSHELS PER ACRE.")
        print("THE RATS ATE", eaten_rats, "BUSHELS.")
        print("YOU NOW HAVE ", grain_stores, "BUSHELS IN STORE.\n")
        C = int(10 * random())  # random number between 1 and 10
        bushels_per_acre = C + 17
        print("LAND IS TRADING AT", bushels_per_acre, "BUSHELS PER ACRE.")

        plague = -99  # dummy value to track status
        while plague == -99:  # always run the loop once
            plague = b_input("HOW MANY ACRES DO YOU WISH TO BUY? ")
            if plague < 0:
                plague = -1  # to avoid the corner case of Q=-99
                bad_input_850()
                year = 99  # jump out of main loop and exit
            elif bushels_per_acre * plague > grain_stores:  # can't afford it
                bad_input_710(grain_stores)
                plague = -99  # give'm a second change to get it right
            elif (
                bushels_per_acre * plague <= grain_stores
            ):  # normal case, can afford it
                acres = acres + plague  # increase the number of acres by Q
                grain_stores = (
                    grain_stores - bushels_per_acre * plague
                )  # decrease the amount of grain in store to pay for it
                C = 0  # WTF is C for?

        if plague == 0 and year != 99:  # maybe you want to sell some land?
            plague = -99
            while plague == -99:
                plague = b_input("HOW MANY ACRES DO YOU WISH TO SELL? ")
                if plague < 0:
                    bad_input_850()
                    year = 99  # jump out of main loop and exit
                elif plague <= acres:  # normal case
                    acres = acres - plague  # reduce the acres
                    grain_stores = (
                        grain_stores + bushels_per_acre * plague
                    )  # add to grain stores
                    C = 0  # still don't know what C is for
                else:  # Q>A error!
                    bad_input_720(acres)
                    plague = -99  # reloop
            print("\n")

        plague = -99
        while plague == -99 and year != 99:
            plague = b_input("HOW MANY BUSHELS DO YOU WISH TO FEED YOUR PEOPLE? ")
            if plague < 0:
                bad_input_850()
                year = 99  # jump out of main loop and exit
            # REM *** TRYING TO USE MORE GRAIN THAN IS IN SILOS?
            elif plague > grain_stores:
                bad_input_710(grain_stores)
                plague = -99  # try again!
            else:  # we're good. do the transaction
                grain_stores = grain_stores - plague  # remove the grain from the stores
                C = 1  # set the speed of light to 1. jk

        print("\n")
        people = -99  # dummy value to force at least one loop
        while people == -99 and year != 99:
            people = b_input("HOW MANY ACRES DO YOU WISH TO PLANT WITH SEED? ")
            if people < 0:
                bad_input_850()
                year = 99  # jump out of main loop and exit
            elif people > 0:
                if people > acres:
                    # REM *** TRYING TO PLANT MORE ACRES THAN YOU OWN?
                    bad_input_720(acres)
                    people = -99
                elif int(people / 2) > grain_stores:
                    # REM *** ENOUGH GRAIN FOR SEED?
                    bad_input_710(grain_stores)
                    people = -99
                elif people > 10 * population:
                    # REM *** ENOUGH PEOPLE TO TEND THE CROPS?
                    print(
                        "BUT YOU HAVE ONLY",
                        population,
                        "PEOPLE TO TEND THE FIELDS!  NOW THEN,",
                    )
                    people = -99
                else:  # we're good. decrement the grain store
                    grain_stores = grain_stores - int(people / 2)

        C = gen_random()
        # REM *** A BOUNTIFUL HARVEST!
        bushels_per_acre = C
        H = people * bushels_per_acre
        eaten_rats = 0

        C = gen_random()
        if int(C / 2) == C / 2:  # even number. 50/50 chance
            # REM *** RATS ARE RUNNING WILD!!
            eaten_rats = int(
                grain_stores / C
            )  # calc losses due to rats, based on previous random number

        grain_stores = grain_stores - eaten_rats + H  # deduct losses from stores

        C = gen_random()
        # REM *** LET'S HAVE SOME BABIES
        immigrants = int(C * (20 * acres + grain_stores) / population / 100 + 1)
        # REM *** HOW MANY PEOPLE HAD FULL TUMMIES?
        C = int(plague / 20)
        # REM *** HORROS, A 15% CHANCE OF PLAGUE
        # yeah, should be HORRORS, but left it
        plague = int(10 * (2 * random() - 0.3))
        if (
            population >= C and year != 99
        ):  # if there are some people without full bellies...
            # REM *** STARVE ENOUGH FOR IMPEACHMENT?
            people = population - C
            if people > 0.45 * population:
                print("\nYOU STARVED", people, "PEOPLE IN ONE YEAR!!!")
                national_fink()
                year = 99  # exit the loop
            P1 = ((year - 1) * P1 + people * 100 / population) / year
            population = C
            D1 = D1 + people

    if year != 99:
        print("IN YOUR 10-YEAR TERM OF OFFICE,", P1, "PERCENT OF THE")
        print("POPULATION STARVED PER YEAR ON THE AVERAGE, I.E. A TOTAL OF")
        print(D1, "PEOPLE DIED!!")
        L = acres / population
        print("YOU STARTED WITH 10 ACRES PER PERSON AND ENDED WITH")
        print(L, "ACRES PER PERSON.\n")
        if P1 > 33 or L < 7:
            national_fink()
        elif P1 > 10 or L < 9:
            print("YOUR HEAVY-HANDED PERFORMANCE SMACKS OF NERO AND IVAN IV.")
            print("THE PEOPLE (REMIANING) FIND YOU AN UNPLEASANT RULER, AND,")
            print("FRANKLY, HATE YOUR GUTS!!")
        elif P1 > 3 or L < 10:
            print("YOUR PERFORMANCE COULD HAVE BEEN SOMEWHAT BETTER, BUT")
            print(
                "REALLY WASN'T TOO BAD AT ALL. ",
                int(population * 0.8 * random()),
                "PEOPLE",
            )
            print("WOULD DEARLY LIKE TO SEE YOU ASSASSINATED BUT WE ALL HAVE OUR")
            print("TRIVIAL PROBLEMS.")
        else:
            print("A FANTASTIC PERFORMANCE!!!  CHARLEMANGE, DISRAELI, AND")
            print("JEFFERSON COMBINED COULD NOT HAVE DONE BETTER!\n")
        for _ in range(1, 10):
            print("\a")

    print("\nSO LONG FOR NOW.\n")


if __name__ == "__main__":
    main()
