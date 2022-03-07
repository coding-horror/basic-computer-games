use std::fs::{self, DirEntry, ReadDir, metadata};
use std::fs::{File, OpenOptions};
use std::io;
use std::io::prelude::*;
use std::path::{Path, PathBuf};


//DATA
const ROOT_DIR: &str = "../../";
const LANGUAGES: [(&str,&str); 9] = [ //first element of tuple is the language name, second element is the file extension
    ("csharp", "cs"),
    ("java", "java"),
    ("javascript", "html"),
    ("pascal", "pas"),
    ("perl", "pl"),
    ("python", "py"),
    ("ruby", "rb"),
    ("rust", "rs"),
    ("vbnet", "vb")
];
const INGORE: [&str;4] = ["../../.git","../../.vscode","../../00_Utilities","../../buildJvm"]; //folders to ignore

fn main() {
    //DATA
    let langs_to_print: Vec<&str>;
    let mut root_folders:Vec<_>;

    //print language - extension table
    print_lang_extension_table();

    //prompt user to input the file extensions of the languages they want the todo-lists for printed to console
    println!("\na todo list with all the languages will be put into todo-list.md");
    println!("enter the file extensions from the table above for the languages you want a todo-list printed to standard output");
    println!("File extensions: (separated by spaces)");
    let mut raw_input = String::new();
    io::stdin().read_line(&mut raw_input).expect("Failed to read input");
    //parse input for valid languages, store them in langs_to_print
    let raw_input = raw_input.as_str().trim().chars().filter(|c| c.is_ascii_alphabetic() || c.is_whitespace()).collect::<String>();//filter out everything but ascii letters and spaces
    langs_to_print =  raw_input.split(' ')//create an iterator with all the words
    .filter(|s| LANGUAGES.iter().any(|tup| tup.1.eq_ignore_ascii_case(*s))) //filter out words that aren't valid extensions (in languages)
    .collect(); //collect words into vector
    println!("\nwill print: {:?}", langs_to_print);

    //get all folders in ROOT_DIR
    match fs::read_dir(ROOT_DIR) {
        Err(why) => {
            println!("! {:?}", why.kind());
            panic!("error");
        },
        Ok(paths) => {
            root_folders = Vec::new();
            for path in paths {
                let full_path = path.unwrap().path();
                if metadata(&full_path).unwrap().is_dir() {
                    println!("> {:?}", full_path);
                    root_folders.push(full_path);
                }
            }
        },
    }

    //for all folders, search for the languages and extensions
    /*
    // Read the contents of a directory, returns `io::Result<Vec<Path>>`
    match fs::read_dir(ROOT_DIR) {
        Err(why) => println!("! {:?}", why.kind()),
        Ok(paths) => for path in paths {
            println!("> {:?}", path.unwrap().path());
        },
    }
    */
}

/**
 * print language - extension table
 */
fn print_lang_extension_table() {
    println!("LANGUAGE\tFILE EXTENSION");
    println!("========\t==============");
    for tup in LANGUAGES.iter() {
        match tup.0 {
            "javascript" => println!("{}\t{}", tup.0,tup.1), //long ones
            _ => println!("{}\t\t{}", tup.0,tup.1),
        };
    }
}

/**
 * returns a vector containing paths to all files in path and subdirectories of path
 */
fn list_files(path: &Path) -> Vec<PathBuf> {
    let mut vec = Vec::new();
    _list_files(&mut vec,&path);
    vec
}
fn _list_files(vec: &mut Vec<PathBuf>, path: &Path) {
    if metadata(&path).unwrap().is_dir() {
        let paths = fs::read_dir(&path).unwrap();
        for path_result in paths {
            let full_path = path_result.unwrap().path();
            if metadata(&full_path).unwrap().is_dir() {
                _list_files(vec, &full_path);
            } else {
                vec.push(full_path);
            }
        }
    }
}

