Original source downloaded from [Vintage Basic](http://www.vintage-basic.net/games.html)

Converted to [D](https://dlang.org/) by [Bastiaan Veelo](https://github.com/veelo).

## Running the code

Assuming the reference [dmd](https://dlang.org/download.html#dmd) compiler:
```shell
dmd -dip1000 -run highiq.d
```

[Other compilers](https://dlang.org/download.html) also exist.


## Discussion

The original BASIC game code made use of calculus and clever choises of field IDs to determine the validity of moves.
This is the original layout of IDs over the board:

```
          13   14   15

          22   23   24

29   30   31   32   33   34   35

38   39   40   41   42   43   44

47   48   49   50   51   52   53

          58   59   60

          67   68   69
```

This seems not very logical, because, wouldn't it make much more sense to let columns increase with 1 and rows increase
with 10, so you'd get a consistent coordinate system? It seems that the original author's first step in validating
moves was to check that moves jumped from one field over another one onto the next. He did this by making sure that
adjacent IDs alter between even and odd horizontally *and* vertically. So a valid move was always from an even ID to an
even ID *or* from an odd ID to an odd ID. So one of the checks that the BASIC code made was that the sum of both IDs
was even. This is of course not a sufficient test, because moves that jump over three fields are illegal. Therefore the
IDs seem to have been carefully laid oud so that the IDs increase with 1 horizontally, and 9 vertically, everywhere. So
the only valid difference between IDs for a horizontal move was always 2, and the only valid difference for a vertical
move was always 18.

Fact of the matter is, however, that checking for difference is sufficient and the even sum rule is superfluous, so
there is no need for the peculiar distribution of field IDs. Therefore I have chosen the following more logical
distribution:

```
          13   14   15

          23   24   25

31   32   33   34   35   36   37

41   42   43   44   45   46   47

51   52   53   54   55   56   57

          63   64   65

          73   74   75
```

As a consequence, the implementation of the game code has become much simpler; Not alone due to one less check, but due
to the fact that conversions between IDs and board coordinates have become unnecessary and thus we can work with a single
representation of the board state.

This version makes a prettier print of the board than the BASIC original, with coordinates for every move, and explains
illegal moves.


## Demo

```
                      H-I-Q
(After Creative Computing  Morristown, New Jersey)


Fields are identified by 2-digit numbers, each
between 1 and 7. Example: the middle field is 44,
the bottom middle is 74.

      _1  _2  _3  _4  _5  _6  _7
            ┌───┬───┬───┐
 1_         │ ■ │ ■ │ ■ │
            ├───┼───┼───┤
 2_         │ ■ │ ■ │ ■ │
    ┌───┬───┼───┼───┼───┼───┬───┐
 3_ │ ■ │ ■ │ ■ │ ■ │ ■ │ ■ │ ■ │
    ├───┼───┼───┼───┼───┼───┼───┤
 4_ │ ■ │ ■ │ ■ │   │ ■ │ ■ │ ■ │
    ├───┼───┼───┼───┼───┼───┼───┤
 5_ │ ■ │ ■ │ ■ │ ■ │ ■ │ ■ │ ■ │
    └───┴───┼───┼───┼───┼───┴───┘
 6_         │ ■ │ ■ │ ■ │
            ├───┼───┼───┤
 7_         │ ■ │ ■ │ ■ │
            └───┴───┴───┘

Move which peg? 23
The peg at 23 has nowhere to go. Try again.

Move which peg? 24
To where? 34
Field 34 is occupied. Try again.
To where? 54
Field 54 is occupied. Try again.
To where? 44

      _1  _2  _3  _4  _5  _6  _7
            ┌───┬───┬───┐
 1_         │ ■ │ ■ │ ■ │
            ├───┼───┼───┤
 2_         │ ■ │   │ ■ │
    ┌───┬───┼───┼───┼───┼───┬───┐
 3_ │ ■ │ ■ │ ■ │   │ ■ │ ■ │ ■ │
    ├───┼───┼───┼───┼───┼───┼───┤
 4_ │ ■ │ ■ │ ■ │ ■ │ ■ │ ■ │ ■ │
    ├───┼───┼───┼───┼───┼───┼───┤
 5_ │ ■ │ ■ │ ■ │ ■ │ ■ │ ■ │ ■ │
    └───┴───┼───┼───┼───┼───┴───┘
 6_         │ ■ │ ■ │ ■ │
            ├───┼───┼───┤
 7_         │ ■ │ ■ │ ■ │
            └───┴───┴───┘

Move which peg? 14
The peg at 14 has nowhere to go. Try again.

Move which peg? 24
There is no peg at 24. Try again.

Move which peg? 44
The peg at 44 has nowhere to go. Try again.

Move which peg? 32
To where? 22
Field 22 is ouside the board. Try again.
To where? 33
Field 33 is occupied. Try again.
To where? 34

      _1  _2  _3  _4  _5  _6  _7
            ┌───┬───┬───┐
 1_         │ ■ │ ■ │ ■ │
            ├───┼───┼───┤
 2_         │ ■ │   │ ■ │
    ┌───┬───┼───┼───┼───┼───┬───┐
 3_ │ ■ │   │   │ ■ │ ■ │ ■ │ ■ │
    ├───┼───┼───┼───┼───┼───┼───┤
 4_ │ ■ │ ■ │ ■ │ ■ │ ■ │ ■ │ ■ │
    ├───┼───┼───┼───┼───┼───┼───┤
 5_ │ ■ │ ■ │ ■ │ ■ │ ■ │ ■ │ ■ │
    └───┴───┼───┼───┼───┼───┴───┘
 6_         │ ■ │ ■ │ ■ │
            ├───┼───┼───┤
 7_         │ ■ │ ■ │ ■ │
            └───┴───┴───┘

Move which peg? 44
To where? 33
You cannot move diagonally. Try again.
To where? 24

      _1  _2  _3  _4  _5  _6  _7
            ┌───┬───┬───┐
 1_         │ ■ │ ■ │ ■ │
            ├───┼───┼───┤
 2_         │ ■ │ ■ │ ■ │
    ┌───┬───┼───┼───┼───┼───┬───┐
 3_ │ ■ │   │   │   │ ■ │ ■ │ ■ │
    ├───┼───┼───┼───┼───┼───┼───┤
 4_ │ ■ │ ■ │ ■ │   │ ■ │ ■ │ ■ │
    ├───┼───┼───┼───┼───┼───┼───┤
 5_ │ ■ │ ■ │ ■ │ ■ │ ■ │ ■ │ ■ │
    └───┴───┼───┼───┼───┼───┴───┘
 6_         │ ■ │ ■ │ ■ │
            ├───┼───┼───┤
 7_         │ ■ │ ■ │ ■ │
            └───┴───┴───┘

Move which peg? 36
To where? 33
You can't jump that far. Try again.
To where? 35
Field 35 is occupied. Try again.
To where? 34

      _1  _2  _3  _4  _5  _6  _7
            ┌───┬───┬───┐
 1_         │ ■ │ ■ │ ■ │
            ├───┼───┼───┤
 2_         │ ■ │ ■ │ ■ │
    ┌───┬───┼───┼───┼───┼───┬───┐
 3_ │ ■ │   │   │ ■ │   │   │ ■ │
    ├───┼───┼───┼───┼───┼───┼───┤
 4_ │ ■ │ ■ │ ■ │   │ ■ │ ■ │ ■ │
    ├───┼───┼───┼───┼───┼───┼───┤
 5_ │ ■ │ ■ │ ■ │ ■ │ ■ │ ■ │ ■ │
    └───┴───┼───┼───┼───┼───┴───┘
 6_         │ ■ │ ■ │ ■ │
            ├───┼───┼───┤
 7_         │ ■ │ ■ │ ■ │
            └───┴───┴───┘

Move which peg? 46
To where? 36
You need to jump over another peg. Try again.
To where? down
Field 00 is ouside the board. Try again.
To where?
```
