## BAGELS

Original source downloaded [from Vintage Basic](http://www.vintage-basic.net/games.html)

Conversion to [Ruby](https://www.ruby-lang.org/en/) by [Tom Armitage](https://github.com/infovore)

## Translator's notes:

This is a highly imperative port. As such, it's very much, in the spirit of David Ahl's original version, and also highly un-Rubyish.

A few decisions I made:

* the main loop is a 'while' loop. Most games are a main loop that runs until it doesn't, and I felt that "while the player wished to keep playing, the game should run" was an appropriate structure.
* lots of puts and gets; that feels appropriate to the Ahl implementation. No clever cli or curses libraries here.
* the number in question, and the player's answer, are stored as numbers. They're only converted into arrays for the purpose of `puts_clue_for` - ie, when comparison is need. The original game stored them as arrays, which made sense, but given the computer says "I have a number in mind", I decided to store what was in its 'mind' as a number.
* the `String#center` method from Ruby 2.5~ sure is handy.
