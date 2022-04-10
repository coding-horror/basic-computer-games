using Basketball;
using Games.Common.IO;
using Games.Common.Randomness;

var game = Game.Create(new ConsoleIO(), new RandomNumberGenerator());

game.Play();