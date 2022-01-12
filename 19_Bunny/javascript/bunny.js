// BUNNY
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

var bunny_string = ["B","U","N","N","Y"];

var bunny_data = [1,2,-1,0,2,45,50,-1,0,5,43,52,-1,0,7,41,52,-1,
	1,9,37,50,-1,2,11,36,50,-1,3,13,34,49,-1,4,14,32,48,-1,
	5,15,31,47,-1,6,16,30,45,-1,7,17,29,44,-1,8,19,28,43,-1,
	9,20,27,41,-1,10,21,26,40,-1,11,22,25,38,-1,12,22,24,36,-1,
	13,34,-1,14,33,-1,15,31,-1,17,29,-1,18,27,-1,
	19,26,-1,16,28,-1,13,30,-1,11,31,-1,10,32,-1,
	8,33,-1,7,34,-1,6,13,16,34,-1,5,12,16,35,-1,
	4,12,16,35,-1,3,12,15,35,-1,2,35,-1,1,35,-1,
	2,34,-1,3,34,-1,4,33,-1,6,33,-1,10,32,34,34,-1,
	14,17,19,25,28,31,35,35,-1,15,19,23,30,36,36,-1,
	14,18,21,21,24,30,37,37,-1,13,18,23,29,33,38,-1,
	12,29,31,33,-1,11,13,17,17,19,19,22,22,24,31,-1,
	10,11,17,18,22,22,24,24,29,29,-1,
	22,23,26,29,-1,27,29,-1,28,29,-1,4096];

print(tab(32) + "BUNNY\n");
print(tab(15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
print("\n");
print("\n");
print("\n");

var l = 64;	// ASCII letter code
var pos = 0;

print("\n");

var str = "";
for (var pos = 0; bunny_data[pos] < 128; pos++) {
	if (bunny_data[pos] < 0) {
		print(str + "\n");
		str = "";
		continue;
	}
	while (str.length < bunny_data[pos])
		str += " ";
	for (var i = bunny_data[pos]; i <= bunny_data[pos + 1]; i++) 
		str += bunny_string[i % 5];
	pos++;
}
