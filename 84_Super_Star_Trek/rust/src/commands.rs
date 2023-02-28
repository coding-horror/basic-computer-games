use crate::model::{Galaxy, Pos, SectorStatus, COURSES};

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

pub fn move_enterprise(course: u8, warp_speed: f32, galaxy: &mut Galaxy) {
    let distance = (warp_speed * 8.0) as i8;
    let galaxy_pos = galaxy.enterprise.quadrant * 8u8 + galaxy.enterprise.sector;

    let (dx, dy): (i8, i8) = COURSES[(course - 1) as usize];

    let mut nx = (galaxy_pos.0 as i8) + dx * distance;
    let mut ny = (galaxy_pos.1 as i8) + dy * distance;

    let mut hit_edge = false;
    if nx < 0 {
        nx = 0;
        hit_edge = true;
    }
    if ny < 0 {
        ny = 0;
        hit_edge = true;
    }
    if nx >= 64 {
        ny = 63;
        hit_edge = true;
    }
    if nx >= 64 {
        ny = 63;
        hit_edge = true;
    }
    
    let new_quadrant = Pos((nx / 8) as u8, (ny / 8) as u8);
    let new_sector = Pos((nx % 8) as u8, (ny % 8) as u8);

    if hit_edge {
        println!("Lt. Uhura report message from Starfleet Command:
        'Permission to attempt crossing of galactic perimeter
        is hereby *Denied*. Shut down your engines.'
      Chief Engineer Scott reports, 'Warp engines shut down
        at sector {} of quadrant {}.'", new_quadrant, new_sector);
    }

    galaxy.enterprise.quadrant = new_quadrant;
    galaxy.enterprise.sector = new_sector;
        
    // if new_quadrant isnt old quadrant print intro

    short_range_scan(&galaxy)
}
