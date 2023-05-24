use std::ffi::OsStr;
use std::{fs, io};
use std::fs::metadata;
use std::path::{Path, PathBuf};
/**
 * todo list generator for this repository, coded in rust
 *
 * @author Anthony Rubick
 */

//DATA
const ROOT_DIR: &str = "../../";
const LANGUAGES: [(&str,&str); 10] = [ //first element of tuple is the language name, second element is the file extension
    ("csharp", "cs"),
    ("java", "java"),
    ("javascript", "html"),
    ("kotlin", "kt"),
    ("lua", "lua"),
    ("perl", "pl"),
    ("python", "py"),
    ("ruby", "rb"),
    ("rust", "rs"),
    ("vbnet", "vb")
];
const OUTPUT_PATH: &str = "../../todo.md";

fn main() {
    //DATA
    let mut root_folders:Vec<PathBuf>;
    let mut output_string: String = String::new();
    let format_game_first: bool;
    let ingore: [PathBuf;5] = [
        PathBuf::from(r"../../.git"),
        PathBuf::from(r"../../.github"),
        PathBuf::from(r"../../00_Alternate_Languages"),
        PathBuf::from(r"../../00_Utilities"),
        PathBuf::from(r"../../00_Common"),
    ]; //folders to ignore

    //print welcome message
    println!("
                             Markdown TODO list maker
               by Anthony Rubick for the basic-computer-games repo


    ");


    //ask user how they want the todo list formatted
    format_game_first = get_yn_from_user("\n\t---====FORMATS====---\ngame first:\n\tGame\n\t\tLanguage ✅/⬜️\n\nlang first:\n\tLanguage\n\t\tGame ✅/⬜️\n\nmake todo list using the game first format? (y/n | default No) ");

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
        //not one of the ignored folders
        !ingore.contains(path)
    }).collect();
    root_folders.sort();

    //create todo list
    if format_game_first {
        //being forming output string
        // for every game
        //      every language + ✅/⬜️
        for g in root_folders.into_iter() {
            //DATA
            let mut num_done:u8 = 0; //number of languages implemented
            let mut total:u8 = 0; //number of languages

            let game = g.clone().into_os_string().into_string().unwrap().chars().filter(|c| !(*c=='.' || *c=='/')).collect::<String>();//get the game name


            //find all the lanuages the game is coded in
            let languages_list_for_game = {
                let mut s:String = String::new();
                //every language + ✅/⬜️
                LANGUAGES.iter().for_each(|lang| {
                    s+="- ";
                    s += lang.0;
                    // + ✅/⬜️
                    let paths:Vec<_> = list_files(&g).into_iter().map(|path| path.into_os_string().into_string().unwrap()).collect();
                    let paths:Vec<String> = paths.into_iter().filter_map(|s| {
                        match  Path::new(s.as_str()).extension().and_then(OsStr::to_str) {
                            None => None,
                            Some(s) => Some(s.to_string()),
                        }
                    }).collect(); //get all the extensions
                    if paths.into_iter().any(|f| f.eq(lang.1)) {//list_files(&game).iter().map(|path| path.into_os_string().into_string().unwrap()).any(|s| LANGUAGES.iter().map(|tup| Some(tup.1)).collect::<Vec<_>>().contains(&s.split('.').next())) {
                        s+="✅";
                        num_done += 1; //increment number done
                    } else {
                        s += "⬜️";  
                    }
                    total += 1; //increment total

                    s += "\n";
                });
                s
            };

            //format message
            output_string += format!(
                "### {}\t{}\n\n{}\n", //message format
                game,
                format!("coded in {}/{} languages", num_done, total),
                languages_list_for_game,
            ).as_str();
        }

        //print the whole list
        println!("\n\n{}", output_string);
    }
    else {
        //figure out what langauges the user wants printed

        //print language - extension table
        println!("\n\t---====LANGUAGES TO PRINT====---\na complete todo list will be output to todo-list.md\n");
        print_lang_extension_table();

        //prompt user to input the file extensions of the languages they want the todo-lists for printed to console
        //parse input for valid languages, store them in langs_to_print
        let langs_to_print = get_str_from_user("\nenter the file extensions, from the table above,\nfor the languages you want printed to standard output\nFile extensions: (separated by spaces)")
        .chars().filter(|c| c.is_ascii_alphabetic() || c.is_whitespace()).collect::<String>();//filter out everything but ascii letters and spaces
        let langs_to_print: Vec<_> = langs_to_print.split(' ')//create an iterator with all the words
        .filter(|s| LANGUAGES.iter().any(|tup| tup.1.eq_ignore_ascii_case(*s))) //filter out words that aren't valid extensions (in languages)
        .collect(); //collect words into vector
        println!("\nwill print: {:#?}\n\n", langs_to_print);


        //being forming output string
        // for every language
        //      every game + ✅/⬜️
        for lang in LANGUAGES.iter() {
            //DATA
            let mut num_done = 0;
            let mut total = 0;
            let games_list_for_language = { //list
                //DATA
                let mut s:String = String::new();

                //go through every game and check if it's coded in the language
                for g in (&root_folders).into_iter() {
                    //data
                    let game = g.clone();
                    let game_name = game.into_os_string().into_string().unwrap().chars().filter(|c| !(*c=='.' || *c=='/')).collect::<String>(); //get the game name
                    let paths:Vec<_> = list_files(g).into_iter().map(|path| path.into_os_string().into_string().unwrap()).collect(); //all subpaths of game
                    let paths:Vec<String> = paths.into_iter().filter_map(|s| {
                        match  Path::new(s.as_str()).extension().and_then(OsStr::to_str) {
                            None => None,
                            Some(s) => Some(s.to_string()),
                        }
                    }).collect(); //get all the extensions

                    //add game name
                    s+="- ";
                    s+= game_name.as_str();

                    //every game + ✅/⬜️
                    if paths.into_iter().any(|f| f.eq(lang.1)) {
                        s+="✅";
                        num_done += 1; //increment num done
                    } else {
                        s += "⬜️";
                    }
                    total += 1; //increment total
                    
                    //new line
                    s += "\n";
                }

                //print desired languages only, what's output to std_out, also not markdown formatted
                if langs_to_print.contains(&lang.1) {
                    print!("###  {}\t{}  ###\n{}\n\n", lang.0, format!("{}/{} games ported", num_done, total), s);
                }
                s
            };

            //format message, what's output to the file
            output_string += format!(
                "### {}\t{}\n\n{}\n\n", //message format
                lang.0, //language
                format!("{}/{} games ported", num_done, total),//number done / total number
                games_list_for_language,
            ).as_str();
        }
    }

    //write output to file
    fs::write(OUTPUT_PATH, output_string).expect("failed to write todo list");
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
 * gets a string from user input
 */
fn get_str_from_user(prompt:&str) -> String {
    //DATA
    let mut raw_input = String::new();

    //print prompt
    println!("{}",prompt);

    //get input and trim whitespaces
    io::stdin().read_line(&mut raw_input).expect("Failed to read input");

    //return raw input
    return raw_input.trim().to_string();
}
/**
 * gets a boolean from user
 */
fn get_yn_from_user(prompt:&str) -> bool {
    //DATA
    let input = get_str_from_user(prompt);

    //default in case of error
    if input.is_empty() {return false;}

    //get and parse input
    match &input[0..1] { //get first character
        "y" | "Y" => return true,
        _ => return false,
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
