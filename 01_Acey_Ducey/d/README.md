Original source downloaded from [Vintage Basic](http://www.vintage-basic.net/games.html)

Converted to [D](https://dlang.org/) by [Bastiaan Veelo](https://github.com/veelo).

Two versions are supplied that are functionally equivalent, but differ in source layout:

<dl>
  <dt><tt>aceyducey_literal.d</tt></dt>
  <dd>A largely literal transcription of the original Basic source. All unnecessary uglyness is preserved.</dd>
  <dt><tt>aceyducey.d</tt></dt>
  <dd>An idiomatic D refactoring of the original, with a focus on increasing the readability and robustness.
      Memory-safety <A href="https://dlang.org/spec/memory-safe-d.html">is ensured by the language</a>, thanks to the
      <tt>@safe</tt> annotation.</dd>
</dl>

## Running the code

Assuming the reference [dmd](https://dlang.org/download.html#dmd) compiler:
```shell
dmd -run aceyducey.d
```

[Other compilers](https://dlang.org/download.html) also exist.

Note that there are compiler switches related to memory-safety (`-preview=dip25` and `-preview=dip1000`) that are not
used here because they are unnecessary in this case. What these do is to make the analysis more thorough, so that with
them some code that needed to be `@system` can then be inferred to be in fact `@safe`. [Code that compiles without
these switches is just as safe as when compiled with them]
(https://forum.dlang.org/post/dftgjalswvwfjpyushgn@forum.dlang.org).
