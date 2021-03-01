import java.util.Scanner;

public class MathDice {

    public static void main(String[] args) {
        Scanner in = new Scanner(System.in);
        Die dieOne = new Die();
        Die dieTwo = new Die();
        int guess = 1;
        int answer;

        System.out.println("Math Dice");
        System.out.println("https://github.com/coding-horror/basic-computer-games");
        System.out.println();
        System.out.print("This program generates images of two dice.\n"
                + "When two dice and an equals sign followed by a question\n"
                + "mark have been printed, type your answer, and hit the ENTER\n" + "key.\n"
                + "To conclude the program, type 0.\n");

        while (true) {
            dieOne.printDie();
            System.out.println("   +");
            dieTwo.printDie();
            System.out.println("   =");
            int tries = 0;
            answer = dieOne.getFaceValue() + dieTwo.getFaceValue();

            while (guess!=answer && tries < 2) {
                if(tries == 1)
                    System.out.println("No, count the spots and give another answer.");
                try{
                    guess = in.nextInt();
                } catch(Exception e) {
                    System.out.println("Thats not a number!");
                    in.nextLine();
                }

                if(guess == 0) 
                    System.exit(0);
                
                tries++;
            }

            if(guess != answer){
                System.out.println("No, the answer is " + answer + "!");
            } else {
                System.out.println("Correct");
            }
            System.out.println("The dice roll again....");
        }
    }

}