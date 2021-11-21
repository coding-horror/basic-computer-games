public class ComputerBug extends Insect {

    // Create messages specific to the computer player.

    public ComputerBug() {
        // Call superclass constructor for initialization.
        super();
        addMessages(new String[]{"I GET A FEELER.", "I HAVE " + MAX_FEELERS + " FEELERS ALREADY.", "I DO NOT HAVE A HEAD."}, PARTS.FEELERS);
        addMessages(new String[]{"I NEEDED A HEAD.", "I DO NOT NEED A HEAD.", "I DO NOT HAVE A NECK."}, PARTS.HEAD);
        addMessages(new String[]{"I NOW HAVE A NECK.", "I DO NOT NEED A NECK.", "I DO NOT HAVE A BODY."}, PARTS.NECK);
        addMessages(new String[]{"I NOW HAVE A BODY.", "I DO NOT NEED A BODY."}, PARTS.BODY);
        addMessages(new String[]{"I NOW HAVE A TAIL.", "I DO NOT NEED A TAIL.", "I DO NOT HAVE A BODY."}, PARTS.TAIL);
        addMessages(new String[]{"I NOW HAVE ^^^" + " LEG", "I HAVE " + MAX_LEGS + " FEET.", "I DO NOT HAVE A BODY."}, PARTS.LEGS);
    }
}
