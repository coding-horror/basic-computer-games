use crate::model::{Galaxy, GameStatus, Quadrant, Pos, Klingon};


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
            } else if quadrant.stars.contains(&pos) {
                print!(" *  ")
            } else if quadrant.star_bases.contains(&pos) {
                print!(">!< ")
            } else if let Some(_) = find_klingon(&pos, &quadrant.klingons) {
                print!("+K+ ")
            }
        }
        print!("\n")
    }
    println!("{:-^33}", "");
}

fn find_klingon<'a>(sector: &Pos, klingons: &'a Vec<Klingon>) -> Option<&'a Klingon> {
    klingons.into_iter().find(|k| &k.sector == sector)
}