# Conway's Life

Original from David Ahl's _Basic Computer Games_, downloaded from http://www.vintage-basic.net/games.html.

Ported to Rust by Jon Fetter-Degges

Developed and tested on Rust 1.64.0

## How to Run

Install Rust using the instructions at [rust-lang.org](https://www.rust-lang.org/tools/install).

At a command or shell prompt in the `rust` subdirectory, enter `cargo run`.

## Differences from Original Behavior

* The simulation stops if all cells die.
* `.` at the beginning of an input line is supported but optional.
* Input of more than 66 columns is rejected. Input will automatically terminate after 20 rows. Beyond these bounds, the original
implementation would have marked the board as invalid, and beyond 68 cols/24 rows it would have had an out of bounds array access.
* The check for the string "DONE" at the end of input is case-independent.
* The program pauses for half a second between each generation.
