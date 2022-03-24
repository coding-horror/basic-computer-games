using Basketball;
using Games.Common.IO;
using Games.Common.Randomness;

var game = new Game(new ConsoleIO(), new RandomNumberGenerator());

game.Play();