using HoopStackWebsite.Models.Level;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace HoopStackWebsite.Solver
{
    public class Move //a single move
    {
        public int step; //step when the move was made
        public int from; //stack hoop was moved from
        public int to; //stack hoop was moved to
        public string color; //color of the moved hoop

        //INF loop is signalled by from and to == -1
        //ERR is signalled by from and to == -2

        public Move(int step, int from, int to, string color)
        {
            this.step = step;
            this.from = from;
            this.to = to;
            this.color = color;
        }
    }

    public class HoopStackSolver
    {
        public static List<List<string>> init() //manual stack input
        {
            List<List<string>> stacks = new List<List<string>>();
            int numStacks = 0;
            Console.WriteLine("Input Number of Stacks:");
            numStacks = Convert.ToInt32(Console.ReadLine());
            string input;
            for (int i = 0; i < numStacks; i++)
            {
                List<string> temp = new List<string>();
                Console.WriteLine("Input Colors From Bottom to Top For Stack # {0} Separated By ','\n", i + 1);
                input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    stacks.Add(temp);
                    break;
                }
                string[] words = input.Split(",");
                foreach (var word in words)
                    temp.Add(word);
                stacks.Add(temp);
            }
            return stacks;
        }

        public static void printStacks(List<List<string>> stacks) //print the current order of the stacks
        {
            Console.WriteLine("\n\t____Current Stacks____");
            for (int i = 0; i < stacks.Count; i++)
            {
                Console.Write("\tStack {0}: \t", i);
                if (stacks[i].Count == 0)
                    Console.WriteLine("Empty");
                else
                {
                    for (int j = 0; j < stacks[i].Count; j++)
                    {
                        if (j == stacks[i].Count - 1)
                            Console.WriteLine(stacks[i][j]);
                        else
                            Console.Write("{0} -> ", stacks[i][j]);
                    }
                }
            }
            Console.WriteLine();
        }

        public static void printInstructions(List<string> ins)
        {
            Console.WriteLine("\n\t____Instructions____");
            for (int i = 0; i < ins.Count; i++)
            {
                Console.WriteLine("\t" + ins[i]);
            }
            Console.WriteLine();
        }

        public static List<Move> numMoves(List<List<string>> stacks, int maxHeight, int step) //return all possible moves at current step
        {
            List<Move> posMoves = new List<Move>();
            for (int i = 0; i < stacks.Count; i++)
            {
                List<string> from = stacks[i];
                int fromSize = from.Count;
                string fromColor = from[fromSize - 1];
                for (int j = 0; j < stacks.Count; j++)
                {
                    List<string> to = stacks[j];
                    int toSize = to.Count;
                    if (from[fromSize - 1] == to[toSize - 1] && i != j && toSize < maxHeight) //if two dif stacks have common TOS & < maxHeight
                    {
                        //List<int> move = new List<int>() { i, j };
                        Move posMove = new Move(step, i, j, fromColor);
                        posMoves.Add(posMove);
                    }
                }
            }
            return posMoves;
        }

        public static void printSol(List<Move> solution) //print moves of the solution
        {
            Console.WriteLine("\n\t____Solution____");
            foreach (var move in solution)
            {
                Console.WriteLine("Step {0}: Move '{1}' From {2} To {3}", (move.step), move.color, move.from, move.to);
            }
        }

        public static bool check(List<List<string>> stacks) //checks if the puzzle is solved
        {
            List<string> usedColors = new List<string>();
            for (int i = 0; i < stacks.Count; i++)
            {
                if (stacks[i].Count != 0)
                {
                    string color = stacks[i][0];
                    bool notUsed = true;
                    foreach (var col in usedColors)
                    {
                        if (col == color)
                            notUsed = false;
                    }
                    if (notUsed)
                        usedColors.Add(color);
                    else
                        return false;
                    for (int j = 0; j < stacks[i].Count; j++)
                    {
                        if (stacks[i][j] != color)
                            return false;
                    }
                }
            }
            return true;
        }

        public static Move move(ref List<List<string>> stacks, int step, int from, int to) //from a hoop FROM one stack TO another
        {
            string fromColor= "";
            string toColor = "";
            int fromSize = stacks[from].Count;
            int toSize = stacks[to].Count;
            Move move;
            if (stacks[from].Count != 0)
                fromColor = stacks[from][fromSize - 1];
            if (stacks[to].Count != 0)
                toColor = stacks[to][toSize - 1];
            if (fromColor == toColor || toColor == "")
            {
                stacks[from].RemoveAt(fromSize - 1);
                stacks[to].Add(fromColor);
                move = new Move(step, from, to, fromColor);
            }
            else
            {
                Console.Write("Move Not Possible: ");
                move = null;
            }
            Console.WriteLine("Move '{0}' from Stack {1} to Stack {2}", fromColor, from, to);
            return move;
        }

        public static bool solveLevel(Level level) //algorithm to solve the puzzle to solve a Level obj 
        {
            List<List<string>> stacks = level.Stacks;
            List<List<Move>> solutions = level.Solutions;
            List<Move> wrongMoves = level.WrongMoves;

            List<List<string>> stacksCopy = new List<List<string>>(); //copy of stacks
            foreach (List<string> s in stacks)
            {
                List<string> temp = new List<string>();
                foreach (var col in s)
                {
                    temp.Add(col);
                }
                stacksCopy.Add(temp);
            }
            List<string> instructions = new List<string>(); //instructions of cur solution
            List<Move> sol = new List<Move>(); //moves of the cur solution

            int prevFrom = -1; //from stack of prev step
            int prevTo = -1; //to stack of prev step
            int step = 0; //counter of steps of the solution
            int numSolutions = solutions.Count;

            if (numSolutions != 0) //if there a prev solution, follow steps until an error of INF or ERR
            {
                List<Move> prevSol = solutions[numSolutions - 1]; //steps of prev sol
                int solSize = prevSol.Count;

                if (prevSol[solSize - 1].color == "INF" || // inf
                    prevSol[solSize - 1].color == "ERR")   // err
                {
                    solSize = prevSol.Count - 2;
                }

                Move wrongMove = prevSol[solSize];  //get the wrong move of prev sol and add it to wrongMoves
                wrongMoves.Add(wrongMove);
                Console.WriteLine("Wrong Move: Step {0}, From {1}, To {2}, Color {3}", wrongMove.step, wrongMove.from, wrongMove.to, wrongMove.color);

                for (step = 0; step < solSize; step++) //do moves of prev sol until the recent wrong move
                {
                    Move solMove = prevSol[step];
                    bool makeMove = true;
                    foreach (var move in wrongMoves) //check the current move of prevsol is not a wrong move
                    {
                        if (move.step == solMove.step && move.from == solMove.from && move.to == solMove.to && move.color == solMove.color)
                        {
                            makeMove = false;
                            break;
                        }
                    }
                    if (makeMove)
                    {
                        Move m = move(ref stacks, solMove.step, solMove.from, solMove.to);
                        string instruction = "Step " + (m.step) + ":  Move '" + m.color + "' from Stack " + m.from + " to Stack " + m.to;
                        instructions.Add(instruction);
                        //Console.WriteLine(instruction);
                        sol.Add(m);
                        prevFrom = m.from;
                        prevTo = m.to;
                    }
                    else break;
                }
            }

            bool inf = false; //true if inf loop
            bool moveMade = false; //true if a move has been made in the switch loop, if false then sol is bricked
            int switchVal = -1;  //switch statement 1(empty) 2(same stack) or 3(shortest stack)

            bool empty = false; //if a stack is empty 
            bool sameColor = false;  //if a stack is made of 1 color only 

            List<int> sameStack = new List<int>(); //vector of indexes of same color stacks
            List<int> emptyStack = new List<int>();  //vector of indexes of empty stacks
            List<int> stackSizes = new List<int>();  //vector increasing stack sizes
            List<int> stackSizesIndexes = new List<int>(); //vector of the indexes of stackSizes 

            int minStack = 100; //index of smallest stack
            int minSize = 100; //size of smallest stack
            string minTos = ""; //tos of minstack
            int maxHeight = -1; //max height of a stack
            int numStacks = stacks.Count; //number of stacks to solve
            int count; //to prevent errors
            int tempCount;

            //vars for empty case
            string commonTos; //most common consecutive tos color
            int maxConsec;  //count for commonTos color

            int loopCount = 1; //count of while loops in solution
            while (!check(stacks))
            {
                Console.WriteLine("Loop: {0}", loopCount++);

                //clear old lists
                sameStack.Clear();
                emptyStack.Clear();
                stackSizes.Clear();
                stackSizesIndexes.Clear();

                //check if any stacks empty
                for (int i = 0; i < numStacks; i++)
                { //find any empty stacks
                    if (stacks[i].Count == 0)
                    {
                        minStack = i;
                        minSize = 0;
                        empty = true;
                        Console.WriteLine("Empty: {0}", minStack);
                        emptyStack.Add(minStack);
                    }
                }
                if (!empty) //if no empty stacks, check for same colors and then small sizes
                {
                    for (int j = 0; j < numStacks; j++)
                    {
                        bool sameColorCheck = true;
                        for (int k = 0; k < stacks[j].Count - 1; k++)
                        {
                            if (stacks[j].Count != 0)
                            {
                                if (stacks[j][k] != stacks[j][k + 1]) sameColorCheck = false;
                            }
                        }
                        if (sameColorCheck)
                        {
                            //check that the stack with the same color has TOS colors that can be moved there 
                            count = 0;
                            string tempCol = stacks[j][stacks[j].Count - 1];
                            foreach (var s in stacks)
                            { //for all stacks that are not the sameColor stack
                                if (count != j && s.Count != 0)
                                {
                                    //if another stack has the same tos color as samecolor stack, then add the stack to samecolor
                                    if (tempCol == s[s.Count - 1])
                                    {
                                        sameColor = true;
                                        sameStack.Add(j);
                                        Console.WriteLine("SameColorStack: {0}", j);
                                        break;
                                    }
                                }
                                count++;
                            }
                        }
                    }
                }

                //check stack sizes
                count = 0;
                foreach (var s in stacks)
                { //add all stack sizes, then sort them 
                    stackSizes.Add(s.Count);
                    stackSizesIndexes.Add(count++);
                }
                //bubble sort sizes and coresponsing indexes
                for (int z = 0; z < numStacks; z++)
                {
                    for (int x = 0; x < numStacks - 1; x++)
                    {
                        if (stackSizes[x] > stackSizes[x + 1])
                        {
                            int size = stackSizes[x];
                            int index = stackSizesIndexes[x];
                            stackSizes[x] = stackSizes[x + 1];
                            stackSizesIndexes[x] = stackSizesIndexes[x + 1];
                            stackSizes[x + 1] = size;
                            stackSizesIndexes[x + 1] = index;
                        }
                    }
                }
                maxHeight = stackSizes[stackSizes.Count - 1]; //set maxheight

                // set switch value
                if (empty) switchVal = 1; //rule 1
                else if (sameColor) switchVal = 2; //rule 2
                else switchVal = 3; //rule 3 

                switch (switchVal)
                {
                    case 1: //empty
                        Console.WriteLine("Rule 1");
                        foreach (var e in emptyStack)
                        {
                            //update minStack info to empty 
                            minStack = e;
                            minSize = 0;
                            minTos = "";
                            //find most common consec tos color that is not same stack
                            commonTos = "";
                            maxConsec = 0;
                            foreach (var s in stacks)
                            { //for every stack 
                                if (s.Count != 0)
                                { //if its not the empty one
                                    int tempCount1 = 1; //initially 1
                                    string tempTos = s[s.Count - 1]; //temp tos consec color
                                    for (int i = s.Count - 2; i >= 0; i--)
                                    { //start on the 2nd from the top color 
                                        if (s[i] == tempTos)
                                        { //if color after tempTos matches, it is consec
                                            tempCount1++;
                                        }
                                    }
                                    if (tempCount1 > maxConsec)
                                    { //if bigger consec color found, update
                                      //if tempCount != stack size, then it is not a same color stack
                                        if (tempCount1 != s.Count)
                                        {
                                            maxConsec = tempCount1;
                                            commonTos = tempTos;
                                        }
                                    }
                                }
                            }
                            //move most common consec tos color to empty stack
                            count = 0; //vector count
                            foreach (var s in stacks)
                            {
                                if (count != minStack)
                                { //dont move from the same stack
                                    string color = s[s.Count - 1];
                                    while (color == commonTos)
                                    {
                                        //check if infinite move 
                                        if (count == prevTo && minStack == prevFrom)
                                        { //if current FROM == prevTo && current TO == prevFrom, must be an infinite loop 
                                            inf = true;
                                            Console.WriteLine("INF 1");
                                            break;
                                        }
                                        //check next move is not in wrong moves 
                                        bool makeMove = true;
                                        for (int j = 0; j < wrongMoves.Count; j++)
                                        { //for all wrong moves 
                                            Move wrongMove = wrongMoves[j];
                                            if (step == wrongMove.step && count == wrongMove.from && minStack == wrongMove.to)
                                            { //if current move is any of the wrong ones, break	(dont check colors bc minstack is empty)
                                                makeMove = false;
                                                break;
                                            }
                                        }
                                        if (!makeMove) break; //if next move is a wrong move, break
                                        Move m = move(ref stacks, step, count, minStack);
                                        string instruction = "Step " + (m.step) + ":  Move '" + m.color + "' from Stack " + m.from + " to Stack " + m.to;
                                        instructions.Add(instruction);
                                        //Console.WriteLine(instruction);
                                        step++; //inc step counter
                                        sol.Add(m);
                                        prevFrom = count;   //update previous moves
                                        prevTo = minStack;
                                        moveMade = true;
                                        if (stacks[count].Count == 0) break;
                                        color = stacks[count][stacks[count].Count - 1]; //update color for while loop
                                    }
                                }
                                count++;
                            }
                        }
                        empty = false; //reset empty to false after done
                        break;

                    case 2: //same color
                        Console.WriteLine("Rule 2");
                        count = 0;
                        foreach (var same in sameStack)
                        {
                            //update minstack info
                            if (stacks[same].Count != 0)
                            { //check to not update info from empty stack; causes segfault
                                minStack = same;
                                minSize = stacks[minStack].Count;
                                minTos = stacks[minStack][stacks[minStack].Count - 1];
                            }
                            //move any consecutive tos colors to the sameColor stack
                            tempCount = 0;
                            foreach (var s in stacks)
                            { //all stacks 
                                if (s.Count != 0)
                                { //empty check; seg fault?
                                    string tempTos = stacks[tempCount][^1];
                                    if (tempTos == minTos && tempCount != minStack)
                                    {
                                        while (tempTos == minTos && stacks[tempCount].Count != 0)
                                        { //for all consecutive matching colors 
                                            if (stacks[minStack].Count < maxHeight)
                                            { //if colors match and stack size < maxheight
                                              //check if infinite move 
                                                if (tempCount == prevTo && minStack == prevFrom)
                                                { //if current FROM == prevTo && current TO == prevFrom, must be an infinite loop 
                                                    inf = true;
                                                    Console.WriteLine("INF 2");
                                                    break;
                                                }
                                                //check next move is not in wrong moves 
                                                bool makeMove = true;
                                                for (int j = 0; j < wrongMoves.Count; j++)
                                                { //for all wrong moves 
                                                    Move wrongMove = wrongMoves[j];
                                                    if (step == wrongMove.step && tempCount == wrongMove.from && minStack == wrongMove.to && stacks[minStack][^1] == wrongMove.color)
                                                    { //if current move is any of the wrong ones, break
                                                        makeMove = false;
                                                        break;
                                                    }
                                                }
                                                if (!makeMove) break; //if next move is a wrong move, break
                                                Move m = move(ref stacks, step, tempCount, minStack);
                                                string instruction = "Step " + (m.step) + ":  Move '" + m.color + "' from Stack " + m.from + " to Stack " + m.to;
                                                instructions.Add(instruction);
                                                //Console.WriteLine(instruction);
                                                step++; //inc step counter
                                                sol.Add(m);
                                                prevFrom = tempCount;   //update previous moves
                                                prevTo = minStack;
                                                moveMade = true;
                                            }
                                            if (stacks[tempCount].Count != 0) tempTos = stacks[tempCount][^1]; //update temp tos after move
                                        }
                                    }
                                }
                                tempCount++;
                            }
                            count++;
                        }
                        sameColor = false;
                        break;

                    case 3: //minstack
                        Console.WriteLine("Rule 3");
                        //get shortest stack from stackSizes 
                        //for all stackSizes of minSize (for multiple stacks of same min size)
                        count = 0;
                        foreach (var index in stackSizesIndexes)
                        {
                            //for each stack of that size 
                            //start with smallest size stack
                            minSize = stackSizes[count++]; //get the smallest stack size 
                            minStack = index;
                            minTos = stacks[minStack][stacks[minStack].Count - 1];
                            maxConsec = 0; //most consec tos colors
                            commonTos = ""; //the max consec color
                            //check that stack has common TOS colors to move to
                            bool hasMoves = false;
                            int stackCount = 0;
                            foreach (var s in stacks)
                            {
                                if (stackCount++ != minStack && s[^1] == minTos)
                                {
                                    hasMoves = true;
                                    break;
                                }
                            }
                            if (hasMoves) break;
                        }
                        //move commonTos colors to minstack
                        tempCount = 0;
                        bool movedToMinStack = false;
                        foreach (var s in stacks)
                        { //all stacks 
                            string tempTos = s[^1]; // s[s.Count - 1] ==  s[^1]
                            if (tempTos == minTos && tempCount != minStack)
                            { //if stack colors match and stack is not minstack
                                while (tempTos == minTos && stacks[tempCount].Count != 0)
                                { //for all consecutive matching colors 
                                    if (stacks[minStack].Count < maxHeight)
                                    { //if colors match and stack size < maxheight
                                        if (tempCount == prevTo && minStack == prevFrom)
                                        { //if current FROM == prevTo && current TO == prevFrom, must be an infinite loop 
                                            inf = true;
                                            Console.WriteLine("INF 3A");
                                            break;
                                        }
                                        //check next move is not in wrong moves 
                                        bool makeMove = true;
                                        for (int j = 0; j < wrongMoves.Count; j++)
                                        { //for all wrong moves 
                                            Move wrongMove = wrongMoves[j];
                                            if (step == wrongMove.step && tempCount == wrongMove.from && minStack == wrongMove.to && stacks[minStack][^1] == wrongMove.color) //stacks[minStack][stacks[minStack].Count - 1] == stacks[minStack][^1]
                                            { //if current move is any of the wrong ones, break
                                                makeMove = false;
                                                break;
                                            }
                                        }
                                        if (!makeMove) break; //if next move is a wrong move, break Move m = move(stacks, step, tempCount, minStack);
                                        Move m = move(ref stacks, step, tempCount, minStack);
                                        string instruction = "Step " + (m.step) + ":  Move '" + m.color + "' from Stack " + m.from + " to Stack " + m.to;
                                        instructions.Add(instruction);
                                        //Console.WriteLine(instruction);
                                        movedToMinStack = true;
                                        step++; //inc step counter
                                        sol.Add(m);
                                        prevFrom = tempCount;   //update previous moves
                                        prevTo = minStack;
                                        moveMade = true;
                                    }
                                    else break;
                                    if (stacks[tempCount].Count != 0)
                                        tempTos = stacks[tempCount][^1]; //update temp tos after move
                                }
                            }
                            tempCount++;
                        }
                        //if no move to shortest stack, do a possible move that is not a wrong move ? 
                        if (!movedToMinStack)
                        {
                            inf = false; //reset inf bool to false in case rule 3A set it to true
                            List<Move> posMoves = numMoves(stacks, maxHeight, step);
                            foreach (var posMove in posMoves)
                            { //for all possible moves at this step
                                bool makeMove = true;
                                for (int i = 0; i < wrongMoves.Count; i++)
                                { //for all wrong moves 
                                    Move wrongMove = wrongMoves[i];
                                    if (step == wrongMove.step && posMove.from == wrongMove.from && posMove.to == wrongMove.to && posMove.color == wrongMove.color)
                                    { //if current move is any of the wrong ones, break
                                        makeMove = false;
                                        break;
                                    }
                                }
                                if (makeMove)
                                {
                                    if (posMove.from == prevTo && posMove.to == prevFrom)
                                    { //if current FROM == prevTo && current TO == prevFrom, must be an infinite loop 
                                        inf = true;
                                        Console.WriteLine("INF 3B");
                                        break;
                                    }
                                    Move m = move(ref stacks, step, posMove.from, posMove.to);
                                    string instruction = "Step " + (m.step) + ":  Move '" + m.color + "' from Stack " + m.from + " to Stack " + m.to;
                                    instructions.Add(instruction);
                                    //Console.WriteLine(instruction);
                                    step++; //inc step counter
                                    sol.Add(m);
                                    prevFrom = posMove.from;   //update previous moves
                                    prevTo = posMove.to;
                                    moveMade = true;
                                    break;
                                }
                            }
                        }
                        break;
                }

                //if infinite loop detected, add "INF" to end of solution and end solution
                if (inf)
                {
                    Console.WriteLine("INF");
                    Move infMove = new Move(step, -1, -1, "INF");
                    sol.Add(infMove);
                    printSol(sol);
                    printStacks(stacks); //temp print
                    break;
                }
                //if solution is bricked (no moves made since last loop), add "ERR" to end of solution and end solution
                if (!moveMade)
                {
                    Console.WriteLine("ERR");
                    Move errMove = new Move(step, -2, -2, "ERR");
                    sol.Add(errMove);
                    printSol(sol);
                    printStacks(stacks); //temp print
                    break;
                }
                else
                { //if move was made, reset moveMade to false for next loop
                    moveMade = false;
                }
                printSol(sol);
                printStacks(stacks); //temp print
                if (loopCount == 20)
                    break; //temp break
            }

            if (check(stacks))
            { //if correct, stop and print instructions
                printInstructions(instructions);
                printStacks(stacks);
                Console.WriteLine("\n\n\t CORRECT \n\n");
                //set the correct instructions to the level and update other vars
                level.Instructions = instructions;
                level.Stacks = stacksCopy;
                level.Solutions = solutions;
                level.WrongMoves = wrongMoves;
                level.Error = "No Error";
                return true;
            }
            else
            { //if incorrect solution was found, try again with recursion
                Console.WriteLine("\n\n\t INCORRECT \n\n");
                solutions.Add(sol); //add prev incorrect solution to solutions
                //reset the levels stacks to the original copied stacks, update solutions and wrongmoves
                level.Stacks = stacksCopy;
                level.Solutions = solutions;
                level.WrongMoves = wrongMoves;
                level.Error = solutions[^1][^1].color;     //error is the 'Color' of the last move in the last solution
                return solveLevel(level);
            }
        }

        /*static void Main(string[] args)
        {
            List<List<string>> stacks; //the stacks of the puzzle
            Moves solutions = new Moves(); //holds all of the attempted solutions of the puzzle
            List<Move> wrongMoves = new List<Move>(); //holds all of the wrong moves for the puzzle 

            //set input 475 w
            *//*List<string> s1 = new List<string>() { "Light Blue", "Light Blue", "Purple", "Purple" };
            List<string> s2 = new List<string>() { "Light Green", "Blue", "Blue", "Yellow", "Yellow" };
            List<string> s3 = new List<string>() { "Pink", "Dark Green", "Dark Green" };
            List<string> s4 = new List<string>() { "Purple", "Dark Green", "Dark Green" };
            List<string> s5 = new List<string>() { "Red", "Orange", "Orange", "Light Green", "Light Green" };
            List<string> s6 = new List<string>() { "Blue", "Red", "Red", "Light Blue", "Light Blue" };
            List<string> s7 = new List<string>() { "Orange", "Yellow", "Yellow", "Pink", "Light Blue" };
            List<string> s8 = new List<string>() { "Dark Green", "Pink", "Pink", "Purple", "Purple" };
            List<string> s9 = new List<string>() { "Yellow", "Red", "Red", "Blue", "Blue" };
            List<string> s10 = new List<string>() { "Pink", "Orange", "Orange", "Light Green", "Light Green" };
            stacks = new List<List<string>>() {s1, s2, s3, s4, s5, s6, s7, s8, s9, s10};*//*

            //set input 482
            *//*List<string> s1 = new List<string>() { "Light Blue", "Blue", "Light Blue", "Purple", "Purple" };
            List<string> s2 = new List<string>() { "Light Green", "Red", "Red" };
            List<string> s3 = new List<string>() { "Pink", "Light Blue", "Light Blue", "Light Green", "Light Green" };
            List<string> s4 = new List<string>() { "Purple" };
            List<string> s5 = new List<string>() { "Red", "Light Green", "Light Green", "Red", "Red", "Pink"};
            List<string> s6 = new List<string>() { "Blue", "Pink", "Pink", "Light Green", "Blue", "Blue" };
            List<string> s7 = new List<string>() { "Orange", "Blue", "Blue", "Orange", "Purple" };
            List<string> s8 = new List<string>() { "Orange", "Orange", "Light Blue", "Light Blue", "Purple", "Purple" };
            List<string> s9 = new List<string>() { "Red", "Pink", "Pink", "Orange", "Orange" };
            stacks = new List<List<string>>() {s1, s2, s3, s4, s5, s6, s7, s8, s9};*//*

            //485  w
            *//*List<string> s1 = new List<string>() { "Light Blue", "Purple", "Purple", "Light Green", "Pink", "Pink"};
            List<string> s2 = new List<string>() { "Light Green", "Light Blue" };
            List<string> s3 = new List<string>() { "Pink", "Pink", "Purple", "Light Blue", "Light Blue" };
            List<string> s4 = new List<string>() { "Purple", "Light Blue", "Light Blue", "Light Green", "Light Green" };
            List<string> s5 = new List<string>() { "Red", "Red", "Pink", "Pink", "Red", "Light Green" };
            List<string> s6 = new List<string>() { "Red", "Purple", "Purple", "Red", "Red", "Light Green" };
            stacks = new List<List<string>>() {s1, s2, s3, s4, s5, s6};*//*

            //487 w
            *//*List<string> s1 = new List<string>() { "Light Blue", "Light Green", "Light Green", "Light Blue", "Light Blue" };
            List<string> s2 = new List<string>() { "Light Green", "Purple", "Purple" };
            List<string> s3 = new List<string>() { "Pink", "Light Blue", "Pink", "Purple", "Purple" };
            List<string> s4 = new List<string>() { "Purple", "Light Blue", "Light Blue", "Light Green", "Pink", "Pink" };
            List<string> s5 = new List<string>() { "Purple", "Light Green", "Light Green", "Pink", "Pink" };
            stacks = new List<List<string>>() { s1, s2, s3, s4, s5 };*//*

            //490 w
            *//*List<string> s1 = new List<string>() { "Light Blue", "Red", "Red" };
            List<string> s2 = new List<string>() { "Light Green", "Dark Green", "Dark Green", "Blue", "Blue" };
            List<string> s3 = new List<string>() { "Pink", "Yellow", "Yellow", "Purple", "Purple" };
            List<string> s4 = new List<string>() { "Purple", "Dark Green", "Dark Green", "Blue", "Blue" };
            List<string> s5 = new List<string>() { "Red", "Orange", "Orange", "Light Blue", "Light Blue" };
            List<string> s6 = new List<string>() { "Blue", "Light Blue", "Light Blue", "Light Green", "Light Green" };
            List<string> s7 = new List<string>() { "Orange", "Pink", "Pink" };
            List<string> s8 = new List<string>() { "Dark Green", "Light Green", "Light Green", "Orange", "Pink" };
            List<string> s9 = new List<string>() { "Yellow", "Red", "Red", "Pink" };
            List<string> s10 = new List<string>() { "Orange", "Yellow", "Yellow", "Purple", "Purple" };
            stacks = new List<List<string>>() { s1, s2, s3, s4, s5, s6, s7, s8, s9, s10 };*//*

            //level 107  w
            *//* List<string> s1 = new List<string>() { "Light Blue", "Light Blue", "Blue", "Pink" };
             List<string> s2 = new List<string>() { "Light Green", "Purple", "Orange" };
             List<string> s3 = new List<string>() { "Pink", "Pink", "Blue", "Light Green" };
             List<string> s4 = new List<string>() { "Orange", "Purple" };
             List<string> s5 = new List<string>() { "Red", "Blue", "Purple", "Light Blue" };
             List<string> s6 = new List<string>() { "Blue", "Light Green", "Light Blue", "Red" };
             List<string> s7 = new List<string>() { "Orange", "Purple", "Orange" };
             List<string> s8 = new List<string>() { "Pink", "Red", "Red", "Light Green" };
             stacks = new List<List<string>>() { s1, s2, s3, s4, s5, s6, s7, s8 };*//*

            //504 w 
            *//*List<string> s1 = new List<string>() { "Blue", "Blue", "Purple", "Pink", "Purple" };
            List<string> s2 = new List<string>() { "Green", "Green", "Blue" };
            List<string> s3 = new List<string>() { "Pink", "Pink", "Green", "Green" };
            List<string> s4 = new List<string>() { "Purple", "Blue", "Green" };
            List<string> s5 = new List<string>() { "Purple", "Pink", "Pink", "Blue", "Purple" };
            stacks = new List<List<string>>() { s1, s2, s3, s4, s5 };*//*

            stacks = init();
            printStacks(stacks);
            solve(stacks, solutions, wrongMoves);
        }*/
    }
}
