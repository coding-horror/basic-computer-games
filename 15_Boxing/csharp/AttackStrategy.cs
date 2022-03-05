namespace Boxing;

public abstract class AttackStrategy
{
    protected const int KnockoutDamageThreshold = 35;
    protected readonly Boxer Other;
    protected readonly Stack<Action> Work;
    private readonly Action _notifyGameEnded;

    public AttackStrategy(Boxer other, Stack<Action> work, Action notifyGameEnded)
    {
        Other = other;
        Work = work;
        _notifyGameEnded = notifyGameEnded;
    }

    public void Attack()
    {
        var punch = GetPunch();
        if (punch.IsBestPunch)
        {
            Other.DamageTaken += 2;
        }

        Work.Push(punch.Punch switch
        {
            Punch.FullSwing => FullSwing,
            Punch.Hook => Hook,
            Punch.Uppercut => Uppercut,
            _ => Jab
        });
    }

    protected abstract AttackPunch GetPunch();
    protected abstract void FullSwing();
    protected abstract void Hook();
    protected abstract void Uppercut();
    protected abstract void Jab();

    protected void RegisterKnockout(string knockoutMessage)
    {
        Work.Clear();
        _notifyGameEnded();
        Console.WriteLine(knockoutMessage);
    }

    protected record AttackPunch(Punch Punch, bool IsBestPunch);
}
