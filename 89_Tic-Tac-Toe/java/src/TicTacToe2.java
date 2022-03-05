import java.util.Scanner;
import java.util.Random;

/**
 * @author Ollie Hensman-Crook
 */
public class TicTacToe2 {
    public static void main(String[] args) {
        Board gameBoard = new Board();
        Random compChoice = new Random();
        char yourChar;
        char compChar;
        Scanner in = new Scanner(System.in);

        System.out.println("              TIC-TAC-TOE");
        System.out.println("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
        System.out.println("\nTHE BOARD IS NUMBERED: ");
        System.out.println(" 1  2  3\n 4  5  6\n 7  8  9\n");

        while (true) {
            // ask if the player wants to be X or O and if their input is valid set their
            // play piece as such
            System.out.println("DO YOU WANT 'X' OR 'O'");
            while (true) {
                try {
                    char input;
                    input = in.next().charAt(0);

                    if (input == 'X' || input == 'x') {
                        yourChar = 'X';
                        compChar = 'O';
                        break;
                    } else if (input == 'O' || input == 'o') {
                        yourChar = 'O';
                        compChar = 'X';
                    } else {
                        System.out.println("THATS NOT 'X' OR 'O', TRY AGAIN");
                        in.nextLine();
                    }

                } catch (Exception e) {
                    System.out.println("THATS NOT 'X' OR 'O', TRY AGAIN");
                    in.nextLine();
                }
            }

            while (true) {
                System.out.println("WHERE DO YOU MOVE");

                // check the user can move where they want to and if so move them there
                while (true) {
                    int input;
                    try {
                        input = in.nextInt();
                        if (gameBoard.getBoardValue(input) == ' ') {
                            gameBoard.setArr(input, yourChar);
                            break;
                        } else {
                            System.out.println("INVALID INPUT, TRY AGAIN");
                        }
                        in.nextLine();
                    } catch (Exception e) {
                        System.out.println("INVALID INPUT, TRY AGAIN");
                        in.nextLine();
                    }
                }

                gameBoard.printBoard();
                System.out.println("THE COMPUTER MOVES TO");

                while (true) {
                    int position = 1 + compChoice.nextInt(9);
                    if (gameBoard.getBoardValue(position) == ' ') {
                        gameBoard.setArr(position, compChar);
                        break;
                    }
                }

                gameBoard.printBoard();

                // if there is a win print if player won or the computer won and ask if they
                // want to play again
                if (gameBoard.checkWin(yourChar)) {
                    System.out.println("YOU WIN, PLAY AGAIN? (Y/N)");
                    gameBoard.clear();
                    while (true) {
                        try {
                            char input;
                            input = in.next().charAt(0);

                            if (input == 'Y' || input == 'y') {
                                break;
                            } else if (input == 'N' || input == 'n') {
                                System.exit(0);
                            } else {
                                System.out.println("THATS NOT 'Y' OR 'N', TRY AGAIN");
                                in.nextLine();
                            }

                        } catch (Exception e) {
                            System.out.println("THATS NOT 'Y' OR 'N', TRY AGAIN");
                            in.nextLine();
                        }
                    }
                    break;
                } else if (gameBoard.checkWin(compChar)) {
                    System.out.println("YOU LOSE, PLAY AGAIN? (Y/N)");
                    gameBoard.clear();
                    while (true) {
                        try {
                            char input;
                            input = in.next().charAt(0);

                            if (input == 'Y' || input == 'y') {
                                break;
                            } else if (input == 'N' || input == 'n') {
                                System.exit(0);
                            } else {
                                System.out.println("THATS NOT 'Y' OR 'N', TRY AGAIN");
                                in.nextLine();
                            }

                        } catch (Exception e) {
                            System.out.println("THATS NOT 'Y' OR 'N', TRY AGAIN");
                            in.nextLine();
                        }
                    }
                    break;
                } else if (gameBoard.checkDraw()) {
                    System.out.println("DRAW, PLAY AGAIN? (Y/N)");
                    gameBoard.clear();
                    while (true) {
                        try {
                            char input;
                            input = in.next().charAt(0);

                            if (input == 'Y' || input == 'y') {
                                break;
                            } else if (input == 'N' || input == 'n') {
                                System.exit(0);
                            } else {
                                System.out.println("THATS NOT 'Y' OR 'N', TRY AGAIN");
                                in.nextLine();
                            }

                        } catch (Exception e) {
                            System.out.println("THATS NOT 'Y' OR 'N', TRY AGAIN");
                            in.nextLine();
                        }
                    }
                    break;
                }

            }
        }
    }
}
