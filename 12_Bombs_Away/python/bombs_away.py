"""
Bombs away

Ported from BASIC to Python3 by Bernard Cooke (bernardcooke53)
Tested with Python 3.8.10, formatted with Black and type checked with mypy.
"""
import random
from typing import Iterable


def _stdin_choice(*, prompt: str, choices: Iterable[str]) -> str:
    ret = input(prompt)
    while ret not in choices:
        print("TRY AGAIN...")
        ret = input(prompt)
    return ret


def player_survived() -> None:
    print("YOU MADE IT THROUGH TREMENDOUS FLAK!!")


def player_death() -> None:
    print("* * * * BOOM * * * *")
    print("YOU HAVE BEEN SHOT DOWN.....")
    print("DEARLY BELOVED, WE ARE GATHERED HERE TODAY TO PAY OUR")
    print("LAST TRIBUTE...")


def mission_success() -> None:
    print(f"DIRECT HIT!!!! {int(100 * random.random())} KILLED.")
    print("MISSION SUCCESSFUL.")


def death_with_chance(p_death: float) -> bool:
    """
    Takes a float between 0 and 1 and returns a boolean
    if the player has survived (based on random chance)

    Returns True if death, False if survived
    """
    return p_death > random.random()


def commence_non_kamikazi_attack() -> None:
    nmissions = int(input("HOW MANY MISSIONS HAVE YOU FLOWN? "))

    while nmissions >= 160:
        print("MISSIONS, NOT MILES...")
        print("150 MISSIONS IS HIGH EVEN FOR OLD-TIMERS")
        nmissions = int(input("NOW THEN, HOW MANY MISSIONS HAVE YOU FLOWN? "))

    if nmissions >= 100:
        print("THAT'S PUSHING THE ODDS!")

    if nmissions < 25:
        print("FRESH OUT OF TRAINING, EH?")

    print()
    return (
        mission_success() if nmissions >= 160 * random.random() else mission_failure()
    )


def mission_failure() -> None:
    weapons_choices = {
        "1": "GUNS",
        "2": "MISSILES",
        "3": "BOTH",
    }
    print(f"MISSED TARGET BY {int(2 + 30 * random.random())} MILES!")
    print("NOW YOU'RE REALLY IN FOR IT !!")
    print()
    enemy_weapons = _stdin_choice(
        prompt="DOES THE ENEMY HAVE GUNS(1), MISSILES(2), OR BOTH(3)? ",
        choices=weapons_choices.keys(),
    )

    # If there are no gunners (i.e. weapon choice 2) then
    # we say that the gunners have 0 accuracy for the purposes
    # of calculating probability of player death

    enemy_gunner_accuracy = 0.0
    if enemy_weapons != "2":
        # If the enemy has guns, how accurate are the gunners?
        enemy_gunner_accuracy = float(
            input("WHAT'S THE PERCENT HIT RATE OF ENEMY GUNNERS (10 TO 50)? ")
        )

        if enemy_gunner_accuracy < 10:
            print("YOU LIE, BUT YOU'LL PAY...")
            return player_death()

    missile_threat_weighting = 0 if enemy_weapons == "1" else 35

    death = death_with_chance(
        p_death=(enemy_gunner_accuracy + missile_threat_weighting) / 100
    )

    return player_survived() if not death else player_death()


def play_italy() -> None:
    targets_to_messages = {
        # 1 - ALBANIA, 2 - GREECE, 3 - NORTH AFRICA
        "1": "SHOULD BE EASY -- YOU'RE FLYING A NAZI-MADE PLANE.",
        "2": "BE CAREFUL!!!",
        "3": "YOU'RE GOING FOR THE OIL, EH?",
    }
    target = _stdin_choice(
        prompt="YOUR TARGET -- ALBANIA(1), GREECE(2), NORTH AFRICA(3)",
        choices=targets_to_messages.keys(),
    )

    print(targets_to_messages[target])
    return commence_non_kamikazi_attack()


def play_allies() -> None:
    aircraft_to_message = {
        "1": "YOU'VE GOT 2 TONS OF BOMBS FLYING FOR PLOESTI.",
        "2": "YOU'RE DUMPING THE A-BOMB ON HIROSHIMA.",
        "3": "YOU'RE CHASING THE BISMARK IN THE NORTH SEA.",
        "4": "YOU'RE BUSTING A GERMAN HEAVY WATER PLANT IN THE RUHR.",
    }
    aircraft = _stdin_choice(
        prompt="AIRCRAFT -- LIBERATOR(1), B-29(2), B-17(3), LANCASTER(4): ",
        choices=aircraft_to_message.keys(),
    )

    print(aircraft_to_message[aircraft])
    return commence_non_kamikazi_attack()


def play_japan() -> None:
    print("YOU'RE FLYING A KAMIKAZE MISSION OVER THE USS LEXINGTON.")
    first_mission = input("YOUR FIRST KAMIKAZE MISSION? (Y OR N): ")
    if first_mission.lower() == "n":
        return player_death()

    if random.random() > 0.65:
        return mission_success()
    return player_death()


def play_germany() -> None:
    targets_to_messages = {
        # 1 - RUSSIA, 2 - ENGLAND, 3 - FRANCE
        "1": "YOU'RE NEARING STALINGRAD.",
        "2": "NEARING LONDON.  BE CAREFUL, THEY'VE GOT RADAR.",
        "3": "NEARING VERSAILLES.  DUCK SOUP.  THEY'RE NEARLY DEFENSELESS.",
    }
    target = _stdin_choice(
        prompt="A NAZI, EH?  OH WELL.  ARE YOU GOING FOR RUSSIA(1),\nENGLAND(2), OR FRANCE(3)? ",
        choices=targets_to_messages.keys(),
    )

    print(targets_to_messages[target])

    return commence_non_kamikazi_attack()


def play_game() -> None:
    print("YOU ARE A PILOT IN A WORLD WAR II BOMBER.")
    sides = {"1": play_italy, "2": play_allies, "3": play_japan, "4": play_germany}
    side = _stdin_choice(
        prompt="WHAT SIDE -- ITALY(1), ALLIES(2), JAPAN(3), GERMANY(4): ",
        choices=sides.keys(),
    )
    return sides[side]()


if __name__ == "__main__":
    again = True
    while again:
        play_game()
        again = True if input("ANOTHER MISSION? (Y OR N): ").upper() == "Y" else False
