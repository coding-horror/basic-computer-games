use std::io;


fn print_instructions() {
    println!("TEST YOUR KNOWLEDGE OF CHILDREN'S LITERATURE.");
    println!();
    println!("THIS IS A MULTIPLE-CHOICE QUIZ.");
    println!("TYPE A 1, 2, 3, OR 4 AFTER THE QUESTION MARK.");
    println!();
    println!("GOOD LUCK!");
    println!();
    println!();
}


fn print_center(text: String, width: usize) {
    let pad_size;
    if width > text.len() {
        pad_size = (width - text.len()) / 2;
    } else {
        pad_size = 0;
    }
    println!("{}{}", " ".repeat(pad_size), text);
}


fn print_results(score: usize, number_of_questions: usize) {
    if score == number_of_questions {
        println!("WOW!  THAT'S SUPER!  YOU REALLY KNOW YOUR NURSERY");
        println!("YOUR NEXT QUIZ WILL BE ON 2ND CENTURY CHINESE");
        println!("LITERATURE (HA, HA, HA)");
    } else if score < number_of_questions / 2 {
        println!("UGH.  THAT WAS DEFINITELY NOT TOO SWIFT.  BACK TO");
        println!("NURSERY SCHOOL FOR YOU, MY FRIEND.");
    } else {
        println!("NOT BAD, BUT YOU MIGHT SPEND A LITTLE MORE TIME");
        println!("READING THE NURSERY GREATS.");
    }
}

fn main() {
    let page_width: usize = 64;

    struct Question<'a> {
        question: &'a str,
        choices: Vec<&'a str>,
        answer: u8,
        correct_response: &'a str,
        wrong_response: &'a str,
    }

    impl Question<'_>{
        fn ask(&self) -> bool {
            println!("{}", self.question);
            for i in 0..4 {
                print!("{}){}", i+1, self.choices[i]);
                if i != 3 { print!(", ")};
            }
            println!("");
            let mut user_input: String = String::new();
            io::stdin()
                .read_line(&mut user_input)
                .expect("Failed to read the line");

            if user_input.starts_with(&self.answer.to_string()) {
                println!("{}", self.correct_response);
                true
            } else {
                println!("{}", self.wrong_response);
                false
            }
        }
    }

    let questions: Vec<Question> = vec![
        Question{
            question: "IN PINOCCHIO, WHAT WAS THE NAME OF THE CAT?",
            choices: vec!["TIGGER", "CICERO", "FIGARO", "GUIPETTO"],
            answer: 3,
            wrong_response: "SORRY...FIGARO WAS HIS NAME.",
            correct_response: "VERY GOOD!  HERE'S ANOTHER.",
        },
        Question{
            question: "FROM WHOSE GARDEN DID BUGS BUNNY STEAL THE CARROTS?",
            choices: vec!["MR. NIXON'S", "ELMER FUDD'S", "CLEM JUDD'S", "STROMBOLI'S"],
            answer: 2,
            wrong_response: "TOO BAD...IT WAS ELMER FUDD'S GARDEN.",
            correct_response: "PRETTY GOOD!",
        },
        Question{
            question: "IN THE WIZARD OF OS, DOROTHY'S DOG WAS NAMED?",
            choices: vec!["CICERO", "TRIXIA", "KING", "TOTO"],
            answer: 4,
            wrong_response: "BACK TO THE BOOKS,...TOTO WAS HIS NAME.",
            correct_response: "YEA!  YOU'RE A REAL LITERATURE GIANT.",
        },
        Question{
            question: "WHO WAS THE FAIR MAIDEN WHO ATE THE POISON APPLE?",
            choices: vec!["SLEEPING BEAUTY", "CINDERELLA", "SNOW WHITE", "WENDY"],
            answer: 3,
            wrong_response: "OH, COME ON NOW...IT WAS SNOW WHITE.",
            correct_response: "GOOD MEMORY!",
        },    
    ];
    let number_of_questions: usize = questions.len();

    print_center("LITERATURE QUIZ".to_string(), page_width);
    print_center("CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY".to_string(), page_width);
    println!();
    println!();
    println!();
    print_instructions();

    let mut score = 0;
    for question in questions {
        if question.ask() {
            score += 1;
        }
        println!();
    }

    print_results(score, number_of_questions);

}



