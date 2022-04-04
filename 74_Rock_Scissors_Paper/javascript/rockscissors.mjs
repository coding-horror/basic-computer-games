#!/usr/bin/env node
// ROCK, SCISSORS, PAPER
//
// Converted from BASIC to Javascript by Alexander Wunschik (mojoaxel)

import { println, tab, input } from '../../00_Common/javascript/common.mjs';

let userWins = 0;
let computerWins = 0;
let ties = 0;

// 30 INPUT "HOW MANY GAMES";Q
// 40 IF Q<11 THEN 60
// 50 PRINT "SORRY, BUT WE AREN'T ALLOWED TO PLAY THAT MANY.": GOTO 30
// 60 FOR G=1 TO Q
async function getGameCount() {
	let gameCount = await input("HOW MANY GAMES");
	if (gameCount > 10) {
		println("SORRY, BUT WE AREN'T ALLOWED TO PLAY THAT MANY.");
		return await getGameCount();
	}
	return gameCount;
}

// #90 PRINT "3=ROCK...2=SCISSORS...1=PAPER"
// #100 INPUT "1...2...3...WHAT'S YOUR CHOICE";K
// #110 IF (K-1)*(K-2)*(K-3)<>0 THEN PRINT "INVALID.": GOTO 90
async function getUserInput() {
	println("3=ROCK...2=SCISSORS...1=PAPER");
	const userChoice = await input("1...2...3...WHAT'S YOUR CHOICE");
	if (userChoice < 1 || userChoice > 3) {
		println("INVALID.");
		return await getUserInput();
	}
	return userChoice;
}

async function game() {
	// 10 PRINT TAB(21);"GAME OF ROCK, SCISSORS, PAPER"
	// 20 PRINT TAB(15);"CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY"
	// 25 PRINT:PRINT:PRINT
	println(tab(21), 'GAME OF ROCK, SCISSORS, PAPER');
	println(tab(15), 'CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY');
	println('\n\n');

	let gameCount = await getGameCount();

	async function playGame(gameNumber) {
		// 70 PRINT: PRINT "GAME NUMBER";G
		println("\nGAME NUMBER ", gameNumber);

		const ROCK = 3;
		const SCISSORS = 2;
		const PAPER = 1;

		const usersChoice = await getUserInput();

		// 80 X=INT(RND(1)*3+1)
		const computersChoice = Math.floor(Math.random()*3) + 1;

		// 120 PRINT "THIS IS MY CHOICE..."
		// 130 ON X GOTO 140,150,160
		// 140 PRINT "...PAPER": GOTO 170
		// 150 PRINT "...SCISSORS": GOTO 170
		// 160 PRINT "...ROCK"
		println("THIS IS MY CHOICE...", 
			computersChoice === PAPER ? "...PAPER" : 
				computersChoice === SCISSORS ? "...SCISSORS" : 
					"...ROCK");


		// 170 IF X=K THEN 250
		// 180 IF X>K THEN 230
		// 190 IF X=1 THEN 210
		// 200 PRINT "YOU WIN!!!":H=H+1: GOTO 260
		// 210 IF K<>3 THEN 200
		// 220 PRINT "WOW!  I WIN!!!":C=C+1:GOTO 260
		// 230 IF K<>1 OR X<>3 THEN 220
		// 240 GOTO 200
		// 250 PRINT "TIE GAME.  NO WINNER."
		if (computersChoice == usersChoice) {
			println("TIE GAME.  NO WINNER.");
			ties++;
		} else if (
			(computersChoice == ROCK && usersChoice == SCISSORS) ||
			(computersChoice == PAPER && usersChoice == ROCK) ||
			(computersChoice == SCISSORS && usersChoice == PAPER)
		) {
			println("WOW!  I WIN!!!");
			computerWins++;
		} else {
			println("YOU WIN!!!");
			userWins++;
		}
	}

	for (let gameNumber = 1; gameNumber <= gameCount; gameNumber++) {
		await playGame(gameNumber);
		// 260 NEXT G
	}

	// 270 PRINT: PRINT "HERE IS THE FINAL GAME SCORE:"
	// 280 PRINT "I HAVE WON";C;"GAME(S)."
	// 290 PRINT "YOU HAVE WON";H;"GAME(S)."
	// 300 PRINT "AND";Q-(C+H20);"GAME(S) ENDED IN A TIE."
	println("\nHERE IS THE FINAL GAME SCORE:");
	println(`I HAVE WON ${computerWins} GAME(S).`);
	println(`YOU HAVE WON ${userWins} GAME(S).`);
	println(`AND ${ties} GAME(S) ENDED IN A TIE.`);

	// 310 PRINT: PRINT "THANKS FOR PLAYING!!"
	println("\nTHANKS FOR PLAYING!!");
	
	// 320 END
	process.exit(0);
}
game();
