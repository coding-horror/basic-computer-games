using Games.Common.IO;
using Love;
using Love.Resources;

var io = new ConsoleIO();

io.Write(Resource.Streams.Intro);

var message = io.ReadString("Your message, please");

io.Write(new LovePattern(message));
