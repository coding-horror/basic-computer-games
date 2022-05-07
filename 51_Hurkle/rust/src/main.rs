use game::Game;

mod game;

fn main() {
    println!("\n\n\t\tHURKLE");
    println!("CREATIVE COMPUTING MORRISTOWN, NEW JERSEY\n");

    println!("A HURKLE IS HIDING ON A 10 BY 10 GRID. HOMEBASE");
    println!("ON THE GRID IS POINT 0,0 IN THE SOUTHWEST CORNER,");
    println!("AND ANY POINT ON THE GRID IS DESIGNATED BY A");
    println!("PAIR OF WHOLE NUMBERS SEPERATED BY A COMMA. THE FIRST");
    println!("NUMBER IS THE HORIZONTAL POSITION AND THE SECOND NUMBER");
    println!("IS THE VERTICAL POSITION. YOU MUST TRY TO");
    println!("GUESS THE HURKLE'S GRIDPOINT. YOU GET 5 TRIES.");
    println!("AFTER EACH TRY, I WILL TELL YOU THE APPROXIMATE");
    println!("DIRECTION TO GO TO LOOK FOR THE HURKLE.\n");

    loop {
        let mut game = Game::new();

        loop {
            if game.update() {
                println!("\nLET'S PLAY AGAIN. HURKLE IS HIDING.");
                break;
            }
        }
    }
}
