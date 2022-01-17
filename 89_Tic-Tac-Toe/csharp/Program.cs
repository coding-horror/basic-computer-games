// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
// Print text on the screen with 30 spaces before text
Console.WriteLine("TIC TAC TOE".PadLeft(30));
// Print text on screen with 15 spaces before text
Console.WriteLine("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY".PadLeft(15));
// Print three blank lines on screen
Console.WriteLine("\n\n\n");
// THIS PROGRAM PLAYS TIC TAC TOE
// THE MACHINE GOES FIRST
Console.WriteLine("THE GAME BOARD IS NUMBERED:\n");
Console.WriteLine("1  2  3");
Console.WriteLine("8  9  4");
Console.WriteLine("7  6  5");

// Main program
while(true) {
	int a, b, c, d, e;
	int p, q, r, s;
	a = 9;
	Console.WriteLine("\n\n");
	computerMoves(a);
	p = readYourMove();
	b = move(p + 1);
	computerMoves(b);
	q = readYourMove();
	if (q == move(b + 4)) {
		c = move(b + 2);
		computerMoves(c);
		r = readYourMove();
		if (r == move(c + 4)) {
			if (p % 2 != 0) {
				d = move(c + 3);
				computerMoves(d);
				s = readYourMove();
				if (s != move(d + 4)) {
					e = move(d + 4);
					computerMoves(e);
				}
				e = move(d + 6);
				computerMoves(e);
				Console.WriteLine("THE GAME IS A DRAW.");
			} else {
				d = move(c + 7);
				computerMoves(d);
				Console.WriteLine("AND WINS ********");
			}
		} else {
			d = move(c + 4);
			computerMoves(d);
			Console.WriteLine("AND WINS ********");
		}
	} else {
		c = move(b + 4);
		computerMoves(c);
		Console.WriteLine("AND WINS ********");
	}
}

void computerMoves(int move) {
		Console.WriteLine("COMPUTER MOVES " + move);
}
int readYourMove() {
	while(true) {
		Console.Write("YOUR MOVE?");
		string input = Console.ReadLine();
		if (int.TryParse(input, out int number)) {
			return number;
		}
	}
}

int move(int number) {
	return number - 8 * (int)((number - 1) / 8);
}
