use std::io;

fn show_intro() {
    // Displays the intro text
    println!("\n                  Love");
    println!("Creative Computing  Morristown, New Jersey");
    println!("\n\n");
    println!("A tribute to the great American artist, Robert Indiana.");
    println!("His great work will be reproduced with a message of");
    println!("your choice up to 60 characters.  If you can't think of");
    println!("a message, simple type the word 'love'\n"); // (sic)
}

fn main() {
    enum PrintOrPass {
        Print,
        Pass,
    }

    let data = [
        vec![60],
        vec![1, 12, 26, 9, 12],
        vec![3, 8, 24, 17, 8],
        vec![4, 6, 23, 21, 6],
        vec![4, 6, 22, 12, 5, 6, 5],
        vec![4, 6, 21, 11, 8, 6, 4],
        vec![4, 6, 21, 10, 10, 5, 4],
        vec![4, 6, 21, 9, 11, 5, 4],
        vec![4, 6, 21, 8, 11, 6, 4],
        vec![4, 6, 21, 7, 11, 7, 4],
        vec![4, 6, 21, 6, 11, 8, 4],
        vec![4, 6, 19, 1, 1, 5, 11, 9, 4],
        vec![4, 6, 19, 1, 1, 5, 10, 10, 4],
        vec![4, 6, 18, 2, 1, 6, 8, 11, 4],
        vec![4, 6, 17, 3, 1, 7, 5, 13, 4],
        vec![4, 6, 15, 5, 2, 23, 5],
        vec![1, 29, 5, 17, 8],
        vec![1, 29, 9, 9, 12],
        vec![1, 13, 5, 40, 1],
        vec![1, 13, 5, 40, 1],
        vec![4, 6, 13, 3, 10, 6, 12, 5, 1],
        vec![5, 6, 11, 3, 11, 6, 14, 3, 1],
        vec![5, 6, 11, 3, 11, 6, 15, 2, 1],
        vec![6, 6, 9, 3, 12, 6, 16, 1, 1],
        vec![6, 6, 9, 3, 12, 6, 7, 1, 10],
        vec![7, 6, 7, 3, 13, 6, 6, 2, 10],
        vec![7, 6, 7, 3, 13, 14, 10],
        vec![8, 6, 5, 3, 14, 6, 6, 2, 10],
        vec![8, 6, 5, 3, 14, 6, 7, 1, 10],
        vec![9, 6, 3, 3, 15, 6, 16, 1, 1],
        vec![9, 6, 3, 3, 15, 6, 15, 2, 1],
        vec![10, 6, 1, 3, 16, 6, 14, 3, 1],
        vec![10, 10, 16, 6, 12, 5, 1],
        vec![11, 8, 13, 27, 1],
        vec![11, 8, 13, 27, 1],
        vec![60],
    ];

    const ROW_LEN: usize = 60;
    show_intro();

    let mut input: String = String::new();
    io::stdin().read_line(&mut input).expect("No valid input");
    let input = if input.len() == 1 {
        "LOVE"
    } else {
        input.trim()
    };
    // repeat the answer to fill the whole line, we will show chunks of this when needed
    let input = input.repeat(ROW_LEN / (input.len()) + 1);

    // Now lets display the Love
    print!("{}", "\n".repeat(9));
    for row in data {
        let mut print_or_pass = PrintOrPass::Print;
        let mut current_start = 0;
        for count in row {
            match print_or_pass {
                PrintOrPass::Print => {
                    print!("{}", &input[current_start..count + current_start]);
                    print_or_pass = PrintOrPass::Pass;
                }
                PrintOrPass::Pass => {
                    print!("{}", " ".repeat(count));
                    print_or_pass = PrintOrPass::Print;
                }
            }
            current_start += count;
        }
        println!();
    }
}
