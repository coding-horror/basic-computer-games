// 3D PLOT
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

function equation(input)
{
	return 30 * Math.exp(-input * input / 100);
}

print(tab(32) + "3D PLOT\n");
print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");

for (x = -30; x <= 30; x += 1.5) {
	l = 0;
	y1 = 5 * Math.floor(Math.sqrt(900 - x * x) / 5);
	str = "";
	for (y = y1; y >= -y1; y -= 5) {
		z = Math.floor(25 + equation(Math.sqrt(x * x + y * y)) - .7 * y);
		if (z > l) {
			l = z;
			while (str.length < z)
				str += " ";
			str += "*";
		}
	}
	print(str + "\n");
}
