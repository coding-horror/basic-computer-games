import java.util.Scanner;
import static java.lang.System.out;

// This is very close to the original BASIC. Changes:
// 1) the array indexing is adjusted by 1
// 2) the user can enter a lower case "y"
// 3) moved the word list to the top 8~)
public class Buzzword {
	private static final String[] A = {
		"ABILITY","BASAL","BEHAVIORAL","CHILD-CENTERED",
		"DIFFERENTIATED","DISCOVERY","FLEXIBLE","HETEROGENEOUS",
		"HOMOGENEOUS","MANIPULATIVE","MODULAR","TAVISTOCK",
		"INDIVIDUALIZED","LEARNING","EVALUATIVE","OBJECTIVE",
		"COGNITIVE","ENRICHMENT","SCHEDULING","HUMANISTIC",
		"INTEGRATED","NON-GRADED","TRAINING","VERTICAL AGE",
		"MOTIVATIONAL","CREATIVE","GROUPING","MODIFICATION",
		"ACCOUNTABILITY","PROCESS","CORE CURRICULUM","ALGORITHM",
		"PERFORMANCE","REINFORCEMENT","OPEN CLASSROOM","RESOURCE",
		"STRUCTURE","FACILITY","ENVIRONMENT"
	};
	private static Scanner scanner = new Scanner( System.in );

	public static void main( final String [] args ) {
		out.println( "              BUZZWORD GENERATOR" );
		out.println( "   CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY" );
		out.println();out.println();out.println();
		out.println( "THIS PROGRAM PRINTS HIGHLY ACCEPTABLE PHRASES IN" );
		out.println( "'EDUCATOR-SPEAK' THAT YOU CAN WORK INTO REPORTS" );
		out.println( "AND SPEECHES.  WHENEVER A QUESTION MARK IS PRINTED," );
		out.println( "TYPE A 'Y' FOR ANOTHER PHRASE OR 'N' TO QUIT." );
		out.println();out.println();out.println( "HERE'S THE FIRST PHRASE:" );
		do {
			out.print( A[ (int)( 13 * Math.random() ) ] + " " );
			out.print( A[ (int)( 13 * Math.random() + 13 ) ] + " " );
			out.print( A[ (int)( 13 * Math.random() + 26 ) ] ); out.println();
			out.print( "?" );
		}
		while ( "Y".equals( scanner.nextLine().toUpperCase() ) );
		out.println( "COME BACK WHEN YOU NEED HELP WITH ANOTHER REPORT!" );
	}
}
