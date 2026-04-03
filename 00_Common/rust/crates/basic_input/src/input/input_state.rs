use crate::{BasicInputError, input::io_layer::IoLayer};

pub struct InputState<'a> {
    pub(crate) io: &'a mut IoLayer,
    pub(crate) data: Vec<Value>,
}

impl<'a> InputState<'a> {
    pub(crate) fn input_f32s<S: AsRef<str>>(
        &mut self,
        s: S,
        count: usize,
    ) -> Result<(), BasicInputError> {
        let vals = crate::parse::parse(s);

        for v in vals {
            if self.data.len() >= count {
                self.io.raw_output_string("?EXTRA IGNORED\n");
                return Ok(());
            }

            let value = v
                .trim()
                .parse::<f32>()
                .map_err(|_| BasicInputError::ParseFailed)?;
            self.data.push(Value::Num(value));
        }

        if self.data.len() < count {
            return Err(BasicInputError::MissingInput);
        }

        Ok(())
    }

    pub(crate) fn input_strings<S: AsRef<str>>(
        &mut self,
        s: S,
        count: usize,
    ) -> Result<(), BasicInputError> {
        let vals = crate::parse::parse(s);

        for v in vals {
            let value = v.trim();
            self.data.push(Value::Str(value.to_string()));
        }
        if self.data.len() < count {
            return Err(BasicInputError::MissingInput);
        }
        if self.data.len() > count {
            self.io.raw_output_string("?EXTRA IGNORED\n");
        }
        Ok(())
    }
}

#[derive(Clone, Debug)]
pub enum Value {
    Num(f32),
    Str(String),
}

impl Value {
    pub(crate) fn as_f32(&self) -> f32 {
        match self {
            Value::Num(num) => *num,
            Value::Str(_) => panic!("tried to extract a number, but Value was a String"),
        }
    }

    pub(crate) fn as_string(&self) -> String {
        match self {
            Value::Num(_) => panic!("tried to extract a string, but Value was a Num"),
            Value::Str(s) => s.clone(),
        }
    }
}
