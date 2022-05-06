use crate::disk::Disk;

pub fn generate_disks(amount: u8) -> Vec<Disk> {
    if amount > 7 {
        println!("CANNOT HAVE MORE THAN 7 DISKS!");
    }

    // check for if amount == 0

    let mut disks = Vec::new();

    let mut half_size = 7;
    for _ in (1..=amount).rev() {
        disks.push(Disk::new(half_size * 2 + 1));
        half_size -= 1;
    }

    disks
}
