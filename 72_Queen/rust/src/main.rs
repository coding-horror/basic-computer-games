use draw::draw_board;

mod draw;
mod util;

fn main() {
    draw_board(158);
    println!("{}",util::is_move_legal(32,63));
}
