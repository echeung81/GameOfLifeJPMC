This is the Game of Life coding exercise for Edward Cheung. I did this in C# sharp as I was time limited and I was most quickly setup for the C# environment under Visual Studio

To build:
Open the GameOfLife.sln file and build in debug mode in Visual Studio

To run:
Go into the bin/debug directory. Then:

1. Edit the input.txt file.
First line is: number of generations to run the game
Second line is: number of rows in the initial cell matrix
Third line is: number of columns in the initial matrix.

Then put the cell pattern in as shown in the example 'input.txt' file in the directory.

2. Run 'GameOfLife.exe'

Notes on Code:
I was a bit time limited in this exercise, but I implemented the code as asked by the assigment
and added ability to go arbitary generatons, and with matrices of arbritrary sizes.

Unit test outline occurs at the bottom of the class. One for the game logic, one for parsing code.
I would normally use something like JUnit for the unit tests.

Also, I might restructure the code to take command line input line by line etc, if i had more time.

Source code is in Program.cs