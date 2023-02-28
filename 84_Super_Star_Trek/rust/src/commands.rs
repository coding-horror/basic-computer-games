use crate::model::{Galaxy, Pos, SectorStatus, COURSES};

pub fn short_range_scan(model: &Galaxy) {
    let quadrant = &model.quadrants[model.enterprise.quadrant.as_index()];

    let data : [String; 8] = [
        format!("Stardate           {}", model.stardate),
        format!("Condition          {:?}", model.enterprise.condition),
        format!("Quadrant           {}", model.enterprise.quadrant),
        format!("Sector             {}", model.enterprise.sector),
        format!("Photon torpedoes   {}", model.enterprise.photon_torpedoes),
        format!("Total energy       {}", model.enterprise.total_energy),
        format!("Shields            {}", model.enterprise.shields),
        format!("Klingons remaining {}", model.remaining_klingons()),
    ];

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
                    _ => print!("    "),
                }                
            } 
        }
        println!("{:>9}{}", "", data[y as usize])
    }
    println!("{:-^33}", "");
}

pub fn move_enterprise(course: u8, warp_speed: f32, galaxy: &mut Galaxy) {
        
    let end = find_end_quadrant_sector(galaxy.enterprise.quadrant, galaxy.enterprise.sector, course, warp_speed);

    if end.hit_edge {
        println!("Lt. Uhura report message from Starfleet Command:
        'Permission to attempt crossing of galactic perimeter
        is hereby *Denied*. Shut down your engines.'
      Chief Engineer Scott reports, 'Warp engines shut down
        at sector {} of quadrant {}.'", end.quadrant, end.sector);
    }

    galaxy.enterprise.quadrant = end.quadrant;
    galaxy.enterprise.sector = end.sector;
        
    // if new_quadrant isnt old quadrant print intro

    short_range_scan(&galaxy)
}

struct EndPosition {
    quadrant: Pos,
    sector: Pos,
    hit_edge: bool
}

fn find_end_quadrant_sector(start_quadrant: Pos, start_sector: Pos, course: u8, warp_speed: f32) -> EndPosition {
    let (dx, dy): (i8, i8) = COURSES[(course - 1) as usize];

    let mut distance = (warp_speed * 8.0) as i8;
    if distance == 0 {
        distance = 1;
    }

    let galaxy_pos = start_quadrant * 8u8 + start_sector;

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
        nx = 63;
        hit_edge = true;
    }
    if ny >= 64 {
        ny = 63;
        hit_edge = true;
    }
    
    let quadrant = Pos((nx / 8) as u8, (ny / 8) as u8);
    let sector = Pos((nx % 8) as u8, (ny % 8) as u8);

    EndPosition { quadrant, sector, hit_edge }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn test_course_east() {
        let start_quadrant = Pos(0,0);
        let start_sector = Pos(0,0);
        let end = find_end_quadrant_sector(start_quadrant, start_sector, 1, 0.1);
        assert_eq!(end.quadrant, start_quadrant, "right quadrant");
        assert_eq!(end.sector, Pos(1,0), "right sector");
        assert!(!end.hit_edge)
    }

    #[test]
    fn test_course_far_east() {
        let start_quadrant = Pos(0,0);
        let start_sector = Pos(0,0);
        let end = find_end_quadrant_sector(start_quadrant, start_sector, 1, 1.0);
        assert_eq!(end.quadrant, Pos(1,0), "right quadrant");
        assert_eq!(end.sector, start_sector, "right sector");
        assert!(!end.hit_edge)
    }

    #[test]
    fn test_course_too_far_east() {
        let start_quadrant = Pos(0,0);
        let start_sector = Pos(0,0);
        let end = find_end_quadrant_sector(start_quadrant, start_sector, 1, 8.0);
        assert_eq!(end.quadrant, Pos(7,0), "right quadrant");
        assert_eq!(end.sector, Pos(7,0), "right sector");
        assert!(end.hit_edge)
    }

    #[test]
    fn test_course_south() {
        let start_quadrant = Pos(0,0);
        let start_sector = Pos(0,0);
        let end = find_end_quadrant_sector(start_quadrant, start_sector, 7, 0.1);
        assert_eq!(end.quadrant, start_quadrant, "right quadrant");
        assert_eq!(end.sector, Pos(0,1), "right sector");
        assert!(!end.hit_edge)
    }

    #[test]
    fn test_course_far_south() {
        let start_quadrant = Pos(0,0);
        let start_sector = Pos(0,0);
        let end = find_end_quadrant_sector(start_quadrant, start_sector, 7, 1.0);
        assert_eq!(end.quadrant, Pos(0,1), "right quadrant");
        assert_eq!(end.sector, start_sector, "right sector");
        assert!(!end.hit_edge)
    }

    #[test]
    fn test_course_too_far_south() {
        let start_quadrant = Pos(0,0);
        let start_sector = Pos(0,0);
        let end = find_end_quadrant_sector(start_quadrant, start_sector, 7, 8.0);
        assert_eq!(end.quadrant, Pos(0,7), "right quadrant");
        assert_eq!(end.sector, Pos(0,7), "right sector");
        assert!(end.hit_edge)
    }

    #[test]
    fn test_course_north_east() {
        let start_quadrant = Pos(0,0);
        let start_sector = Pos(0,1);
        let end = find_end_quadrant_sector(start_quadrant, start_sector, 2, 0.1);
        assert_eq!(end.quadrant, start_quadrant, "right quadrant");
        assert_eq!(end.sector, Pos(1,0), "right sector");
        assert!(!end.hit_edge)
    }
}