use std::{io, thread, time};

const HEIGHT:usize = 24;
const WIDTH:usize = 70;

// The BASIC implementation uses a 24x70 array of integers to represent the board state.
// 1 is alive, 2 is about to die, 3 is about to be born, all other values are dead.
// (I'm not actually sure whether there are other values besides zero.)
// Here, we'll use an enum instead.
#[derive(Clone, Copy, PartialEq)]
enum CellState {
    Empty,
    Alive,
    AboutToDie,
    AboutToBeBorn
}

// Following the BASIC implementation, we will bound the board at 24 rows x 70 columns.
// Since that isn't too big (even in the 70's), we just store the whole board as an
// array of CellState. I'm experimenting with using an array-of-arrays to make references
// more convenient.
struct Board {
    cells: [[CellState; WIDTH]; HEIGHT],
    min_row: usize,
    max_row: usize,
    min_col: usize,
    max_col: usize,
    population: usize,
    generation: usize,
    invalid: bool
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
            generation: 1,
            invalid: false,
        }
    }
}

fn main() {
    println!(); println!(); println!();
    println!("Enter your pattern: ");
    let mut board = parse_pattern(get_pattern());
    loop {
        finish_cell_transitions(&mut board);
        print_board(&board);
        update_bounds(&mut board);
        update_board(&mut board);
        if board.population == 0 {
            break; // this isn't in the original implementation but I wanted it
        }
        delay();
    }
}

fn get_pattern() -> Vec<String> {
    let mut lines = Vec::new();
    loop {
        let mut line = String::new();
        // read_line reads into the buffer (appending if it's not empty).
        // It returns the number of characters read, including the newline. This will be 0 on EOF.
        // unwrap() will panic and terminate the program if there is an error reading from stdin.
        // I think that's reasonable behavior in this case.
        let nread = io::stdin().read_line(&mut line).unwrap();
        let line = line.trim_end();
        if nread == 0 || line.eq_ignore_ascii_case("DONE") {
            return lines;
        }
        lines.push(line.to_string());
    }
}

fn parse_pattern(rows: Vec<String>) -> Board {
    // A robust program would check the bounds of the inputs here. I'm not doing that,
    // because the BASIC implementation didn't, and for me, part of the joy of these
    // books back in the day was learning how my inputs could break things.

    let mut board = Board::new();

    // Strings are UTF-8 in Rust, so characters can take multiple bytes. We will convert
    // each to a Vec<char> up front so that we don't have to do that conversion multiple
    // times (to find the length of the strings in chars, then to parse each char).
    // The into_iter() method consumes rows() so it can no longer be used.
    let char_vecs = Vec::from_iter(rows.into_iter().map(|s| Vec::from_iter(s.chars())));

    // The BASIC implementation puts the pattern roughly in the center of the board,
    // assuming that there are no blank rows at the beginning or end, or blanks entered
    // at the beginning or end of every row. It wouldn't be hard to check for that, but
    // for now we'll preserve the original behavior.
    let nrows = char_vecs.len();
    let ncols = char_vecs.iter()
        .map(|l| l.len())
        .max()
        .unwrap_or(0);  // handles the case where rows is empty

    // Note that there's a subtlety here. The len() method returns a usize, i.e., an
    // unsigned int, so the result type is the same. If nlines >= 24 or ncols >= 68, the
    // result will wrap around to a giant value. These are stricter limits than you'd
    // expect from just looking at the 24x70 bounds, but again, we're preserving the
    // original behavior.
    board.min_row = 11 - nrows / 2;
    board.min_col = 33 - ncols / 2;
    board.max_row = board.min_row + nrows - 1;
    board.max_col = board.min_col + ncols - 1;

    // Loop over the rows provided. The enumerate() method augments the iterator with an index.
    for (row_index, pattern) in char_vecs.iter().enumerate()
    {
        let row = board.min_row + row_index;
        // Now loop over the non-empty cells in the current row. filter_map takes a closure that
        // returns an Option. If the Option is None, filter_map filters out that entry from the
        // for loop. If it's Some(x), filter_map executes the loop body with the value x.
        for col in pattern.iter().enumerate().filter_map(|(col_index, chr)| {
                                if *chr == ' ' || (*chr == '.' && col_index == 0) {
                                    None
                                } else {
                                    Some(board.min_col + col_index)
                                }})
        {
            board.cells[row][col] = CellState::Alive;
            board.population += 1;
        }
    }


    board
}

fn finish_cell_transitions(board: &mut Board) {
    for row in board.cells[board.min_row-1..=board.max_row+1].iter_mut() {
        for cell in row[board.min_col-1..=board.max_col+1].iter_mut() {
            if *cell == CellState::AboutToBeBorn {
                *cell = CellState::Alive;
                board.population += 1;
            } else if *cell == CellState::AboutToDie {
                *cell = CellState::Empty;
                board.population -= 1;
            }
        }
    }
}

fn print_board(board: &Board) {
    println!(); println!(); println!();
    println!("Generation: {}", board.generation);
    println!("Population: {}", board.population);
    if board.invalid {
        println!("Invalid!");
    }
    for row_index in 0..HEIGHT {
        for col_index in 0..WIDTH {
            let rep = if board.cells[row_index][col_index] == CellState::Alive { "*" } else { " " };
            print!("{rep}");
        }
        println!();
    }
}

fn update_bounds(board: &mut Board) {
    // In the BASIC implementation, this happens in the same loop that prints the board.
    // We're breaking it out to improve separation of concerns.
    // We could improve efficiency here by only searching one row outside the previous bounds.
    board.min_row = HEIGHT;
    board.max_row = 0;
    board.min_col = WIDTH;
    board.max_col = 0;
    for (irow, row) in board.cells.iter().enumerate() {
        let mut any_set = false;
        for (icol, cell) in row.iter().enumerate() {
            if *cell == CellState::Alive {
                any_set = true;
                if board.min_col > icol {
                    board.min_col = icol;
                }
                if board.max_col < icol {
                    board.max_col = icol;
                }
            }
        }
        if any_set {
            if board.min_row > irow {
                board.min_row = irow;
            }
            if board.max_row < irow {
                board.max_row = irow;
            }
        }
    }
    // If anything is alive within two cells of the boundary, mark the board invalid and
    // clamp the bounds. We need a two-cell margin because we'll count neighbors on cells
    // one space outside the min/max, and when we count neighbors we go out by an
    // additional space.
    if board.min_row < 2 {
        board.min_row = 2;
        board.invalid = true;
    }
    if board.max_row > HEIGHT - 3 {
        board.max_row = HEIGHT - 3;
        board.invalid = true;
    }
    if board.min_col < 2 {
        board.min_col = 2;
        board.invalid = true;
    }
    if board.max_col > WIDTH - 3 {
        board.max_col = WIDTH - 3;
        board.invalid = true;
    }
}

fn count_neighbors(board: &Board, row_index: usize, col_index: usize) -> i32 {
    let mut count = 0;
    assert!((1..=HEIGHT-2).contains(&row_index));
    assert!((1..=WIDTH-2).contains(&col_index));
    for i in row_index-1..=row_index+1 {
        for j in col_index-1..=col_index+1 {
            if i == row_index && j == col_index {
                continue;
            }
            if board.cells[i][j] == CellState::Alive || board.cells [i][j] == CellState::AboutToDie {
                count += 1;
            }
        }
    }
    count
}

fn update_board(board: &mut Board) {
    for row_index in board.min_row-1..=board.max_row+1 {
        for col_index in board.min_col-1..=board.max_col+1 {
            let neighbors = count_neighbors(board, row_index, col_index);
            let this_cell_state = &mut board.cells[row_index][col_index]; // borrow a mutable reference to the array cell
            *this_cell_state = match *this_cell_state {
                CellState::Empty if neighbors == 3 => CellState::AboutToBeBorn,
                CellState::Alive if !(2..=3).contains(&neighbors) => CellState::AboutToDie,
                _ => *this_cell_state
            }
        }
    }
    board.generation += 1;
}

fn delay() {
    thread::sleep(time::Duration::from_millis(500));
}
