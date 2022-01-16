
import java.time.temporal.ValueRange;
import java.util.Arrays;
import java.util.Random;
import java.util.Scanner;

public class Number1 {

    public static int points = 0;

    public static void printempty() { System.out.println(" "); }

    public static void print(String toprint) { System.out.println(toprint); }

    public static void main(String[] args) {
        print("YOU HAVE 100 POINTS.  BY GUESSING NUMBERS FROM 1 TO 5, YOU");
        print("CAN GAIN OR LOSE POINTS DEPENDING UPON HOW CLOSE YOU GET TO");
        print("A RANDOM NUMBER SELECTED BY THE COMPUTER.");
        printempty();
        print("YOU OCCASIONALLY WILL GET A JACKPOT WHICH WILL DOUBLE(!)");
        print("YOUR POINT COUNT.  YOU WIN WHEN YOU GET 500 POINTS.");
        printempty();

        try {
            while (true) {
                print("GUESS A NUMBER FROM 1 TO 5");


                Scanner numbersc = new Scanner(System.in);
                String numberstring = numbersc.nextLine();

                int number = Integer.parseInt(numberstring);

                if (!(number < 1| number > 5)) {

                    Random rand = new Random();

                    int randomNum = rand.nextInt((5 - 1) + 1) + 1;

                    if (randomNum == number) {
                        print("YOU HIT THE JACKPOT!!!");
                        points = points * 2;
                    } else if(ValueRange.of(randomNum, randomNum + 1).isValidIntValue(number)) {
                        print("+5");
                        points = points + 5;
                    } else if(ValueRange.of(randomNum - 1, randomNum + 2).isValidIntValue(number)) {
                        print("+1");
                        points = points + 1;
                    } else if(ValueRange.of(randomNum - 3, randomNum + 1).isValidIntValue(number)) {
                        print("-1");
                        points = points - 1;
                    } else {
                        print("-half");
                        points = (int) (points * 0.5);
                    }

                    print("YOU HAVE " + points + " POINTS.");
                }

                if (points >= 500) {
                    print("!!!!YOU WIN!!!! WITH " + points + " POINTS.");
                    return;
                }
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
}
