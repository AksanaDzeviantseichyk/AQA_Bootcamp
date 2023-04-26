# Task5

#### Write positive unit tests using NUnit library for public methods Open() and GetCurrentField() of 'GameProcessor'  class.

'Console' project contains logic about input data from console and output data to console.

'Core' project contains main game logic.

This game has the same rules to classic Windows Minesweeper video game.

There are 2 enums:
1. PointState – type of game field cell, possible values:|
2. GameState – current game state
 
'GameProcessor' - main class for this game.
 
It has public members:
1. property GameState - current state of game. 
2. Constructor. 
It receives an initial field with mine locations (bool array) as a parameter
3. Method Open(). 
This method allows you to open a new cell. It returns a new GameState after this action.
It is possible to try opening cells those are already opened.
This method can’t be called if we already finished game (if GameState is not ‘Active’).
4. Method GetCurrentField(). 
This methods returns current game field with current state for every cell.

