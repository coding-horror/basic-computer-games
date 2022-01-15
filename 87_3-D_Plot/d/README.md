Original source downloaded from [Vintage Basic](http://www.vintage-basic.net/games.html)

Converted to [D](https://dlang.org/) by [Bastiaan Veelo](https://github.com/veelo).

## Running the code

Assuming the reference [dmd](https://dlang.org/download.html#dmd) compiler:
```shell
dmd -dip1000 -run threedeeplot.d
```

[Other compilers](https://dlang.org/download.html) also exist.

## On rounding floating point values to integer values

The D equivalent of Basic `INT` is [`floor`](https://dlang.org/phobos/std_math_rounding.html#.floor),
which rounds towards negative infinity. If you change occurrences of `floor` to
[`lrint`](https://dlang.org/phobos/std_math_rounding.html#.lrint), you'll see that the plots show a bit more detail,
as is done in the bonus below.

## Bonus: Self-writing programs

With a small modification to the source, the program can be extended to **plot a random function**, and **print its formula**.

```shell
rdmd -dip1000 threedeeplot_random.d
```
(`rdmd` caches the executable, which results in speedy execution when the source does not change.)

### Example output
```
                                    3D Plot
              (After Creative Computing  Morristown, New Jersey)


                           f(z) = 30 * sin(z / 10.0)

                             *
                      *      *    * *
                *         *      *    * *
                    *         *      *    * *
            *           *        *       *   *  *
               *           *         *      *   *  *
                  *           *         *     *    * **
       *             *           *        *      *   *  *
         *              *           *       *     *   *  **
            *              *          *       *    *   *  * *
              *              *          *      *   *   *  *  *
                *              *          *     *  *  *   *   **
                  *              *         *    * *  *   *    * *
   *                *             *        *    ** *    *     * *
    *                *             *        *  **     *      *   *
     *                 *            *       * *     *       *    *
      *                 *            *      * *   *         *    *
       *                *             *     ** *           *     **
        *                *            *     **            *      **
        *                *            *     *            *       **
        *                *            *     *            *       **
        *                *            *     *            *       **
        *                *            *     **            *      **
       *                *             *     ** *           *     **
      *                 *            *      * *   *         *    *
     *                 *            *       * *     *       *    *
    *                *             *        *  **     *      *   *
   *                *             *        *    ** *    *     * *
                  *              *         *    * *  *   *    * *
                *              *          *     *  *  *   *   **
              *              *          *      *   *   *  *  *
            *              *          *       *    *   *  * *
         *              *           *       *     *   *  **
       *             *           *        *      *   *  *
                  *           *         *     *    * **
               *           *         *      *   *  *
            *           *        *       *   *  *
                    *         *      *    * *
                *         *      *    * *
                      *      *    * *
                             *
```

### Breakdown of differences

Have a look at the relevant differences between `threedeeplot.d` and `threedeeplot_random.d`.
This is the original function with the single expression that is evaluated for the plot:
```d
    static float fna(float z)
    {
        return 30.0 * exp(-z * z / 100.0);
    }
```
Here `static` means that the nested function does not need acces to its enclosing scope.

Now, by inserting the following:
```d
    enum functions = ["30.0 * exp(-z * z / 100.0)",
                      "sqrt(900.01 - z * z) * .9 - 2",
                      "30 * (cos(z / 16.0) + .5)",
                      "30 - 30 * sin(z / 18.0)",
                      "30 * exp(-cos(z / 16.0)) - 30",
                      "30 * sin(z / 10.0)"];

    size_t index = uniform(0, functions.length);
    writeln(center("f(z) = " ~ functions[index], width), "\n");
```
and changing the implementation of `fna` to
```d
    float fna(float z)
    {
        final switch (index)
        {
            static foreach (i, f; functions)
                case i:
                    mixin("return " ~ f ~ ";");
        }
    }
```
we unlock some very special abilities of D. Let's break it down:

```d
    enum functions = ["30.0 * exp(-z * z / 100.0)", /*...*/];
```
This defines an array of strings, each containing a mathematical expression. Due to the `enum` keyword, this is an
array that really only exists at compile-time.

```d
    size_t index = uniform(0, functions.length);
```
This defines a random index into the array. `functions.length` is evaluated at compile-time, due to D's compile-time
function evaluation (CTFE).

```d
    writeln(center("f(z) = " ~ functions[index], width), "\n");
```
Unmistakenly, this prints the formula centered on a line. What happens behind the scenes is that `functions` (which
only existed at compile-time before now) is pasted in, so that an instance of that array actually exists at run-time
at this spot, and is instantly indexed.

```d
    float fna(float z)
    {
        final switch (index)
        {
            // ...
        }
    }
```
`static` has been dropped from the nested function because we want to evaluate `index` inside it. The function contains
an ordinary `switch`, with `final` providing some extra robustness. It disallows a `default` case and produces an error
when the switch doesn't handle all cases. The `switch` body is where the magic happens and consists of these three
lines:
```d
            static foreach (i, f; functions)
                case i:
                    mixin("return " ~ f ~ ";");
```
The `static foreach` iterates over `functions` at compile-time, producing one `case` for every element in `functions`.
`mixin` takes a string, which is constructed at compile-time, and pastes it right into the source.

In effect, the implementation of `float fna(float z)` unrolls itself into
```d
    float fna(float z)
    {
        final switch (index)
        {
            case 0:
                return 30.0 * exp(-z * z / 100.0);
            case 1:
                return sqrt(900.01 - z * z) * .9 - 2;
            case 2:
                return 30 * (cos(z / 16.0) + .5);
            case 3:
                return 30 - 30 * sin(z / 18.0);
            case 4:
                return 30 * exp(-cos(z / 16.0)) - 30;
            case 5:
                return 30 * sin(z / 10.0)";
        }
    }
```

So if you feel like adding another function, all you need to do is append it to the `functions` array, and the rest of
the program *rewrites itself...*
