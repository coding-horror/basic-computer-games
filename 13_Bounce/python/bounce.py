"""
BOUNCE

A physics simulation

Ported by Dave LeCompte
"""

from typing import Tuple, List

PAGE_WIDTH = 64


def print_centered(msg: str) -> None:
    spaces = " " * ((PAGE_WIDTH - len(msg)) // 2)
    print(spaces + msg)


def print_header(title: str) -> None:
    print_centered(title)
    print_centered("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY")
    print()
    print()
    print()


def print_instructions() -> None:
    print("THIS SIMULATION LETS YOU SPECIFY THE INITIAL VELOCITY")
    print("OF A BALL THROWN STRAIGHT UP, AND THE COEFFICIENT OF")
    print("ELASTICITY OF THE BALL.  PLEASE USE A DECIMAL FRACTION")
    print("COEFFICIENCY (LESS THAN 1).")
    print()
    print("YOU ALSO SPECIFY THE TIME INCREMENT TO BE USED IN")
    print("'STROBING' THE BALL'S FLIGHT (TRY .1 INITIALLY).")
    print()


def get_initial_conditions() -> Tuple[float, float, float]:
    delta_t = float(input("TIME INCREMENT (SEC)? "))
    print()
    v0 = float(input("VELOCITY (FPS)? "))
    print()
    coeff_rest = float(input("COEFFICIENT? "))
    print()

    return delta_t, v0, coeff_rest


def print_at_tab(line: str, tab: int, s: str) -> str:
    line += (" " * (tab - len(line))) + s
    return line


def run_simulation(delta_t: float, v0: float, coeff_rest: float) -> None:
    bounce_time: List[float] = [0] * 20  # time of each bounce

    print("FEET")
    print()

    sim_dur = int(70 / (v0 / (16 * delta_t)))
    for i in range(1, sim_dur + 1):
        bounce_time[i] = v0 * coeff_rest ** (i - 1) / 16

    # Draw the trajectory of the bouncing ball, one slice of height at a time
    h: float = int(-16 * (v0 / 32) ** 2 + v0**2 / 32 + 0.5)
    while h >= 0:
        line = ""
        if int(h) == h:
            line += str(int(h))
        total_time: float = 0
        for i in range(1, sim_dur + 1):
            tm: float = 0
            while tm <= bounce_time[i]:
                total_time += delta_t
                if (
                    abs(h - (0.5 * (-32) * tm**2 + v0 * coeff_rest ** (i - 1) * tm))
                    <= 0.25
                ):
                    line = print_at_tab(line, int(total_time / delta_t), "0")
                tm += delta_t
            tm = bounce_time[i + 1] / 2

            if -16 * tm**2 + v0 * coeff_rest ** (i - 1) * tm < h:
                break
        print(line)
        h = h - 0.5

    print("." * (int((total_time + 1) / delta_t) + 1))
    print
    line = " 0"
    for i in range(1, int(total_time + 0.9995) + 1):
        line = print_at_tab(line, int(i / delta_t), str(i))
    print(line)
    print()
    print(print_at_tab("", int((total_time + 1) / (2 * delta_t) - 2), "SECONDS"))
    print()


def main() -> None:
    print_header("BOUNCE")
    print_instructions()

    while True:
        delta_t, v0, coeff_rest = get_initial_conditions()

        run_simulation(delta_t, v0, coeff_rest)
        break


if __name__ == "__main__":
    main()
