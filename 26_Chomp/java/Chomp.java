import java.util.Scanner;
public class Chomp{
	int rows;
	int cols;
	int numberOfPlayers;
	int []board;
	Scanner scanner;
	Chomp(){
		System.out.println("\t\t\t\tCHOMP");
		System.out.println("\t\tCREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n");
		System.out.println("THIS IS THE GAME OF CHOMP (SCIENTIFIC AMERICAN, JAN 1973)");
		System.out.print("Do you want the rules (1=Yes, 0=No!)  ");

		scanner = new Scanner(System.in);
		int choice = scanner.nextInt();
		if(choice != 0){
			System.out.println("Chomp is for 1 or more players (Humans only).\n");
			System.out.println("Here's how a board looks (This one is 5 by 7):");
			System.out.println("\t1 2 3 4 5 6 7");
			System.out.println(" 1     P * * * * * *\n 2     * * * * * * *\n 3     * * * * * * *\n 4     * * * * * * *\n 5     * * * * * * *");
			System.out.println("\nThe board is a big cookie - R rows high and C columns \nwide. You input R and C at the start. In the upper left\ncorner of the cookie is a poison square (P). The one who\nchomps the poison square loses. To take a chomp, type the\nrow and column of one of the squares on the cookie.\nAll of the squares below and to the right of that square\n(Including that square, too) disappear -- CHOMP!!\nNo fair chomping squares that have already been chomped,\nor that are outside the original dimensions of the cookie.\n");
			System.out.println("Here we go...\n");
		}
		startGame();
	}

	private void startGame(){
		System.out.print("How many players ");
		numberOfPlayers = scanner.nextInt();
		while(numberOfPlayers < 2){
			System.out.print("How many players ");
                	numberOfPlayers = scanner.nextInt();
		}
		System.out.print("How many rows ");
		rows = scanner.nextInt();
		while(rows<=0 || rows >9){
			if(rows <= 0){
				System.out.println("Minimun 1 row is required !!");
			}
			else{
				System.out.println("Too many rows(9 is maximum). ");
			}
			System.out.print("How many rows ");
			rows = scanner.nextInt();
		}
		System.out.print("How many columns ");
                cols = scanner.nextInt();
                while(cols<=0 || cols >9){
                        if(cols <= 0){
                                System.out.println("Minimun 1 column is required !!");
                        }
                        else{
                                System.out.println("Too many columns(9 is maximum). ");
                        }
                        System.out.print("How many columns ");
                        cols = scanner.nextInt();
                }
		board = new int[rows];
		for(int i=0;i<rows;i++){
			board[i]=cols;
		}
		printBoard();
		scanner.nextLine();
		move(0);
	}

	private void printBoard(){
		System.out.print("        ");
		for(int i=0;i<cols;i++){
			System.out.print(i+1);
			System.out.print(" ");
		}
		for(int i=0;i<rows;i++){
			System.out.print("\n ");
			System.out.print(i+1);
			System.out.print("      ");
			for(int j=0;j<board[i];j++){
				if(i == 0 && j == 0){
					System.out.print("P ");
				}
				else{
					System.out.print("* ");
				}
			}
		}
		System.out.println("");
	}

	private void move(int player){
		System.out.println(String.format("Player %d",(player+1)));
			
		String input;
		String [] coordinates;
		int x=-1,y=-1;
		while(true){
			try{
				System.out.print("Coordinates of chomp (Row, Column) ");
				input = scanner.nextLine();
				coordinates = input.split(",");
				x = Integer.parseInt(coordinates[0]);
				y = Integer.parseInt(coordinates[1]);
				break;
			}
			catch(Exception e){
				System.out.println("Please enter valid coordinates.");
				continue;
			}
		}

		while(x>rows || x <1 || y>cols || y<1 || board[x-1]<y){
			System.out.println("No fair. You're trying to chomp on empty space!");
	                while(true){
                        	try{
					System.out.print("Coordinates of chomp (Row, Column) ");
                	                input = scanner.nextLine();
        	                        coordinates = input.split(",");
	                                x = Integer.parseInt(coordinates[0]);
                        	        y = Integer.parseInt(coordinates[1]);
                	                break;
        	                }
	                        catch(Exception e){
                        	        System.out.println("Please enter valid coordinates.");
                	                continue;
        	                }
	                }
		}

		if(x == 1 && y == 1){
			System.out.println("You lose player "+(player+1));
			int choice = -1 ;
			while(choice != 0 && choice != 1){
				System.out.print("Again (1=Yes, 0=No!) ");
				choice = scanner.nextInt();
			}
			if(choice == 1){
				startGame();
			}
			else{
				System.exit(0);
			}
		}
		else{
			for(int i=x-1;i<rows;i++){
				if(board[i] >= y){
					board[i] = y-1;
				}
			}
			printBoard();
			move((player+1)%numberOfPlayers);
		}
	}


	public static void main(String []args){
		new Chomp();
	}
}
