Original source downloaded [from Vintage Basic](http://www.vintage-basic.net/games.html)

Conversion to [Python](https://www.python.org/about/)


## Porting notes

Variables:

* A: Available rallods (money)
* B: Current countrymen
* C: foreign_workers
* C1: foreign_workers_influx
* D: Available land (farmland=D-1000)
* F1: polution_deaths (last round)
* B5: died_contrymen (starvation + pollution)
* H: sm_sell_to_industry
* I: distributed_rallods
* J: planted_sq in a round
* K: pollution_control_spendings in a round
* X5: years in office
* N5: YEARS_IN_TERM - how many years one term in office has
* P1: population_change (positive means people come, negative means people leave)
* W: land_buy_price
* V9: planting_cost
* U2: crop_loss
* V1-V2: Earnings from tourist trade
* V3: tourism_earnings
* T1: crop_loss_last_year
* W: land_buy_price
* X: only show an error message once

Functions:

* `RND(1)`: `random.random()`
* `INT(...)`: `int(...)`
* `ABS(...)`: `abs(...)`

Bugs: See [53 King README](../README.md)

Implicit knowledge:

* `COST_OF_LIVING`: One countryman needs 100 for food. Otherwise they will die of starvation
* `COST_OF_FUNERAL`: A funeral costs 9
