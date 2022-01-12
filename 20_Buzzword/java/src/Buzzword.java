import java.util.Scanner;

public class Buzzword {

	public static void main(final String[] args) {
		try (
			// Scanner is a Closeable so it must be closed
			// before the program ends.
			final Scanner scanner = new Scanner(System.in);
		) {
			final BuzzwordSupplier buzzwords = new BuzzwordSupplier();
			final UserInterface userInterface = new UserInterface(
					scanner, buzzwords);
			userInterface.run();
		}
	}
}
