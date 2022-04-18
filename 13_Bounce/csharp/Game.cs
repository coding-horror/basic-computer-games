using static Bounce.Resources.Resource;

namespace Bounce;

internal class Game
{
    private readonly IReadWrite _io;

    public Game(IReadWrite io)
    {
        _io = io;
    }

    public void Play(Func<bool> playAgain)
    {
        _io.Write(Streams.Title);
        _io.Write(Streams.Instructions);

        while (playAgain.Invoke())
        {
            var timeIncrement = _io.ReadParameter("Time increment (sec)");
            var velocity = _io.ReadParameter("Velocity (fps)");
            var elasticity = _io.ReadParameter("Coefficient");

            var bounce = new Bounce(velocity);
            var bounceCount = (int)(Graph.Row.Width * timeIncrement / bounce.Duration);
            var graph = new Graph(bounce.MaxHeight, timeIncrement);

            var time = 0f;
            for (var i = 0; i < bounceCount; i++, bounce = bounce.Next(elasticity))
            {
                time = bounce.Plot(graph, time);
            }

            _io.WriteLine(graph);
        }
    }
}
