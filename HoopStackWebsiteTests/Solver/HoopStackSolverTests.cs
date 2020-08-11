using Microsoft.VisualStudio.TestTools.UnitTesting;
using HoopStackWebsite.Solver;
using System;
using System.Collections.Generic;
using System.Text;
using HoopStackWebsite.Models.Level;

namespace HoopStackWebsite.Solver.Tests
{
    [TestClass()]
    public class HoopStackSolverTests
    {
        public Level Level { get; private set; }

        // not used in web app, no console input required
        /*[TestMethod()]
        public void initTest()
        {
            //arrange

            //act

            //assert
            Assert.Fail();
        }*/

        // not used in web app, no console to print to
        /*[TestMethod()]
        public void printStacksTest()
        {
            //arrange
            
            //act

            //assert
            Assert.Fail();
        }*/

        // not used in web app, no console to print to
        /*[TestMethod()]
        public void printInstructionsTest()
        {
            //arrange

            //act

            //assert
            Assert.Fail();
        }*/

        [TestMethod()]
        public void numMovesTest()
        {
            //arrange
            // 0 moves list
            List<List<string>> stacks1 = new List<List<string>>
                                        {   new List<string> {"Blue", "Red", "Red"},
                                            new List<string> {"Blue", "Green"},
                                            new List<string> {"Blue"}
                                        };
            // 1 move list
            List<List<string>> stacks2 = new List<List<string>>
                                        {   new List<string> {"Blue", "Red", "Green"},
                                            new List<string> {"Blue", "Green"},
                                            new List<string> {"Blue"}
                                        };
            // 1+ move list 
            List<List<string>> stacks3 = new List<List<string>>
                                        {   new List<string> {"Blue", "Red", "Green"},
                                            new List<string> {"Blue", "Green"},
                                            new List<string> {"Blue"},
                                            new List<string>()
                                        };
            //act
            List<Move> moves1 = HoopStackSolver.numMoves(stacks1, 3, 0);
            List<Move> moves2 = HoopStackSolver.numMoves(stacks2, 3, 0);
            List<Move> moves3 = HoopStackSolver.numMoves(stacks3, 3, 0);
            //assert
            Assert.AreEqual(moves1.Count, 0);
            Assert.AreEqual(moves2.Count, 1);
            Assert.IsTrue(moves3.Count > 1);
        }

        // not used in web app, no console to print to
        /*[TestMethod()]
        public void printSolTest()
        {
            //arrange

            //act

            //assert
            Assert.Fail();
        }
*/
        [TestMethod()]
        public void checkTest()
        {
            //arrange
            List<List<string>> stacksEmpty = new List<List<string>>();
            List<List<string>> stacksFull = new List<List<string>>
                                        {   new List<string> {"Blue", "Blue", "Blue"},
                                            new List<string> {"Green", "Green", "Green"},
                                            new List<string> {"Red", "Red","Red"}
                                        }; ;
            List<List<string>> stacksNotDone = new List<List<string>>
                                        {   new List<string> {"Blue", "Red", "Green"},
                                            new List<string> {"Blue", "Green"},
                                            new List<string> {"Blue"}
                                        }; ;
            //act
            bool checkEmpty = HoopStackSolver.check(stacksEmpty);
            bool checkFull = HoopStackSolver.check(stacksFull);
            bool checkNotDone = HoopStackSolver.check(stacksNotDone);
            //assert
            Assert.IsTrue(checkEmpty);
            Assert.IsTrue(checkFull);
            Assert.IsFalse(checkNotDone);
        }

        [TestMethod()]
        public void moveTest()
        {
            //arrange
            List<List<string>> stacks = new List<List<string>>
                                        {   new List<string> {"Blue", "Red", "Green"},
                                            new List<string> {"Blue", "Green"},
                                            new List<string> {"Blue"}
                                        }; ;
            //act
            Move possible = HoopStackSolver.move(ref stacks, 0, 0, 1);
            Move notPossible = HoopStackSolver.move(ref stacks, 0, 0, 2);
            //assert
            Assert.IsNotNull(possible);
            Assert.IsNull(notPossible);
        }

        [TestMethod()]
        public void solveLevelTest()
        {
            //arrange
            List<string> s1, s2, s3, s4, s5, s6, s7, s8, s9, s10; 

            s1 = new List<string>() { "Blue", "Blue", "Purple", "Pink", "Purple" };
            s2 = new List<string>() { "Green", "Green", "Blue" };
            s3 = new List<string>() { "Pink", "Pink", "Green", "Green" };
            s4 = new List<string>() { "Purple", "Blue", "Green" };
            s5 = new List<string>() { "Purple", "Pink", "Pink", "Blue", "Purple" };
            Level level504 = new Level(504, new List<List<string>>() { s1, s2, s3, s4, s5 });

            s1 = new List<string>() { "Light Blue", "Light Blue", "Blue", "Pink" };
            s2 = new List<string>() { "Light Green", "Purple", "Orange" };
            s3 = new List<string>() { "Pink", "Pink", "Blue", "Light Green" };
            s4 = new List<string>() { "Orange", "Purple" };
            s5 = new List<string>() { "Red", "Blue", "Purple", "Light Blue" };
            s6 = new List<string>() { "Blue", "Light Green", "Light Blue", "Red" };
            s7 = new List<string>() { "Orange", "Purple", "Orange" };
            s8 = new List<string>() { "Pink", "Red", "Red", "Light Green" };
            Level level107 = new Level(107, new List<List<string>>() { s1, s2, s3, s4, s5, s6, s7, s8 });

            //act
            //**solver is called in level initialization** 

            //assert
            Assert.AreEqual(level504.Error, "No Error");
            Assert.AreEqual(level107.Error, "No Error");

            //** add more levels to be tested if needed ***
        }
    }
}