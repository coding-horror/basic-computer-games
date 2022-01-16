import java.util.ArrayList;
import java.util.List;
import java.util.Locale;
import java.util.Scanner;
import java.util.stream.Collectors;

/**
 * ANIMAL
 * <p>
 * Converted from BASIC to Java by Aldrin Misquitta (@aldrinm)
 * The original BASIC program uses an array to maintain the questions and answers and to decide which question to
 * ask next. Updated this Java implementation to use a tree instead of the earlier faulty one based on a list (thanks @patimen).
 */
public class Animal {

    public static void main(String[] args) {
        printIntro();
        Scanner scan = new Scanner(System.in);

        Node root = new QuestionNode("DOES IT SWIM",
                new AnimalNode("FISH"), new AnimalNode("BIRD"));

        boolean stopGame = false;
        while (!stopGame) {
            String choice = readMainChoice(scan);
            switch (choice) {
                case "TREE":
                    printTree(root);
                    break;
                case "LIST":
                    printKnownAnimals(root);
                    break;
                case "Q":
                case "QUIT":
                    stopGame = true;
                    break;
                default:
                    if (choice.toUpperCase(Locale.ROOT).startsWith("Y")) {
                        Node current = root; //where we are in the question tree
                        Node previous; //keep track of parent of current in order to place new questions later on.

                        while (current instanceof QuestionNode) {
                            var currentQuestion = (QuestionNode) current;
                            var reply = askQuestionAndGetReply(currentQuestion, scan);

                            previous = current;
                            current = reply ? currentQuestion.getTrueAnswer() : currentQuestion.getFalseAnswer();
                            if (current instanceof AnimalNode) {
                                //We have reached a animal node, so offer it as the guess
                                var currentAnimal = (AnimalNode) current;
                                System.out.printf("IS IT A %s ? ", currentAnimal.getAnimal());
                                var animalGuessResponse = readYesOrNo(scan);
                                if (animalGuessResponse) {
                                    //we guessed right! end this round
                                    System.out.println("WHY NOT TRY ANOTHER ANIMAL?");
                                } else {
                                    //we guessed wrong :(, ask for feedback
                                    //cast previous to QuestionNode since we know at this point that it is not a leaf node
                                    askForInformationAndSave(scan, currentAnimal, (QuestionNode) previous, reply);
                                }
                            }
                        }
                    }
            }
        }
    }

    /**
     * Prompt for information about the animal we got wrong
     * @param current The animal that we guessed wrong
     * @param previous The root of current
     * @param previousToCurrentDecisionChoice Whether it was a Y or N answer that got us here. true = Y, false = N
     */
    private static void askForInformationAndSave(Scanner scan, AnimalNode current, QuestionNode previous, boolean previousToCurrentDecisionChoice) {
        //Failed to get it right and ran out of questions
        //Let's ask the user for the new information
        System.out.print("THE ANIMAL YOU WERE THINKING OF WAS A ");
        String animal = scan.nextLine();
        System.out.printf("PLEASE TYPE IN A QUESTION THAT WOULD DISTINGUISH A %s FROM A %s ", animal, current.getAnimal());
        String newQuestion = scan.nextLine();
        System.out.printf("FOR A %s THE ANSWER WOULD BE ", animal);
        boolean newAnswer = readYesOrNo(scan);
        //Add it to our question store
        addNewAnimal(current, previous, animal, newQuestion, newAnswer, previousToCurrentDecisionChoice);
    }

    private static void addNewAnimal(Node current,
                                     QuestionNode previous,
                                     String animal,
                                     String newQuestion,
                                     boolean newAnswer,
                                     boolean previousToCurrentDecisionChoice) {
        var animalNode = new AnimalNode(animal);
        var questionNode = new QuestionNode(newQuestion,
                newAnswer ? animalNode : current,
                !newAnswer ? animalNode : current);

        if (previous != null) {
            if (previousToCurrentDecisionChoice) {
                previous.setTrueAnswer(questionNode);
            } else {
                previous.setFalseAnswer(questionNode);
            }
        }
    }

    private static boolean askQuestionAndGetReply(QuestionNode questionNode, Scanner scanner) {
        System.out.printf("%s ? ", questionNode.question);
        return readYesOrNo(scanner);
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

    private static void printKnownAnimals(Node root) {
        System.out.println("\nANIMALS I ALREADY KNOW ARE:");

        List<AnimalNode> leafNodes = collectLeafNodes(root);
        String allAnimalsString = leafNodes.stream().map(AnimalNode::getAnimal).collect(Collectors.joining("\t\t"));

        System.out.println(allAnimalsString);
    }

    //Traverse the tree and collect all the leaf nodes, which basically have all the animals.
    private static List<AnimalNode> collectLeafNodes(Node root) {
        List<AnimalNode> collectedNodes = new ArrayList<>();
        if (root instanceof AnimalNode) {
            collectedNodes.add((AnimalNode) root);
        } else {
            var q = (QuestionNode) root;
            collectedNodes.addAll(collectLeafNodes(q.getTrueAnswer()));
            collectedNodes.addAll(collectLeafNodes(q.getFalseAnswer()));
        }
        return collectedNodes;
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

    //Based on https://stackoverflow.com/a/8948691/74057
    private static void printTree(Node root) {
        StringBuilder buffer = new StringBuilder(50);
        print(root, buffer, "", "");
        System.out.println(buffer);
    }

    private static void print(Node root, StringBuilder buffer, String prefix, String childrenPrefix) {
        buffer.append(prefix);
        buffer.append(root.toString());
        buffer.append('\n');

        if (root instanceof QuestionNode) {
            var questionNode = (QuestionNode) root;
            print(questionNode.getTrueAnswer(), buffer, childrenPrefix + "├─Y─ ", childrenPrefix + "│   ");
            print(questionNode.getFalseAnswer(), buffer, childrenPrefix + "└─N─ ", childrenPrefix + "    ");
        }
    }


    /**
     * Base interface for all nodes in our question tree
     */
    private interface Node {
    }

    private static class QuestionNode implements Node {
        private final String question;
        private Node trueAnswer;
        private Node falseAnswer;

        public QuestionNode(String question, Node trueAnswer, Node falseAnswer) {
            this.question = question;
            this.trueAnswer = trueAnswer;
            this.falseAnswer = falseAnswer;
        }

        public String getQuestion() {
            return question;
        }

        public Node getTrueAnswer() {
            return trueAnswer;
        }

        public void setTrueAnswer(Node trueAnswer) {
            this.trueAnswer = trueAnswer;
        }

        public Node getFalseAnswer() {
            return falseAnswer;
        }

        public void setFalseAnswer(Node falseAnswer) {
            this.falseAnswer = falseAnswer;
        }

        @Override
        public String toString() {
            return "Question{'" + question + "'}";
        }
    }

    private static class AnimalNode implements Node {
        private final String animal;

        public AnimalNode(String animal) {
            this.animal = animal;
        }

        public String getAnimal() {
            return animal;
        }

        @Override
        public String toString() {
            return "Animal{'" + animal + "'}";
        }
    }

}

