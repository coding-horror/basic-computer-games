Original source downloaded [from Vintage Basic](http://www.vintage-basic.net/games.html)

Conversion to [Oracle Java](https://openjdk.java.net/) by [Taciano Dreckmann Perez](https://github.com/taciano-perez).

Overview of Java classes:
- SuperStarTrekInstructions: displays game instructions
- SuperStarTrekGame: main game class
- GalaxyMap: map of the galaxy divided in quadrants and sectors, containing stars, bases, klingons, and the Enterprise
- Enterprise: the starship Enterprise
- GameCallback: interface allowing other classes to interact with the game class without circular dependencies 
- Util: utility methods

[This video](https://www.youtube.com/watch?v=cU3NKOnRNCI) describes the approach and the different steps followed to translate the game.