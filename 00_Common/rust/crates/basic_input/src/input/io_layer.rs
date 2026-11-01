use std::io::{BufRead, StdoutLock, Write};

pub struct IoLayer {
    pub(super) output: Box<dyn TextOutput>,
    pub(super) input: Box<dyn TextInput>,
}

impl IoLayer {
    pub(crate) fn get_io() -> Self {
        Self {
            output: StdoutLayer::new(),
            input: StdinLayer::new(),
        }
    }

    pub(crate) fn raw_output_string<S: AsRef<str>>(&mut self, s: S) {
        self.output.write(s.as_ref());
    }

    pub(crate) fn raw_input_string(&mut self) -> String {
        self.input.read()
    }
}

pub(crate) trait TextOutput {
    fn write(&mut self, to_write: &str);
    #[cfg(test)]
    fn get_last_line(&mut self) -> Option<String>;
}

pub(crate) trait TextInput {
    fn read(&mut self) -> String;
    #[cfg(test)]
    fn set_input_text(&mut self, text: &str);
}

struct StdoutLayer<'a> {
    stdout: StdoutLock<'a>,
}

impl<'a> TextOutput for StdoutLayer<'a> {
    fn write(&mut self, to_write: &str) {
        print!("{to_write}");
        let _ = self.stdout.flush();
    }

    #[cfg(test)]
    fn get_last_line(&mut self) -> Option<String> {
        unreachable!();
    }
}

struct StdinLayer;
impl TextInput for StdinLayer {
    fn read(&mut self) -> String {
        let mut buffer = String::new();
        let stdin = std::io::stdin();
        let mut handle = stdin.lock();
        let _ = handle.read_line(&mut buffer);
        buffer
    }

    #[cfg(test)]
    fn set_input_text(&mut self, _text: &str) {
        unreachable!();
    }
}

impl<'a> StdoutLayer<'a> {
    fn new() -> Box<Self> {
        Box::new(Self {
            stdout: std::io::stdout().lock(),
        })
    }
}
impl StdinLayer {
    fn new() -> Box<Self> {
        Box::new(Self)
    }
}
