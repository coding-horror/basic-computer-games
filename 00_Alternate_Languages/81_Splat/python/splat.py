"""
SPLAT

Splat similates a parachute jump in which you try to open your parachute
at the last possible moment without going splat! You may select your own
terminal velocity or let the computer do it for you. You may also select
the acceleration due to gravity or, again, let the computer do it
in which case you might wind up on any one of the eight planets (out to
Neptune), the moon, or the sun.

The computer then tells you the height you're jumping from and asks for
the seconds of free fall. It then divides your free fall time into eight
intervals and gives you progress reports on the way down. The computer
also keeps track of all prior jumps and lets you know how you compared
with previous successful jumps. If you want to recall information from
previous runs, then you should store the array `successful_jumps` on
disk and read it before each run.

John Yegge created this program while at the Oak Ridge Associated
Universities.

Ported in 2021 by Jonas Nockert / @lemonad

"""
from math import sqrt
from random import choice, random, uniform

PAGE_WIDTH = 72


def numeric_input(question, default=0):
    """Ask user for a numeric value."""
    while True:
        answer = input(f"{question} [{default}]: ").strip() or default
        try:
            return float(answer)
        except ValueError:
            pass


def yes_no_input(question, default="YES"):
    """Ask user a yes/no question and returns True if yes, otherwise False."""
    answer = input(f"{question} (YES OR NO) [{default}]: ").strip() or default
    while answer.lower() not in ["n", "no", "y", "yes"]:
        answer = input(f"YES OR NO [{default}]: ").strip() or default
    return answer.lower() in ["y", "yes"]


def get_terminal_velocity():
    """Terminal velocity by user or picked by computer."""
    if yes_no_input("SELECT YOUR OWN TERMINAL VELOCITY", default="NO"):
        v1 = numeric_input("WHAT TERMINAL VELOCITY (MI/HR)", default=100)
    else:
        # Computer picks 0-1000 terminal velocity.
        v1 = int(1000 * random())
        print(f"OK.  TERMINAL VELOCITY = {v1} MI/HR")

    # Convert miles/h to feet/s.
    return v1 * (5280 / 3600)


def get_acceleration():
    """Acceleration due to gravity by user or picked by computer."""
    if yes_no_input("WANT TO SELECT ACCELERATION DUE TO GRAVITY", default="NO"):
        a2 = numeric_input("WHAT ACCELERATION (FT/SEC/SEC)", default=32.16)
    else:
        body, a2 = pick_random_celestial_body()
        print(f"FINE. YOU'RE ON {body}. ACCELERATION={a2} FT/SEC/SEC.")
    return a2


def get_freefall_time():
    """User-guessed freefall time.

    The idea of the game is to pick a freefall time, given initial
    altitude, terminal velocity and acceleration, so the parachute
    as close to the ground as possible without going splat.
    """
    t_freefall = 0
    # A zero or negative freefall time is not handled by the motion
    # equations during the jump.
    while t_freefall <= 0:
        t_freefall = numeric_input("HOW MANY SECONDS", default=10)
    return t_freefall


def jump():
    """Simulate a jump and returns the altitude where the chute opened.

    The idea is to open the chute as late as possible -- but not too late.
    """
    v = 0  # Terminal velocity.
    a = 0  # Acceleration.
    initial_altitude = int(9001 * random() + 1000)

    v1 = get_terminal_velocity()
    # Actual terminal velocity is +/-5% of v1.
    v = v1 * uniform(0.95, 1.05)

    a2 = get_acceleration()
    # Actual acceleration is +/-5% of a2.
    a = a2 * uniform(0.95, 1.05)

    print(
        "\n"
        f"    ALTITUDE         = {initial_altitude} FT\n"
        f"    TERM. VELOCITY   = {v1:.2f} FT/SEC +/-5%\n"
        f"    ACCELERATION     = {a2:.2f} FT/SEC/SEC +/-5%\n"
        "SET THE TIMER FOR YOUR FREEFALL."
    )
    t_freefall = get_freefall_time()
    print(
        "HERE WE GO.\n\n"
        "TIME (SEC)\tDIST TO FALL (FT)\n"
        "==========\t================="
    )

    terminal_velocity_reached = False
    is_splat = False
    for i in range(9):
        # Divide time for freefall into 8 intervals.
        t = i * (t_freefall / 8)
        # From the first equation of motion, v = v_0 + a * delta_t, with
        # initial velocity v_0 = 0, we can get the time when terminal velocity
        # is reached: delta_t = v / a.
        if t > v / a:
            if not terminal_velocity_reached:
                print(f"TERMINAL VELOCITY REACHED AT T PLUS {v / a:.2f} SECONDS.")
                terminal_velocity_reached = True
            # After having reached terminal velocity, the displacement is
            # composed of two parts:
            # 1. Displacement up to reaching terminal velocity:
            #    From the third equation of motion, v^2 = v_0^2 + 2 * a * d,
            #    with v_0 = 0, we can get the displacement using
            #    d1 = v^2 / (2 * a).
            # 2. Displacement beyond having reached terminal velocity:
            #    here, the displacement is just a function of the terminal
            #    velocity and the time passed after having reached terminal
            #    velocity: d2 = v * (t - t_reached_term_vel)
            d1 = (v**2) / (2 * a)
            d2 = v * (t - (v / a))
            altitude = initial_altitude - (d1 + d2)
            if altitude <= 0:
                # Time taken for an object to fall to the ground given
                # an initial altitude is composed of two parts after having
                # reached terminal velocity:
                # 1. time up to reaching terminal velocity: t1 = v / a
                # 2. time beyond having reached terminal velocity:
                #    here, the altitude that remains after having reached
                #    terminal velocity can just be divided by the constant
                #    terminal velocity to get the time it takes to reach the
                #    ground: t2 = altitude_remaining / v
                t1 = v / a
                t2 = (initial_altitude - d1) / v
                print_splat(t1 + t2)
                is_splat = True
                break
        else:
            # 1. Displacement before reaching terminal velocity:
            #    From the second equation of motion,
            #    d = v_0 * t + 0.5 * a * t^2, with v_0 = 0, we can get
            #    the displacement using d1 = a / 2 * t^2
            d1 = (a / 2) * (t**2)
            altitude = initial_altitude - d1
            if altitude <= 0:
                # Time taken for an object to fall to the ground given that
                # it never reaches terminal velocity can be calculated by
                # using the second equation of motion:
                # d = v_0 * t + 0.5 * a * t^2, with v_0 = 0, which
                # when solved for t becomes
                # t1 = sqrt(2 * d / a).
                t1 = sqrt(2 * initial_altitude / a)
                print_splat(t1)
                is_splat = True
                break
        print(f"{t:.2f}\t\t{altitude:.1f}")

    if not is_splat:
        print("CHUTE OPEN")
    return altitude


def pick_random_celestial_body():
    """Pick a random planet, the moon, or the sun with associated gravity."""
    body, gravity = choice(
        [
            ("MERCURY", 12.2),
            ("VENUS", 28.3),
            ("EARTH", 32.16),
            ("THE MOON", 5.15),
            ("MARS", 12.5),
            ("JUPITER", 85.2),
            ("SATURN", 37.6),
            ("URANUS", 33.8),
            ("NEPTUNE", 39.6),
            ("THE SUN", 896.0),
        ]
    )
    return body, gravity


def jump_stats(previous_jumps, chute_altitude):
    """Compare altitude when chute opened with previous successful jumps.

    Return the number of previous jumps and the number of times
    the current jump is better.
    """
    n_previous_jumps = len(previous_jumps)
    n_better = sum(1 for pj in previous_jumps if chute_altitude < pj)
    return n_previous_jumps, n_better


def print_splat(time_on_impact):
    """Parachute opened too late!"""
    print(f"{time_on_impact:.2f}\t\tSPLAT")
    print(
        choice(
            [
                "REQUIESCAT IN PACE.",
                "MAY THE ANGEL OF HEAVEN LEAD YOU INTO PARADISE.",
                "REST IN PEACE.",
                "SON-OF-A-GUN.",
                "#$%&&%!$",
                "A KICK IN THE PANTS IS A BOOST IF YOU'RE HEADED RIGHT.",
                "HMMM. SHOULD HAVE PICKED A SHORTER TIME.",
                "MUTTER. MUTTER. MUTTER.",
                "PUSHING UP DAISIES.",
                "EASY COME, EASY GO.",
            ]
        )
    )


def print_results(n_previous_jumps, n_better):
    """Compare current jump to previous successful jumps."""
    k = n_previous_jumps
    k1 = n_better
    n_jumps = k + 1
    if n_jumps <= 3:
        order = ["1ST", "2ND", "3RD"]
        nth = order[n_jumps - 1]
        print(f"AMAZING!!! NOT BAD FOR YOUR {nth} SUCCESSFUL JUMP!!!")
    elif k - k1 <= 0.1 * k:
        print(
            f"WOW!  THAT'S SOME JUMPING.  OF THE {k} SUCCESSFUL JUMPS\n"
            f"BEFORE YOURS, ONLY {k - k1} OPENED THEIR CHUTES LOWER THAN\n"
            "YOU DID."
        )
    elif k - k1 <= 0.25 * k:
        print(
            f"PRETTY GOOD!  {k} SUCCESSFUL JUMPS PRECEDED YOURS AND ONLY\n"
            f"{k - k1} OF THEM GOT LOWER THAN YOU DID BEFORE THEIR CHUTES\n"
            "OPENED."
        )
    elif k - k1 <= 0.5 * k:
        print(
            f"NOT BAD.  THERE HAVE BEEN {k} SUCCESSFUL JUMPS BEFORE YOURS.\n"
            f"YOU WERE BEATEN OUT BY {k - k1} OF THEM."
        )
    elif k - k1 <= 0.75 * k:
        print(
            f"CONSERVATIVE, AREN'T YOU?  YOU RANKED ONLY {k - k1} IN THE\n"
            f"{k} SUCCESSFUL JUMPS BEFORE YOURS."
        )
    elif k - k1 <= 0.9 * k:
        print(
            "HUMPH!  DON'T YOU HAVE ANY SPORTING BLOOD?  THERE WERE\n"
            f"{k} SUCCESSFUL JUMPS BEFORE YOURS AND YOU CAME IN {k1} JUMPS\n"
            "BETTER THAN THE WORST.  SHAPE UP!!!"
        )
    else:
        print(
            f"HEY!  YOU PULLED THE RIP CORD MUCH TOO SOON.  {k} SUCCESSFUL\n"
            f"JUMPS BEFORE YOURS AND YOU CAME IN NUMBER {k - k1}!"
            "  GET WITH IT!"
        )


def print_centered(msg):
    """Print centered text."""
    spaces = " " * ((PAGE_WIDTH - len(msg)) // 2)
    print(spaces + msg)


def print_header():
    print_centered("SPLAT")
    print_centered("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print(
        "\n\n\n"
        "WELCOME TO 'SPLAT' -- THE GAME THAT SIMULATES A PARACHUTE\n"
        "JUMP.  TRY TO OPEN YOUR CHUTE AT THE LAST POSSIBLE\n"
        "MOMENT WITHOUT GOING SPLAT.\n\n"
    )


#
# Main program.
#

print_header()

successful_jumps = []
while True:
    chute_altitude = jump()
    if chute_altitude > 0:
        # We want the statistics on previous jumps (i.e. not including the
        # current jump.)
        n_previous_jumps, n_better = jump_stats(successful_jumps, chute_altitude)
        successful_jumps.append(chute_altitude)
        print_results(n_previous_jumps, n_better)
    else:
        # Splat!
        print("I'LL GIVE YOU ANOTHER CHANCE.")
    z = yes_no_input("DO YOU WANT TO PLAY AGAIN")
    if not z:
        z = yes_no_input("PLEASE")
        if not z:
            print("SSSSSSSSSS.")
            break
