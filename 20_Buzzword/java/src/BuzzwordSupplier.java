import java.util.Random;
import java.util.function.Supplier;

/**
 * A string supplier that provides an endless stream of random buzzwords.
 */
public class BuzzwordSupplier implements Supplier<String> {

	private static final String[] SET_1 = {
			"ABILITY","BASAL","BEHAVIORAL","CHILD-CENTERED",
			"DIFFERENTIATED","DISCOVERY","FLEXIBLE","HETEROGENEOUS",
			"HOMOGENEOUS","MANIPULATIVE","MODULAR","TAVISTOCK",
			"INDIVIDUALIZED" };

	private static final String[] SET_2 = {
			"LEARNING","EVALUATIVE","OBJECTIVE",
			"COGNITIVE","ENRICHMENT","SCHEDULING","HUMANISTIC",
			"INTEGRATED","NON-GRADED","TRAINING","VERTICAL AGE",
			"MOTIVATIONAL","CREATIVE" };

	private static final String[] SET_3 = {
			"GROUPING","MODIFICATION", "ACCOUNTABILITY","PROCESS",
			"CORE CURRICULUM","ALGORITHM", "PERFORMANCE",
			"REINFORCEMENT","OPEN CLASSROOM","RESOURCE", "STRUCTURE",
			"FACILITY","ENVIRONMENT" };

	private final Random random = new Random();

	/**
	 * Create a buzzword by concatenating a random word from each of the
	 * three word sets.
	 */
	@Override
	public String get() {
		return SET_1[random.nextInt(SET_1.length)] + ' ' +
				SET_2[random.nextInt(SET_2.length)] + ' ' +
				SET_3[random.nextInt(SET_3.length)];
	}
}
