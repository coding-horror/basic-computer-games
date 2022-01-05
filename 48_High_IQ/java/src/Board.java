import java.util.Map;
import java.util.HashMap;

public class Board {
  
    private final Map<Integer,Integer> board;
  
    public Board() {
        board = new HashMap<>();
        
        int[] locations = new int[] {13,14,15,
          22,23,24,
    29,30,31,32,33,34,35,
    38,39,40,42,43,44,
    47,48,49,50,51,52,53,
          58,59,60,
          67,68,69};
      
        for(int i : locations) {
            //put board(i) in
        }
        
        //set the center position as 0
        
    }
  
    public String toString() {
        return "";
    }
}
