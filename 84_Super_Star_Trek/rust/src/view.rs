use crate::model::{Galaxy, Pos, EndPosition, SectorStatus};

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

pub fn print_command_help() {
    println!("Enter one of the following:
  NAV  (To set course)
  SRS  (For short range sensor scan)
  LRS  (For long range sensor scan)
  PHA  (To fire phasers)
  TOR  (To fire photon torpedoes)
  SHE  (To raise or lower shields)
  DAM  (For damage control reports)
  COM  (To call on library-computer)
  XXX  (To resign your command)
    ")
}

pub fn end_game_failure(galaxy: &Galaxy) {
    println!("Is is stardate {}.
There were {} Klingon battle cruisers left at
the end of your mission.
", galaxy.stardate, galaxy.remaining_klingons());
}

pub fn enterprise_destroyed() {
    println!("The Enterprise has been destroyed.  The Federation will be conquered.");
}

pub fn bad_nav() {
    println!("   Lt. Sulu reports, 'Incorrect course data, sir!'")
}

pub fn enterprise_hit(hit_strength: &u16, from_sector: &Pos) {
    println!("{hit_strength} unit hit on Enterprise from sector {from_sector}");
}

pub fn hit_edge(end: &EndPosition) {
    println!("Lt. Uhura report message from Starfleet Command:
        'Permission to attempt crossing of galactic perimeter
        is hereby *Denied*. Shut down your engines.'
      Chief Engineer Scott reports, 'Warp engines shut down
        at sector {} of quadrant {}.'", end.quadrant, end.sector);
}
