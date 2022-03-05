"""
BOUNCE

A physics simulation

Ported by Dave LeCompte
"""


PAGE_WIDTH = 64


def print_centered(msg):
    spaces = " " * ((PAGE_WIDTH - len(msg)) // 2)
    print(spaces + msg)


def print_header(title):
    print_centered(title)
    print_centered("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print()
    print()
    print()


def print_instructions():
    print("THIS SIMULATION LETS YOU SPECIFY THE INITIAL VELOCITY")
    print("OF A BALL THROWN STRAIGHT UP, AND THE COEFFICIENT OF")
    print("ELASTICITY OF THE BALL.  PLEASE USE A DECIMAL FRACTION")
    print("COEFFICIENCY (LESS THAN 1).")
    print()
    print("YOU ALSO SPECIFY THE TIME INCREMENT TO BE USED IN")
    print("'STROBING' THE BALL'S FLIGHT (TRY .1 INITIALLY).")
    print()


def get_initial_conditions():
    delta_t = float(input("TIME INCREMENT (SEC)? "))
    print()
    v0 = float(input("VELOCITY (FPS)? "))
    print()
    coeff_rest = float(input("COEFFICIENT? "))
    print()

    return delta_t, v0, coeff_rest


def print_at_tab(line, tab, s):
    line += (" " * (tab - len(line))) + s
    return line


def run_simulation(delta_t, v0, coeff_rest):
    t = [0] * 20  # time of each bounce

    print("FEET")
    print()

    sim_dur = int(70 / (v0 / (16 * delta_t)))
    for i in range(1, sim_dur + 1):
        t[i] = v0 * coeff_rest ** (i - 1) / 16

    # Draw the trajectory of the bouncing ball, one slice of height at a time
    h = int(-16 * (v0 / 32) ** 2 + v0**2 / 32 + 0.5)
    while h >= 0:
        line = ""
        if int(h) == h:
            line += str(int(h))
        l = 0
        for i in range(1, sim_dur + 1):
            tm = 0
            while tm <= t[i]:
                l += delta_t
                if (
                    abs(h - (0.5 * (-32) * tm**2 + v0 * coeff_rest ** (i - 1) * tm))
                    <= 0.25
                ):
                    line = print_at_tab(line, int(l / delta_t), "0")
                tm += delta_t
            tm = t[i + 1] / 2

            if -16 * tm**2 + v0 * coeff_rest ** (i - 1) * tm < h:
                break
        print(line)
        h = h - 0.5

    print("." * (int((l + 1) / delta_t) + 1))
    print
    line = " 0"
    for i in range(1, int(l + 0.9995) + 1):
        line = print_at_tab(line, int(i / delta_t), str(i))
    print(line)
    print()
    print(print_at_tab("", int((l + 1) / (2 * delta_t) - 2), "SECONDS"))
    print()


def main():
    print_header("BOUNCE")
    print_instructions()

    while True:
        delta_t, v0, coeff_rest = get_initial_conditions()

        run_simulation(delta_t, v0, coeff_rest)
        break


if __name__ == "__main__":
    main()
