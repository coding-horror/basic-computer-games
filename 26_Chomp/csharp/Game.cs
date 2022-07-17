namespace Chomp;

internal class Game
{
    private readonly IReadWrite _io;

    public Game(IReadWrite io)
    {
        _io = io;
    }

    internal void Play()
    {
        _io.Write(Resource.Streams.Introduction);
        if (_io.ReadNumber("Do you want the rules (1=Yes, 0=No!)") != 0)
        {
            _io.Write(Resource.Streams.Rules);
        }

        while (true)
        {
            _io.Write(Resource.Streams.HereWeGo);

            var (playerCount, rowCount, columnCount) = _io.ReadParameters();

            var loser = Play(new Cookie(rowCount, columnCount), new PlayerNumber(playerCount));

            _io.WriteLine(string.Format(Resource.Formats.YouLose, loser));

            if (_io.ReadNumber("Again (1=Yes, 0=No!)") != 1) { break; }
        }
    }

    private PlayerNumber Play(Cookie cookie, PlayerNumber player)
    {
        while (true)
        {
            _io.WriteLine(cookie);

            var poisoned = Chomp(cookie, player);

            if (poisoned) { return player; }

            player++;
        }
    }

    private bool Chomp(Cookie cookie, PlayerNumber player)
    {
        while (true)
        {
            _io.WriteLine(string.Format(Resource.Formats.Player, player));

            var (row, column) = _io.Read2Numbers(Resource.Prompts.Coordinates);

            if (cookie.TryChomp((int)row, (int)column, out char chomped))
            {
                return chomped == 'P';
            }

            _io.Write(Resource.Streams.NoFair);
        }
    }
}
