# Tasks

Started after movement and display of stats was finished (no energy management or collision detection or anything).

- [x] klingon movement
- [x] klingon firing, game over etc
- [x] add intro
- [x] add entering (and starting in) sector headers
- [x] conditions and danger messages
- [x] remove energy on move
- [x] shields
    - [x] shield control
    - [x] shield hit absorption
- [x] subsystem damage
    - [x] and support for reports
- [x] random system damage or repairs on move
- [x] lrs?
- [x] stranded...
- [ ] stop before hitting an object
    - when moving across a sector, the enterprise should stop before it runs into something
    - the current move is a jump, which makes this problematic. would need to rewrite it
    - also, movement courses could be floats, according to the instructions, allowing for more precise movement and aiming
- [x] better command reading - support entering multiple values on a line (e.g. nav 3 0.1)
- [x] starbases
    - [x] proximity detection for docking
    - [x] repair on damage control
    - [x] protection from shots
- [ ] weapons
    - [ ] phasers
    - [ ] torpedoes
- [ ] computer
    - [x] 0 - output of all short and long range scans (requires tracking if a system has been scanned)
    - [ ] 1 - klingons, starbases, stardate and damage control
    - [ ] 2 - photon torpedo data: direction and distance to all local klingons
    - [ ] 3 - starbase distance and dir locally
    - [ ] 4 - direction/distance calculator (useful for nav actions I guess)
    - [x] 5 - galactic name map
 
- [ ] restarting the game
- [ ] time progression
    - check all areas where time should move, and adjust accordingly
- [ ] intro instructions
- [ ] victory
