### Salvo

The rules are _not_ explained by the program, so read carefully this description by Larry Siegel, the program author.

SALVO is played on a 10x10 grid or board using an x,y coordinate system. The player has 4 ships:
- battleship (5 squares)
- cruiser (3 squares)
- two destroyers (2 squares each)

The ships may be placed horizontally, vertically, or diagonally and must not overlap. The ships do not move during the game.

As long as any square of a battleship still survives, the player is allowed three shots, for a cruiser 2 shots, and for each destroyer 1 shot. Thus, at the beginning of the game the player has 3+2+1+1=7 shots. The players enters all of his shots and the computer tells what was hit. A shot is entered by its grid coordinates, x,y. The winner is the one who sinks all of the opponents ships.

Important note: Your ships are located and the computer’s ships are located on 2 _separate_ 10x10 boards.

Author of the program is Lawrence Siegel of Shaker Heights, Ohio.

---

As published in Basic Computer Games (1978):
- [Atari Archives](https://www.atariarchives.org/basicgames/showpage.php?page=142)
- [Annarchive](https://annarchive.com/files/Basic_Computer_Games_Microcomputer_Edition.pdf#page=157)

Downloaded from Vintage Basic at
http://www.vintage-basic.net/games.html

#### Porting Notes

As per the analysis in

https://forums.raspberrypi.com/viewtopic.php?p=1997950#p1997950

see also the earlier post

https://forums.raspberrypi.com/viewtopic.php?p=1994961#p1994961

in the same thread, there is a typo in later published versions of the SALVO Basic source code compared to the original edition of 101 Basic Computer Games.

This typo is interesting because it causes the program to play by a much weaker strategy while exhibiting no other obvious side effects. I would recommend changing the line 3970 in the Basic program back to the original

`3970 K(R,S)=K(R,S)+E(U)-2*INT(H(U)+.5)`

and to change the JavaScript program accordingly.
