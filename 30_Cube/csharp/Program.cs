global using Games.Common.IO;
global using Games.Common.Randomness;

global using static Cube.Resources.Resource;

using Cube;

IRandom random = args.Contains("--non-random") ? new ZerosGenerator() : new RandomNumberGenerator();

new Game(new ConsoleIO(), random).Play();
