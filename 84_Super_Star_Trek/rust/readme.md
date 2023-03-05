# Super Star Trek - Rust version

Explanation of modules:

- main.rs - creates the galaxy (generation functions are in model.rs as impl methods) then loops listening for commands. after each command checks for victory or defeat condtions.
- model.rs - all the structs and enums that represent the galaxy. key methods in here (as impl methods) are generation functions on galaxy and quadrant, and various comparison methods on the 'Pos' tuple type.
- commands.rs - most of the code that implements instructions given by the player (some code logic is in the model impls, and some in view.rs if its view only).
- view.rs - all text printed to the output, mostly called by command.rs (like view::bad_nav for example). also contains the prompts printed to the user (e.g. view::prompts::COMMAND).
- input.rs - utility methods for getting input from the user, including logic for parsing numbers, repeating prompts until a correct value is provided etc.

