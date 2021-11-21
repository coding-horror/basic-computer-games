import java.util.ArrayList;
import java.util.Arrays;

/**
 * This tracks the insect (bug) and has methods to
 * add body parts, create an array of output so it
 * can be drawn and to determine if a bug is complete.
 * N.B. This is a super class for ComputerBug and PlayerBug
 */
public class Insect {

    public static final int MAX_FEELERS = 2;
    public static final int MAX_LEGS = 6;

    public static final int ADDED = 0;
    public static final int NOT_ADDED = 1;
    public static final int MISSING = 2;

    // Various parts of the bug
    public enum PARTS {
        FEELERS,
        HEAD,
        NECK,
        BODY,
        TAIL,
        LEGS
    }

    // Tracks what parts of the bug have been added
    private boolean body;
    private boolean neck;
    private boolean head;
    private int feelers;
    private boolean tail;
    private int legs;

    // Messages about for various body parts
    // These are set in the subclass ComputerBug or PlayerBug
    private String[] bodyMessages;
    private String[] neckMessages;
    private String[] headMessages;
    private String[] feelerMessages;
    private String[] tailMessages;
    private String[] legMessages;

    public Insect() {
        init();
    }

    /**
     * Add a body to the bug if there is not one already added.
     *
     * @return return an appropriate message about the status of the operation.
     */
    public String addBody() {

        boolean currentState = false;

        if (!body) {
            body = true;
            currentState = true;
        }

        return addBodyMessage(currentState);
    }

    /**
     * Create output based on adding the body or it being already added previously
     *
     * @return contains the output message
     */

    private String addBodyMessage(boolean wasAdded) {

        // Return the appropriate message depending on whether the
        // body was added or not.
        if (wasAdded) {
            return bodyMessages[ADDED];
        } else {
            return bodyMessages[NOT_ADDED];
        }
    }

    /**
     * Add a neck if a) a body has previously been added and
     * b) a neck has not previously been added.
     *
     * @return text containing the status of the operation
     */
    public String addNeck() {

        int status = NOT_ADDED;  // Default is not added

        if (!body) {
            // No body, cannot add a neck
            status = MISSING;
        } else if (!neck) {
            neck = true;
            status = ADDED;
        }

        return neckMessages[status];
    }

    /**
     * Add a head to the bug if a) there already exists a neck and
     * b) a head has not previously been added
     *
     * @return text outlining the success of the operation
     */
    public String addHead() {

        int status = NOT_ADDED;  // Default is not added

        if (!neck) {
            // No neck, cannot add a head
            status = MISSING;
        } else if (!head) {
            head = true;
            status = ADDED;
        }

        return headMessages[status];
    }

    /**
     * Add a feeler to the head if a) there has been a head added to
     * the bug previously, and b) there are not already 2 (MAX_FEELERS)
     * feelers previously added to the bug.
     *
     * @return text outlining the status of the operation
     */
    public String addFeelers() {

        int status = NOT_ADDED;  // Default is not added

        if (!head) {
            // No head, cannot add a feeler
            status = MISSING;
        } else if (feelers < MAX_FEELERS) {
            feelers++;
            status = ADDED;
        }

        return feelerMessages[status];
    }

    /**
     * Add a tail to the bug if a) there is already a body previously added
     * to the bug and b) there is not already a tail added.
     *
     * @return text outlining the status of the operation.
     */
    public String addTail() {

        int status = NOT_ADDED;  // Default is not added

        if (!body) {
            // No body, cannot add a tail
            status = MISSING;
        } else if (!tail) {
            tail = true;
            status = ADDED;
        }

        return tailMessages[status];
    }

    /**
     * Add a leg to the bug if a) there is already a body previously added
     * b) there are less than 6 (MAX_LEGS) previously added.
     *
     * @return text outlining status of the operation.
     */
    public String addLeg() {

        int status = NOT_ADDED;  // Default is not added

        if (!body) {
            // No body, cannot add a leg
            status = MISSING;
        } else if (legs < MAX_LEGS) {
            legs++;
            status = ADDED;
        }

        String message = "";

        // Create a string showing the result of the operation

        switch(status) {
            case ADDED:
                // Replace # with number of legs
                message = legMessages[status].replace("^^^", String.valueOf(legs));
                // Add text S. if >1 leg, or just . if one leg.
                if (legs > 1) {
                    message += "S.";
                } else {
                    message += ".";
                }
                break;

            case NOT_ADDED:

                // Deliberate fall through to next case as its the
                // same code to be executed
            case MISSING:
                message = legMessages[status];
                break;
        }

        return message;
    }

    /**
     * Initialise
     */
    public void init() {
        body = false;
        neck = false;
        head = false;
        feelers = 0;
        tail = false;
        legs = 0;
    }

    /**
     * Add unique messages depending on type of player
     * A subclass of this class calls this method
     * e.g. See ComputerBug or PlayerBug classes
     *
     * @param messages an array of messages
     * @param bodyPart the bodypart the messages relate to.
     */
    public void addMessages(String[] messages, PARTS bodyPart) {

        switch (bodyPart) {
            case FEELERS:
                feelerMessages = messages;
                break;

            case HEAD:
                headMessages = messages;
                break;

            case NECK:
                neckMessages = messages;
                break;

            case BODY:
                bodyMessages = messages;
                break;

            case TAIL:
                tailMessages = messages;
                break;

            case LEGS:
                legMessages = messages;
                break;
        }
    }

    /**
     * Returns a string array containing
     * the "bug" that can be output to console
     *
     * @return the bug ready to draw
     */
    public ArrayList<String> draw() {
        ArrayList<String> bug = new ArrayList<>();
        StringBuilder lineOutput;

        // Feelers
        if (feelers > 0) {
            for (int i = 0; i < 4; i++) {
                lineOutput = new StringBuilder(addSpaces(10));
                for (int j = 0; j < feelers; j++) {
                    lineOutput.append("A ");
                }
                bug.add(lineOutput.toString());
            }
        }

        if (head) {
            lineOutput = new StringBuilder(addSpaces(8) + "HHHHHHH");
            bug.add(lineOutput.toString());
            lineOutput = new StringBuilder(addSpaces(8) + "H" + addSpaces(5) + "H");
            bug.add(lineOutput.toString());
            lineOutput = new StringBuilder(addSpaces(8) + "H O O H");
            bug.add(lineOutput.toString());
            lineOutput = new StringBuilder(addSpaces(8) + "H" + addSpaces(5) + "H");
            bug.add(lineOutput.toString());
            lineOutput = new StringBuilder(addSpaces(8) + "H" + addSpaces(2) + "V" + addSpaces(2) + "H");
            bug.add(lineOutput.toString());
            lineOutput = new StringBuilder(addSpaces(8) + "HHHHHHH");
            bug.add(lineOutput.toString());
        }

        if (neck) {
            for (int i = 0; i < 2; i++) {
                lineOutput = new StringBuilder(addSpaces(10) + "N N");
                bug.add(lineOutput.toString());
            }
        }

        if (body) {
            lineOutput = new StringBuilder(addSpaces(5) + "BBBBBBBBBBBB");
            bug.add(lineOutput.toString());
            for (int i = 0; i < 2; i++) {
                lineOutput = new StringBuilder(addSpaces(5) + "B" + addSpaces(10) + "B");
                bug.add(lineOutput.toString());
            }
            if (tail) {
                lineOutput = new StringBuilder("TTTTTB" + addSpaces(10) + "B");
                bug.add(lineOutput.toString());
            }
            lineOutput = new StringBuilder(addSpaces(5) + "BBBBBBBBBBBB");
            bug.add(lineOutput.toString());
        }

        if (legs > 0) {
            for (int i = 0; i < 2; i++) {
                lineOutput = new StringBuilder(addSpaces(5));
                for (int j = 0; j < legs; j++) {
                    lineOutput.append(" L");
                }
                bug.add(lineOutput.toString());
            }
        }

        return bug;
    }

    /**
     * Check if the bug is complete i.e. it has
     * 2 (MAX_FEELERS) feelers, a head, a neck, a body
     * a tail and 6 (MAX_FEET) feet.
     *
     * @return true if complete.
     */
    public boolean complete() {
        return (feelers == MAX_FEELERS)
                && head
                && neck
                && body
                && tail
                && (legs == MAX_LEGS);
    }

    /**
     * Simulate tabs be creating a string of X spaces.
     *
     * @param number contains number of spaces needed.
     * @return a String containing the spaces
     */
    private String addSpaces(int number) {
        char[] spaces = new char[number];
        Arrays.fill(spaces, ' ');
        return new String(spaces);

    }
}