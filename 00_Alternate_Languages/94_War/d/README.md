Original source downloaded from [Vintage Basic](http://www.vintage-basic.net/games.html)

Converted to [D](https://dlang.org/) by [Bastiaan Veelo](https://github.com/veelo).

## Running the code

Assuming the reference [dmd](https://dlang.org/download.html#dmd) compiler:
```shell
dmd -preview=dip1000 -run war.d
```

[Other compilers](https://dlang.org/download.html) also exist.

## Specialties explained

This game code contains some specialties that you might want to know more about. Here goes.

### Suits

Most modern consoles are capable of displaying more than just ASCII, and so I have chosen to display the actual ♠, ♥, ♦
and ♣ instead of substituting them by letters like the BASIC original did. Only the Windows console needs a nudge in
the right direction with these instructions:
```d
SetConsoleOutputCP(CP_UTF8); // Set code page
SetConsoleOutputCP(GetACP);  // Restore the default
```
Instead of cluttering the `main()` function with these lesser important details, we can move them into a
[module constructor and module destructor](https://dlang.org/spec/module.html#staticorder), which run before and after
`main()` respectively. And because order of declaration is irrelevant in a D module, we can push those all the way
down to the bottom of the file. This is of course only necessary on Windows (and won't even work anywhere else) so
we'll need to wrap this in a `version (Windows)` conditional code block:
```d
version (Windows)
{
    import core.sys.windows.windows;

    shared static this() @trusted
    {
        SetConsoleOutputCP(CP_UTF8);
    }

    shared static ~this() @trusted
    {
        SetConsoleOutputCP(GetACP);
    }
}
```
Although it doesn't matter much in this single-threaded program, the `shared` attribute makes that these
constructors/destructors are run once per program invocation; non-shared module constructors and module destructors are
run for every thread. The `@trusted` annotation is necessary because these are system API calls; The compiler cannot
check these for memory-safety, and so we must indicate that we have reviewed the safety manually.

### Uniform Function Call Syntax

In case you wonder why this line works:
```d
if ("Do you want instructions?".yes)
    // ...
```
then it is because this is equivalent to
```d
if (yes("Do you want instructions?"))
    // ...
```
where `yes()` is a Boolean function that is defined below `main()`. This is made possible by the language feature that
is called [uniform function call syntax (UFCS)](https://dlang.org/spec/function.html#pseudo-member). UFCS works by
passing what is in front of the dot as the first parameter to the function, and it was invented to make it possible to
call free functions on objects as if they were member functions. UFCS can also be used to obtain a more natural order
of function calls, such as this line inside `yes()`:
```d
return trustedReadln.strip.toLower.startsWith("y");
```
which reads easier than the equivalent
```d
return startsWith(toLower(strip(trustedReadln())), "y");
```

### Type a lot or not?

It would have been straight forward to define the `cards` array explicitly like so:
```d
const cards = ["2♠", "2♥", "2♦", "2♣", "3♠", "3♥", "3♦", "3♣",
               "4♠", "4♥", "4♦", "4♣", "5♠", "5♥", "5♦", "5♣",
               "6♠", "6♥", "6♦", "6♣", "7♠", "7♥", "7♦", "7♣",
               "8♠", "8♥", "8♦", "8♣", "9♠", "9♥", "9♦", "9♣",
               "10♠", "10♥", "10♦", "10♣", "J♥", "J♦", "J♣", "J♣",
               "Q♠", "Q♥", "Q♦", "Q♣", "K♠", "K♥", "K♦", "K♣",
               "A♠", "A♥", "A♦", "A♣"];
```
but that's tedious, difficult to spot errors in (*can you?*) and looks like something a computer can automate. Indeed
it can:
```d
static const cards = cartesianProduct(["2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A"],
                                      ["♠", "♥", "♦", "♣"]).map!(a => a.expand.only.join).array;
```
The function [`cartesianProduct`](https://dlang.org/phobos/std_algorithm_setops.html#cartesianProduct) takes two
ranges, like the horizontal and vertical headers of a spreadsheet, and fills the table with the combinations that form
the coordinates of the cells. But the output of that function is in the form of an array of
[`Tuple`](https://dlang.org/phobos/std_typecons.html#Tuple)s, which looks like `[Tuple!(string, string)("2", "♠"),
Tuple!(string, string)("2", "♥"), ... etc]`. [`map`](https://dlang.org/phobos/std_algorithm_iteration.html#map)
comes to the rescue, converting each Tuple to a string, by calling
[`expand`](https://dlang.org/phobos/std_typecons.html#.Tuple.expand), then
[`only`](https://dlang.org/phobos/std_range.html#only) and then [`join`](https://dlang.org/phobos/std_array.html#join)
on them. The result is a lazily evaluated range of strings. Finally,
[`array`](https://dlang.org/phobos/std_array.html#array) turns the range into a random access array. The `static`
attribute makes that all this is performed at compile-time, so the result is exactly the same as the manually entered
data, but without the typo's.

### Shuffle the cards or not?

The original BASIC code works with a constant array of cards, ordered by increasing numerical value, and indexing it
with indices that have been shuffled. This is efficient because in comparing who wins, the indices can be compared
directly, since a higher index correlates to a card with a higher numerical value (when divided by the number of suits,
4). Some of the other reimplementations in other languages have been written in a lesser efficient way by shuffling the
array of cards itself. This then requires the use of a lookup table or searching for equality in an auxiliary array
when comparing cards.

I find the original more elegant, so that's what you see here:
```d
const indices = iota(0, cards.length).array.randomShuffle;
```
[`iota`](https://dlang.org/phobos/std_range.html#iota) produces a range of integers, in this case starting at 0 and
increasing up to the number of cards in the deck (exclusive). [`array`](https://dlang.org/phobos/std_array.html#array)
turns the range into an array, so that [`randomShuffle`](https://dlang.org/phobos/std_random.html#randomShuffle) can
do its work.
