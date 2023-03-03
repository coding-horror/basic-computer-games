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
    let value = input::param_or_prompt_value(&provided, 0, "Number of units to shields", 0, i32::MAX);
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

    let course = input::param_or_prompt_value(&provided, 0, "Course (1-9)?", 1, 9);
    if course.is_none() {
        view::bad_nav();
        return;
    }

    let course = course.unwrap();

    let mut max_warp = 8.0;
    if galaxy.enterprise.damaged.contains_key(systems::WARP_ENGINES) {
        max_warp = 0.2;
    }

    let speed = input::param_or_prompt_value(&provided, 1, format!("Warp Factor (0-{})?", max_warp).as_str(), 0.0, 8.0);
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

fn move_enterprise(course: u8, warp_speed: f32, galaxy: &mut Galaxy) {

    let ship = &mut galaxy.enterprise;

    // todo account for being blocked

    let end = find_end_quadrant_sector(ship.quadrant, ship.sector, course, warp_speed);

    if end.energy_cost > ship.total_energy {
        view::insuffient_warp_energy(warp_speed);
        return
    }

    if end.hit_edge {
        view::hit_edge(&end);
    }
    
    if ship.quadrant != end.quadrant {
        view::enter_quadrant(&end.quadrant);
        galaxy.scanned.insert(end.quadrant);
        
        if galaxy.quadrants[end.quadrant.as_index()].klingons.len() > 0 {
            view::condition_red();
            if ship.shields <= 200 {
                view::danger_shields();
            }
        }
    }

    ship.quadrant = end.quadrant;
    ship.sector = end.sector;

    let quadrant = &galaxy.quadrants[end.quadrant.as_index()];
    if quadrant.docked_at_starbase(ship.sector) {
        ship.shields = 0;
        ship.photon_torpedoes = MAX_PHOTON_TORPEDOES;
        ship.total_energy = MAX_ENERGY;
    } else {
        ship.total_energy = (ship.total_energy - end.energy_cost).max(0);
        if ship.shields > ship.total_energy {
            view::divert_energy_from_shields();
            ship.shields = ship.total_energy;
        }
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

    let hit_edge = nx < 0 || ny < 0 || nx >= 64 || ny >= 64;
    nx = nx.min(63).max(0);
    ny = ny.min(63).max(0);
    
    let quadrant = Pos((nx / 8) as u8, (ny / 8) as u8);
    let sector = Pos((nx % 8) as u8, (ny % 8) as u8);
    let energy_cost = distance as u16 + 10;

    EndPosition { quadrant, sector, hit_edge, energy_cost }
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
    if !input::prompt_yes_no("Will you authorize the repair order") {
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
        let entered = input::param_or_prompt_value(&provided, 0, "Computer active and waiting command?", 0, 5);
        if entered.is_none() {
            view::computer_options();
        } else {
            operation = entered.unwrap();
            break;
        }
    } 
    
    match operation {
        0 => view::galaxy_scanned_map(galaxy),
        5 => view::galaxy_region_map(),
        _ => todo!() // todo implement others
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
        let setting = param_or_prompt_value(&provided, 0, "Number of units to fire", 0, available_energy);
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