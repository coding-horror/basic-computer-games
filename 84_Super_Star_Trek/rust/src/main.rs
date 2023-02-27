use model::{Galaxy, GameStatus, Pos, Quadrant};

mod model;

fn main() {
    let mut galaxy = Galaxy::generate_new();
    // create the model
    // start the loop
    loop {
        view(&galaxy);
        galaxy = wait_for_command(&galaxy);
    }
    // rather than using a loop, recursion and passing the ownership might be better
}

fn view(model: &Galaxy) {
    match model.game_status {
        GameStatus::ShortRangeScan => {
            let quadrant = &model.quadrants[model.enterprise.sector.as_index()];
            render_quadrant(&model.enterprise.sector, quadrant)
        }
    }
}

fn render_quadrant(enterprise_sector: &Pos, quadrant: &Quadrant) {
    
}

fn wait_for_command(galaxy: &Galaxy) -> Galaxy {
    // listen for command from readline
    // handle bad commands
    // update model
    Galaxy::generate_new()
}