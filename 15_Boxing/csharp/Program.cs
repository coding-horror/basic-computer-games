using Boxing;
using static Boxing.GameUtils;
using static System.Console;

WriteLine(new string('\t', 33) + "BOXING");
WriteLine(new string('\t', 15) + "CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
WriteLine("{0}{0}{0}BOXING OLYMPIC STYLE (3 ROUNDS -- 2 OUT OF 3 WINS){0}", Environment.NewLine);

var opponent = new Opponent(); 
opponent.SetName("WHAT IS YOUR OPPONENT'S NAME"); // J$
var player = new Boxer(); 
player.SetName("INPUT YOUR MAN'S NAME"); // L$

PrintPunchDescription();
player.BestPunch = GetPunch("WHAT IS YOUR MANS BEST"); // B
player.Vulnerability = GetPunch("WHAT IS HIS VULNERABILITY"); // D
opponent.SetRandomPunches();
WriteLine($"{opponent}'S ADVANTAGE IS {opponent.BestPunch.ToFriendlyString()} AND VULNERABILITY IS SECRET.");


for (var i = 1; i <= 3; i ++) // R
{
    var round = new Round(player, opponent, i);
    round.Start();
    round.CheckOpponentWin();
    round.CheckPlayerWin();
    if (round.GameEnded) break;
}
WriteLine("{0}{0}AND NOW GOODBYE FROM THE OLYMPIC ARENA.{0}", Environment.NewLine);