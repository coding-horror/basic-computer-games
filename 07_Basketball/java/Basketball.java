import java.lang.Math;
import java.util.*; 
import java.util.Scanner;

/* The basketball class is a computer game that allows you to play as
  Dartmouth College's captain and playmaker
  The game uses set probabilites to simulate outcomes of each posession
  You are able to choose your shot types as well as defensive formations */

public class Basketball {
    int time = 0;
    int[] score = {0, 0};
    double defense = -1;
    List<Double> defense_choices = Arrays.asList(6.0, 6.5, 7.0, 7.5);
    int shot = -1;
    List<Integer> shot_choices = Arrays.asList(0, 1, 2, 3, 4);
    double opponent_chance = 0;
    String opponent = null; 

    public Basketball() {

        // Explains the keyboard inputs
        System.out.println("\t\t\t Basketball");
        System.out.println("\t Creative Computing  Morristown, New Jersey\n\n\n");
        System.out.println("This is Dartmouth College basketball. ");
        System.out.println("Υou will be Dartmouth captain and playmaker.");
        System.out.println("Call shots as follows:");
        System.out.println("1. Long (30ft.) Jump Shot; 2. Short (15 ft.) Jump Shot; "
              + "3. Lay up; 4. Set Shot");
        System.out.println("Both teams will use the same defense. Call Defense as follows:");
        System.out.println("6. Press; 6.5 Man-to-Man; 7. Zone; 7.5 None.");
        System.out.println("To change defense, just type 0 as your next shot.");
        System.out.print("Your starting defense will be? ");

        Scanner scanner = new Scanner(System.in); // creates a scanner

        // takes input for a defense
        if (scanner.hasNextDouble()) {
            defense = scanner.nextDouble();
        }
        else {
            scanner.next();
        }
        
        // makes sure that input is legal
        while (!defense_choices.contains(defense)) {
            System.out.print("Your new defensive allignment is? ");
            if (scanner.hasNextDouble()) {
                defense = scanner.nextDouble();
            }
            else {
                scanner.next();
                continue;
            }
        }

        // takes input for opponent's name
        System.out.print("\nChoose your opponent? ");

        opponent = scanner.next(); 
        start_of_period();
    }

    // adds points to the score
    // team can take 0 or 1, for opponent or Dartmouth, respectively
    private void add_points(int team, int points) {
        score[team] += points;
        print_score();
    }
        

    private void ball_passed_back() {
        System.out.print("Ball passed back to you. ");
        dartmouth_ball();
    }
        
    // change defense, called when the user enters 0 for their shot
    private void change_defense() {
        defense = -1;
        Scanner scanner = new Scanner(System.in); // creates a scanner

        while (!defense_choices.contains(defense)) {
            System.out.println("Your new defensive allignment is? ");
            if (scanner.hasNextDouble()) {
                defense = (double)(scanner.nextDouble()); 
            }
            else {
                continue; 
            }
        }
            
        dartmouth_ball();
    }

    // simulates two foul shots for a player and adds the points
    private void foul_shots(int team) {
        System.out.println("Shooter fouled.  Two shots.");

        if (Math.random() > .49) {
            if (Math.random() > .75) {
                System.out.println("Both shots missed.");
            }
            else {
                System.out.println("Shooter makes one shot and misses one.");
                score[team] += 1; 
            } 
        }
        else {
            System.out.println("Shooter makes both shots.");
            score[team] += 2;
        }
        
        print_score(); 
    }

    // called when time = 50, starts a new period
    private void halftime() {
        System.out.println("\n   ***** End of first half *****\n");
        print_score();
        start_of_period();
    }

    // prints the current score
    private void print_score() {
        System.out.println("Score:  " + score[1] + " to " + score[0] + "\n");
    }

    // simulates a center jump for posession at the beginning of a period
    private void start_of_period() {
        System.out.println("Center jump"); 
        if (Math.random() > .6) {
            System.out.println("Dartmouth controls the tap.\n"); 
            dartmouth_ball();
        }
        else {
            System.out.println(opponent + " controls the tap.\n"); 
            opponent_ball();
        }
    }

    // called when t = 92
    private void two_minute_warning() {
        System.out.println("   *** Two minutes left in the game ***");
    }
        
    // called when the user enters 1 or 2 for their shot
    private void dartmouth_jump_shot() {
        time ++;  
        if (time == 50) {
            halftime();
        }
        else if (time == 92) {
            two_minute_warning();
        }

        System.out.println("Jump Shot.");
        // simulates chances of different possible outcomes
        if (Math.random() > .341 * defense / 8) {
            if (Math.random() > .682 * defense / 8) {
                if (Math.random() > .782 * defense / 8) {
                    if (Math.random() > .843 * defense / 8) {
                        System.out.println("Charging foul. Dartmouth loses ball.\n");
                        opponent_ball();
                    }
                    else {
                        // player is fouled
                        foul_shots(1);
                        opponent_ball();
                    }
                }
                else {
                    if (Math.random() > .5) {
                        System.out.println("Shot is blocked. Ball controlled by " +
                              opponent + ".\n"); 
                        opponent_ball(); 
                    } 
                    else {
                        System.out.println("Shot is blocked. Ball controlled by Dartmouth."); 
                        dartmouth_ball(); 
                    } 
                }
            }
            else {
                System.out.println("Shot is off target."); 
                if (defense / 6 * Math.random() > .45) {
                    System.out.println("Rebound to " + opponent + "\n"); 
                    opponent_ball(); 
                } 
                else {
                    System.out.println("Dartmouth controls the rebound."); 
                    if (Math.random() > .4) {
                        if (defense == 6 && Math.random() > .6) {
                            System.out.println("Pass stolen by " + opponent
                                  + ", easy lay up"); 
                            add_points(0, 2); 
                            dartmouth_ball(); 
                        }    
                        else {
                            // ball is passed back to you
                            ball_passed_back(); 
                        }  
                    }
                    else {
                        System.out.println(""); 
                        dartmouth_non_jump_shot(); 
                    }
                }
            }
        } 
        else {
            System.out.println("Shot is good."); 
            add_points(1, 2); 
            opponent_ball(); 
        }
    }
        
    // called when the user enters 0, 3, or 4
    // lay up, set shot, or defense change
    private void dartmouth_non_jump_shot() {
        time ++; 
        if (time == 50) {
            halftime();
        }
        else if (time == 92) {
            two_minute_warning();
        }
            
        if (shot == 4) {
            System.out.println("Set shot.");
        }
        else if (shot == 3) {
            System.out.println("Lay up."); 
        }
        else if (shot == 0) {
            change_defense(); 
        }
            
        // simulates different outcomes after a lay up or set shot
        if (7/defense*Math.random() > .4) {
            if (7/defense*Math.random() > .7) {
                if (7/defense*Math.random() > .875) {
                    if (7/defense*Math.random() > .925) {
                        System.out.println("Charging foul. Dartmouth loses the ball.\n"); 
                        opponent_ball(); 
                    }
                    else {
                        System.out.println("Shot blocked. " + opponent + "'s ball.\n"); 
                        opponent_ball(); 
                    }
                }
                else {
                    foul_shots(1); 
                    opponent_ball(); 
                }
            }
            else {
                System.out.println("Shot is off the rim."); 
                if (Math.random() > 2/3) {
                    System.out.println("Dartmouth controls the rebound."); 
                    if (Math.random() > .4) {
                        System.out.println("Ball passed back to you.\n"); 
                        dartmouth_ball(); 
                    }
                    else {
                        dartmouth_non_jump_shot(); 
                    }
                }
                else {
                    System.out.println(opponent + " controls the rebound.\n"); 
                    opponent_ball(); 
                }
            }
        }
        else {
            System.out.println("Shot is good. Two points."); 
            add_points(1, 2); 
            opponent_ball(); 
        }
    }
        
    
    // plays out a Dartmouth posession, starting with your choice of shot
    private void dartmouth_ball() {
        Scanner scanner = new Scanner(System.in); // creates a scanner
        System.out.print("Your shot? "); 
        shot = -1; 
        if (scanner.hasNextInt()) {
            shot = scanner.nextInt(); 
        }
        else {
            System.out.println("");
            scanner.next(); 
        }
        
        while (!shot_choices.contains(shot)) {
            System.out.print("Incorrect answer. Retype it. Your shot?");
            if (scanner.hasNextInt()) {
                shot = scanner.nextInt(); 
            }
            else {
                System.out.println("");
                scanner.next(); 
            }
        }

        if (time < 100 || Math.random() < .5) {
            if (shot == 1 || shot == 2) {
                dartmouth_jump_shot();
            }
            else {
                dartmouth_non_jump_shot(); 
            }
        }
        else {
            if (score[0] != score[1]) {
                System.out.println("\n   ***** End Of Game *****"); 
                System.out.println("Final Score: Dartmouth: " + score[1] + "  "
                      + opponent + ": " + score[0]); 
                System.exit(0);
            }
            else {
                System.out.println("\n   ***** End Of Second Half *****"); 
                System.out.println("Score at end of regulation time:"); 
                System.out.println("     Dartmouth: " + score[1] + " " +
                      opponent + ": " + score[0]); 
                System.out.println("Begin two minute overtime period"); 
                time = 93; 
                start_of_period(); 
            }
        }
    }
        
    // simulates the opponents jumpshot
    private void opponent_jumpshot() {
        System.out.println("Jump Shot."); 
        if (8/defense*Math.random() > .35) {
            if (8/defense*Math.random() > .75) {
                if (8/defense*Math.random() > .9) {
                    System.out.println("Offensive foul. Dartmouth's ball.\n"); 
                    dartmouth_ball(); 
                }
                else {
                    foul_shots(0); 
                    dartmouth_ball(); 
                }
            }
            else {
                System.out.println("Shot is off the rim."); 
                if (defense/6*Math.random() > .5) {
                    System.out.println(opponent + " controls the rebound.");
                    if (defense == 6) {
                        if (Math.random() > .75) {
                            System.out.println("Ball stolen. Easy lay up for Dartmouth."); 
                            add_points(1, 2); 
                            opponent_ball(); 
                        }
                        else {
                            if (Math.random() > .5) {
                                System.out.println(""); 
                                opponent_non_jumpshot(); 
                            }
                            else {
                                System.out.println("Pass back to " + opponent +
                                      " guard.\n"); 
                                opponent_ball(); 
                            }
                        }
                    }
                    else {
                        if (Math.random() > .5) {
                            opponent_non_jumpshot(); 
                        }
                        else {
                            System.out.println("Pass back to " + opponent +
                                  " guard.\n"); 
                            opponent_ball(); 
                        }
                    }
                }
                else {
                    System.out.println("Dartmouth controls the rebound.\n"); 
                    dartmouth_ball(); 
                }
            }
        } 
        else {
            System.out.println("Shot is good."); 
            add_points(0, 2); 
            dartmouth_ball(); 
        }
    }

    // simulates opponents lay up or set shot
    private void opponent_non_jumpshot() {
        if (opponent_chance > 3) {
            System.out.println("Set shot."); 
        }
        else {
            System.out.println("Lay up"); 
        }
        if (7/defense*Math.random() > .413) {
            System.out.println("Shot is missed."); 
            if (defense/6*Math.random() > .5) {
                System.out.println(opponent + " controls the rebound."); 
                if (defense == 6) {
                    if (Math.random() > .75) {
                        System.out.println("Ball stolen. Easy lay up for Dartmouth."); 
                        add_points(1, 2); 
                        opponent_ball(); 
                    }
                    else {
                        if (Math.random() > .5) {
                            System.out.println(""); 
                            opponent_non_jumpshot(); 
                        }
                        else {
                            System.out.println("Pass back to " + opponent +
                                  " guard.\n"); 
                            opponent_ball(); 
                        }
                    }
                }
                else {
                    if (Math.random() > .5) {
                        System.out.println(""); 
                        opponent_non_jumpshot(); 
                    }
                    else {
                        System.out.println("Pass back to " + opponent + " guard\n"); 
                        opponent_ball(); 
                    }
                }
            }
            else {
                System.out.println("Dartmouth controls the rebound.\n"); 
                dartmouth_ball(); 
            }
        }
        else {
            System.out.println("Shot is good."); 
            add_points(0, 2); 
            dartmouth_ball(); 
        }
    }

    // simulates an opponents possesion
    // #randomly picks jump shot or lay up / set shot.
    private void opponent_ball() {
        time ++; 
        if (time == 50) {
            halftime();
        }
        opponent_chance = 10/4*Math.random()+1; 
        if (opponent_chance > 2) {
            opponent_non_jumpshot(); 
        }
        else {
            opponent_jumpshot(); 
        }
    }

    public static void main(String[] args) {
        Basketball new_game = new Basketball();
    }
}




