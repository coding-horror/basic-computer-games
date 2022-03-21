"""
LUNAR

Lunar landing simulation

Ported by Dave LeCompte
"""

import collections
import math

PAGE_WIDTH = 64

COLUMN_WIDTH = 2
SECONDS_WIDTH = 4
MPH_WIDTH = 6
ALT_MI_WIDTH = 6
ALT_FT_WIDTH = 4
MPH_WIDTH = 6
FUEL_WIDTH = 8
BURN_WIDTH = 10

SECONDS_LEFT = 0
SECONDS_RIGHT = SECONDS_LEFT + SECONDS_WIDTH
ALT_LEFT = SECONDS_RIGHT + COLUMN_WIDTH
ALT_MI_RIGHT = ALT_LEFT + ALT_MI_WIDTH
ALT_FT_RIGHT = ALT_MI_RIGHT + COLUMN_WIDTH + ALT_FT_WIDTH
MPH_LEFT = ALT_FT_RIGHT + COLUMN_WIDTH
MPH_RIGHT = MPH_LEFT + MPH_WIDTH
FUEL_LEFT = MPH_RIGHT + COLUMN_WIDTH
FUEL_RIGHT = FUEL_LEFT + FUEL_WIDTH
BURN_LEFT = FUEL_RIGHT + COLUMN_WIDTH
BURN_RIGHT = BURN_LEFT + BURN_WIDTH

PhysicalState = collections.namedtuple("PhysicalState", ["velocity", "altitude"])


def print_centered(msg: str) -> None:
    spaces = " " * ((PAGE_WIDTH - len(msg)) // 2)
    print(spaces + msg)


def print_header(title: str) -> None:
    print_centered(title)
    print_centered("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print()
    print()
    print()


def add_rjust(line, s, pos):
    # adds a new field to a line right justified to end at pos

    s = str(s)
    slen = len(s)
    if len(line) + slen > pos:
        new_len = pos - slen
        line = line[:new_len]
    if len(line) + slen < pos:
        spaces = " " * (pos - slen - len(line))
        line = line + spaces
    return line + s


def add_ljust(line, s, pos):
    # adds a new field to a line left justified starting at pos

    s = str(s)
    if len(line) > pos:
        line = line[:pos]
    if len(line) < pos:
        spaces = " " * (pos - len(line))
        line = line + spaces
    return line + s


def print_instructions() -> None:
    # Somebody had a bad experience with Xerox.

    print("THIS IS A COMPUTER SIMULATION OF AN APOLLO LUNAR")
    print("LANDING CAPSULE.")
    print()
    print()
    print("THE ON-BOARD COMPUTER HAS FAILED (IT WAS MADE BY")
    print("XEROX) SO YOU HAVE TO LAND THE CAPSULE MANUALLY.")
    print()


def print_intro() -> None:
    print("SET BURN RATE OF RETRO ROCKETS TO ANY VALUE BETWEEN")
    print("0 (FREE FALL) AND 200 (MAXIMUM BURN) POUNDS PER SECOND.")
    print("SET NEW BURN RATE EVERY 10 SECONDS.")
    print()
    print("CAPSULE WEIGHT 32,500 LBS; FUEL WEIGHT 16,500 LBS.")
    print()
    print()
    print()
    print("GOOD LUCK")
    print()


def show_landing(sim_clock, capsule):
    w = 3600 * capsule.v
    print(
        f"ON MOON AT {sim_clock.elapsed_time:.2f} SECONDS - IMPACT VELOCITY {w:.2f} MPH"
    )
    if w < 1.2:
        print("PERFECT LANDING!")
    elif w < 10:
        print("GOOD LANDING (COULD BE BETTER)")
    elif w <= 60:
        print("CRAFT DAMAGE... YOU'RE STRANDED HERE UNTIL A RESCUE")
        print("PARTY ARRIVES. HOPE YOU HAVE ENOUGH OXYGEN!")
    else:
        print("SORRY THERE WERE NO SURVIVORS. YOU BLEW IT!")
        print(f"IN FACT, YOU BLASTED A NEW LUNAR CRATER {w*.227:.2f} FEET DEEP!")
    end_sim()


def show_out_of_fuel(sim_clock, capsule):
    print(f"FUEL OUT AT {sim_clock.elapsed_time} SECONDS")
    delta_t = (
        -capsule.v + math.sqrt(capsule.v**2 + 2 * capsule.a * capsule.g)
    ) / capsule.g
    capsule.v += capsule.g * delta_t
    sim_clock.advance(delta_t)
    show_landing(sim_clock, capsule)


def format_line_for_report(t, miles, feet, velocity, fuel, burn_rate, is_header) -> str:
    line = add_rjust("", t, SECONDS_RIGHT)
    line = add_rjust(line, miles, ALT_MI_RIGHT)
    line = add_rjust(line, feet, ALT_FT_RIGHT)
    line = add_rjust(line, velocity, MPH_RIGHT)
    line = add_rjust(line, fuel, FUEL_RIGHT)
    if is_header:
        line = add_rjust(line, burn_rate, BURN_RIGHT)
    else:
        line = add_ljust(line, burn_rate, BURN_LEFT)
    return line


class Capsule:
    def __init__(
        self,
        altitude=120,
        velocity=1,
        mass_with_fuel=33000,
        mass_without_fuel=16500,
        g=1e-3,
        z=1.8,
    ):
        self.a = altitude  # in miles above the surface
        self.v = velocity  # downward
        self.m = mass_with_fuel
        self.n = mass_without_fuel
        self.g = g
        self.z = z
        self.fuel_per_second = 0

    def remaining_fuel(self):
        return self.m - self.n

    def is_out_of_fuel(self):
        return self.remaining_fuel() < 1e-3

    def update_state(self, sim_clock, delta_t, new_state):
        sim_clock.advance(delta_t)
        self.m = self.m - delta_t * self.fuel_per_second
        self.a = new_state.altitude
        self.v = new_state.velocity

    def fuel_time_remaining(self):
        # extrapolates out how many seconds we have at the current fuel burn rate
        assert self.fuel_per_second > 0
        return self.remaining_fuel() / self.fuel_per_second

    def predict_motion(self, delta_t):
        # Perform an Euler's Method numerical integration of the equations of motion.

        q = delta_t * self.fuel_per_second / self.m

        # new velocity
        new_velocity = (
            self.v
            + self.g * delta_t
            + self.z * (-q - q**2 / 2 - q**3 / 3 - q**4 / 4 - q**5 / 5)
        )

        # new altitude
        new_altitude = (
            self.a
            - self.g * delta_t**2 / 2
            - self.v * delta_t
            + self.z
            * delta_t
            * (q / 2 + q**2 / 6 + q**3 / 12 + q**4 / 20 + q**5 / 30)
        )

        return PhysicalState(altitude=new_altitude, velocity=new_velocity)

    def make_state_display_string(self, sim_clock) -> str:
        seconds = sim_clock.elapsed_time
        miles = int(self.a)
        feet = int(5280 * (self.a - miles))
        velocity = int(3600 * self.v)
        fuel = int(self.remaining_fuel())
        burn_rate = " ? "

        return format_line_for_report(
            seconds, miles, feet, velocity, fuel, burn_rate, False
        )

    def prompt_for_burn(self, sim_clock):
        msg = self.make_state_display_string(sim_clock)

        self.fuel_per_second = float(input(msg))
        sim_clock.time_until_next_prompt = 10


class SimulationClock:
    def __init__(self, elapsed_time, time_until_next_prompt):
        self.elapsed_time = elapsed_time
        self.time_until_next_prompt = time_until_next_prompt

    def time_for_prompt(self):
        return self.time_until_next_prompt < 1e-3

    def advance(self, delta_t):
        self.elapsed_time += delta_t
        self.time_until_next_prompt -= delta_t


def process_final_tick(delta_t, sim_clock, capsule):
    # When we extrapolated our position based on our velocity
    # and delta_t, we overshot the surface. For better
    # accuracy, we will back up and do shorter time advances.

    while True:
        if delta_t < 5e-3:
            show_landing(sim_clock, capsule)
            return
        # line 35
        average_vel = (
            capsule.v
            + math.sqrt(
                capsule.v**2
                + 2
                * capsule.a
                * (capsule.g - capsule.z * capsule.fuel_per_second / capsule.m)
            )
        ) / 2
        delta_t = capsule.a / average_vel
        new_state = capsule.predict_motion(delta_t)
        capsule.update_state(sim_clock, delta_t, new_state)


def handle_flyaway(sim_clock, capsule):
    """
    The user has started flying away from the moon. Since this is a
    lunar LANDING simulation, we wait until the capsule's velocity is
    positive (downward) before prompting for more input.

    Returns True if landed, False if simulation should continue.
    """

    while True:
        w = (1 - capsule.m * capsule.g / (capsule.z * capsule.fuel_per_second)) / 2
        delta_t = (
            capsule.m
            * capsule.v
            / (
                capsule.z
                * capsule.fuel_per_second
                * math.sqrt(w**2 + capsule.v / capsule.z)
            )
        ) + 0.05

        new_state = capsule.predict_motion(delta_t)

        if new_state.altitude <= 0:
            # have landed
            return True

        capsule.update_state(sim_clock, delta_t, new_state)

        if (new_state.velocity > 0) or (capsule.v <= 0):
            # return to normal sim
            return False


def end_sim():
    print()
    print()
    print()
    print("TRY AGAIN??")
    print()
    print()
    print()


def run_simulation():
    print()
    print(
        format_line_for_report("SEC", "MI", "FT", "MPH", "LB FUEL", "BURN RATE", True)
    )

    sim_clock = SimulationClock(0, 10)
    capsule = Capsule()

    capsule.prompt_for_burn(sim_clock)

    while True:
        if capsule.is_out_of_fuel():
            show_out_of_fuel(sim_clock, capsule)
            return

        if sim_clock.time_for_prompt():
            capsule.prompt_for_burn(sim_clock)
            continue

        # clock advance is the shorter of the time to the next prompt,
        # or when we run out of fuel.
        if capsule.fuel_per_second > 0:
            delta_t = min(
                sim_clock.time_until_next_prompt, capsule.fuel_time_remaining()
            )
        else:
            delta_t = sim_clock.time_until_next_prompt

        new_state = capsule.predict_motion(delta_t)

        if new_state.altitude <= 0:
            process_final_tick(delta_t, sim_clock, capsule)
            return

        if capsule.v > 0 and new_state.velocity < 0:
            # moving away from the moon

            landed = handle_flyaway(sim_clock, capsule)
            if landed:
                process_final_tick(delta_t, sim_clock, capsule)
                return

        else:
            capsule.update_state(sim_clock, delta_t, new_state)


def main() -> None:
    print_header("LUNAR")
    print_instructions()
    while True:
        print_intro()
        run_simulation()


if __name__ == "__main__":
    main()
