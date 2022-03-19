#!/usr/bin/env python3

# 3D PLOT
#
# Converted from BASIC to Python by Trevor Hobson

from math import exp, floor, sqrt


def equation(x: float) -> float:
    return 30 * exp(-x * x / 100)


def main():
    print(" " * 32 + "3D PLOT")
    print(" " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n\n")

    for x in range(-300, 315, 15):
        x1 = x / 10
        max_column = 0
        y1 = 5 * floor(sqrt(900 - x1 * x1) / 5)
        y_plot = [" "] * 80

        for y in range(y1, -(y1 + 5), -5):
            column = floor(25 + equation(sqrt(x1 * x1 + y * y)) - 0.7 * y)
            if column > max_column:
                max_column = column
                y_plot[column] = "*"
        print("".join(y_plot))


if __name__ == "__main__":
    main()
