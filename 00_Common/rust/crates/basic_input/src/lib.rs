//! A library for reading input from a basic program.
//!
//! In BASIC, you can have a statement like
//! ```basic
//! 10 INPUT "PROMPT";A$
//! ```
//! This crate emulates this with:
//! ```no_run
//! let a_value: String = basic_input::get_input("PROMPT");
//! ```
//!
//! BASIC also allows programmers to read multiple values at a time. For example, to
//! read two numbers, the program will have `INPUT "PROMPT";X1,Y1`. This can be done
//! in Rust with a tuple:
//! ```no_run
//! let (x1, y1): (f32, f32) = basic_input::get_input("Enter coordinates");
//! ```
//! The code will automatically re-prompt if the user doesn't enter all the values.
//!
//! Guided by the README.md under 00_Common, we know that there are `INPUT` statements
//! in the various games which ask for
//!
//! * 1 or 2 strings,
//! * 1-4 floating-point numbers, or 10(!) floating-point numbers
//!
//! and there are no other kinds of input required. All of these have been accounted
//! for in the `input` module (see `impl BasicInput for <type>`) but they are easy
//! enough to extend/enhance for different kinds of input.
//!
//! To use this in your Rust code, you can reference the local crate by adding the
//! following line to your `Cargo.toml`:
//!
//! ```toml
//! basic_input = { path = "../../00_Common/rust/crates/basic_input" }
//! ```
//!
//! (adjust the number of `../` for the depth of your own subdirectory)

pub mod input;
mod parse;

pub use input::*;
