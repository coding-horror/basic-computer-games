"""
KING

A strategy game where the player is the king.

Ported to Python by Martin Thoma in 2022
"""

import sys
from dataclasses import dataclass
from random import randint, random

FOREST_LAND = 1000
INITIAL_LAND = FOREST_LAND + 1000
COST_OF_LIVING = 100
COST_OF_FUNERAL = 9
YEARS_IN_TERM = 8
POLLUTION_CONTROL_FACTOR = 25


def ask_int(prompt) -> int:
    while True:
        try:
            return int(input(prompt))
        except ValueError:
            continue


@dataclass
class GameState:
    rallods: int = -1
    countrymen: int = -1
    land: int = INITIAL_LAND
    foreign_workers: int = 0
    years_in_office: int = 0

    # previous year stats
    crop_loss_last_year: int = 0

    # current year stats
    died_contrymen: int = 0
    pollution_deaths: int = 0
    population_change: int = 0

    # current year - market situation (in rallods per square mile)
    planting_cost: int = -1
    land_buy_price: int = -1

    tourism_earnings: int = 0

    def set_market_conditions(self) -> None:
        self.land_buy_price = randint(95, 105)
        self.planting_cost = randint(10, 15)

    @property
    def farmland(self) -> int:
        return self.land - FOREST_LAND

    @property
    def settled_people(self) -> int:
        return self.countrymen - self.population_change

    def sell_land(self, amount: int) -> None:
        assert amount < self.farmland
        self.land -= amount
        self.rallods += self.land_buy_price * amount

    def distribute_rallods(self, distribute: int) -> None:
        self.rallods -= distribute

    def spend_pollution_control(self, spend: int) -> None:
        self.rallods -= spend

    def plant(self, sq_to_plant: int) -> None:
        self.rallods -= sq_to_plant * self.planting_cost

    def print_status(self) -> None:
        print(f"\n\nYOU NOW HAVE {self.rallods} RALLODS IN THE TREASURY.")
        print(f"{int(self.countrymen)} COUNTRYMEN, ", end="")
        if self.foreign_workers > 0:
            print(f"{int(self.foreign_workers)} FOREIGN WORKERS, ", end="")
        print(f"AND {self.land} SQ. MILES OF LAND.")
        print(
            f"THIS YEAR INDUSTRY WILL BUY LAND FOR {self.land_buy_price} "
            "RALLODS PER SQUARE MILE."
        )
        print(
            f"LAND CURRENTLY COSTS {self.planting_cost} RALLODS "
            "PER SQUARE MILE TO PLANT.\n"
        )

    def handle_deaths(
        self, distributed_rallods: int, pollution_control_spendings: int
    ) -> None:
        starved_countrymen = max(
            0, int(self.countrymen - distributed_rallods / COST_OF_LIVING)
        )

        if starved_countrymen > 0:
            print(f"{starved_countrymen} COUNTRYMEN DIED OF STARVATION")

        self.pollution_deaths = int(random() * (INITIAL_LAND - self.land))
        if pollution_control_spendings >= POLLUTION_CONTROL_FACTOR:
            self.pollution_deaths = int(
                self.pollution_deaths
                / (pollution_control_spendings / POLLUTION_CONTROL_FACTOR)
            )
        if self.pollution_deaths > 0:
            print(
                f"{self.pollution_deaths} COUNTRYMEN DIED OF CARBON-MONOXIDE "
                f"AND DUST INHALATION"
            )

        self.died_contrymen = starved_countrymen + self.pollution_deaths
        if self.died_contrymen > 0:
            funeral_cost = self.died_contrymen * COST_OF_FUNERAL
            print(f"   YOU WERE FORCED TO SPEND {funeral_cost} RALLODS ON ")
            print("FUNERAL EXPENSES.")
            self.rallods -= funeral_cost
            if self.rallods < 0:
                print("   INSUFFICIENT RESERVES TO COVER COST - LAND WAS SOLD")
                self.land += int(self.rallods / self.land_buy_price)
                self.rallods = 0
            self.countrymen -= self.died_contrymen

    def handle_tourist_trade(self) -> None:
        V1 = int(self.settled_people * 22 + random() * 500)
        V2 = int((INITIAL_LAND - self.land) * 15)
        tourist_trade_earnings = int(V1 - V2)
        print(f" YOU MADE {tourist_trade_earnings} RALLODS FROM TOURIST TRADE.")
        if V2 != 0 and not (V1 - V2 >= self.tourism_earnings):
            print("   DECREASE BECAUSE ")
            reason = randint(0, 10)
            if reason <= 2:
                print("FISH POPULATION HAS DWINDLED DUE TO WATER POLLUTION.")
            if reason <= 4:
                print("AIR POLLUTION IS KILLING GAME BIRD POPULATION.")
            if reason <= 6:
                print("MINERAL BATHS ARE BEING RUINED BY WATER POLLUTION.")
            if reason <= 8:
                print("UNPLEASANT SMOG IS DISCOURAGING SUN BATHERS.")
            if reason <= 10:
                print("HOTELS ARE LOOKING SHABBY DUE TO SMOG GRIT.")

        # NOTE: The following two lines had a bug in the original game:
        self.tourism_earnings = abs(int(V1 - V2))
        self.rallods += self.tourism_earnings

    def handle_harvest(self, planted_sq: int) -> None:
        crop_loss = int((INITIAL_LAND - self.land) * ((random() + 1.5) / 2))
        if self.foreign_workers != 0:
            print(f"OF {planted_sq} SQ. MILES PLANTED,")
        if planted_sq <= crop_loss:
            crop_loss = planted_sq
        harvested = int(planted_sq - crop_loss)
        print(f" YOU HARVESTED {harvested} SQ. MILES OF CROPS.")
        unlucky_harvesting_worse = crop_loss - self.crop_loss_last_year
        if crop_loss != 0:
            print("   (DUE TO ", end="")
            if unlucky_harvesting_worse > 2:
                print("INCREASED ", end="")
            print("AIR AND WATER POLLUTION FROM FOREIGN INDUSTRY.)")
        revenue = int((planted_sq - crop_loss) * (self.land_buy_price / 2))
        print(f"MAKING {revenue} RALLODS.")
        self.crop_loss_last_year = crop_loss
        self.rallods += revenue

    def handle_foreign_workers(
        self,
        sm_sell_to_industry: int,
        distributed_rallods: int,
        polltion_control_spendings: int,
    ) -> None:
        foreign_workers_influx = 0
        if sm_sell_to_industry != 0:
            foreign_workers_influx = int(
                sm_sell_to_industry + (random() * 10) - (random() * 20)
            )
            if self.foreign_workers <= 0:
                foreign_workers_influx = foreign_workers_influx + 20
            print(f"{foreign_workers_influx} WORKERS CAME TO THE COUNTRY AND")

        surplus_distributed = distributed_rallods / COST_OF_LIVING - self.countrymen
        population_change = int(
            (surplus_distributed / 10)
            + (polltion_control_spendings / POLLUTION_CONTROL_FACTOR)
            - ((INITIAL_LAND - self.land) / 50)
            - (self.died_contrymen / 2)
        )
        print(f"{abs(population_change)} COUNTRYMEN ", end="")
        if population_change < 0:
            print("LEFT ", end="")
        else:
            print("CAME TO ", end="")
        print("THE ISLAND")
        self.countrymen += population_change
        self.foreign_workers += int(foreign_workers_influx)

    def handle_too_many_deaths(self) -> None:
        print(f"\n\n\n{self.died_contrymen} COUNTRYMEN DIED IN ONE YEAR!!!!!")
        print("\n\n\nDUE TO THIS EXTREME MISMANAGEMENT, YOU HAVE NOT ONLY")
        print("BEEN IMPEACHED AND THROWN OUT OF OFFICE, BUT YOU")
        message = randint(0, 10)
        if message <= 3:
            print("ALSO HAD YOUR LEFT EYE GOUGED OUT!")
        if message <= 6:
            print("HAVE ALSO GAINED A VERY BAD REPUTATION.")
        if message <= 10:
            print("HAVE ALSO BEEN DECLARED NATIONAL FINK.")
        sys.exit()

    def handle_third_died(self) -> None:
        print()
        print()
        print("OVER ONE THIRD OF THE POPULTATION HAS DIED SINCE YOU")
        print("WERE ELECTED TO OFFICE. THE PEOPLE (REMAINING)")
        print("HATE YOUR GUTS.")
        self.end_game()

    def handle_money_mismanagement(self) -> None:
        print()
        print("MONEY WAS LEFT OVER IN THE TREASURY WHICH YOU DID")
        print("NOT SPEND. AS A RESULT, SOME OF YOUR COUNTRYMEN DIED")
        print("OF STARVATION. THE PUBLIC IS ENRAGED AND YOU HAVE")
        print("BEEN FORCED TO EITHER RESIGN OR COMMIT SUICIDE.")
        print("THE CHOICE IS YOURS.")
        print("IF YOU CHOOSE THE LATTER, PLEASE TURN OFF YOUR COMPUTER")
        print("BEFORE PROCEEDING.")
        sys.exit()

    def handle_too_many_foreigners(self) -> None:
        print("\n\nTHE NUMBER OF FOREIGN WORKERS HAS EXCEEDED THE NUMBER")
        print("OF COUNTRYMEN. AS A MINORITY, THEY HAVE REVOLTED AND")
        print("TAKEN OVER THE COUNTRY.")
        self.end_game()

    def end_game(self) -> None:
        if random() <= 0.5:
            print("YOU HAVE BEEN ASSASSINATED.")
        else:
            print("YOU HAVE BEEN THROWN OUT OF OFFICE AND ARE NOW")
            print("RESIDING IN PRISON.")
        sys.exit()

    def handle_congratulations(self) -> None:
        print("\n\nCONGRATULATIONS!!!!!!!!!!!!!!!!!!")
        print(f"YOU HAVE SUCCESFULLY COMPLETED YOUR {YEARS_IN_TERM} YEAR TERM")
        print("OF OFFICE. YOU WERE, OF COURSE, EXTREMELY LUCKY, BUT")
        print("NEVERTHELESS, IT'S QUITE AN ACHIEVEMENT. GOODBYE AND GOOD")
        print("LUCK - YOU'LL PROBABLY NEED IT IF YOU'RE THE TYPE THAT")
        print("PLAYS THIS GAME.")
        sys.exit()


def print_header() -> None:
    print(" " * 34 + "KING")
    print(" " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n")


def print_instructions() -> None:
    print(
        f"""\n\n\nCONGRATULATIONS! YOU'VE JUST BEEN ELECTED PREMIER OF SETATS
DETINU, A SMALL COMMUNIST ISLAND 30 BY 70 MILES LONG. YOUR
JOB IS TO DECIDE UPON THE CONTRY'S BUDGET AND DISTRIBUTE
MONEY TO YOUR COUNTRYMEN FROM THE COMMUNAL TREASURY.
THE MONEY SYSTEM IS RALLODS, AND EACH PERSON NEEDS {COST_OF_LIVING}
RALLODS PER YEAR TO SURVIVE. YOUR COUNTRY'S INCOME COMES
FROM FARM PRODUCE AND TOURISTS VISITING YOUR MAGNIFICENT
FORESTS, HUNTING, FISHING, ETC. HALF YOUR LAND IS FARM LAND
WHICH ALSO HAS AN EXCELLENT MINERAL CONTENT AND MAY BE SOLD
TO FOREIGN INDUSTRY (STRIP MINING) WHO IMPORT AND SUPPORT
THEIR OWN WORKERS. CROPS COST BETWEEN 10 AND 15 RALLODS PER
SQUARE MILE TO PLANT.
YOUR GOAL IS TO COMPLETE YOUR {YEARS_IN_TERM} YEAR TERM OF OFFICE.
GOOD LUCK!"""
    )


def ask_how_many_sq_to_plant(state: GameState) -> int:
    while True:
        sq = ask_int("HOW MANY SQUARE MILES DO YOU WISH TO PLANT? ")
        if sq < 0:
            continue
        elif sq > 2 * state.countrymen:
            print("   SORRY, BUT EACH COUNTRYMAN CAN ONLY PLANT 2 SQ. MILES.")
        elif sq > state.farmland:
            print(
                f"   SORRY, BUT YOU ONLY HAVE {state.farmland} "
                "SQ. MILES OF FARM LAND."
            )
        elif sq * state.planting_cost > state.rallods:
            print(
                f"   THINK AGAIN. YOU'VE ONLY {state.rallods} RALLODS "
                "LEFT IN THE TREASURY."
            )
        else:
            return sq


def ask_pollution_control(state: GameState) -> int:
    while True:
        rallods = ask_int(
            "HOW MANY RALLODS DO YOU WISH TO SPEND ON POLLUTION CONTROL? "
        )
        if rallods > state.rallods:
            print(f"   THINK AGAIN. YOU ONLY HAVE {state.rallods} RALLODS REMAINING.")
        elif rallods < 0:
            continue
        else:
            return rallods


def ask_sell_to_industry(state: GameState) -> int:
    had_first_err = False
    first = """(FOREIGN INDUSTRY WILL ONLY BUY FARM LAND BECAUSE
FOREST LAND IS UNECONOMICAL TO STRIP MINE DUE TO TREES,
THICKER TOP SOIL, ETC.)"""
    err = f"""***  THINK AGAIN. YOU ONLY HAVE {state.farmland} SQUARE MILES OF FARM LAND."""
    while True:
        sm = input("HOW MANY SQUARE MILES DO YOU WISH TO SELL TO INDUSTRY? ")
        try:
            sm_sell = int(sm)
        except ValueError:
            if not had_first_err:
                print(first)
                had_first_err = True
            print(err)
            continue
        if sm_sell > state.farmland:
            print(err)
        elif sm_sell < 0:
            continue
        else:
            return sm_sell


def ask_distribute_rallods(state: GameState) -> int:
    while True:
        rallods = ask_int(
            "HOW MANY RALLODS WILL YOU DISTRIBUTE AMONG YOUR COUNTRYMEN? "
        )
        if rallods < 0:
            continue
        elif rallods > state.rallods:
            print(
                f"   THINK AGAIN. YOU'VE ONLY {state.rallods} RALLODS IN THE TREASURY"
            )
        else:
            return rallods


def resume() -> GameState:
    while True:
        years = ask_int("HOW MANY YEARS HAD YOU BEEN IN OFFICE WHEN INTERRUPTED? ")
        if years < 0:
            sys.exit()
        if years >= YEARS_IN_TERM:
            print(f"   COME ON, YOUR TERM IN OFFICE IS ONLY {YEARS_IN_TERM} YEARS.")
        else:
            break
    treasury = ask_int("HOW MUCH DID YOU HAVE IN THE TREASURY? ")
    if treasury < 0:
        sys.exit()
    countrymen = ask_int("HOW MANY COUNTRYMEN? ")
    if countrymen < 0:
        sys.exit()
    workers = ask_int("HOW MANY WORKERS? ")
    if workers < 0:
        sys.exit()
    while True:
        land = ask_int("HOW MANY SQUARE MILES OF LAND? ")
        if land < 0:
            sys.exit()
        if land > INITIAL_LAND:
            farm_land = INITIAL_LAND - FOREST_LAND
            print(f"   COME ON, YOU STARTED WITH {farm_land:,} SQ. MILES OF FARM LAND")
            print(f"   AND {FOREST_LAND:,} SQ. MILES OF FOREST LAND.")
        if land > FOREST_LAND:
            break
    return GameState(
        rallods=treasury,
        countrymen=countrymen,
        foreign_workers=workers,
        years_in_office=years,
    )


def main() -> None:
    print_header()
    want_instructions = input("DO YOU WANT INSTRUCTIONS? ").upper()
    if want_instructions == "AGAIN":
        state = resume()
    else:
        state = GameState(
            rallods=randint(59000, 61000),
            countrymen=randint(490, 510),
            planting_cost=randint(10, 15),
        )
    if want_instructions != "NO":
        print_instructions()

    while True:
        state.set_market_conditions()
        state.print_status()

        # Users actions
        sm_sell_to_industry = ask_sell_to_industry(state)
        state.sell_land(sm_sell_to_industry)

        distributed_rallods = ask_distribute_rallods(state)
        state.distribute_rallods(distributed_rallods)

        planted_sq = ask_how_many_sq_to_plant(state)
        state.plant(planted_sq)
        polltion_control_spendings = ask_pollution_control(state)
        state.spend_pollution_control(polltion_control_spendings)

        # Run the year
        state.handle_deaths(distributed_rallods, polltion_control_spendings)
        state.handle_foreign_workers(
            sm_sell_to_industry, distributed_rallods, polltion_control_spendings
        )
        state.handle_harvest(planted_sq)
        state.handle_tourist_trade()

        if state.died_contrymen > 200:
            state.handle_too_many_deaths()
        if state.countrymen < 343:
            state.handle_third_died()
        elif (
            state.rallods / 100
        ) > 5 and state.died_contrymen - state.pollution_deaths >= 2:
            state.handle_money_mismanagement()
        if state.foreign_workers > state.countrymen:
            state.handle_too_many_foreigners()
        elif YEARS_IN_TERM - 1 == state.years_in_office:
            state.handle_congratulations()
        else:
            state.years_in_office += 1
            state.died_contrymen = 0


if __name__ == "__main__":
    main()
