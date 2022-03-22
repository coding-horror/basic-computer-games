using Games.Common.IO;
using Games.Common.Randomness;
using Stars;

var game = new Game(new ConsoleIO(), new RandomNumberGenerator(), maxNumber: 100, maxGuessCount: 7);

game.Play(() => true);
