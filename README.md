# Task 4 - OOP

## Part 1
Create classes Square, Triangle, Circle, Shape.  Define methods for calculating area and perimeter for them.  Determine which of these classes can be abstract.


## Part 2
Create and describe a hierarchy for the entities Animal, Herbal, Herbivore, Carnivore, Alive, Wolf (predator), Rabbit (herbivore), Bear (omnivore), Rose, Grass.

 - all living beings must have a method that returns the name of the species.
 - each item of an animal must have a method that returns the name of a particular individual.
 - animals can be omnivores
 - herbivores must have an Eat() method that takes a plant as a parameter
 - predators must have an Eat() method that takes an animal as a parameter


## Part 3 (tasks 3 and 4 should be in the same repository)
Implement an application that accepts a natural number from the console and prints all Fibonacci numbers up to the entered number to the console. Implement the task using a loop.

34, 35 => 0 1 1 2 3 5 8 13 21 34


## Part 4
Implement an application that accepts a natural number from the console and prints all Fibonacci numbers up to the entered number to the console.  Implement the task using recursion.


## Part 5
Implement the Polynomial class.
The coefficients of the polynomial are set through the constructor.
Implement the ToString() method in this class, which returns a string representation of the class in the form of

 -3 + 4x^2 + 5x^5 – 12x^7

Implement overloading of binary operators +, -, *


## Part 6
Write an abstract class Lightstring.  Expand it with simple and colored Lightstring classes.  As elements of the Lightstring, use the class Bulb and Colored Bulb.

A colored light bulb has a color that can be red, yellow, blue or green.  For a colored light bulb, it should be possible to set the color using the Color type, and get the color as a string.
For a colored Lightstring, when creating, set the color of the light bulb depending on the multiplicity of the serial number (1 - red, 2 - yellow, 3 - green, 4 - blue, 5 - red, etc.) The number of lights in the Lightstring is set by the user.
The state of the bulb (on - off) is calculated from its serial number in the garland and the current minute of time.  If the current minute is even, only the bulbs with an even number in the garland are lit.  If the current minute is odd, the odd-numbered lights are on.

For both lightstrings, create a method that returns the current state of the lights (on/off) and the color in the case of a colored one.

Print the current state of both lightstrings to the console by calling the implemented class methods.
