using System;

namespace GameOfLife
{
    class GameOfLife
    {
        static void Main(string[] args)
        {
            try
            {
                int numGenerations;
          
                bool[,] initState = cellsFromFile(@"input.txt", out numGenerations);

                bool[,] finalState = runGame(initState, numGenerations);

                printCellsToConsole(finalState);

                Console.ReadLine();
            }
            catch (Exception e)
            {
                //deal with all sorts of exceptions. file reading, parsing etc.
                Console.WriteLine("Exception encountered: " + e.Message);
                Console.WriteLine(e.StackTrace);
                Console.ReadLine();
            }
        }

        /*
         * Read the number of generations to run a GOL game, number of rows, number of columns
         * and the inital state of the GOL cell matrix from the file at 'path'
         * 
         * @param path: location of the file
         * @param numGenerationsToRun: output variable from file specifiying number of generations to run the game
         * 
         * @return: 2D bool array with cell matrix from the file, TRUE if cell is alive at row,col; FALSE if dead.
         * 
         */
        private static bool[,] cellsFromFile(string path, out int numGenerationsToRun)
        {
            string line;
            System.IO.StreamReader file = new System.IO.StreamReader(path);

            //first 3 lines in file are num generations to run, num rows, and num cols respectively
            numGenerationsToRun = Int32.Parse(file.ReadLine());
            int numRows = Int32.Parse(file.ReadLine());
            int numCols = Int32.Parse(file.ReadLine());

            //read in initial cell matrix
            bool[,] initState = new bool[numRows, numCols];

            int row = 0;
            while ((line = file.ReadLine()) != null)
            {
                //check that line.length == num columns. if not throw exception.
                for (int col = 0; col < line.Length; col++)
                {

                    if (line[col] == 'o')
                    {
                        initState[row, col] = true;
                    }
                    else if (line[col] == '.')
                    {
                        initState[row, col] = false;
                    } else
                    {
                        //throw exception. unexpected char
                    }
                }
                row++;
            }

            //check that row == number of rows we expect. if not throw exception
            file.Close();

            return initState;
        }

        /*
         * Run the game of life algorithm on an initial state matrix for 'generation' generations
         * 
         * @param initial state: a 2D cell matrix containing initial state of our cells
         * @param generations: number of generations to run the game
         * 
         * @return: a 2D cell matrix containing final state of game after algorithm is run 'generations' generations.
         */
        public static bool[,] runGame(bool[,] initialState, int generations)
        {
            int numRows = initialState.GetUpperBound(0) + 1;
            int numCols = initialState.GetUpperBound(1) + 1;

            //use double buffered system. calc next live/dead cells from currBuf and store in nextBuff
            //swap buffers at end of iteration
            bool[,] currBuf = new bool[numRows, numCols];
            bool[,] nextBuf = new bool[numRows, numCols];

            //copy over stuff from initial state to front buffer
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numCols; col++)
                {
                    currBuf[row, col] = initialState[row, col];
                }
            }

            int neighborsAlive;
            bool cellAlive;
            bool[,] tmp; //for buffer swapping

            int genCounter = 0;
            while (genCounter < generations)
            {
                for (int row = 0; row < numRows; row++)
                {
                    for (int col = 0; col < numCols; col++)
                    {

                        //count how many neighbors adjacent to current cell are dead/alive
                        neighborsAlive = 0;
                        //NOTE: could use enums below if we wanted to make code more concise
                        //nw cell.
                        neighborsAlive += CellAliveOrDead(row - 1, col - 1, currBuf, numRows, numCols);
                        //w cell
                        neighborsAlive += CellAliveOrDead(row, col - 1, currBuf, numRows, numCols);
                        //sw cell
                        neighborsAlive += CellAliveOrDead(row + 1, col - 1, currBuf, numRows, numCols);
                        //n cell
                        neighborsAlive += CellAliveOrDead(row - 1, col, currBuf, numRows, numCols);
                        //s cell
                        neighborsAlive += CellAliveOrDead(row + 1, col, currBuf, numRows, numCols);
                        //ne cell
                        neighborsAlive += CellAliveOrDead(row - 1, col + 1, currBuf, numRows, numCols);
                        //e cell
                        neighborsAlive += CellAliveOrDead(row, col + 1, currBuf, numRows, numCols);
                        //se cell
                        neighborsAlive += CellAliveOrDead(row + 1, col + 1, currBuf, numRows, numCols);

                        cellAlive = currBuf[row, col];

                        if (cellAlive)
                        {
                            //cell alive in this generation
                            if (neighborsAlive < 2 || neighborsAlive > 3)
                            {
                                nextBuf[row, col] = false;
                            }
                            else
                            {
                                nextBuf[row, col] = true;
                            }
                        }
                        else
                        {
                            //cell currently dead
                            nextBuf[row, col] = (neighborsAlive == 3);
                        }
                    }
                }

                //swap buffers
                tmp = nextBuf;
                nextBuf = currBuf;
                currBuf = tmp;

                genCounter++;
            }


            return currBuf;
        }

        //return the state of the cell at row,col of the game of life matrix. out of bounds means that the cell is dead
        private static int CellAliveOrDead(int row, int col, bool[,] mat, int maxRows, int maxCols)
        {

            //assume that border of the matrix is made up of dead cells. 
            //alternate game rules include continuing the simulation
            //outside of the bounds of the initial state, but for simplicity of this game, assume we're in something like a petri dish
            if (row >= maxRows || col >= maxCols || row < 0 || col < 0)
            {
                return 0;
            }

            if (mat[row, col])
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public static void printCellsToConsole(bool[,] mat)
        {
            string str;
            for (int row = 0; row <= mat.GetUpperBound(0); row++)
            {
                //print a row in the matrix
                str = "";
                for (int col = 0; col <= mat.GetUpperBound(1); col++)
                {
                    if (mat[row, col])
                    {
                        str += "o";
                    }
                    else
                    {
                        str += ".";
                    }
                }
                Console.WriteLine(str);
            }
        }

        //UNIT TESTS:
        public static bool runGameUnitTests()
        {
            //Unit tests for runGame
            //set up various init state matrixes, run the runGame function and assert final state matrix is what we expect.
            //i normally would use a test framework like JUnit to do these assertions, but don't have java currently set up on my home machine

            //TEST SUITE 0:
            //Simple test cases with center as the test point:
            /* 
    
            ...
            .o. 
            .oo //live cell, 2 neighbors (live)

            ...
            .oo
            .oo //live cell, 3 neighbors (live)


            o.o
            .o.
            .oo //live cell, 4 neighbors (overcrowding, die)
            
            ...
            .o.
            .o. //live cell, 1 neighbor (not enough, die)

            ...
            ...
            ..o //dead cell, 1 neighbor (stay dead)
            
            ..o
            ..o
            ..o //dead cell, 3 neighbors (come alive)

            o.o
            ..o
            ..o //dead cell, 4 neighbors (stay dead)
            
            //also include tests for edge cases when cells are on edge of the cell matrix
            
            */

            //TEST SUITE 1: (example test code. might find a way to make the true/false parts more concise though cause it's a bit tedious to write out)
            bool[,] initState = new bool[,] {
                                                { false, false, false, false, false, false, true, false },
                                                { true, true, true, false, false, false, true, false },
                                                { false, false, false, false, false, false, false, false },
                                                { false, false, false, true, true, false, false, false },
                                                { false, false, false, true, true, false, false, false }
                                            };

            bool[,] finalState = runGame(initState, 1);

            /* assert that final state is:
            bool[,] initState = new bool[,] {
                                                { false, true, false, false, false, false, false, false },
                                                { false, true, false, false, false, true, true, true },
                                                { false, true, false, false, false, false, false, false },
                                                { false, false, false, false, false, false, false, false },
                                                { false, false, false, true, true, false, false, false }
                                                { false, false, false, true, true, false, false, false }
                                            }; */

            //TEST 2,3,4,5, etc.:
            //initState = new bool [,] ...
            //try matrixes with various different initial states, but single generation
            //finalState = runGame(initState, 1);
            //assert expected result

            //TEST 6, 7, 8 etc:
            //try various matrixes but this time with more than 1 generation.
            //i.e. finalState = runGame(initState, n);
            //assert expected result

            return true; 
        }

        public static bool runFileParsingUnitTests()
        {
            //run tests to parse files and ensure that the init state matrix we get out of the files is correct
            return true;
        }
    }
}
