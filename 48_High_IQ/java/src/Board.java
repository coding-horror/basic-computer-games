public class Board {
  
    private int[][] board;
  
    public Board() {
        board = new int[7][7];
    
        //Set all of the corners to -1, and place pegs in proper spaces
        for(int i = 0; i < 7; i++) {
            for(int j = 0; j < 7; j++) {
                if((i < 3 || i > 5) && (j < 3 || j > 5)) {
                    //If both i and j are either less than 3 or greater than 5, then the index is a corner
                    board[i][j] = -1;
                } else if(i == 4 && j == 4) {
                    //Do not place a peg in the center
                    board[i][j] = 0;
                } else {
                    //Place a peg everywhere else
                    board[i][j] = 1;
                }
            }
        }
    }
  
    public String toString() {
        return "";
    }
}
