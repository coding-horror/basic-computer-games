using static Boxing.GameUtils;
using static System.Console;

namespace Boxing;

public class OpponentAttackStrategy : AttackStrategy
{
    private readonly Opponent _opponent;

    public OpponentAttackStrategy(Opponent opponent, Boxer player,  Action notifyGameEnded, Stack<Action> work) : base(player, work, notifyGameEnded)
    {
        _opponent = opponent;
    }

    protected override AttackPunch GetPunch()
    {
        var punch = (Punch)Roll(4);
        return new AttackPunch(punch, punch == _opponent.BestPunch);
    }

    protected override void FullSwing() // 720
    {
        Write($"{_opponent}  TAKES A FULL SWING AND");
        if (Other.Vulnerability == Punch.FullSwing)
        {
            ScoreFullSwing();
        }
        else
        {
            if (RollSatisfies(60, x => x < 30))
            {
                WriteLine(" IT'S BLOCKED!");
            }
            else
            {
                ScoreFullSwing();
            }
        }

        void ScoreFullSwing()
        {
            WriteLine(" POW!!!!! HE HITS HIM RIGHT IN THE FACE!");
            if (Other.DamageTaken > KnockoutDamageThreshold)
            {
                Work.Push(RegisterOtherKnockedOut);
            }
            Other.DamageTaken += 15;
        }
    }

    protected override void Hook() // 810
    {
        Write($"{_opponent} GETS {Other} IN THE JAW (OUCH!)");
        Other.DamageTaken += 7;
        WriteLine("....AND AGAIN!");
        Other.DamageTaken += 5;
        if (Other.DamageTaken > KnockoutDamageThreshold)
        {
            Work.Push(RegisterOtherKnockedOut);
        }
    }

    protected override void Uppercut() // 860
    {
        Write($"{Other} IS ATTACKED BY AN UPPERCUT (OH,OH)...");
        if (Other.Vulnerability == Punch.Uppercut)
        {
            ScoreUppercut();
        }
        else
        {
            if (RollSatisfies(200, x => x > 75))
            {
                WriteLine($" BLOCKS AND HITS {_opponent} WITH A HOOK.");
                _opponent.DamageTaken += 5;
            }
            else
            {
                ScoreUppercut();
            }
        }

        void ScoreUppercut()
        {
            WriteLine($"AND {_opponent} CONNECTS...");
            Other.DamageTaken += 8;
        }
    }

    protected override void Jab() // 640
    {
        Write($"{_opponent}  JABS AND ");
        if (Other.Vulnerability == Punch.Jab)
        {
            ScoreJab();
        }
        else
        {
            if (RollSatisfies(7, x => x > 4))
            {
                WriteLine("BLOOD SPILLS !!!");
                ScoreJab();
            }
            else
            {
                WriteLine("IT'S BLOCKED!");
            }
        }

        void ScoreJab() => Other.DamageTaken += 5;
    }

    private void RegisterOtherKnockedOut()
        => RegisterKnockout($"{Other} IS KNOCKED COLD AND {_opponent} IS THE WINNER AND CHAMP!");
}
