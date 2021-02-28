import java.util.ArrayList;
import java.util.Arrays;

/**
 * Stores a word and a list of synonyms for that word
 */
public class SynonymList {

    private final String word;

    private final ArrayList<String> synonyms;

    public SynonymList(String word, String[] synonyms) {
        this.word = word;
        this.synonyms = new ArrayList<>(Arrays.asList(synonyms));
    }

    /**
     * Check if the word passed to this method exists in the list of synonyms
     * N.B. Case insensitive
     *
     * @param word word to search for
     * @return true if found, otherwise false
     */
    public boolean exists(String word) {
        return synonyms.stream().anyMatch(str -> str.equalsIgnoreCase(word));
    }

    public String getWord() {
        return word;
    }

    public int size() {
        return synonyms.size();
    }

    /**
     * Returns all synonyms for this word in string array format
     *
     * @return
     */
    public String[] getSynonyms() {
        // Parameter to toArray method determines type of the resultant array
        return synonyms.toArray(new String[0]);
    }
}
