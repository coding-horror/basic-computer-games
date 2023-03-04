use rand::Rng;

use crate::{model::*, view, input::{self, param_or_prompt_value}};

pub fn perform_short_range_scan(galaxy: &Galaxy) {
    if galaxy.enterprise.damaged.contains_key(systems::SHORT_RANGE_SCAN) {
        view::scanners_out();
        return;
    }

    view::short_range_scan(&galaxy)
}

pub fn get_amount_and_set_shields(galaxy: &mut Galaxy, provided: Vec<String>) {

    if galaxy.enterprise.damaged.contains_key(systems::SHIELD_CONTROL) {
        view::inoperable(&systems::name_for(systems::SHIELD_CONTROL));
        return;
    }

    view::energy_available(galaxy.enterprise.total_energy);
    let value = input::param_or_prompt_value(&provided, 0, view::prompts::SHIELDS, 0, i32::MAX);
    if value.is_none() {
        view::shields_unchanged();
        return;
    }

    let value = value.unwrap() as u16;
    if value > galaxy.enterprise.total_energy {
        view::ridiculous();
        view::shields_unchanged();
        return;
    }

    galaxy.enterprise.shields = value;
    view::shields_set(value);
}

pub fn gather_dir_and_speed_then_move(galaxy: &mut Galaxy, provided: Vec<String>) {

    let course = input::param_or_prompt_value(&provided, 0, view::prompts::COURSE, 1.0, 9.0);
    if course.is_none() {
        view::bad_nav();
        return;
    }

    let course = course.unwrap();

    let mut max_warp = 8.0;
    if galaxy.enterprise.damaged.contains_key(systems::WARP_ENGINES) {
        max_warp = 0.2;
    }

    let speed = input::param_or_prompt_value(&provided, 1, &view::prompts::warp_factor(max_warp), 0.0, 8.0);
    if speed.is_none() {
        view::bad_nav();
        return;
    }
    
    let speed = speed.unwrap();

    if speed > max_warp {
        view::damaged_engines(max_warp, speed);
        return;
    }

    klingons_move(galaxy);
    klingons_fire(galaxy);

    if galaxy.enterprise.destroyed {
        return;
    }

    repair_systems(&mut galaxy.enterprise, speed);
    repair_or_damage_random_system(&mut galaxy.enterprise);

    move_enterprise(course, speed, galaxy);
}

fn repair_systems(enterprise: &mut Enterprise, amount: f32) {

    let keys: Vec<String> = enterprise.damaged.keys().map(|k| k.to_string()).collect();
    let mut repaired = Vec::new();
    for key in keys {
        let fully_fixed = enterprise.repair_system(&key, amount);
        if fully_fixed {
            repaired.push(systems::name_for(&key));
        }
    }

    if repaired.len() <= 0 {
        return;
    }

    view::damage_control_report();
    for name in repaired {
        view::system_repair_completed(name);
    }
}

fn repair_or_damage_random_system(enterprise: &mut Enterprise) {
    let mut rng = rand::thread_rng();

    if rng.gen::<f32>() > 0.2 {
        return;
    }

    if rng.gen::<f32>() >= 0.6 {
        if enterprise.damaged.len() == 0 {
            return;
        }

        let damaged: Vec<String> = enterprise.damaged.keys().map(|k| k.to_string()).collect();
        let system = damaged[rng.gen_range(0..damaged.len())].to_string();
        let system_name = &systems::name_for(&system);

        enterprise.repair_system(&system, rng.gen::<f32>() * 3.0 + 1.0);
        view::random_repair_report_for(system_name, false);
        return;
    }
    
    let system = systems::KEYS[rng.gen_range(0..systems::KEYS.len())].to_string();
    let system_name = &systems::name_for(&system);

    enterprise.damage_system(&system, rng.gen::<f32>() * 5.0 + 1.0);
    view::random_repair_report_for(system_name, true);
}

fn move_enterprise(course: f32, warp_speed: f32, galaxy: &mut Galaxy) {

    let ship = &mut galaxy.enterprise;

    // todo account for being blocked

    let (path, hit_edge) = find_nav_path(ship.quadrant, ship.sector, course, warp_speed);
    let energy_cost = path.len() as u16 + 10;

    if energy_cost > ship.total_energy {
        view::insuffient_warp_energy(warp_speed);
        return
    }

    let (end_quadrant, end_sector) = path[path.len() - 1].to_local_quadrant_sector();
    if hit_edge {
        view::hit_edge(end_quadrant, end_sector);
    }
    
    if ship.quadrant != end_quadrant {
        view::enter_quadrant(end_quadrant);
        galaxy.scanned.insert(end_quadrant);
        
        if galaxy.quadrants[end_quadrant.as_index()].klingons.len() > 0 {
            view::condition_red();
            if ship.shields <= 200 {
                view::danger_shields();
            }
        }
    }

    ship.quadrant = end_quadrant;
    ship.sector = end_sector;

    let quadrant = &galaxy.quadrants[end_quadrant.as_index()];
    if quadrant.docked_at_starbase(ship.sector) {
        ship.shields = 0;
        ship.photon_torpedoes = MAX_PHOTON_TORPEDOES;
        ship.total_energy = MAX_ENERGY;
    } else {
        ship.total_energy = ship.total_energy - energy_cost;
        if ship.shields > ship.total_energy {
            view::divert_energy_from_shields();
            ship.shields = ship.total_energy;
        }
    }

    view::short_range_scan(&galaxy)
}

fn find_nav_path(start_quadrant: Pos, start_sector: Pos, course: f32, warp_speed: f32) -> (Vec<Pos>, bool) {

    let (dx, dy) = calculate_delta(course);

    let mut distance = (warp_speed * 8.0) as i8;
    if distance == 0 {
        distance = 1;
    }

    let mut last_sector = start_sector.as_galactic_sector(start_quadrant);
    let mut path = Vec::new();
    let mut hit_edge;

    loop {
        let nx = (last_sector.0 as f32 + dx) as i8;
        let ny = (last_sector.1 as f32 + dy) as i8;
        hit_edge = nx < 0 || ny < 0 || nx >= 64 || ny >= 64;
        if hit_edge {
            break;
        }
        last_sector = Pos(nx as u8, ny as u8);
        path.push(last_sector);

        distance -= 1;
        if distance == 0 {
            break;
        }
    }
  
    (path, hit_edge)
}

fn calculate_delta(course: f32) -> (f32, f32) {
    // this course delta stuff is a translation (of a translation, of a translation...) of the original basic calcs
    let dir = (course - 1.0) % 8.0;
    let (dx1, dy1) = COURSES[dir as usize];
    let (dx2, dy2) = COURSES[(dir + 1.0) as usize];
    let frac = dir - (dir as i32) as f32;

    let dx = dx1 + (dx2 - dx1) * frac;
    let dy = dy1 + (dy2 - dy1) * frac;

    (dx, dy)
}

fn klingons_move(galaxy: &mut Galaxy) {
    let quadrant = &mut galaxy.quadrants[galaxy.enterprise.quadrant.as_index()];
    for k in 0..quadrant.klingons.len() {
        let new_sector: Pos;
        loop {
            let candidate = quadrant.find_empty_sector();
            if candidate != galaxy.enterprise.sector {
                new_sector = candidate;
                break;
            }
        }
        quadrant.klingons[k].sector = new_sector;
    }
}

fn klingons_fire(galaxy: &mut Galaxy) {
    let quadrant = &mut galaxy.quadrants[galaxy.enterprise.quadrant.as_index()];
    if quadrant.docked_at_starbase(galaxy.enterprise.sector) {
        view::starbase_shields();
        return;
    }

    for k in 0..quadrant.klingons.len() {
        quadrant.klingons[k].fire_on(&mut galaxy.enterprise);
    }
}

pub fn run_damage_control(galaxy: &mut Galaxy) {

    let ship = &mut galaxy.enterprise;

    if ship.damaged.contains_key(systems::DAMAGE_CONTROL) {
        view::inoperable(&systems::name_for(systems::DAMAGE_CONTROL));
    } else {
        view::damage_control(&ship);
    }

    let quadrant = &galaxy.quadrants[ship.quadrant.as_index()];
    if ship.damaged.len() == 0 || !quadrant.docked_at_starbase(ship.sector) {
        return;
    }

    let repair_delay = quadrant.star_base.as_ref().unwrap().repair_delay;
    let repair_time = (ship.damaged.len() as f32 * 0.1 + repair_delay).max(0.9);

    view::repair_estimate(repair_time);
    if !input::prompt_yes_no(view::prompts::REPAIR) {
        return;
    }

    ship.damaged.clear();
    galaxy.stardate += repair_time;
    view::damage_control(&ship);
}

pub fn perform_long_range_scan(galaxy: &mut Galaxy) {
    if galaxy.enterprise.damaged.contains_key(systems::LONG_RANGE_SCAN) {
        view::inoperable(&systems::name_for(systems::LONG_RANGE_SCAN));
        return;
    }

    let seen = view::long_range_scan(galaxy);
    for pos in seen {
        galaxy.scanned.insert(pos);
    }
}

pub fn access_computer(galaxy: &Galaxy, provided: Vec<String>) {
    if galaxy.enterprise.damaged.contains_key(systems::COMPUTER) {
        view::inoperable(&systems::name_for(systems::COMPUTER));
        return;
    }

    let operation : i32;
    loop {
        let entered = input::param_or_prompt_value(&provided, 0, view::prompts::COMPUTER, 0, 5);
        if entered.is_none() {
            view::computer_options();
        } else {
            operation = entered.unwrap();
            break;
        }
    } 
    
    match operation {
        0 => view::galaxy_scanned_map(galaxy),
        3 => show_starbase_data(galaxy),
        5 => view::galaxy_region_map(),
        _ => todo!() // todo implement others
    }
}

fn show_starbase_data(galaxy: &Galaxy) {
    let quadrant = &galaxy.quadrants[galaxy.enterprise.quadrant.as_index()];
    match &quadrant.star_base {
        None => {
            view::no_local_starbase();
            return;
        },
        Some(s) => {
            view::starbase_report();
            let origin = galaxy.enterprise.sector;
            let target = s.sector;
            view::direction_distance(origin.direction(target), origin.dist(target))
        }
    }
}

pub fn get_power_and_fire_phasers(galaxy: &mut Galaxy, provided: Vec<String>) {
    if galaxy.enterprise.damaged.contains_key(systems::PHASERS) {
        view::inoperable(&systems::name_for(systems::PHASERS));
        return;
    }

    let quadrant = &mut galaxy.quadrants[galaxy.enterprise.quadrant.as_index()];
    if quadrant.klingons.len() == 0 {
        view::no_local_enemies();
        return;
    }

    let computer_damaged = galaxy.enterprise.damaged.contains_key(systems::COMPUTER);
    if computer_damaged {
        view::computer_accuracy_issue();
    }

    let available_energy = galaxy.enterprise.total_energy - galaxy.enterprise.shields;
    view::phasers_locked(available_energy);
    let mut power: f32;
    loop {
        let setting = param_or_prompt_value(&provided, 0, view::prompts::PHASERS, 0, available_energy);
        if setting.is_some() {
            power = setting.unwrap() as f32;
            break;
        }
    }

    if power == 0.0 {
        return;
    }

    galaxy.enterprise.total_energy -= power as u16;
    
    let mut rng = rand::thread_rng();
    if computer_damaged {
        power *= rng.gen::<f32>();
    }

    let per_enemy = power / quadrant.klingons.len() as f32;

    for k in &mut quadrant.klingons {
        let dist = k.sector.abs_diff(galaxy.enterprise.sector) as f32;
        let hit_strength = per_enemy / dist * (2.0 + rng.gen::<f32>());
        if hit_strength < 0.15 * k.energy {
            view::no_damage(k.sector);
        } else {
            k.energy -= hit_strength;
            view::hit_on_klingon(hit_strength, k.sector);
            if k.energy > 0.0 {
                view::klingon_remaining_energy(k.energy);
            } else {
                view::klingon_destroyed();
            }
        }
    }

    quadrant.klingons.retain(|k| k.energy > 0.0);

    klingons_fire(galaxy);
}

pub fn gather_dir_and_launch_torpedo(galaxy: &mut Galaxy, provided: Vec<String>) {
    let star_bases = galaxy.remaining_starbases();
    let ship = &mut galaxy.enterprise;

    if ship.damaged.contains_key(systems::TORPEDOES) {
        view::inoperable(&systems::name_for(systems::TORPEDOES));
        return;
    }

    if ship.photon_torpedoes == 0 {
        view::no_torpedoes_remaining();
        return;
    }

    let course = input::param_or_prompt_value(&provided, 0, view::prompts::TORPEDO_COURSE, 1.0, 9.0);
    if course.is_none() {
        view::bad_torpedo_course();
        return;
    }

    ship.photon_torpedoes -= 1;
    view::torpedo_track();

    let path = find_torpedo_path(ship.sector, course.unwrap());
    let quadrant = &mut galaxy.quadrants[ship.quadrant.as_index()];
    let mut hit = false;
    for p in path {
        view::torpedo_path(p);
        match quadrant.sector_status(p) {
            SectorStatus::Empty => continue,
            SectorStatus::Star => {
                hit = true;
                view::star_absorbed_torpedo(p);
                break;
            },
            SectorStatus::Klingon => {
                hit = true;
                quadrant.get_klingon(p).unwrap().energy = 0.0;
                quadrant.klingons.retain(|k| k.energy > 0.0);
                view::klingon_destroyed();
                break;
            },
            SectorStatus::StarBase => {
                hit = true;
                quadrant.star_base = None;
                let remaining = star_bases - 1;
                view::destroyed_starbase(remaining > 0);
                if remaining == 0 {
                    ship.destroyed = true;
                }
                break;
            }
        }
    }

    if ship.destroyed { // if you wiped out the last starbase, trigger game over
        return;
    }

    if !hit {
        view::torpedo_missed();
    }

    klingons_fire(galaxy);
}

fn find_torpedo_path(start_sector: Pos, course: f32) -> Vec<Pos> {

    let (dx, dy) = calculate_delta(course);

    let mut last_sector = start_sector;
    let mut path = Vec::new();

    loop {
        let nx = (last_sector.0 as f32 + dx) as i8;
        let ny = (last_sector.1 as f32 + dy) as i8;
        if nx < 0 || ny < 0 || nx >= 8 || ny >= 8 {
            break;
        }
        last_sector = Pos(nx as u8, ny as u8);
        path.push(last_sector);
    }
  
    path
}