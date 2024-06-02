### Lunar LEM Rocket

This game in its many different versions and names (ROCKET, LUNAR, LEM, and APOLLO) is by far and away the single most popular computer game. It exists in various versions that start you anywhere from 500 feet to 200 miles away from the moon, or other planets, too. Some allow the control of directional stabilization rockets and/or the retro rocket. The three versions presented here represent the most popular of the many variations.

In most versions of this game, the temptation is to slow up too soon and then have no fuel left for the lower part of the journey. This, of course, is disastrous (as you will find out when you land your own capsule)!

LUNAR was originally in FOCAL by Jim Storer while a student at Lexington High School and subsequently converted to BASIC by David Ahl. ROCKET was written by Eric Peters at DEC and LEM by William Labaree II of Alexandria, Virginia.

In this program, you set the burn rate of the retro rockets (pounds of fuel per second) every 10 seconds and attempt to achieve a soft landing on the moon. 200 lbs/sec really puts the brakes on, and 0 lbs/sec is free fall. Ignition occurs a 8 lbs/sec, so _do not_ use burn rates between 1 and 7 lbs/sec. To make the landing more of a challenge, but more closely approximate the real Apollo LEM capsule, you should make the available fuel at the start (N) equal to 16,000 lbs, and the weight of the capsule (M) equal to 32,500 lbs.

#### LEM
This is the most comprehensive of the three versions and permits you to control the time interval of firing, the thrust, and the attitude angle. It also allows you to work in the metric or English system of measurement. The instructions in the program dialog are very complete, so you shouldn’t have any trouble.

#### ROCKET
In this version, you start 500 feet above the lunar surface and control the burn rate in 1-second bursts. Each unit of fuel slows your descent by 1 ft/sec. The maximum thrust of your engine is 30 ft/sec/sec.

---

As published in Basic Computer Games (1978):
- [Atari Archives](https://www.atariarchives.org/basicgames/showpage.php?page=106)
- [Annarchive](https://annarchive.com/files/Basic_Computer_Games_Microcomputer_Edition.pdf#page=121)

Downloaded from Vintage Basic at
http://www.vintage-basic.net/games.html

#### Known Bugs

### lem.bas

- The input validation on the thrust value (displayed as P, stored internally as F) appears to be incorrect.  It allows negative values up up to -95, but at -96 or more balks and calls it negative.  I suspect the intent was to disallow any value less than 0 (in keeping with the instructions), *or* nonzero values less than 10.

- The physics calculations seem very sus.  If you enter "1000,0,0" (i.e. no thrust at all, integrating 1000 seconds at a time) four times in a row, you first fall, but then mysteriously gain vertical speed, and end up being lost in space.  This makes no sense.  A similar result happened when just periodically applying 10% thrust in an attempt to hover.


#### Porting Notes

(please note any difficulties or challenges in porting here)

### LUNAR

Variables:

`A`: Altitude in miles.  Up is positive.
`V`: Velocity in miles / sec.  Down is positive.

`M`: Weight of capsule in pounds, both fuel and machine
`N`: Empty weight of capsule in pounds.  So, weight of fuel is M - N.

`G`: Gravity in miles / sec^2, down is positive.
`Z`: Exhaust velocity in miles / sec

`L`: time in seconds since start of simulation.
`K`: Burn rate for this 10 second turn, pounds of fuel per sec
`T`: Time left in this 10 second turn, in seconds.
`S`: Burn time in this 10 second turn, input to subroutine 420.

Subroutines:

330, Apply updates from one call to subroutine 420.

370, If you started descending and ended ascending, figure out whether you hit the surface in between.

420, Compute new velocity and altitude using the Tsiolkovsky rocket equation for S seconds:

`Q`: Fraction of initial mass that's burnt, i.e. 1 - mf / mo, exactly what we need for the Taylor series of `ln` in the rocket equation. Local to this subroutine.
`J`: Final velocity after S seconds, down is positive.  Return value.
`I`: Altitude after S seconds, up is positive.  Return value.
