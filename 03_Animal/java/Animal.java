import java.util.ArrayList;
import java.util.List;
import java.util.Locale;
import java.util.Scanner;

/**
 * ANIMAL
 * <p>
 * Converted from BASIC to Java by Aldrin Misquitta (@aldrinm)
 */
public class Animal {

	public static void main(String[] args) {
		printIntro();
		Scanner scan = new Scanner(System.in);

		List<Question> questions = new ArrayList<>();
		questions.add(new Question("DOES IT SWIM", "FISH", "BIRD"));

		boolean stopGame = false;
		while (!stopGame) {
			String choice = readMainChoice(scan);
			switch (choice) {
				case "LIST":
					printKnownAnimals(questions);
					break;
				case "Q":
				case "QUIT":
					stopGame = true;
					break;
				default:
					if (choice.toUpperCase(Locale.ROOT).startsWith("Y")) {
						int k = 0;
						boolean correctGuess = false;
						while (questions.size() > k && !correctGuess) {
							Question question = questions.get(k);
							correctGuess = askQuestion(question, scan);
							if (correctGuess) {
								System.out.println("WHY NOT TRY ANOTHER ANIMAL?");
							} else {
								k++;
							}
						}

						if (!correctGuess) {
							askForInformationAndSave(scan, questions);
						}
					}
			}
		}

	}

	private static void askForInformationAndSave(Scanner scan, List<Question> questions) {
		//Failed to get it right and ran out of questions
		//Let's ask the user for the new information
		System.out.print("THE ANIMAL YOU WERE THINKING OF WAS A ");
		String animal = scan.nextLine();
		System.out.printf("PLEASE TYPE IN A QUESTION THAT WOULD DISTINGUISH A %s FROM A %s ", animal, questions.get(
				questions.size() - 1).falseAnswer);
		String newQuestion = scan.nextLine();
		System.out.printf("FOR A %s THE ANSWER WOULD BE ", animal);
		boolean newAnswer = readYesOrNo(scan);
		//Add it to our list
		addNewAnimal(questions, animal, newQuestion, newAnswer);
	}

	private static void addNewAnimal(List<Question> questions, String animal, String newQuestion, boolean newAnswer) {
		Question lastQuestion = questions.get(questions.size() - 1);
		String lastAnimal = lastQuestion.falseAnswer;
		lastQuestion.falseAnswer = null; //remove the false option to indicate that there is a next question

		Question newOption;
		if (newAnswer) {
			newOption = new Question(newQuestion, animal, lastAnimal);
		} else {
			newOption = new Question(newQuestion, lastAnimal, animal);
		}
		questions.add(newOption);
	}

	private static boolean askQuestion(Question question, Scanner scanner) {
		System.out.printf("%s ? ", question.question);

		boolean chosenAnswer = readYesOrNo(scanner);
		if (chosenAnswer) {
			if (question.trueAnswer != null) {
				System.out.printf("IS IT A %s ? ", question.trueAnswer);
				return readYesOrNo(scanner);
			}
			//else go to the next question
		} else {
			if (question.falseAnswer != null) {
				System.out.printf("IS IT A %s ? ", question.falseAnswer);
				return readYesOrNo(scanner);
			}
			//else go to the next question
		}
		return false;
	}

	private static boolean readYesOrNo(Scanner scanner) {
		boolean validAnswer = false;
		Boolean choseAnswer = null;
		while (!validAnswer) {
			String answer = scanner.nextLine();
			if (answer.toUpperCase(Locale.ROOT).startsWith("Y")) {
				validAnswer = true;
				choseAnswer = true;
			} else if (answer.toUpperCase(Locale.ROOT).startsWith("N")) {
				validAnswer = true;
				choseAnswer = false;
			}
		}
		return choseAnswer;
	}

	private static void printKnownAnimals(List<Question> questions) {
		System.out.println("\nANIMALS I ALREADY KNOW ARE:");
		List<String> animals = new ArrayList<>();
		questions.forEach(q -> {
			if (q.trueAnswer != null) {
				animals.add(q.trueAnswer);
			}
			if (q.falseAnswer != null) {
				animals.add(q.falseAnswer);
			}
		});
		System.out.println(String.join("\t\t", animals));
	}

	private static String readMainChoice(Scanner scan) {
		System.out.print("ARE YOU THINKING OF AN ANIMAL ? ");
		return scan.nextLine();
	}

	private static void printIntro() {
		System.out.println("                                ANIMAL");
		System.out.println("              CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY");
		System.out.println("\n\n");
		System.out.println("PLAY 'GUESS THE ANIMAL'");
		System.out.println("\n");
		System.out.println("THINK OF AN ANIMAL AND THE COMPUTER WILL TRY TO GUESS IT.");
	}


	public static class Question {
		String question;
		String trueAnswer;
		String falseAnswer;

		public Question(String question, String trueAnswer, String falseAnswer) {
			this.question = question;
			this.trueAnswer = trueAnswer;
			this.falseAnswer = falseAnswer;
		}
	}

}

