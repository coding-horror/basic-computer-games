using Games.Common.IO;

var io = new ConsoleIO();

var name = io.ReadString("What's your name");

io.WriteLine($"Hello, {name}");
