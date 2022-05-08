/*******************************************************************************
 * Animal
 * 
 * From: Basic computer Games(1978)
 * 
 *    Unlike other computer games in which the computer
 *   picks a number or letter and you must guess what it is,
 *   in this game you think of an animal and the computer asks
 *   you questions and tries to guess the name of your animal.
 *   If the computer guesses incorrectly, it will ask you for a
 *   question that differentiates the animal it guessed
 *   from the one you were thinking of. In this way the
 *   computer "learns" new animals. Questions to differentiate
 *   new animals should be input without a question mark.
 *    This version of the game does not have a SAVE feature.
 *   If your sistem allows, you may modify the program to
 *   save array A$, then reload the array  when you want
 *   to play the game again. This way you can save what the
 *   computer learns over a series of games.
 *    At any time if you reply 'LIST' to the question "ARE YOU
 *   THINKING OF AN ANIMAL", the computer will tell you all the
 *   animals it knows so far.
 *    The program starts originally by knowing only FISH and BIRD.
 *   As you build up a file of animals you should use broad,
 *   general questions first and then narrow down to more specific
 *   ones with later animals. For example, If an elephant was to be
 *   your first animal, the computer would ask for a question to distinguish
 *   an elephant from a bird. Naturally there are hundreds of possibilities,
 *   however, if you plan to build a large file of animals a good question
 *   would be "IS IT A MAMAL".
 *    This program can be easily modified to deal with categories of
 *   things other than animals by simply modifying the initial data
 *   in Line 530 and the dialogue references to animal in Lines 10,
 *   40, 50, 130, 230, 240 and 600. In an educational environment, this
 *   would be a valuable program to teach the distinguishing chacteristics
 *   of many classes of objects -- rock formations, geography, marine life,
 *   cell structures, etc.
 *    Originally developed by Arthur Luehrmann at Dartmouth College,
 *   Animal was subsequently shortened and modified by Nathan Teichholtz at
 *   DEC and Steve North at Creative Computing
 ******************************************************************************/
/*******************************************************************************
 * Porting notes:
 * 
 * The data structure used for the game is B-Tree where each leaf is an animal
 * and non-leaf node is a question.
 * 
 * B-Tree is implemented in non-traditional way. It uses HashMap for string
 * nodes data and use determenistic method to calculate keys for left (yes) and 
 * right (no) nodes. (See comments in the code.)
 * 
 * The logic of the game mostly kept in `main` function with the use of some
 * helper functions.
 ******************************************************************************/
use std::collections::HashMap;
use std::io;

/// Main function that contains all the logic.
fn main() {
    println!("{: ^80}", "Animal");
    println!("{: ^80}\n", "Creative Computing Morristown, New Jersey");
    println!("Play ´Guess the Animal´");
    println!("Think of an animal and the computer will try to guess it.\n");

    // Initial game data
    let mut animal = BTree::new(
        "Does it swim".to_string(),
        "Fish".to_string(),
        "Bird".to_string(),
    );
    
    // Main game loop
    while keep_playing() {
        animal.restart();
        // Ask questions until player reaches an animal.
        while ! animal.is_leaf() {
            println!("{}? ", animal.get());
            if yes_no() {
                animal.yes();
            } else {
                animal.no();
            }
        }
        // Ask if this is the animal player is thinking of.
        println!("Is it a {}? ", animal.get());
        if ! yes_no() {
            // Add a new animal if it's not, and distiguish it from the existing
            // one with a question.
            println!("The animal you were thinking of was a ? ");
            let new_animal = read_input();
            let old_animal = animal.get();
            println!("Please type in a question that would distinguish a {} from a {}: ",
                new_animal, old_animal );
            let new_question = read_input();
                println!("for a {} the answer would be: ", new_animal);
            if yes_no() {
                animal.set(new_question, new_animal, old_animal)
            } else {
                animal.set(new_question, old_animal, new_animal)
            }
        }
        println!("Why not try another animal?");
    }
}

/// Reads the input line from [`io::stdin`] and returns it as a [`String`].
fn read_input() -> String {
    let mut input = String::new();
    io::stdin().read_line(&mut input).unwrap();
    input.trim().parse::<String>().unwrap()
}

/// Asks the player whether the player wants to continue playing. Returns 
/// [`true`] if the answer is yes.
fn keep_playing() -> bool {
    println!("Are you thinking of an animal? ");
    yes_no()
}

/// Checks whether given answer is `yes` or `no`, returns [`true`] if yes.
fn yes_no() -> bool {
    loop {
        let answer = read_input().to_lowercase();
        if answer != "y" && answer != "yes" && answer != "n" && answer != "no" {
            println!("Please type `yes` or `no`");
            continue;
        }
        return answer == "y" || answer == "yes";
    }
}

/// Binary Tree data structure. Implemented the similar way to Max/Min Heap.
struct BTree {
    /// contains all nodes data (questions and animals).
    nodes: HashMap<usize,String>,
    /// contains the key to current node in the game.
    cursor: usize,
}

impl BTree {
    /// Creates new [`BTree`] with one root node (question) and two childs 
    /// (animals).
    fn new(value: String, yes: String, no: String) -> BTree {
        let nodes: HashMap<usize,String> = HashMap::from([
            (0, value),
            (1, yes),
            (2, no),
        ]);
        BTree { nodes: nodes, cursor: 0 }
    }
    
    /// Returns the key for left node (yes) based on the postition of the 
    /// [`BTree::cursor`].
    fn get_yes_key(&self) -> usize {
        &self.cursor * 2 + 1
    }

    /// Returns the key for right node (no) based on the postition of the 
    /// [`BTree::cursor`].
    fn get_no_key(&self) -> usize {
        &self.cursor * 2 + 2
    }
    
    /// Check if current node is a leaf (has no children).
    fn is_leaf(&self) -> bool {
        ! ( self.nodes.contains_key(&self.get_yes_key()) || 
            self.nodes.contains_key(&self.get_no_key()) )
    }

    /// Moves cursor to `yes` (left node) if the node exists.
    fn yes(&mut self) {
        if self.nodes.contains_key(&self.get_yes_key()) {
            self.cursor = self.get_yes_key();   
        }
    }

    /// Moves cursor to `no` (right node) if the node exists.
    fn no(&mut self) {
        if self.nodes.contains_key(&self.get_no_key()) {
            self.cursor = self.get_no_key();   
        }
    }
    
    /// Sets new value (question) and two children (animals) at current position
    /// of the [`BTree::cursor`].
    fn set(&mut self, value: String, yes: String, no: String) {
        if let Some(v) = self.nodes.get_mut(&self.cursor) {
            *v = value;
        }
        self.nodes.insert(self.get_yes_key(), yes);
        self.nodes.insert(self.get_no_key(), no);
    }
    
    /// Returns the value (question or animal) of the current node.
    fn get(&self) -> String {
        if let Some(t) = self.nodes.get(&self.cursor) {
            return t.to_string();
        }
        "".to_string()
    }

    /// Reset cursor to 0 (root node).
    fn restart(&mut self) {
        self.cursor = 0;
    }
}

#[cfg(test)]
mod tests {
    use super::*;
    
    #[test]
    fn test_new() {
        let got = BTree::new("root".to_string(), "yes".to_string(), "no".to_string());
        let want = BTree{nodes: HashMap::from([
            (0, "root".to_string()),
            (1, "yes".to_string()),
            (2, "no".to_string()),
        ]), cursor: 0};
        assert_eq!(got.nodes, want.nodes);
        assert_eq!(got.cursor, want.cursor);
    }

    #[test]
    fn test_get() {
        let got = BTree::new("root".to_string(), "yes".to_string(), "no".to_string());
        let want = "root".to_string();
        assert_eq!(got.get(), want);
    }

    #[test]
    fn test_set() {
        let mut got = BTree::new("root".to_string(), "yes".to_string(), "no".to_string());
        got.set("Root".to_string(), "Yes".to_string(), "No".to_string());
        let want = BTree{nodes: HashMap::from([
                (0, "Root".to_string()),
                (1, "Yes".to_string()),
                (2, "No".to_string()),
            ]), cursor: 0};
        assert_eq!(got.nodes, want.nodes);
        assert_eq!(got.cursor, want.cursor);
    }

    #[test]
    fn test_yes() {
        let mut got = BTree::new("root".to_string(), "yes".to_string(), "no".to_string());
        let want = "yes".to_string();
        let want_cursor = 1;
        got.yes();
        assert_eq!(got.get(), want);
        assert_eq!(got.cursor, want_cursor);
    }

    #[test]
    fn test_no() {
        let mut got = BTree::new("root".to_string(), "yes".to_string(), "no".to_string());
        let want = "no".to_string();
        let want_cursor = 2;
        got.no();
        assert_eq!(got.get(), want);
        assert_eq!(got.cursor, want_cursor);
    }

    #[test]
    fn test_is_leaf() {
        let mut got = BTree::new("root".to_string(), "yes".to_string(), "no".to_string());
        assert!(!got.is_leaf(), "should not be leaf");
        got.yes();
        assert!(got.is_leaf(), "should be leaf");
    }

    #[test]
    fn test_get_key() {
        let mut got = BTree::new("root".to_string(), "yes".to_string(), "no".to_string());
        assert_eq!(got.get_yes_key(), 1);
        assert_eq!(got.get_no_key(), 2);
        got.yes();
        assert_eq!(got.get_yes_key(), 3);
        assert_eq!(got.get_no_key(), 4);
    }

    #[test]
    fn test_restart() {
        let mut got = BTree::new("root".to_string(), "yes".to_string(), "no".to_string());
        assert_eq!(got.cursor, 0);
        got.yes();
        assert_eq!(got.cursor, 1);
        got.restart();
        assert_eq!(got.cursor, 0);
    }
}
