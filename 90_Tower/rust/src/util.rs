use crate::disk::Disk;

pub fn generate_disks(amount: u8) -> Vec<Disk> {
    if amount > 7 {
        println!("CANNOT HAVE MORE THAN 7 DISKS!");
    }

    let mut disks = Vec::new();

    for i in (1..=amount).rev() {
        disks.push(Disk::new(i * 2 + 1));
    }

    disks
}
