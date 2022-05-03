pub struct Progress {
    chars: [char; 5],
}

impl Progress {
    pub fn new() -> Self {
        Progress { chars: ['-'; 5] }
    }

    pub fn set_char_at(&mut self, c: char, i: usize) {
        self.chars[i] = c;
    }

    pub fn print(&self) {
        for c in self.chars {
            print!("{}", c);
        }
    }

    pub fn done(&self) -> bool {
        for c in self.chars {
            if c == '-' {
                return false;
            }
        }

        true
    }
}
