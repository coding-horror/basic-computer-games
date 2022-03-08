use std::ffi::{OsString, OsStr};
/**
 * todo list generator for this repository, coded in rust
 * 
 * @author Anthony Rubick 
 */

use std::fs;
use std::fs::metadata;
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
const OUTPUT_PATH: &str = "../../todo.md";
//const INGORE: [&str;5] = ["../../.git","../../.vscode","../../00_Utilities","../../buildJvm","../../node_modules"]; //folders to ignore

fn main() {
    //DATA
    let mut root_folders:Vec<PathBuf>;
    let mut output_string_1: String = String::new();
    //let mut game_list:Vec<_>;

    //print language - extension table
    print_lang_extension_table();

    //prompt user to input the file extensions of the languages they want the todo-lists for printed to console
    /*
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
    */

    //get all folders in ROOT_DIR
    root_folders = Vec::new();
    match fs::read_dir(ROOT_DIR) {
        Err(why) => {
            println!("! {:?}", why.kind());
            panic!("error");
        },
        Ok(paths) => {
            for path in paths {
                let full_path = path.unwrap().path();
                if metadata(&full_path).unwrap().is_dir() {
                    root_folders.push(full_path);
                }
            }
        },
    }

    //for i in root_folders {println!("> {:?}", &i);}

    //for all folders, search for the languages and extensions
    root_folders = root_folders.into_iter().filter(|path| {
        match fs::read_dir(path) {
            Err(why) => {println!("! {:?}", why.kind()); false},
            Ok(paths) => {
                paths.into_iter().filter(|f| metadata(f.as_ref().unwrap().path()).unwrap().is_dir()) //filter to only folders
                .filter_map( |path| path.ok() ) //extract only the DirEntries
                .any(|f| LANGUAGES.iter().any(|tup| OsString::from(tup.1).eq_ignore_ascii_case(f.file_name()))) //filter out ones that don't contain folders with the language names
            }
        }
    }).collect();

    //being forming output string
    // for every game
    //      every language + ✅/⬜️
    for g in root_folders.into_iter() {
        let game = g.clone();
        output_string_1 += format!(
            "{}\n{}\n", //message format
            g.into_os_string().into_string().unwrap().chars().filter(|c| !(*c=='.' || *c=='/')).collect::<String>(),//get the game name
            {
                let mut s:String = String::new();
                //every language + ✅/⬜️
                LANGUAGES.iter().for_each(|lang| {
                    s += lang.0;
                    // + ✅/⬜️
                    let paths = list_files(&game).into_iter().map(|path| path.into_os_string().into_string().unwrap());
                    let paths:Vec<String> = paths.into_iter().filter_map(|s| {
                        match  Path::new(s.as_str()).extension().and_then(OsStr::to_str) {
                            None => None,
                            Some(s) => Some(s.to_string()),
                        }
                    }).collect(); //get all the extensions
                    if paths.into_iter().any(|f| f.eq(lang.1)) {//list_files(&game).iter().map(|path| path.into_os_string().into_string().unwrap()).any(|s| LANGUAGES.iter().map(|tup| Some(tup.1)).collect::<Vec<_>>().contains(&s.split('.').next())) {
                        s+="✅";
                    } else {s += "⬜️";}

                    s += "\n";
                });
                s
            }
        ).as_str();
    }
    //print output, and write output to file
    println!("\n\n{}", output_string_1);
    fs::write(OUTPUT_PATH, output_string_1).expect("failed to write todo list");
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

