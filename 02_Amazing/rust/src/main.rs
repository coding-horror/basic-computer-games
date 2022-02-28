use rand::{Rng, thread_rng, prelude::SliceRandom};
use  std::{io, collections::HashSet};

fn main() {
    //DATA
    enum Direction {
        LEFT=0,
        UP=1,
        RIGHT=2,
        DOWN=3,
    }
    impl Direction {
        fn val(&self) -> usize {
            match self {
                Direction::LEFT=>0,
                Direction::UP=>1,
                Direction::RIGHT=>2,
                Direction::DOWN=>3,
            }
        }
    }
    const EXIT_DOWN:usize = 1;
    const EXIT_RIGHT:usize = 2;
    let mut rng = thread_rng(); //rng
    /*
    vector of:
        vectors of:
            integers 
                Initially set to 0, unprocessed cells. 
                Filled in with consecutive non-zero numbers as cells are processed
    */
    let mut used; //2d vector
    /*
    vector of:
        vectors of:
            integers 
                Remains 0 if there is no exit down or right
                Set to 1 if there is an exit down
                Set to 2 if there is an exit right
                Set to 3 if there are exits down and right
    */
    let mut walls; //2d vector
    let width;
    let height;
    let entrance_column; //rng, column of entrance
    let mut row; 
    let mut col;
    let mut count;

    

    //print welcome message
    println!("
                 AMAZING PROGRAM
    CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY\n\n\n\n");

    //prompt for input
    width = get_user_input("What is your width?");
    print!("\n"); //one blank line below
    height = get_user_input("What is your height?");
    print!("\n\n\n\n");//4 blank lines below

    //generate maze
    //initialize used and wall vectors
    //2d vectors when you don't know the sizes at compile time are wierd, but here's how it's done :)
    used = vec![0; (width * height) as usize];
    let mut used: Vec<_> = used.as_mut_slice().chunks_mut(width as usize).collect();
    let used = used.as_mut_slice(); //accessible as used[][]
    //2d vectors when you don't know the sizes at compile time are wierd, but here's how it's done :)
    walls = vec![0; (width * height) as usize];
    let mut walls: Vec<_> = walls.as_mut_slice().chunks_mut(width as usize).collect();
    let walls = walls.as_mut_slice(); //accessible as walls[][]

    entrance_column=rng.gen_range(0..width-1);
    row = 0;
    col = entrance_column;
    count = 1;
    used[row][col] = count;
    count += 1;

    while count != width*height + 1 {
        //remove possible directions that are blocked or
        //hit cells already processed
        let mut possible_directions: HashSet<usize> = vec![Direction::LEFT.val(),Direction::UP.val(),Direction::RIGHT.val(),Direction::DOWN.val()].into_iter().collect(); //create it as a vector bc that's easy, then convert it to a hashset
        if col==0 || used[row][col-1]!=0 {
            possible_directions.remove(&Direction::LEFT.val());
        }
        if row==0 || used[row-1][col]!=0 {
            possible_directions.remove(&Direction::UP.val());
        }
        if col==width-1 || used[row][col+1]!=0 {
            possible_directions.remove(&Direction::RIGHT.val());
        }
        if row==height-1 || used[row+1][col]!=0 {
            possible_directions.remove(&Direction::DOWN.val());
        }

        //If we can move in a direction, move and make opening
        if possible_directions.len() != 0 { //all values in possible_directions are not NONE
            let pos_dir_vec: Vec<_> = possible_directions.into_iter().collect(); // convert the set to a vector to get access to the choose method
            //select a random direction
            match pos_dir_vec.choose(&mut rng).expect("error") {
                0=> {
                    col -= 1;
                    walls[row][col] = EXIT_RIGHT;
                },
                1=> {
                    row -= 1;
                    walls[row][col] = EXIT_DOWN;
                },
                2=>{
                    walls[row][col] = walls[row][col] + EXIT_RIGHT;
                    col += 1;
                },
                3=>{
                    walls[row][col] = walls[row][col] + EXIT_DOWN;
                    row += 1;
                },
                _=>{},
            }
            used[row][col]=count;
            count += 1;
        }
        //otherwise, move to the next used cell, and try again
        else {
            loop {
                if col != width-1 {col += 1;}
                else if row != height-1 {row+=1; col=0;}
                else {row=0;col=0;}

                if used[row][col] != 0 {break;}
            }
        }

    }
    // Add a random exit
    col=rng.gen_range(0..width);
    row=height-1;
    walls[row][col]+=1;

    //print maze
    //first line
    for c in 0..width {
        if c == entrance_column {
            print!(".  ");
        }
        else {
            print!(".--");
        }
    }
    println!(".");
    //rest of maze
    for r in 0..height {
        print!("I");
        for c in 0..width {
            if walls[r][c]<2 {print!("  I");}
            else {print!("   ");}
        }
        println!();
        for c in 0..width {
            if walls[r][c] == 0 || walls[r][c]==2 {print!(":--");}
            else {print!(":  ");}
        }
        println!(".");
    }
}

fn get_user_input(prompt: &str) -> usize {
    //DATA
    let mut raw_input = String::new(); // temporary variable for user input that can be parsed later
    
    //input loop
    return loop {
        
        //print prompt
        println!("{}", prompt);
        
        //read user input from standard input, and store it to raw_input
        raw_input.clear(); //clear input
        io::stdin().read_line(&mut raw_input).expect( "CANNOT READ INPUT!");

        //from input, try to read a number
        match raw_input.trim().parse::<usize>() {
            Ok(i) => {
                if i>1 { //min size 1
                    break i; // this escapes the loop, returning i 
                }
                else {
                    println!("INPUT OUT OF RANGE.  TRY AGAIN.");
                    continue;// run the loop again
                }
            }
            Err(e) => {
                println!("MEANINGLESS DIMENSION.  TRY AGAIN.  {}", e.to_string().to_uppercase());
                continue; // run the loop again
            }
        };
    }
}
