use crate::model::{Galaxy, Pos, SectorStatus};

pub fn short_range_scan(model: &Galaxy) {
    let quadrant = &model.quadrants[model.enterprise.sector.as_index()];

    println!("{:-^33}", "");
    for y in 0..=7 {
        for x in 0..=7 {
            let pos = Pos(x, y);
            if &pos == &model.enterprise.sector {
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
