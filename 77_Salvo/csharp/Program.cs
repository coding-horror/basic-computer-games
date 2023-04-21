global using System;
global using Games.Common.IO;
global using Games.Common.Randomness;
global using Salvo;
global using Salvo.Ships;
global using static Salvo.Resources.Resource;

new Game(new ConsoleIO(), new RandomNumberGenerator()).Play();
