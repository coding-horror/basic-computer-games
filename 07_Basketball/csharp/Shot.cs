namespace Basketball;

public class Shot
{
    private readonly string _name;

    public Shot(string name)
    {
        _name = name;
    }

    public static bool TryGet(int shotNumber, out Shot? shot)
    {
        shot = shotNumber switch
        {
            // Although the game instructions reference two different jump shots,
            // the original game code treats them both the same and just prints "Jump shot"
            0 => null,
            <= 2 => new JumpShot(),
            3 => new Shot("Lay up"),
            4 => new Shot("Set shot"),
            _ => null
        };
        return shotNumber == 0 || shot is not null;
    }

    public static Shot Get(float shotNumber) =>
        shotNumber switch
        {
            <= 2 => new JumpShot(),
            > 3 => new Shot("Set shot"),
            > 2 => new Shot("Lay up"),
            _ => throw new Exception("Unexpected value")
        };

    public override string ToString() => _name;
}
