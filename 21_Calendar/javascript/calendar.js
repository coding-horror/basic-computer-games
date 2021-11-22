// CALENDAR
//
// Converted from BASIC to Javascript by Oscar Toledo G. (nanochess)
//
function print(str)
{
	document.getElementById("output").appendChild(document.createTextNode(str));
}

function tab(space)
{
	var str = "";
	while (space-- > 0)
		str += " ";
	return str;
}

print(tab(32) + "CALENDAR\n");
print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
print("\n");
print("\n");
print("\n");

//       0, 31, 29  ON LEAP YEARS
var m = [0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

// VALUES FOR 1979 - SEE NOTES
for (i = 1; i <= 6; i++)
	print("\n");

d = -1;	// 1979 starts on Monday (0 = Sun, -1 = Monday, -2 = Tuesday)
s = 0;

for (n = 1; n <= 12; n++) {
	print("\n");
	print("\n");
	s = s + m[n - 1];
	str = "**" + s;
	while (str.length < 7)
		str += " ";
	for (i = 1; i <= 18; i++)
		str += "*";
	switch (n) {
		case  1:	str += " JANUARY "; break;
		case  2:	str += " FEBRUARY"; break;
		case  3:	str += "  MARCH  "; break;
		case  4:	str += "  APRIL  "; break;
		case  5:	str += "   MAY   "; break;
		case  6:	str += "   JUNE  "; break;
		case  7:	str += "   JULY  "; break;
		case  8:	str += "  AUGUST "; break;
		case  9:	str += "SEPTEMBER"; break;
		case 10:	str += " OCTOBER "; break;
		case 11:	str += " NOVEMBER"; break;
		case 12:	str += " DECEMBER"; break;
	}
	for (i = 1; i <= 18; i++)
		str += "*";
	str += (365 - s) + "**";
	     // 366 - s on leap years
	print(str + "\n");
	print("     S       M       T       W       T       F       S\n");
	print("\n");
	str = "";
	for (i = 1; i <= 59; i++)
		str += "*";    
	for (week = 1; week <= 6; week++) {
		print(str + "\n");
		str = "    ";
		for (g = 1; g <= 7; g++) {
			d++;
			d2 = d - s;
			if (d2 > m[n]) {
				week = 6;
				break;
			}
			if (d2 > 0)
				str += d2;
			while (str.length < 4 + 8 * g)
				str += " ";
		}
		if (d2 == m[n]) {
			d += g;
			break;
		}
	} 
	d -= g;
	print(str + "\n");
}
