mod input_state;
mod io_layer;

use input_state::InputState;
use io_layer::IoLayer;

/// Main entrypoint for this module. Display a prompt and read one or more values from the user.
/// See [the crate docs](module@crate)
pub fn get_input<BI: BasicInput, S: AsRef<str>>(prompt: S) -> BI {
    let mut io = IoLayer::get_io();
    get_input_from_io(prompt, &mut io)
}

// This is the trait that we return to the user, which allows us to have
// a different function for each return type.
// let one_input : f32 = get_input("...");
// let (x, y) : (f32, f32) = get_input("...");
// let (a, b, c) : (f32, f32, f32) = get_input("...");
pub trait BasicInput {
    fn from_string<S: AsRef<str>>(s: S, state: &mut InputState) -> Result<Self, BasicInputError>
    where
        Self: Sized;
}

// --------------------------------------------------------------------------------

impl BasicInput for f32 {
    fn from_string<S: AsRef<str>>(s: S, state: &mut InputState) -> Result<Self, BasicInputError> {
        state.input_f32s(s, 1)?;
        Ok(state.data[0].as_f32())
    }
}

impl BasicInput for (f32, f32) {
    fn from_string<S: AsRef<str>>(s: S, state: &mut InputState) -> Result<Self, BasicInputError> {
        state.input_f32s(s, 2)?;

        Ok((state.data[0].as_f32(), state.data[1].as_f32()))
    }
}

impl BasicInput for (f32, f32, f32) {
    fn from_string<S: AsRef<str>>(s: S, state: &mut InputState) -> Result<Self, BasicInputError> {
        state.input_f32s(s, 3)?;

        Ok((
            state.data[0].as_f32(),
            state.data[1].as_f32(),
            state.data[2].as_f32(),
        ))
    }
}

impl BasicInput for (f32, f32, f32, f32) {
    fn from_string<S: AsRef<str>>(s: S, state: &mut InputState) -> Result<Self, BasicInputError> {
        state.input_f32s(s, 4)?;

        Ok((
            state.data[0].as_f32(),
            state.data[1].as_f32(),
            state.data[2].as_f32(),
            state.data[3].as_f32(),
        ))
    }
}

impl BasicInput for (f32, f32, f32, f32, f32, f32, f32, f32, f32, f32) {
    fn from_string<S: AsRef<str>>(s: S, state: &mut InputState) -> Result<Self, BasicInputError> {
        state.input_f32s(s, 10)?;

        Ok((
            state.data[0].as_f32(),
            state.data[1].as_f32(),
            state.data[2].as_f32(),
            state.data[3].as_f32(),
            state.data[4].as_f32(),
            state.data[5].as_f32(),
            state.data[6].as_f32(),
            state.data[7].as_f32(),
            state.data[8].as_f32(),
            state.data[9].as_f32(),
        ))
    }
}

impl BasicInput for String {
    fn from_string<S: AsRef<str>>(s: S, state: &mut InputState) -> Result<Self, BasicInputError> {
        state.input_strings(s, 1)?;
        Ok(state.data[0].as_string())
    }
}

impl BasicInput for (String, String) {
    fn from_string<S: AsRef<str>>(s: S, state: &mut InputState) -> Result<Self, BasicInputError> {
        state.input_strings(s, 2)?;
        Ok((state.data[0].as_string(), state.data[1].as_string()))
    }
}

// --------------------------------------------------------------------------------

fn get_input_from_io<BI: BasicInput, S: AsRef<str>>(prompt: S, io: &mut IoLayer) -> BI {
    let mut state = InputState {
        io,
        data: Vec::new(),
    };

    state.io.raw_output_string(format!("{}? ", prompt.as_ref()));

    loop {
        let buffer = state.io.raw_input_string();
        match BI::from_string(buffer, &mut state) {
            Ok(result) => return result,
            Err(e) => match e {
                BasicInputError::ParseFailed => {
                    state.io.raw_output_string("?REENTER\n");
                    state.io.raw_output_string(format!("{}? ", prompt.as_ref()));
                    state.data.clear();
                }
                BasicInputError::MissingInput => {
                    state.io.raw_output_string("??? ");
                }
            },
        }
    }
}

#[derive(Debug)]
pub enum BasicInputError {
    ParseFailed,
    MissingInput,
}

#[cfg(test)]
mod test {
    use crate::input::get_input_from_io;
    use crate::input::io_layer::{IoLayer, TextInput, TextOutput};
    use std::collections::VecDeque;

    fn getio() -> IoLayer {
        IoLayer {
            output: Box::new(WriteTestLayer::new()),
            input: Box::new(ReadTestLayer::new()),
        }
    }

    #[test]
    fn test_trait_single_input() {
        let mut testio = getio();
        testio.input.set_input_text("2");
        let result: f32 = get_input_from_io("prompt", &mut testio);
        assert_eq!(2.0, result);
    }

    #[test]
    fn test_trait_dual_input() {
        let mut testio = getio();
        testio.input.set_input_text("2,3");
        let result: (f32, f32) = get_input_from_io("prompt", &mut testio);
        assert_eq!(
            "prompt? ".to_string(),
            testio.output.get_last_line().unwrap()
        );
        assert_eq!(2.0, result.0);
        assert_eq!(3.0, result.1);
    }

    #[test]
    fn test_invalid_input() {
        let mut testio = getio();
        testio.input.set_input_text("abc");
        testio.input.set_input_text("2");
        let result: f32 = get_input_from_io("prompt", &mut testio);
        assert_eq!(
            "prompt? ".to_string(),
            testio.output.get_last_line().unwrap()
        );
        assert_eq!(
            "?REENTER\n".to_string(),
            testio.output.get_last_line().unwrap()
        );
        assert_eq!(2.0, result);
    }

    #[test]
    fn test_multiple_invalid_input() {
        let mut testio = getio();
        testio.input.set_input_text("abc");
        testio.input.set_input_text("2,a");
        testio.input.set_input_text("2,3");
        let result: (f32, f32) = get_input_from_io("prompt", &mut testio);
        assert_eq!(
            "prompt? ".to_string(),
            testio.output.get_last_line().unwrap()
        );
        assert_eq!(
            "?REENTER\n".to_string(),
            testio.output.get_last_line().unwrap()
        );
        assert_eq!(
            "prompt? ".to_string(),
            testio.output.get_last_line().unwrap()
        );
        assert_eq!(
            "?REENTER\n".to_string(),
            testio.output.get_last_line().unwrap()
        );
        assert_eq!(
            "prompt? ".to_string(),
            testio.output.get_last_line().unwrap()
        );
        assert_eq!(2.0, result.0);
        assert_eq!(3.0, result.1);
    }

    #[test]
    fn two_inputs_over_multiple_lines() {
        let mut testio = getio();
        testio.input.set_input_text("2");
        testio.input.set_input_text("3");
        let result: (f32, f32) = get_input_from_io("prompt", &mut testio);
        assert_eq!(
            "prompt? ".to_string(),
            testio.output.get_last_line().unwrap()
        );
        assert_eq!("??? ".to_string(), testio.output.get_last_line().unwrap());
        assert_eq!(2.0, result.0);
        assert_eq!(3.0, result.1);
    }

    #[test]
    fn test_triple_f32() {
        let mut testio = getio();
        testio.input.set_input_text("1, 2, 3");
        let result: (f32, f32, f32) = get_input_from_io("enter 3 numbers", &mut testio);
        assert_eq!(1.0, result.0);
        assert_eq!(2.0, result.1);
        assert_eq!(3.0, result.2);
    }

    #[test]
    fn test_input_string() {
        let mut testio = getio();
        testio.input.set_input_text("hello");
        let result: String = get_input_from_io("String prompt", &mut testio);
        assert_eq!("hello".to_string(), result);
    }

    #[test]
    fn test_two_input_strings() {
        let mut testio = getio();
        testio.input.set_input_text("hello, world");
        let result: (String, String) = get_input_from_io("Double string prompt", &mut testio);
        assert_eq!("hello".to_string(), result.0);
        assert_eq!("world".to_string(), result.1);
    }

    #[test]
    fn test_four_nums() {
        let mut testio = getio();
        testio.input.set_input_text("1, 2, 3, 4");
        let result: (f32, f32, f32, f32) = get_input_from_io("whatever", &mut testio);
        assert_eq!(4.0, result.3);
    }

    #[test]
    fn test_extra_ignored() {
        let mut testio = getio();
        testio.input.set_input_text("1, 2, 3, 4");
        let result: (f32, f32) = get_input_from_io("whatever", &mut testio);
        assert_eq!(Some("whatever? "), testio.output.get_last_line().as_deref());
        assert_eq!(
            Some("?EXTRA IGNORED\n"),
            testio.output.get_last_line().as_deref()
        );
        assert_eq!(2.0, result.1);
    }

    #[test]
    fn test_extra_ignored_bad_parse() {
        let mut testio = getio();
        testio.input.set_input_text("1, 2, hello");
        let result: (f32, f32) = get_input_from_io("whatever", &mut testio);
        assert_eq!(Some("whatever? "), testio.output.get_last_line().as_deref());
        assert_eq!(
            Some("?EXTRA IGNORED\n"),
            testio.output.get_last_line().as_deref()
        );
        assert_eq!(2.0, result.1);
    }

    #[test]
    fn test_ten_nums() {
        let mut testio = getio();
        testio.input.set_input_text("1, 2, 3, 4, 5, 6, 7, 8, 9, 10");
        let result: (f32, f32, f32, f32, f32, f32, f32, f32, f32, f32) =
            get_input_from_io("whatever", &mut testio);
        assert_eq!(Some("whatever? "), testio.output.get_last_line().as_deref());
        assert_eq!(None, testio.output.get_last_line());
        assert_eq!(10.0, result.9);
    }

    //
    // Helpers for test i/o
    //
    pub struct ReadTestLayer {
        next_line: VecDeque<String>,
    }

    impl ReadTestLayer {
        pub fn new() -> Self {
            Self {
                next_line: VecDeque::new(),
            }
        }
    }

    impl TextInput for ReadTestLayer {
        fn read(&mut self) -> String {
            if let Some(line) = self.next_line.pop_front() {
                println!(">> returning {line}");
                line
            } else {
                panic!("Test tried to read too much input");
            }
        }

        fn set_input_text(&mut self, text: &str) {
            self.next_line.push_back(text.to_string());
        }
    }

    pub struct WriteTestLayer {
        last_items_written: VecDeque<String>,
    }

    impl WriteTestLayer {
        pub fn new() -> Self {
            Self {
                last_items_written: VecDeque::new(),
            }
        }
    }

    impl TextOutput for WriteTestLayer {
        fn write(&mut self, to_write: &str) {
            self.last_items_written.push_back(to_write.into());
        }

        fn get_last_line(&mut self) -> Option<String> {
            self.last_items_written.pop_front()
        }
    }
}
