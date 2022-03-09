# Common Library

## Purpose

The primary purpose of this library is to implement common behaviours of the BASIC interpreter that impact gameplay, to
free coders porting the games to concentrate on the explicit game logic.

The behaviours implemented by this library are:

* Complex interactions involved in text input.
* Formatting of number in text output.
* Behaviour of the BASIC `RND(float)` PRNG function.

A secondary purpose is to provide common services that, with dependency injection, would allow a ported game to be
driven programmatically to permit full-game acceptance tests to be written.

The library is **NOT** intended to be:

* a repository for common game logic that is implemented in the BASIC code of the games; or
* a DSL allowing the BASIC code to be compiled in a different language environment with minimal changes. This implies
  that implementations of the above behaviours should use method names, etc, that are idiomatic of the specific
  language's normal routines for that behaviour.

## Text Input

The behaviour of the BASIC interpreter when accepting text input from the user is a major element of the original
gameplay experience that is seen to be valuable to maintain. This behaviour is complex and non-trivial to implement, and
is better implemented once for other developers to use so they can concentrate on the explicit game logic.

The text input/output behaviour can be investigated using a basic program such as:

**`BASIC_Tests/InputTest.bas`**

```basic
10 INPUT "Enter 3 numbers";A,B,C
20 PRINT "You entered: ";A;B;C
30 PRINT "--------------------------"
40 GOTO 10
```

The following transcript shows the use of this program, and some interesting behaviours of the BASIC interpreter INPUT
routine. There are some other behaviours which can be seen in the unit tests for the C# library implementation.

```dos
Enter 3 numbers? -1,2,3.141             <-- multiple numbers are separated by commas
You entered: -1  2  3.141
--------------------------
Enter 3 numbers? 1                      <-- ... or entered on separate lines
?? 2
?? 3
You entered:  1  2  3
--------------------------
Enter 3 numbers? 1,2                    <-- ... or both
?? 3
You entered:  1  2  3
--------------------------
Enter 3 numbers? 1,-2,3,4               <-- Extra input is ignore with a warning
!EXTRA INPUT IGNORED
You entered:  1 -2  3
--------------------------
Enter 3 numbers?   5  , 6.7, -8   ,10   <-- Whitespace around values is ignored
!EXTRA INPUT IGNORED
You entered:  5  6.7 -8
--------------------------
Enter 3 numbers? abcd,e,f               <-- Non-numeric entries must be retried
!NUMBER EXPECTED - RETRY INPUT LINE
? 1,2,abc                               <-- A single non-numeric invalidates the whole line
!NUMBER EXPECTED - RETRY INPUT LINE
? 1de,2.3f,10k,abcde                    <-- ... except for trailing non-digit chars  and extra input
!EXTRA INPUT IGNORED
You entered:  1  2.3  10
--------------------------
Enter 3 numbers? 1,"2,3",4              <-- Double-quotes enclose a single parsed value
You entered:  1  2  4
--------------------------
Enter 3 numbers? 1,2,"3                 <-- An unmatched double-quote crashes the interpreter
vintbas.exe: Mismatched inputbuf in inputVars
CallStack (from HasCallStack):
  error, called at src\Language\VintageBasic\Interpreter.hs:436:21 in main:Language.VintageBasic.Interpreter
```

I propose to ignore this last behaviour - the interpreter crash - and instead treat the end of the input line as the end
of a quoted value.  There are some additional behaviours to those shown above which can be seen in the unit tests for
the C# implementation of the library.

Note also that BASIC numeric variables store a single-precision floating point value, so numeric input functions should
return a value of that type.

### Implementation Notes

The usage of the `INPUT` command in the BASIC code of the games was analysed, with the variables used designated as
`num`, for a numeric variable (eg. `M1`), or `str`, for a string variable (eg. `C$`). The result of this usage analysis
across all game programs is:

Variable number and type|Count
---|---
str|137
str  str|2
num|187
num  num|27
num  num  num|7
num  num  num  num|1
num  num  num  num  num  num  num  num  num  num|1

The usage count is interesting, but not important. What is important is the variable type and number for each usage.
Implementers if the above behaviours do not need to cater for mixed variable types in their input routines (although the
BASIC interpreter does support this). Input routines also need to cater only for 1 or 2 string values, and 1, 2, 3, 4,
or 10 numeric values.

## Numbers in Text Output

As seen in the transcript above, the BASIC interpreter has some particular rules for formatting numbers in text output.
Negative numbers are printed with a leading negative sign (`-`) and a trailing space. Positive numbers are printed also
with the trailing space, but with a leading space in place of the sign character.

Additional formatting rules can be seen by running this program:

**`BASIC_Tests/OutputTest.bas`**

```basic
10 A=1: B=-2: C=0.7: D=123456789: E=-0.0000000001
20 PRINT "|";A;"|";B;"|";C;"|";D;"|";E;"|"
```

The output is:

```dos
| 1 |-2 | .7 | 1.2345679E+8 |-1.E-10 |
```

This clearly shows the leading and trailing spaces, but also shows that:

* numbers without an integer component are printed without a leading zero to the left of the decimal place;
* numbers above and below a certain magnitude are printed in scientific notation.

<!-- markdownlint-disable MD024 -->
### Implementation Notes
<!-- markdownlint-enable MD024 -->

I think the important piece of this number formatting behaviour, in terms of its impact on replicating the text output
of the original games, is the leading and trailing spaces. This should be the minimum behaviour supported for numeric
output. Additional formatting behaviours may be investigated and supported by library implementers as they choose.

## The BASIC `RND(float)` function

The [Vintage BASIC documentation](http://vintage-basic.net/downloads/Vintage_BASIC_Users_Guide.html) describes the
behaviour of the `RND(float)` function:

> Psuedorandom number generator. The behavior is different depending on the value passed. If the value is positive, the
> result will be a new random value between 0 and 1 (including 0 but not 1). If the value is zero, the result will be a
> repeat of the last random number generated. If the value is negative, it will be rounded down to the nearest integer
> and used to reseed the random number generator. Pseudorandom sequences can be repeated by reseeding with the same
> number.

This behaviour can be shown by the program:

**`BASIC_Tests/RndTest.bas`**

```basic
10 PRINT "1: ";RND(1);RND(1);RND(0);RND(0);RND(1)
20 PRINT "2: ";RND(-2);RND(1);RND(1);RND(1)
30 PRINT "3: ";RND(-5);RND(1);RND(1);RND(1)
40 PRINT "4: ";RND(-2);RND(1);RND(1);RND(1)
```

The output of this program is:

```dos
1:  .97369444  .44256502  .44256502  .44256502  .28549057    <-- Repeated values due to RND(0)
2:  .4986506  4.4510484E-2  .96231  .35997057
3:  .8113741  .13687313  6.1034977E-2  .7874807
4:  .4986506  4.4510484E-2  .96231  .35997057                <-- Same sequence as line 2 due to same seed
```

<!-- markdownlint-disable MD024 -->
### Implementation Notes
<!-- markdownlint-enable MD024 -->

While the BASIC `RND(x)` function always returns a number between 0 (inclusive) and 1 (exclusive) for positive non-zero
values of `x`, game porters would find it convenient for the library to include functions returning a random float or
integer in a range from an inclusive minimum to an exclusive maximum.

As one of the games, "Football", makes use of `RND(0)` with different scaling applied than the previous use of `RND(1)`,
a common library implementation should always generate a value between 0 and 1, and scale that for a function with a
range, so that a call to the equivalent of the `RND(0)` function can return the previous value between 0 and 1.
