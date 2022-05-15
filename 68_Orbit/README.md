### Orbit

ORBIT challenges you to visualize spacial positions in polar coordinates. The object is to detonate a Photon explosive within a certain distance of a germ laden Romulan spaceship. This ship is orbiting a planet at a constant altitude and orbital rate (degrees/hour). The location of the ship is hidden by a device that renders the ship invisible, but after each bomb you are told how close to the enemy ship your bomb exploded. The challenge is to hit an invisible moving target with a limited number of shots.

The planet can be replaced by a point at its center (called the origin); then the ship’s position can be given as a distance form the origin and an angle between its position and the eastern edge of the planet.

```
direction
of orbit    <       ^ ship
              \     ╱
                \  ╱ <
                 |╱   \
                 ╱      \
                ╱         \
               ╱           | angle
              ╱           /
             ╱          /
            ╱         /
           ╱——————————————————— E

```

The distance of the bomb from the ship is computed using the law of consines. The law of cosines states:

```
D = SQUAREROOT( R**2 + D1**2 - 2*R*D1*COS(A-A1) )
```

Where D is the distance between the ship and the bomb, R is the altitude of the ship, D1 is the altitude of the bomb, and A-A1 is the angle between the ship and the bomb.


```
                 bomb  <
                        ╲                   ^ ship
                         ╲                  ╱
                          ╲                ╱ <
                           ╲              ╱   \
                        D1  ╲            ╱      \
                             ╲        R ╱         \
                              ╲   A1   ╱           | A
                               ╲⌄——— ◝╱           /
                                ╲    ╱ \        /
                                 ╲  ╱   \      /
                                  ╲╱───────────────────── E

```

ORBIT was originally called SPACE WAR and was written by Jeff Lederer of Project SOLO Pittsburgh, Pennsylvania.

---

As published in Basic Computer Games (1978):
- [Atari Archives](https://www.atariarchives.org/basicgames/showpage.php?page=124)
- [Annarchive](https://annarchive.com/files/Basic_Computer_Games_Microcomputer_Edition.pdf#page=139)

Downloaded from Vintage Basic at
http://www.vintage-basic.net/games.html

#### Porting Notes

(please note any difficulties or challenges in porting here)
