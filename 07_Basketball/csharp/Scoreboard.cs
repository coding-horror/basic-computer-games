using Basketball.Resources;
using Games.Common.IO;

namespace Basketball;

internal class Scoreboard
{
    private readonly Dictionary<Team, uint> _scores;
    private readonly IReadWrite _io;

    private Team _home;
    private Team _visitors;
    private Team _offense;

    public Scoreboard(Team home, Team visitors, IReadWrite io)
    {
        _scores = new() { [home] = 0, [visitors] = 0 };
        _home = home;
        _visitors = visitors;
        _offense = home;
        _io = io;
    }

    public bool ScoresAreEqual => _scores[_home] == _scores[_visitors];
    public Team Offense { set => _offense = value; }

    public void AddBasket(string message) => AddScore(2, message);

    public void AddFreeThrows(uint count, string message) => AddScore(count, message);

    private void AddScore(uint score, string message)
    {
        _io.WriteLine(message);
        _scores[_offense] += score;
        Turnover();
        Display();
    }

    public void Turnover() => _offense = _offense == _home ? _visitors : _home;

    public void Display(string? format = null) =>
        _io.WriteLine(format ?? Resource.Formats.Score, _home, _scores[_home], _visitors, _scores[_visitors]);
}
