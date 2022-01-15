using static Boxing.GameUtils;
using static System.Console;
namespace Boxing;

public class PlayerAttackStrategy : AttackStrategy
{
    private readonly Boxer _player;

    public PlayerAttackStrategy(Boxer player, Opponent opponent, Action notifyGameEnded, Stack<Action> work) 
        : base(opponent, work, notifyGameEnded) => _player = player;

    protected override AttackPunch GetPunch()
    {
        var punch = GameUtils.GetPunch($"{_player}'S PUNCH");
        return new AttackPunch(punch, punch == _player.BestPunch);
    }

    protected override void FullSwing() // 340
    {
        Write($"{_player} SWINGS AND ");
        if (Other.Vulnerability == Punch.FullSwing)
        {
            ScoreFullSwing();
        }
        else
        {
            if (RollSatisfies(30, x => x < 10))
            {
                ScoreFullSwing();
            }
            else
            {
                WriteLine("HE MISSES");
            }
        }

        void ScoreFullSwing()
        {
            WriteLine("HE CONNECTS!");
            if (Other.DamageTaken > KnockoutDamageThreshold)
            {
                Work.Push(() => RegisterKnockout($"{Other} IS KNOCKED COLD AND {_player} IS THE WINNER AND CHAMP!"));
            }
            Other.DamageTaken += 15;
        }
    }

    protected override void Uppercut() // 520
    {
        Write($"{_player} TRIES AN UPPERCUT ");
        if (Other.Vulnerability == Punch.Uppercut)
        {
            ScoreUpperCut();
        }
        else
        {
            if (RollSatisfies(100, x => x < 51))
            {
                ScoreUpperCut();
            }
            else
            {
                WriteLine("AND IT'S BLOCKED (LUCKY BLOCK!)");
            }
        }

        void ScoreUpperCut()
        {
            WriteLine("AND HE CONNECTS!");
            Other.DamageTaken += 4;
        }
    }

    protected override void Hook() // 450
    {
        Write($"{_player} GIVES THE HOOK... ");
        if (Other.Vulnerability == Punch.Hook)
        {
            ScoreHookOnOpponent();
        }
        else
        {
            if (RollSatisfies(2, x => x == 1))
            {
                WriteLine("BUT IT'S BLOCKED!!!!!!!!!!!!!");
            }
            else
            {
                ScoreHookOnOpponent();
            }
        }

        void ScoreHookOnOpponent()
        {
            WriteLine("CONNECTS...");
            Other.DamageTaken += 7;
        }
    }

    protected override void Jab()
    {
        WriteLine($"{_player} JABS AT {Other}'S HEAD");
        if (Other.Vulnerability == Punch.Jab)
        {
            ScoreJabOnOpponent();
        }
        else
        {
            if (RollSatisfies(8, x => x < 4))
            {
                WriteLine("IT'S BLOCKED.");
            }
            else
            {
                ScoreJabOnOpponent();
            }
        }

        void ScoreJabOnOpponent() => Other.DamageTaken += 3;
    }
}