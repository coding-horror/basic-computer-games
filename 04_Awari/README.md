### Awari

Awari is an ancient African game played with seven sticks and thirty-six stones or beans laid out as shown above. The board is divided into six compartments or pits on each side. In addition, there are two special home pits at the ends.

A move is made by taking all the beans from any (non-empty) pit on your own side. Starting from the pit to the right of this one, these beans are ‘sown’ one in each pit working around the board anticlockwise.

A turn consists of one or two moves. If the last bean of your move is sown in your own home you may take a second move.

If the last bean sown in a move lands in an empty pit, provided that the opposite pit is not empty, all the beans in the opposite pit, together with the last bean sown are ‘captured’ and moved to the player’s home.

When either side is empty, the game is finished. The player with the most beans in his home has won.

In the computer version, the board is printed as 14 numbers representing the 14 pits.

```
    3   3   3   3   3   3
0                           0
    3   3   3   3   3   3
```

The pits on your (lower) side are numbered 1-6 from left to right. The pits on my (the computer’s) side are numbered from my left (your right).

To make a move you type in the number of a pit. If the last bean lands in your home, the computer types ‘AGAIN?’ and then you type in your second move.

The computer’s move is typed, followed by a diagram of the board in its new state. The computer always offers you the first move. This is considered to be a slight advantage.

There is a learning mechanism in the program that causes the play of the computer to improve as it playes more games.

The original version of Awari is adopted from one originally written by Geoff Wyvill of Bradford, Yorkshire, England.

---

As published in Basic Computer Games (1978)
- [Atari Archives](https://www.atariarchives.org/basicgames/showpage.php?page=6)
- [Annarchive](https://annarchive.com/files/Basic_Computer_Games_Microcomputer_Edition.pdf#page=21)

Downloaded from Vintage Basic at
http://www.vintage-basic.net/games.html

#### Porting Notes

(please note any difficulties or challenges in porting here)

