# Super Star Trek - Rust version

Explanation of modules:

- [main.rs](./src/main.rs) - creates the galaxy (generation functions are in model.rs as impl methods) then loops listening for commands. after each command checks for victory or defeat condtions.
- [model.rs](./src/model.rs) - all the structs and enums that represent the galaxy. key methods in here (as impl methods) are generation functions on galaxy and quadrant, and various comparison methods on the 'Pos' tuple type.
- [commands.rs](./src/commands.rs) - most of the code that implements instructions given by the player (some code logic is in the model impls, and some in view.rs if its view only).
- [view.rs](./src/view.rs) - all text printed to the output, mostly called by command.rs (like view::bad_nav for example). also contains the prompts printed to the user (e.g. view::prompts::COMMAND).
- [input.rs](./src/input.rs) - utility methods for getting input from the user, including logic for parsing numbers, repeating prompts until a correct value is provided etc.

Basically the user is asked for the next command, this runs a function that usually checks if the command system is working, and if so will gather additional input (see next note for a slight change here), then either the model is read and info printed, or its mutated in some way (e.g. firing a torpedo, which reduces the torpedo count on the enterprise and can destroy klingons and star bases; finally the klingons fire back and can destroy the enterprise). Finally the win/lose conditions are checked before the loop repeats.

## Changes from the original

I have tried to keep it as close as possible. Notable changes are:

- commands can be given with parameters in line. e.g. while 'nav' will ask for course and then warp speed in the original, here you can *optionally* also do this as one line, e.g. `nav 1 0.1` to move one sector east. I'm sorry - it was driving me insane in its original form (which is still sorted, as is partial application e.g. nav 1 to preset direction and then provide speed).
- text is mostly not uppercase, as text was in the basic version. this would be easy to change however as all text is in view.rs, but I chose not to.
- the navigation system (plotting direction, paths and collision detection) is as close as I could make it to the basic version (by using other language conversions as specification sources) but I suspect is not perfect. seems to work well enough however.