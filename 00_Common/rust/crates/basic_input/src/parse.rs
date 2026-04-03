pub(crate) fn parse<B: AsRef<str>>(buffer: B) -> Vec<String> {
    let mut result = Vec::new();
    let mut in_quote = false;
    let mut need_comma = false;

    let mut current = String::new();
    for ch in buffer.as_ref().chars() {
        match (ch, in_quote) {
            (',', false) => {
                result.push(current.trim().to_string());
                current.clear();
            }
            ('"', true) => {
                need_comma = true;
                result.push(current.trim().to_string());
                current.clear();
            }
            ('"', false) => {
                in_quote = true;
            }

            (',', true) if need_comma => {
                need_comma = false;
                in_quote = false;
            }
            (',', true) => current.push(ch),

            _ => {
                current.push(ch);
            }
        }
    }
    if !current.is_empty() {
        result.push(current.trim().to_string());
    }

    result
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn parse_single_value() {
        let result = parse("123");
        assert_eq!("123".to_string(), result[0]);
        assert_eq!(1, result.len());
    }

    #[test]
    fn parse_two_values() {
        let result = parse("123, 456");
        assert_eq!("123".to_string(), result[0]);
        assert_eq!("456".to_string(), result[1]);
        assert_eq!(2, result.len());
    }

    #[test]
    fn parse_quoted_value() {
        let result = parse("123, \"456,789\", 10");
        assert_eq!("123".to_string(), result[0]);
        assert_eq!("456,789".to_string(), result[1]);
        assert_eq!("10".to_string(), result[2]);
        assert_eq!(3, result.len());
    }

    #[test]
    fn parse_missing_closing_quote() {
        let result = parse("123, \"456,789, 10");
        assert_eq!("123".to_string(), result[0]);
        assert_eq!("456,789, 10".to_string(), result[1]);
        assert_eq!(2, result.len());
    }

    #[test]
    fn parse_quoted_value_twice() {
        let result = parse("123, \"456,789\", 10, 11");
        assert_eq!("123".to_string(), result[0]);
        assert_eq!("456,789".to_string(), result[1]);
        assert_eq!("10".to_string(), result[2]);
        assert_eq!("11".to_string(), result[3]);
        assert_eq!(4, result.len());
    }
}
