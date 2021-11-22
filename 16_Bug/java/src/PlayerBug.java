public class PlayerBug extends Insect {

    // Create messages specific to the player.

    public PlayerBug() {
        // Call superclass constructor for initialization.
        super();
        addMessages(new String[]{"I NOW GIVE YOU A FEELER.", "YOU HAVE " + MAX_FEELERS + " FEELERS ALREADY.", "YOU DO NOT HAVE A HEAD."}, PARTS.FEELERS);
        addMessages(new String[]{"YOU NEEDED A HEAD.", "YOU HAVE A HEAD.", "YOU DO NOT HAVE A NECK."}, PARTS.HEAD);
        addMessages(new String[]{"YOU NOW HAVE A NECK.", "YOU DO NOT NEED A NECK.", "YOU DO NOT HAVE A BODY."}, PARTS.NECK);
        addMessages(new String[]{"YOU NOW HAVE A BODY.", "YOU DO NOT NEED A BODY."}, PARTS.BODY);
        addMessages(new String[]{"I NOW GIVE YOU A TAIL.", "YOU ALREADY HAVE A TAIL.", "YOU DO NOT HAVE A BODY."}, PARTS.TAIL);
        addMessages(new String[]{"YOU NOW HAVE ^^^ LEG", "YOU HAVE " + MAX_LEGS + " FEET ALREADY.", "YOU DO NOT HAVE A BODY."}, PARTS.LEGS);
    }


}

