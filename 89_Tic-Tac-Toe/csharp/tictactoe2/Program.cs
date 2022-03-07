// See https://aka.ms/new-console-template for more information
char[] board = new char[10];
char human;
char computer;
int move = 0;
char result;
for(;;){
    // Print text on the screen with 30 spaces before text
    Console.WriteLine("TIC TAC TOE".PadLeft(30));
    // Print text on screen with 15 spaces before text
    Console.WriteLine("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY".PadLeft(15));
    // THIS PROGRAM PLAYS TIC TAC TOE
    Console.WriteLine("THE BOARD IS NUMBERED:");
    Console.WriteLine("1  2  3");
    Console.WriteLine("4  5  6");
    Console.WriteLine("7  8  9");
    Console.Write("\n\nDO YOU WANT 'X' OR 'O'");
    var key = Console.ReadKey();
	Console.WriteLine();
	// cleanup the board
	for(int i=0; i < 10; i++) {
		board[i]=' ';
	}
	// X GOES FIRST
    if (key.Key == ConsoleKey.X) {
		human = 'X';
		computer = 'O';
		move = readYourMove();
		board[move] = human;
		printBoard();
    } else {
		human = 'O';
		computer = 'X';
    }
	for(;;){
		Console.WriteLine("THE COMPUTER MOVES TO...");
		move = computerMove(move);
		board[move] = computer;
		result = printBoard();
		printResult(result);
		move = readYourMove();
		board[move] = human;
		result = printBoard();
		printResult(result);
	}
}

void printResult(int result) {
	if (result == '\0') {
		Console.WriteLine("IT'S A DRAW. THANK YOU.");
		Environment.Exit(0);
	} else if (result == computer) {
		Console.WriteLine("I WIN, TURKEY!!!");
		Environment.Exit(0);
	} else if (result == human) {
		Console.WriteLine("YOU BEAT ME!! GOOD GAME.");
		Environment.Exit(0);
	}
}
char printBoard() {
	for (int i=1; i < 10; i++){
		Console.Write($" {board[i]} ");
		if (i % 3 == 0) {
			if (i < 9) {
				Console.Write("\n---+---+---\n");
			} else {
				Console.Write("\n");
			}
		} else {
			Console.Write("!");
		}
	}
	// horizontal check
	for (int i = 1; i <= 9; i += 3) {
		if (board[i] != ' ' && (board[i] == board[i+1]) && (board[i+1] == board[i+2])) {
			return board[i];
		}
	}
	// vertical check
	for (int i = 1; i <= 3; i++) {
		if (board[i] != ' ' && (board[i] == board[i+3]) && (board[i] == board[i+6])) {
			return board[i];
		}
	}
	// cross
	if (board[5] != ' ') {
		if ((board[1] == board[5] && board[9] == board[5]) || (board[3] == board[5] && board[7] == board[5])) {
			return board[5];
		}
	}
	// draw check
	for (int i = 1; i <= 9; i++) {
		if (board[i] == ' ') {
			return ' ';
		}
	}
	return '\0';
}

int readYourMove()  {
	int number = 0;
    for(;;) {
		Console.Write("\n\nWHERE DO YOU MOVE? ");
		var key = Console.ReadKey();
        Console.WriteLine();
		if (key.Key == ConsoleKey.D0) {
			Console.WriteLine("THANKS FOR THE GAME.");
            Environment.Exit(0);
        }
        if (key.Key >= ConsoleKey.D1 && key.Key <= ConsoleKey.D9) {
            number = key.Key - ConsoleKey.D0;
            if (number > 9 || board[number] != ' ') {
                Console.WriteLine("THAT SQUARE IS OCCUPIED.\n");
                continue;
            }
        }
		return number;
	}
}

int getIndex(int number) {
	return ((number - 1) % 8) + 1; //number - 8 * (int)((number - 1) / 8);
}
int computerMove(int lastMove) {
	int[] boardMap = new int[] {0, 1, 2, 3, 6, 9, 8, 7, 4, 5};
	int index = Array.IndexOf(boardMap, lastMove);
	if (lastMove == 0 || board[5] == ' '){
		return 5;
	}
	if (lastMove == 5) {
		return 1;
	}
	if (board[5] == human) {
		// check possible win
		if (board[1] == computer && board[2] == ' ' && board[3] == computer) {
			return 2;
		}
		if (board[7] == computer && board[8] == ' ' && board[9] == computer) {
			return 8;
		}
		if (board[1] == computer && board[4] == ' ' && board[7] == computer) {
			return 4;
		}
		if (board[3] == computer && board[6] == ' ' && board[7] == computer) {
			return 6;
		}
		// check cross
		int crossIndex = boardMap[getIndex(index + 4)];
		if (board[crossIndex] == ' ') {
			return crossIndex;
		}
		int stepForward2 = boardMap[getIndex(index + 2)];
		if (board[stepForward2] == ' ') {
			return stepForward2;
		}
		int stepBackward2 = boardMap[getIndex(index + 6)];
		if (board[stepBackward2] == ' ') {
			return stepBackward2;
		}
		int stepForward1 = boardMap[getIndex(index + 1)];
		if (board[stepForward1] == ' ') {
			return stepForward1;
		}
		int stepBackward1 = boardMap[getIndex(index + 7)];
		if (board[stepBackward1] == ' ') {
			return stepBackward1;
		}
		int stepForward3 = boardMap[getIndex(index + 3)];
		if (board[stepForward3] == ' ') {
			return stepForward3;
		}
		int stepBackward3 = boardMap[getIndex(index + 5)];
		if (board[stepBackward3] == ' ') {
			return stepBackward3;
		}
	} else {
		// check possible win
		if (board[1] == computer && board[9] == ' ') {
			return 9;
		}
		if (board[9] == computer && board[1] == ' ') {
			return 1;
		}
		if (board[3] == computer && board[7] == ' ') {
			return 7;
		}
		if (board[7] == computer && board[3] == ' ') {
			return 3;
		}
		// if corner
		if (index % 2 == 1) {
			int stepForward2 = boardMap[getIndex(index + 2)];
			if (board[stepForward2] == ' ') {
				return stepForward2;
			}
			int stepBackward2 = boardMap[getIndex(index + 6)];
			if (board[stepBackward2] == ' ') {
				return stepBackward2;
			}
		} else {
			int stepForward1 = boardMap[getIndex(index + 1)];
			if (board[stepForward1] == ' ') {
				return stepForward1;
			}
			int stepBackward1 = boardMap[getIndex(index + 7)];
			if (board[stepBackward1] == ' ') {
				return stepBackward1;
			}
			int stepForward3 = boardMap[getIndex(index + 3)];
			if (board[stepForward3] == ' ') {
				return stepForward3;
			}
			int stepBackward3 = boardMap[getIndex(index + 5)];
			if (board[stepBackward3] == ' ') {
				return stepBackward3;
			}
					int crossIndex = boardMap[getIndex(index + 4)];
			if (board[crossIndex] == ' ') {
				return crossIndex;
			}
			int stepForward2 = boardMap[getIndex(index + 2)];
			if (board[stepForward2] == ' ') {
				return stepForward2;
			}
			int stepBackward2 = boardMap[getIndex(index + 6)];
			if (board[stepBackward2] == ' ') {
				return stepBackward2;
			}
		}
	}
	return 0;
}
