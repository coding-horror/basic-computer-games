global using System;
global using Games.Common.IO;
global using Games.Common.Randomness;

using Mugwump;

var random = new RandomNumberGenerator();
var io = new ConsoleIO();

var game = new Game(io, random);

game.Play();
