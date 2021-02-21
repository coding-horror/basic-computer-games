#!/usr/bin/env python3

# 3D PLOT
#
# Converted from BASIC to Python by Trevor Hobson

import math

def equation(input):
	return 30 * math.exp(-input * input / 100)

print(" " * 32 + "3D PLOT")
print(" " * 15 + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n\n")

for x in range (-300, 315, 15):
	x1 = x / 10
	l = 0
	y1 = 5 * math.floor(math.sqrt(900 - x1 * x1) / 5)
	yPlot = [" "] * 80

	for y in range (y1, -(y1 + 5), -5):
		z = math.floor(25 + equation(math.sqrt(x1 * x1 + y * y)) - .7 * y)
		if z > l:
			l = z
			yPlot[z] = "*"
	print("".join(yPlot))