// Rust implementation of the "Basic Computer Games" version of Conway's Life
//
// Jon Fetter-Degges
// October 2022

// I am a Rust newbie. Corrections and suggestions are welcome.

use std::{cmp, fmt, io, thread, time};

// The BASIC implementation uses integers to represent the state of each cell: 1 is
// alive, 2 is about to die, 3 is about to be born, 0 is dead. Here, we'll use an enum
// instead.
// Deriving Copy (which requires Clone) allows us to use this enum value in assignments,
// and deriving Eq (or PartialEq) allows us to use the == operator. These need to be
// explicitly specified because some enums may have associated data that makes copies and
// comparisons more complicated or expensive.
#[derive(Clone, Copy, PartialEq, Eq)]
enum CellState {
    Empty,
    Alive,
    AboutToDie,
    AboutToBeBorn,
}

// Support direct printing of the cell. In this program cells will only be Alive or Empty
// when they are printed.
impl fmt::Display for CellState {
    fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {
        let rep = match *self {
            CellState::Empty => ' ',
            CellState::Alive => '*',
            CellState::AboutToDie => 'o',
            CellState::AboutToBeBorn => '.',
        };
        write!(f, "{}", rep)
    }
}

// Following the BASIC implementation, we will bound the board at 24 rows x 70 columns.
// The board is an array of CellState. Using an array of arrays gives us bounds checking
// in both dimensions.
const HEIGHT: usize = 24;
const WIDTH: usize = 70;

struct Board {
    cells: [[CellState; WIDTH]; HEIGHT],
    min_row: usize,
    max_row: usize,
    min_col: usize,
    max_col: usize,
    population: usize,
    generation: usize,
    invalid: bool,
}

impl Board {
    fn new() -> Board {
        Board {
            cells: [[CellState::Empty; WIDTH]; HEIGHT],
            min_row: 0,
            max_row: 0,
            min_col: 0,
            max_col: 0,
            population: 0,
            generation: 0,
            invalid: false,
        }
    }
}

fn main() {
    println!(); println!(); println!();
    println!("{:33}{}", " ", "Life");
    println!("{:14}{}", " ", "Creative Computing  Morristown, New Jersey");
    println!("Enter your pattern: ");
    let mut board = parse_pattern(get_pattern());
    loop {
        finish_cell_transitions(&mut board);
        print_board(&board);
        mark_cell_transitions(&mut board);
        if board.population == 0 {
            break; // this isn't in the original implementation but it seemed better than
                   // spewing blank screens
        }
        delay();
    }
}

fn get_pattern() -> Vec<Vec<char>> {
    let max_line_len = WIDTH - 4;
    let max_line_count = HEIGHT - 4;
    let mut lines = Vec::new();
    loop {
        let mut line = String::new();
        // read_line reads into the buffer (appending if it's not empty). It returns the
        // number of characters read, including the newline. This will be 0 on EOF.
        // unwrap() will panic and terminate the program if there is an error reading
        // from stdin. That's reasonable behavior in this case.
        let nread = io::stdin().read_line(&mut line).unwrap();
        let line = line.trim_end();
        if nread == 0 || line.eq_ignore_ascii_case("DONE") {
            return lines;
        }
        // Handle Unicode by converting the string to a vector of characters up front. We
        // do this here because we check the number of characters several times, so we
        // might as well just do the Unicode parsing once.
        let line = Vec::from_iter(line.chars());
        if line.len() > max_line_len {
            println!("Line too long - the maximum is {max_line_len} characters.");
            continue;
        }
        lines.push(line);
        if lines.len() == max_line_count {
            println!("Maximum line count reached. Starting simulation.");
            return lines;
        }
    }
}

fn parse_pattern(rows: Vec<Vec<char>>) -> Board {
    // This function assumes that the input pattern in rows is in-bounds. If the pattern
    // is too large, this function will panic. get_pattern checks the size of the input,
    // so it is safe to call this function with its results.

    let mut board = Board::new();

    // The BASIC implementation puts the pattern roughly in the center of the board,
    // assuming that there are no blank rows at the beginning or end, or blanks entered
    // at the beginning or end of every row. It wouldn't be hard to check for that, but
    // for now we'll preserve the original behavior.
    let nrows = rows.len();
    // If rows is empty, the call to max will return None. The unwrap_or then provides a
    // default value
    let ncols = rows.iter().map(|l| l.len()).max().unwrap_or(0);

    // The min and max values here are unsigned. If nrows >= 24 or ncols >= 68, these
    // assignments will panic - they do not wrap around unless we use a function with
    // that specific behavior. Again, we expect bounds checking on the input before this
    // function is called.
    board.min_row = 11 - nrows / 2;
    board.min_col = 33 - ncols / 2;
    board.max_row = board.min_row + nrows - 1;
    board.max_col = board.min_col + ncols - 1;

    // Loop over the rows provided. enumerate() augments the iterator with an index.
    for (row_index, pattern) in rows.iter().enumerate() {
        let row = board.min_row + row_index;
        // Now loop over the non-empty cells in the current row. filter_map takes a
        // closure that returns an Option. If the Option is None, filter_map filters out
        // that entry from the for loop. If it's Some(x), filter_map executes the loop
        // body with the value x.
        for col in pattern.iter().enumerate().filter_map(|(col_index, chr)| {
            if *chr == ' ' || (*chr == '.' && col_index == 0) {
                None
            } else {
                Some(board.min_col + col_index)
            }
        }) {
            board.cells[row][col] = CellState::Alive;
            board.population += 1;
        }
    }

    board
}

fn finish_cell_transitions(board: &mut Board) {
    // In the BASIC implementation, this happens in the same loop that prints the board.
    // We're breaking it out to improve separation of concerns.
    let mut min_row = HEIGHT - 1;
    let mut max_row = 0usize;
    let mut min_col = WIDTH - 1;
    let mut max_col = 0usize;
    for row_index in board.min_row-1..=board.max_row+1 {
        let mut any_alive_this_row = false;
        for col_index in board.min_col-1..=board.max_col+1 {
            let cell = &mut board.cells[row_index][col_index];
            if *cell == CellState::AboutToBeBorn {
                *cell = CellState::Alive;
                board.population += 1;
            } else if *cell == CellState::AboutToDie {
                *cell = CellState::Empty;
                board.population -= 1;
            }
            if *cell == CellState::Alive {
                any_alive_this_row = true;
                min_col = cmp::min(min_col, col_index);
                max_col = cmp::max(max_col, col_index);
            }
        }
        if any_alive_this_row {
            min_row = cmp::min(min_row, row_index);
            max_row = cmp::max(max_row, row_index);
    }
    }
    // If anything is alive within two cells of the boundary, mark the board invalid and
    // clamp the bounds. We need a two-cell margin because we'll count neighbors on cells
    // one space outside the min/max, and when we count neighbors we go out by an
    // additional space.
    if min_row < 2 {
        min_row = 2;
        board.invalid = true;
    }
    if max_row > HEIGHT - 3 {
        max_row = HEIGHT - 3;
        board.invalid = true;
    }
    if min_col < 2 {
        min_col = 2;
        board.invalid = true;
    }
    if max_col > WIDTH - 3 {
        max_col = WIDTH - 3;
        board.invalid = true;
    }

    board.min_row = min_row;
    board.max_row = max_row;
    board.min_col = min_col;
    board.max_col = max_col;
}

fn print_board(board: &Board) {
    println!(); println!(); println!();
    print!("Generation: {}  Population: {}", board.generation, board.population);
    if board.invalid {
        print!("  Invalid!");
    }
    println!();
    for row_index in 0..HEIGHT {
        for col_index in 0..WIDTH {
            // This print uses the Display implementation for cell_state, above.
            print!("{}", board.cells[row_index][col_index]);
        }
        println!();
    }
}

fn count_neighbors(board: &Board, row_index: usize, col_index: usize) -> i32 {
    // Simply loop over all the immediate neighbors of a cell. We assume that the row and
    // column indices are not on (or outside) the boundary of the arrays; if they are,
    // the function will panic instead of going out of bounds.
    let mut count = 0;
    for i in row_index-1..=row_index+1 {
        for j in col_index-1..=col_index+1 {
            if i == row_index && j == col_index {
                continue;
            }
            if board.cells[i][j] == CellState::Alive || board.cells[i][j] == CellState::AboutToDie {
                count += 1;
            }
        }
    }
    count
}

fn mark_cell_transitions(board: &mut Board) {
    for row_index in board.min_row-1..=board.max_row+1 {
        for col_index in board.min_col-1..=board.max_col+1 {
            let neighbors = count_neighbors(board, row_index, col_index);
            // Borrow a mutable reference to the array cell
            let this_cell_state = &mut board.cells[row_index][col_index];
            *this_cell_state = match *this_cell_state {
                CellState::Empty if neighbors == 3 => CellState::AboutToBeBorn,
                CellState::Alive if !(2..=3).contains(&neighbors) => CellState::AboutToDie,
                _ => *this_cell_state,
            }
        }
    }
    board.generation += 1;
}

fn delay() {
    thread::sleep(time::Duration::from_millis(500));
}
