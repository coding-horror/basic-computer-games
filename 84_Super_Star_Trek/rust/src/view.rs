use crate::model::{Galaxy, Pos, SectorStatus, Enterprise, systems};

pub mod prompts {
    pub const INSTRUCTIONS: &str = "Do you need instructions";
    pub const COURSE: &str = "Course (1-9)?";
    pub const TORPEDO_COURSE: &str = "Photon torpedo course (1-9)?";
    pub const SHIELDS: &str = "Number of units to shields";
    pub const REPAIR: &str = "Will you authorize the repair order";
    pub const COMPUTER: &str = "Computer active and waiting command?";
    pub const PHASERS: &str = "Number of units to fire";
    pub const WHEN_READY: &str = "Press Enter when ready to accept command";
    pub const COMMAND: &str = "Command?";

    pub fn warp_factor(max_warp: f32) -> String {
        format!("Warp Factor (0-{})?", max_warp)
    }
}

pub fn title() {
    println!("











          *************************************
          *                                   *
          *                                   *
          *      * * SUPER STAR TREK * *      *
          *                                   *
          *                                   *
          *************************************









    ");
}

pub fn full_instructions() {
    println!(
"        INSTRUCTIONS FOR 'SUPER STAR TREK'

    1. When you see \"Command ?\" printed, enter one of the legal
         commands (NAV, SRS, LRS, PHA, TOR, SHE, DAM, COM, OR XXX).
    2. If you should type in an illegal command, you'll get a short
         list of the legal commands printed out.
    3. Some commands require you to enter data (for example, the
         'NAV' command comes back with 'Course (1-9) ?'.)  If you
         type in illegal data (like negative numbers), then command
         will be aborted.
    
         The galaxy is divided into an 8 X 8 quadrant grid,
    and each quadrant is further divided into an 8 X 8 sector grid.
    
         You will be assigned a starting point somewhere in the
    galaxy to begin a tour of duty as commander of the starship
    Enterprise; your mission: to seek and destroy the fleet of
    Klingon warships which are menacing the United Federation of
    Planets.
    
         You have the following commands available to you as captain
    of the starship Enterprise:
    
    NAV command = Warp Engine Control
         Course is in a circular numerical      4  3  2
         vector arrangement as shown             . . .
         integer and real values may be           ...
         used.  (Thus course 1.5 is half-     5 ---*--- 1
         way between 1 and 2.                     ...
                                                 . . .
         Values may approach 9.0, which         6  7  8
         itself is equivalent to 1.0
                                                COURSE
         One warp factor is the size of
         one quadrant.  Therefore, to get
         from quadrant 6,5 to 5,5, you WOULD
         use course 3, warp factor 1.
    
    SRS command = Short Range Sensor Scan
         Shows you a scan of your present quadrant.
    
         Symbology on your sensor screen is as follows:
            <*> = Your starship's position
            +K+ = Klingon battle cruiser
            >!< = Federation starbase (refuel/repair/re-arm here!)
             *  = Star
    
         A condensed 'status report' will also be presented.
    
    LRS command = Long Range Sensor Scan
         Shows conditions in space for one quadrant on each side
         of the Enterprise (which is in the middle of the scan).
         The scan is coded in the form ###, where the units digit
         is the number of stars, the tens digit is the number of
         starbases, and the hundreds digit is the number of
         Klingons.
    
         Example - 207 = 2 Klingons, No starbases, & 7 stars.
    
    PHA command = Phaser Control
         Allows you to destroy the Klingon battle cruisers by
         zapping them with suitably large units of energy to
         deplete their shield power.  (Remember, Klingons have
         phasers, too!)
    
    TOR command = Photon Torpedo Control
         Torpedo course is the same as used in warp engine control.
         If you hit the Klingon vessel, he is destroyed and
         cannot fire back at you.  If you miss, you are subject to
         his phaser fire.  In either case, you are also subject to
         the phaser fire of all other Klingons in the quadrant.
    
         The library-computer (COM command) has an option to
         compute torpedo trajectory for you (Option 2).
    
    SHE command = Shield Control
         Defines the number of energy units to be assigned to the
         shields.  Energy is taken from total ship's energy.  Note
         that the status display total energy includes shield energy.
    
    DAM command = Damage Control Report
         Gives the state of repair of all devices.  Where a negative
         'state of repair' shows that the device is temporarily
         damaged.
    
    COM command = Library-Computer
         The library-computer contains six options:
         Option 0 = Cumulative Galactic Record
            This option shows computer memory of the results of all
            previous short and long range sensor scans.
         Option 1 = Status Report
            This option shows the number of Klingons, Stardates,
            and starbases remaining in the game.
         Option 2 = Photon Torpedo Data
            Which gives directions and distance from the Enterprise
            to all Klingons in your quadrant.
         Option 3 = Starbase Nav Data
            This option gives direction and distance to any
            starbase within your quadrant.
         Option 4 = Direction/Distance Calculator
            This option allows you to enter coordinates for
            direction/distance calculations.
         Option 5 = Galactic Region Name Map
            This option prints the names of the sixteen major
            galactic regions referred to in the game.

")
}

pub fn enterprise() {
    println!("










                                    ,------*------,
                    ,-------------   '---  ------'
                     '-------- --'      / /
                         ,---' '-------/ /--,
                          '----------------'

                    THE USS ENTERPRISE --- NCC-1701






")
}

pub fn intro(model: &Galaxy) {
    let star_bases = model.remaining_starbases();
    let mut star_base_message: String = "There is 1 starbase".into();
    if star_bases > 1 {
        star_base_message = format!("There are {} starbases", star_bases);
    }
    println!(
"Your orders are as follows:
    Destroy the {} Klingon warships which have invaded
    the galaxy before they can attack federation headquarters
    on stardate {}. This gives you {} days. {} in the galaxy for resupplying your ship.\n", 
    model.remaining_klingons(), model.final_stardate, model.final_stardate - model.stardate, star_base_message)
}

const REGION_NAMES: [&str; 16] = [
    "Antares",
    "Sirius",
    "Rigel",
    "Deneb",
    "Procyon",
    "Capella",
    "Vega",
    "Betelgeuse",
    "Canopus",
    "Aldebaran",
    "Altair",
    "Regulus",
    "Sagittarius",
    "Arcturus",
    "Pollux",
    "Spica"
];

const SUB_REGION_NAMES: [&str; 4] = ["I", "II", "III", "IV"];

fn quadrant_name(quadrant: Pos) -> String {
    format!("{} {}", 
        REGION_NAMES[((quadrant.1 << 1) + (quadrant.0 >> 2)) as usize],
        SUB_REGION_NAMES[(quadrant.1 % 4) as usize])
}

pub fn starting_quadrant(quadrant: Pos) {
    println!(
"\nYour mission begins with your starship located
in the galactic quadrant, '{}'.\n", quadrant_name(quadrant))
}

pub fn enter_quadrant(quadrant: Pos) {
    println!("\nNow entering {} quadrant . . .\n", quadrant_name(quadrant))
}

pub fn short_range_scan(model: &Galaxy) {
    let quadrant = &model.quadrants[model.enterprise.quadrant.as_index()];
    let mut condition = "GREEN";
    if quadrant.docked_at_starbase(model.enterprise.sector) {
        println!("Shields dropped for docking purposes");
        condition = "DOCKED";
    } else if quadrant.klingons.len() > 0 {
        condition = "*RED*";
    } else if model.enterprise.damaged.len() > 0 {
        condition = "YELLOW";
    }

    let data : [String; 8] = [
        format!("Stardate           {}", model.stardate),
        format!("Condition          {}", condition),
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
                match quadrant.sector_status(pos) {
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
    println!(
"Enter one of the following:
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
    println!(
"Is is stardate {}.
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

pub fn bad_torpedo_course() {
    println!("   Ensign Chekov reports, 'Incorrect course data, sir!'")
}

pub fn enterprise_hit(hit_strength: &u16, from_sector: &Pos) {
    println!("{hit_strength} unit hit on Enterprise from sector {from_sector}");
}

pub fn hit_edge(quadrant: Pos, sector: Pos) {
    println!(
"Lt. Uhura report message from Starfleet Command:
    'Permission to attempt crossing of galactic perimeter
        is hereby *Denied*. Shut down your engines.'
    Chief Engineer Scott reports, 'Warp engines shut down
        at sector {} of quadrant {}.'", sector, quadrant);
}

pub fn condition_red() {
    println!("COMBAT AREA      CONDITION RED")
}

pub fn danger_shields() {
    println!("   SHIELDS DANGEROUSLY LOW    ")
}

pub fn insuffient_warp_energy(warp_speed: f32) {
    println!(
"Engineering reports, 'Insufficient energy available
    for maneuvering at warp {warp_speed} !'")
}

pub fn divert_energy_from_shields() {
    println!("Shield Control supplies energy to complete the maneuver.")
}

pub fn energy_available(total_energy: u16) {
    println!("Energy available = {{{total_energy}}}")
}

pub fn shields_unchanged() {
    println!("<SHIELDS UNCHANGED>")
}

pub fn ridiculous() {
    println!("Shield Control reports, 'This is not the Federation Treasury.'")
}

pub fn shields_set(value: u16) {
    println!(
"Deflector control room report:
  'Shields now at {value} units per your command.'")
}

pub fn shields_hit(shields: u16) {
    println!("      <Shields down to {shields} units>")
}

pub fn inoperable(arg: &str) {
    println!("{} inoperable", arg)
}

pub fn scanners_out() {
    println!("*** Short Range Sensors are out ***")
}

pub fn damaged_engines(max_warp: f32, warp_factor: f32) {
    println!(
"Warp engines are damaged.  Maximum speed = warp {max_warp}
    Chief Engineer Scott reports, 'The engines won't take warp {warp_factor} !'")
}

pub fn damage_control_report() {
    println!("Damage Control report:")
}

pub fn random_repair_report_for(name: &str, damaged: bool) {
    let mut message = "state of repair improved";
    if damaged {
        message = "damaged";
    }
    println!("Damage Control report:  {name} {message}")
}

pub fn system_repair_completed(name: String) {
    println!("        {name} repair completed.")
}

pub fn damage_control(enterprise: &Enterprise) {
    println!("Device             State of Repair");
    for key in systems::KEYS {
        let damage = enterprise.damaged.get(key).unwrap_or(&0.0);
        println!("{:<25}{}", systems::name_for(key), damage)
    }
    println!();
}

pub fn long_range_scan(galaxy: &Galaxy) -> Vec<Pos> {

    let cx = galaxy.enterprise.quadrant.0 as i8;
    let cy = galaxy.enterprise.quadrant.1 as i8;

    let mut seen = Vec::new();

    println!("Long range scan for quadrant {}", galaxy.enterprise.quadrant);
    println!("{:-^19}", "");
    for y in cy - 1..=cy + 1 {
        for x in cx - 1..=cx + 1 {
            let mut klingons = "*".into();
            let mut star_bases = "*".into();
            let mut stars = "*".into();

            if y >= 0 && y < 8 && x >= 0 && x < 8 {
                let pos = Pos(x as u8, y as u8);
                seen.push(pos);
                
                let quadrant = &galaxy.quadrants[pos.as_index()];
                klingons = format!("{}", quadrant.klingons.len());
                star_bases = quadrant.star_base.as_ref().map_or("0", |_| "1");
                stars = format!("{}", quadrant.stars.len());
            }

            print!(": {}{}{} ", klingons, stars, star_bases)
        }
        println!(":");
        println!("{:-^19}", "");
    } 

    seen
}

pub fn stranded() {
    println!(
"** FATAL ERROR **   You've just stranded your ship in space
You have insufficient maneuvering energy, and shield control
is presently incapable of cross-circuiting to engine room!!")
}

pub fn computer_options() {
    println!(
"   0 = Cumulative galactic record
    1 = Status report
    2 = Photon torpedo data
    3 = Starbase nav data
    4 = Direction/distance calculator
    5 = Galaxy 'region name' map")
}

pub fn galaxy_region_map() {
    println!(
"                        The Galaxy
      1     2     3     4     5     6     7     8
    ----- ----- ----- ----- ----- ----- ----- -----");
    for i in (0..REGION_NAMES.len()-1).step_by(2) {
        println!(
"{} {:^23} {:^23}
    ----- ----- ----- ----- ----- ----- ----- -----", (i/2)+1, REGION_NAMES[i], REGION_NAMES[i+1]);
    }    
}

pub fn galaxy_scanned_map(galaxy: &Galaxy) {
    println!(
"Computer record of galaxy for quadrant {}
      1     2     3     4     5     6     7     8
    ----- ----- ----- ----- ----- ----- ----- -----", galaxy.enterprise.quadrant);
    for y in 0..8 {
        print!("{}   ", y+1);
        for x in 0..8 {
            let pos = Pos(x, y);
            if galaxy.scanned.contains(&pos) {
                let quadrant = &galaxy.quadrants[pos.as_index()];
                print!(" {}{}{}  ", quadrant.klingons.len(), quadrant.stars.len(), quadrant.star_base.as_ref().map_or("0", |_| "1"))
            } else {
                print!(" ***  ");
            }
        }
        println!(
"\n    ----- ----- ----- ----- ----- ----- ----- -----")
    }
}

pub fn no_local_enemies() {
    println!(
"Science Officer Spock reports, 'Sensors show no enemy ships
                                 in this quadrant'")
}

pub fn computer_accuracy_issue() {
    println!("Computer failure hampers accuracy")
}

pub fn phasers_locked(available_energy: u16) {
    println!("Phasers locked on target;  Energy available = {available_energy} units")
}

pub fn starbase_shields() {
    println!("Starbase shields protect the Enterprise")
}

pub fn repair_estimate(repair_time: f32) {
    println!(
"Technicians standing by to effect repairs to your ship;
Estimated time to repair: {repair_time} stardates.")
}

pub fn no_damage(sector: Pos) {
    println!("Sensors show no damage to enemy at {sector}")
}

pub fn hit_on_klingon(hit_strength: f32, sector: Pos) {
    println!("{hit_strength} unit hit on Klingon at sector {sector}")
}

pub fn klingon_remaining_energy(energy: f32) {
    println!("   (sensors show {energy} units remaining)")
}

pub fn klingon_destroyed() {
    println!("*** Klingon destroyed ***")
}

pub fn congratulations(efficiency: f32) {
    println!("
Congratulations, Captain!  The last Klingon battle cruiser
menacing the Federation has been destroyed.

Your efficiency rating is {efficiency}.
    ")
}

pub fn replay() {
    println!("
The Federation is in need of a new starship commander
for a similar mission -- if there is a volunteer
let him step forward and enter 'Aye'")
}

pub fn no_torpedoes_remaining() {
    println!("All photon torpedoes expended")
}

pub fn torpedo_track() {
    println!("Torpedo track:")
}

pub fn torpedo_path(sector: Pos) {
    println!("{:<16}{}", "", sector)
}

pub fn torpedo_missed() {
    println!("Torpedo missed!")
}

pub fn star_absorbed_torpedo(sector: Pos) {
    println!("Star at {sector} absorbed torpedo energy.")
}

pub fn destroyed_starbase(not_the_last_starbase: bool) {
    println!("*** Starbase destroyed ***");
    if not_the_last_starbase {
        println!("
Starfleet Command reviewing your record to consider
court martial!")
    } else {
        println!("
That does it, Captain!!  You are hereby relieved of command
and sentenced to 99 stardates at hard labor on Cygnus 12!!")
    }
}