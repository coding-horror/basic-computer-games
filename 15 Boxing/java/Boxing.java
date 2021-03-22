/**
 * Boxing
 *
 * <p>
 * Based on the Basic game of BatNum here
 * https://github.com/coding-horror/basic-computer-games/tree/main/15%20Boxing
 * <p>
 */
public class Boxing {
    
    private static final Basic.Console console = new Basic.Console();
    
    private GameSession session;

    public void play() {
        showIntro();

        loadPlayers();

        console.print("%s'S ADVANTAGE IS %d AND VULNERABILITY IS SECRET.\n", session.getOpponent().getName(), session.getOpponent().getBestPunch().getCode());


        for (int roundNro = 1; roundNro <= 3; roundNro++) {
            if (session.isOver())
                break;

            session.resetPoints();
            console.print("\nROUND %d BEGINS...%n", roundNro);

            for (int majorPunches = 1; majorPunches <= 7; majorPunches++) {
                long i = Basic.randomOf(10);

                if (i > 5) {
                    boolean stopPunches = opponentPunch();
                    if (stopPunches ) break;
                } else {
                    playerPunch();
                }
            }
            showRoundWinner(roundNro);
        }
        showWinner();
    }

    private boolean opponentPunch() {
        final Punch punch = Punch.random();

        if (punch == session.getOpponent().getBestPunch()) session.addOpponentPoints(2);

        if (punch == Punch.FULL_SWING) {
            console.print("%s TAKES A FULL SWING AND", session.getOpponent().getName());
            long r6 = Basic.randomOf(60);

            if (session.getPlayer().hitVulnerability(Punch.FULL_SWING) || r6 < 30) {
                console.print(" POW!!!!! HE HITS HIM RIGHT IN THE FACE!\n");
                if (session.getPoints(session.getOpponent()) > 35) {

                    session.setKnocked();
                    return true;
                }
                session.addOpponentPoints(15);
            } else {
                console.print(" IT'S BLOCKED!\n");
            }
        }

        if (punch == Punch.HOOK  || punch == Punch.UPPERCUT) {
            if (punch == Punch.HOOK) {
                console.print("%s GETS %s IN THE JAW (OUCH!)\n", session.getOpponent().getName(), session.getPlayer().getName());

                session.addOpponentPoints(7);
                console.print("....AND AGAIN!\n");

                session.addOpponentPoints(5);
                if (session.getPoints(session.getOpponent()) > 35) {
                    session.setKnocked();
                    return true;
                }
                console.print("\n");

            }
            console.print("%s IS ATTACKED BY AN UPPERCUT (OH,OH)...\n", session.getPlayer().getName());
            long q4 = Basic.randomOf(200);
            if (session.getPlayer().hitVulnerability(Punch.UPPERCUT) || q4 <= 75) {
                console.print("AND %s CONNECTS...\n", session.getOpponent().getName());

                session.addOpponentPoints(8);
            } else {
                console.print(" BLOCKS AND HITS %s WITH A HOOK.\n", session.getOpponent().getName());

                session.addPlayerPoints(5);
            }
        }
        else {
            console.print("%s JABS AND ", session.getOpponent().getName());
            long z4 = Basic.randomOf(7);
            if (session.getPlayer().hitVulnerability(Punch.JAB))

                session.addOpponentPoints(5);
            else if (z4 > 4) {
                console.print(" BLOOD SPILLS !!!\n");

                session.addOpponentPoints(5);
            } else {
                console.print("IT'S BLOCKED!\n");
            }
        }
        return true;
    }

    private void playerPunch() {
        console.print("%s'S PUNCH? ", session.getPlayer().getName());
        final Punch punch = Punch.fromCode(console.readInt());

        if (punch == session.getPlayer().getBestPunch()) session.addPlayerPoints(2);

        switch (punch) {
            case FULL_SWING -> {
                console.print("%s SWINGS AND ", session.getPlayer().getName());
                if (session.getOpponent().getBestPunch() == Punch.JAB) {
                    console.print("HE CONNECTS!\n");
                    if (session.getPoints(session.getPlayer()) <= 35) session.addPlayerPoints(15);
                } else {
                    long x3 = Basic.randomOf(30);
                    if (x3 < 10) {
                        console.print("HE CONNECTS!\n");
                        if (session.getPoints(session.getPlayer()) <= 35) session.addPlayerPoints(15);
                    } else {
                        console.print("HE MISSES \n");
                        if (session.getPoints(session.getPlayer()) != 1) console.print("\n\n");
                    }
                }
            }
            case HOOK -> {
                console.print("\n%s GIVES THE HOOK... ", session.getPlayer().getName());
                long h1 = Basic.randomOf(2);
                if (session.getOpponent().getBestPunch() == Punch.HOOK) {

                    session.addPlayerPoints(7);
                } else if (h1 == 1) {
                    console.print("BUT IT'S BLOCKED!!!!!!!!!!!!!\n");
                } else {
                    console.print("CONNECTS...\n");

                    session.addPlayerPoints(7);
                }
            }
            case UPPERCUT -> {
                console.print("\n%s  TRIES AN UPPERCUT ", session.getPlayer().getName());
                long d5 = Basic.randomOf(100);
                if (session.getOpponent().getBestPunch() == Punch.UPPERCUT || d5 < 51) {
                    console.print("AND HE CONNECTS!\n");

                    session.addPlayerPoints(4);
                } else {
                    console.print("AND IT'S BLOCKED (LUCKY BLOCK!)\n");
                }
            }
            default -> {
                console.print("%s JABS AT %s'S HEAD \n", session.getPlayer().getName(), session.getOpponent().getName());
                if (session.getOpponent().getBestPunch() == Punch.JAB) {

                    session.addPlayerPoints(3);
                } else {
                    long c = Basic.randomOf(8);
                    if (c < 4) {
                        console.print("IT'S BLOCKED.\n");
                    } else {

                        session.addPlayerPoints(3);
                    }
                }
            }
        }
    }

    private void showRoundWinner(int roundNro) {
        if (session.isRoundWinner(session.getPlayer())) {
            console.print("\n %s WINS ROUND %d\n", session.getPlayer().getName(), roundNro);
            session.addRoundWind(session.getPlayer());
        } else {
            console.print("\n %s WINS ROUND %d\n", session.getOpponent().getName(), roundNro);
            session.addRoundWind(session.getOpponent());
        }
    }

    private void showWinner() {
        if (session.isGameWinner(session.getOpponent())) {
            console.print("%s WINS (NICE GOING, " + session.getOpponent().getName() + ").", session.getOpponent().getName());
        } else if (session.isGameWinner(session.getPlayer())) {
            console.print("%s AMAZINGLY WINS!!", session.getPlayer().getName());
        } else if (session.isPlayerKnocked()) {
            console.print("%s IS KNOCKED COLD AND %s IS THE WINNER AND CHAMP!", session.getPlayer().getName(), session.getOpponent().getName());
        } else {
            console.print("%s IS KNOCKED COLD AND %s IS THE WINNER AND CHAMP!", session.getOpponent().getName(), session.getPlayer().getName());
        }

        console.print("\n\nAND NOW GOODBYE FROM THE OLYMPIC ARENA.\n");
    }

    private void loadPlayers() {
        console.print("WHAT IS YOUR OPPONENT'S NAME? ");
        final String opponentName = console.readLine();

        console.print("INPUT YOUR MAN'S NAME? ");
        final String playerName = console.readLine();

        console.print("DIFFERENT PUNCHES ARE: (1) FULL SWING; (2) HOOK; (3) UPPERCUT; (4) JAB.\n");
        console.print("WHAT IS YOUR MANS BEST? ");

        final int b = console.readInt();

        console.print("WHAT IS HIS VULNERABILITY? ");
        final int d = console.readInt();

        final Player player = new Player(playerName, Punch.fromCode(b), Punch.fromCode(d));
        final Player opponent = new Player(opponentName);

        session = new GameSession(player, opponent);
    }

    private void showIntro () {
        console.print("                                 BOXING\n");
        console.print("               CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n");
        console.print("BOXING OLYMPIC STYLE (3 ROUNDS -- 2 OUT OF 3 WINS)\n\n");
    }
}