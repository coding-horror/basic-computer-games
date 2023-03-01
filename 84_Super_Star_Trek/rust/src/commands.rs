use crate::{model::{Galaxy, Pos, COURSES, EndPosition}, view};

pub fn move_enterprise(course: u8, warp_speed: f32, galaxy: &mut Galaxy) {

    let ship = &mut galaxy.enterprise;

    // todo account for being blocked

    let end = find_end_quadrant_sector(ship.quadrant, ship.sector, course, warp_speed);

    // todo account for engine damage

    if end.energy_cost > ship.total_energy {
        view::insuffient_warp_energy(warp_speed);
        return
    }

    if end.hit_edge {
        view::hit_edge(&end);
    }
    

    if ship.quadrant != end.quadrant {
        view::enter_quadrant(&end.quadrant);
        
        if galaxy.quadrants[end.quadrant.as_index()].klingons.len() > 0 {
            view::condition_red();
            if ship.shields <= 200 {
                view::danger_shields();
            }
        }
    }

    ship.quadrant = end.quadrant;
    ship.sector = end.sector;

    ship.total_energy = (ship.total_energy - end.energy_cost).max(0);
    if ship.shields > ship.total_energy {
        view::divert_energy_from_shields();
        ship.shields = ship.total_energy;
    }

    view::short_range_scan(&galaxy)
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
    let energy_cost = distance as u16 + 10;

    EndPosition { quadrant, sector, hit_edge, energy_cost }
}

pub fn move_klingons_and_fire(galaxy: &mut Galaxy) {
    let quadrant = &mut galaxy.quadrants[galaxy.enterprise.quadrant.as_index()];
    for k in 0..quadrant.klingons.len() {
        let new_sector = quadrant.find_empty_sector();
        quadrant.klingons[k].sector = new_sector;
    }

    // todo: check if enterprise is protected by a starbase

    for k in 0..quadrant.klingons.len() {
        quadrant.klingons[k].fire_on(&mut galaxy.enterprise);
    }
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
