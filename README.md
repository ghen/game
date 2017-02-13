# Game #

This project is a sample implementation for the test programming problem described bellow.

>> Please note, that this project does not use any external libraries that would otherwise simplify our life a lot, such as Newtonsoft JSON, or NUnit Mock, etc.

## Problem ##

Consider the following children’s game:
* n children stand around a circle.
* Starting with a given child and working clockwise, each child gets a sequential number, which we will refer to as its id.
* Then starting with the first child, they count out from 1 until k. The k’th child is now out and leaves the circle. The count starts again with the child immediately next to the eliminated one.
* Children are so removed from the circle one by one. The winner is the last child left standing.

Write some code which, when given n and k, calculates:
* the sequence of children as they are eliminated, and
* the id of the winning child.

Program should use following API to get game parameters (n and k) and provide game results:
* [GET] https://7annld7mde.execute-api.ap-southeast-2.amazonaws.com/main/game 
  * to get the children count (n), elimination count (k), and the game id
* [POST] https://7annld7mde.execute-api.ap-southeast-2.amazonaws.com/main/game/{id}
  * to provide game results back – the winning child and order of elimination

## Example ##

[GET] https://7annld7mde.execute-api.ap-southeast-2.amazonaws.com/main/game
Accept: application/json
```
{
   "id": 81381,
   "children_count": 3,
   "eliminate_each": 1
}
```

The API returned number of children (n) 3 and eliminate every (k) 1. The game logic should run and identify winning child and order elimination. After that it should provide results back via post API as follows:

[POST] https://7annld7mde.execute-api.ap-southeast-2.amazonaws.com/main/game/81381
Content-Type: application/json
```
{
   "id": 81381,
   "last_child": 3,
   "order_of_elimination": [1,2]
}
```

# Support #

Send your questions to Eugene (yarshevich [att] gmail [dott] com).
Git: https://github.com/ghen/trains
