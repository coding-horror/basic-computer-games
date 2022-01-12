import java.util.Arrays;

/**
 * Types of Punches
 */
public enum Punch {
    FULL_SWING(1),
    HOOK(2),
    UPPERCUT(3),
    JAB(4);

    private final int code;

    Punch(int code) {
        this.code = code;
    }

    int getCode() { return  code;}

    public static Punch fromCode(int code) {
        return Arrays.stream(Punch.values()).filter(p->p.code == code).findAny().orElse(null);
    }

    public static Punch random() {
        return Punch.fromCode(Basic.randomOf(4));
    }
}
