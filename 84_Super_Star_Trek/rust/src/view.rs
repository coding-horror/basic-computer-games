use crate::model::{Galaxy, GameStatus, Quadrant, Pos, SectorStatus};


pub fn view(model: &Galaxy) {
    match model.game_status {
        GameStatus::ShortRangeScan => {
            let quadrant = &model.quadrants[model.enterprise.sector.as_index()];
            render_quadrant(&model.enterprise.sector, quadrant)
        }
    }
}

fn render_quadrant(enterprise_sector: &Pos, quadrant: &Quadrant) {
    println!("{:-^33}", "");
    for y in 0..=7 {
        for x in 0..=7 {
            let pos = Pos(x, y);
            if &pos == enterprise_sector {
                print!("<*> ")
            } else {
                match quadrant.sector_status(&pos) {
                    SectorStatus::Star => print!(" *  "),
                    SectorStatus::StarBase => print!(">!< "),
                    SectorStatus::Klingon => print!("+K+ "),
                    _ => print!("   "),
                }                
            } 
        }
        print!("\n")
    }
    println!("{:-^33}", "");
}
