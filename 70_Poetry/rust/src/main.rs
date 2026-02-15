use rand::{rngs::ThreadRng, *};

fn main() {
    println!("{:>30}POETRY", "");
    println!("{:>15}CREATIVE COMPUTING  MORRISTOWN, NEW JERSEY", "");
    println!("\n\n");

    let mut poetry = Poetry::new();
    poetry.run();
}

struct Poetry {
    current_phrase: usize,         // "I" variable in basic
    section: Section,              // "J" variable in basic
    phrase_end: PhrasePunctuation, // "U" variable in basic
    phrases_output: usize,         // "K" variable in basic
    rng: ThreadRng,
}

impl Poetry {
    fn new() -> Self {
        Self {
            current_phrase: 0,
            section: Section::Intro,
            phrase_end: PhrasePunctuation::Beginning,
            phrases_output: 0,
            rng: rand::rng(),
        }
    }

    fn run(&mut self) {
        // Note: this code runs forever until forcibly interrupted
        loop {
            // Generate a phrase of output - lines 100 through 135
            let stage = self.output_phrase();

            // Line 210: Check to see if we should print a comma
            if stage == NextStage::Comma
                && self.phrase_end != PhrasePunctuation::Beginning
                && self.rng.random_bool(0.19)
            {
                // line 211 - generate command record that in phrase_end
                print!(",");
                self.phrase_end = PhrasePunctuation::NeedSpace;
            }

            // line 212 - randomly generate a space or a newline
            if !self.rng.random_bool(0.65) {
                // line 213
                print!(" ");
                self.phrase_end = self.phrase_end.inc();
            } else {
                // line 214
                println!();
                self.phrase_end = PhrasePunctuation::Beginning;
            }

            // line 215 - pick next phrase
            self.current_phrase = self.rng.random_range(1..=5);

            // line 220 - move to next section, and increment phrases output
            self.section = self.section.inc();
            self.phrases_output += 1;

            // line 230-235 - occasionally generate some indentation
            if self.phrase_end == PhrasePunctuation::Beginning && !self.section.intro_or_second() {
                print!("     ");
            }

            // line 240 - ignore until we hit Section::Reset
            if self.section == Section::Reset {
                // line 250 - reset section counter and check for 20-phrase limit
                self.section = Section::Intro;
                println!();
                if self.phrases_output > 20 {
                    // line 270 - reset counters and start again
                    println!();
                    self.phrase_end = PhrasePunctuation::Beginning;
                    self.phrases_output = 0;

                    // line 260 - this line goes back to picking a new phrase
                    // and sets up J and K for new output. We've already done
                    // this work so we can ignore that
                }
            }
        }
    }

    // This function emulates line 240 - picking the next section to print a phrase from
    fn output_phrase(&mut self) -> NextStage {
        match self.section {
            Section::Intro => self.print_intro(),
            Section::First => self.print_first(),
            Section::Second => self.print_second(),
            Section::Final => self.print_final(),
            Section::Reset => NextStage::Reset,
        }
    }

    // Lines 90-104 - introductory phrase
    fn print_intro(&self) -> NextStage {
        match self.current_phrase {
            2 => print!("firey eyes"),
            3 => print!("bird or fiend"),
            4 => print!("thing of evil"),
            5 => print!("prophet"),
            _ => print!("midnight dreary"), // "fallthrough" case, as well as case 1
        }
        NextStage::Comma
    }

    // For the next 4 functions, we emulate the "fall-through" behavior of
    // "ON x GOTO" by using rusts "_" pattern matching option

    // Lines 110-115: next phrase, with some modifications for numbers 1 and 4
    fn print_first(&mut self) -> NextStage {
        match self.current_phrase {
            2 => {
                print!("thrilled me");
                NextStage::Comma
            }
            3 => {
                print!("still sitting....");
                NextStage::Space
            }
            4 => {
                print!("never flitting");
                self.phrase_end = PhrasePunctuation::NeedSpace;
                NextStage::Comma
            }
            5 => {
                print!("burned");
                NextStage::Comma
            }
            _ => {
                print!("beguiling me");
                self.phrase_end = PhrasePunctuation::NeedSpace;
                NextStage::Comma
            }
        }
    }

    // Lines 120-125 - next phrase, with occasional skipping of phrase 5
    fn print_second(&self) -> NextStage {
        match self.current_phrase {
            2 => print!("darkness there"),
            3 => print!("shall be lifted"),
            4 => print!("quoth the raven"),
            5 => {
                if self.phrase_end != PhrasePunctuation::Beginning {
                    print!("sign of parting")
                }
            }
            _ => print!("and my soul"),
        }
        NextStage::Comma
    }

    // Line 130-135 - final phrase
    fn print_final(&self) -> NextStage {
        match self.current_phrase {
            2 => print!("yet again"),
            3 => print!("slowly creeping"),
            4 => print!("...evermore"),
            5 => print!("nevermore"),
            _ => print!("nothing more"),
        }
        NextStage::Comma
    }
}

#[derive(Debug, PartialEq, Eq)]
enum PhrasePunctuation {
    Beginning,
    MaybeComma,
    NeedSpace,
}
impl PhrasePunctuation {
    fn inc(&self) -> Self {
        match self {
            Self::Beginning => Self::MaybeComma,
            Self::MaybeComma => Self::NeedSpace,
            Self::NeedSpace => Self::NeedSpace,
        }
    }
}

#[derive(PartialEq, Eq)]
enum NextStage {
    Comma,
    Space,
    Reset,
}

#[derive(PartialEq, Eq)]
enum Section {
    Intro,
    First,
    Second,
    Final,
    Reset,
}

impl Section {
    fn inc(&self) -> Self {
        match self {
            Section::Intro => Section::First,
            Section::First => Section::Second,
            Section::Second => Section::Final,
            Section::Final => Section::Reset,
            Section::Reset => panic!("can't increment from reset"),
        }
    }

    fn intro_or_second(&self) -> bool {
        matches!(self, Self::Intro | Self::Second)
    }
}
